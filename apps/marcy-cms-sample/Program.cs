using Autofac.Extensions.DependencyInjection;

using CandyKingdom.Marcy.Storage;
using CandyKingdom.MarcyCms;
using CandyKingdom.MarcyCms.Sample;
using CandyKingdom.MarcyCms.Sample.Core;
using CandyKingdom.MarcyCms.Sample.Data;
using CandyKingdom.MarcyCms.Sample.Serialization;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var contratsDir = Path.Combine("..", "..", "apps", "bonnie-cms-sample", "src", "app", "generated");
SpecGenerator.GenerateTsFiles<MarcyCmsSampleGenerationSpec>(contratsDir);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies()
    .ApplicationCookie!.Configure
    (
        opt => opt.Events = new CookieAuthenticationEvents()
        {
            OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = 401;
                return Task.CompletedTask;
            }
        }
    );

builder.Services.AddAuthorizationBuilder();

builder.Services.AddResponseCompression
(
    options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<BrotliCompressionProvider>();
    }
);

builder.Services.AddDbContextFactory<CmsSampleDbContext>
(
    options => options.UseSqlite("Data Source=marcy-cms-sample.db")
);

builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<CmsSampleDbContext>()
    .AddApiEndpoints();

// services.AddSingleton<IFileStorage, LocalFileStorage>();
// services.AddSingleton<IEmailSender, FakeEmailSender>();
// config.MinificationVendors.Add(new ImageMinWebp(new ImageMinWebpOptions(quality: 90)));
builder.Services.AddMarcyCms<CmsSampleDbContext>(config =>
{
    config.FileStorage = new LocalFileStorage("/files/", Path.Combine(builder.Environment.WebRootPath ?? "", "files"));
});

builder.Services.AddSingleton<InitialDataFiller>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(MarcyCmsServiceCollectionExtension).Assembly)
    .AddJsonOptions(o => CmsJsonSerializationOptions.Configure(o.JsonSerializerOptions));

builder.Services.ConfigureHttpJsonOptions(o => CmsJsonSerializationOptions.Configure(o.SerializerOptions));

var app = builder.Build();

app.MapControllers();

app.UseResponseCompression();

app.MapCustomIdentityApi<ApplicationUser>();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// protection from cross-site request forgery (CSRF/XSRF) attacks with empty body
// form can't post anything useful so the body is null, the JSON call can pass
// an empty object {} but doesn't allow cross-Psite due to CORS.

app.MapPost("/api/logout", async (SignInManager<ApplicationUser> signInManager, [FromBody] object empty) =>
        {
            if (empty is not null)
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            }
            return Results.NotFound();
        }
    )
    .RequireAuthorization();

var filler = app.Services.GetRequiredService<InitialDataFiller>();

await filler.InitializeIfNecessary();

app.Run();

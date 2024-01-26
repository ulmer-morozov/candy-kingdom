using System.Text.Json;

using Autofac.Extensions.DependencyInjection;

using CandyKingdom.Marcy;
using CandyKingdom.MarcyCms;
using CandyKingdom.MarcyCms.Sample;
using CandyKingdom.MarcyCms.Sample.Content;
using CandyKingdom.MarcyCms.Sample.Core;
using CandyKingdom.MarcyCms.Sample.Data;
using CandyKingdom.MarcyCms.Sample.Serialization;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var srcSetJson = /*lang=json,strict*/ """{"meta":{"duration":"00:12:14.2600000","fullFormat":"h264 (High) (avc1 / 0x31637661), yuv420p(progressive)","hasAudio":true,"frameRate":24,"width":1280,"height":534,"ratio":2.397,"byteCount":185765954},"mimeType":"video/mp4","url":"https://storage.googleapis.com/gtv-videos-bucket/sample/TearsOfSteel.mp4"}""";

var filesrc = JsonSerializer.Deserialize<FileSrc<VideoMeta>>(srcSetJson, CmsJsonSerializationOptions.New());

var videoSourceJson = /*lang=json,strict*/ """{"mediaQuery":"somequery","srcSet":[{"meta":{"duration":"00:12:14.2600000","fullFormat":"h264 (High) (avc1 / 0x31637661), yuv420p(progressive)","hasAudio":true,"frameRate":24,"width":1280,"height":534,"ratio":2.397,"byteCount":185765954},"mimeType":"video/mp4","url":"https://storage.googleapis.com/gtv-videos-bucket/sample/TearsOfSteel.mp4"}]}""";

var videoSource = JsonSerializer.Deserialize<VideoSource>(videoSourceJson, CmsJsonSerializationOptions.New());

var videoJson = /*lang=json,strict*/ """{"$$type":"video","sources":[{"mediaQuery":"","srcSet":[{"meta":{"duration":"00:12:14.2600000","fullFormat":"h264 (High) (avc1 / 0x31637661), yuv420p(progressive)","hasAudio":true,"frameRate":24,"width":1280,"height":534,"ratio":2.397,"byteCount":185765954},"mimeType":"video/mp4","url":"https://storage.googleapis.com/gtv-videos-bucket/sample/TearsOfSteel.mp4"}]}]}""";

var video = JsonSerializer.Deserialize<PixMedia>(videoJson, CmsJsonSerializationOptions.New());

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

builder.Services.AddSingleton<IPageManager, PageManager<CmsSampleDbContext>>();
// services.AddSingleton<ISettingsManager, SettingsManager>();

builder.Services.AddSingleton<InitialDataFiller>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddControllers()
    .AddJsonOptions(o => CmsJsonSerializationOptions.Configure(o.JsonSerializerOptions));

builder.Services.ConfigureHttpJsonOptions(o => CmsJsonSerializationOptions.Configure(o.SerializerOptions));

var app = builder.Build();

app.MapControllers();

app.UseResponseCompression();

app.MapCustomIdentityApi<ApplicationUser>();
// app.UseDefaultFiles();
// app.UseStaticFiles();

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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/api/weatherforecast", () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        }
    )
    .WithName("GetWeatherForecast")
    .WithOpenApi()
    .RequireAuthorization();

var filler = app.Services.GetRequiredService<InitialDataFiller>();

await filler.InitializeIfNecessary();

app.Run();

namespace CandyKingdom.MarcyCms.Sample
{
    internal sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}

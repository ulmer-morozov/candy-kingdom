
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms;

public class AppDbContext(DbContextOptions<AppDbContext> options) :
    IdentityDbContext<MyUser>(options)
{
}

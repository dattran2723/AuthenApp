using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Entity.Contexts
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext()
            : base()
        {
        }
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}

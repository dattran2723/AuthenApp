using Abstractions.UnitOfWork;
using Entities.Entities;
using Entity.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Entity.UnitOfWork
{
    public class UserRepo : IUserRepo
    {
        private readonly AuthDbContext _context;
        public UserRepo(DbContext context)
        {
            _context = new AuthDbContext();
        }
        public User Insert(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            var userId = user.Id;

            return user;
        }
    }
}

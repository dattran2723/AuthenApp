using Entities.Entities;

namespace Abstractions.UnitOfWork
{
    public interface IUserRepo
    {
        User Insert(User user);
    }
}

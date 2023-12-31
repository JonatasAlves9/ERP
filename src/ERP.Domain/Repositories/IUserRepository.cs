using ERP.Domain.Entities;
using ERP.Domain.ValueObjects;
using ERP.Shared.Abstractions;

namespace ERP.Domain.Repositories;

public interface IUserRepository : IAsyncRepository<User>
{
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(Guid id);
    Task<IUserNotificationSettings> GetNotificationSettings(Guid userId);
    Task<IEnumerable<User>> GetAll();
}

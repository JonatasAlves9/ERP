using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Domain.ValueObjects;
using ERP.Infrastructure.Data.Context;
using ERP.Infrastructure.Data.Repositories.Common;

namespace ERP.Infrastructure.Data.Repositories;

public class UserRepository : EfRepository<User>, IUserRepository
{
    public UserRepository(ErpContext context) : base(context)
    {
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await DbSet.FirstOrDefaultAsync(u => u.Email == email);

        return user;
    }
   
}

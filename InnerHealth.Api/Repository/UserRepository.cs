using InnerHealth.Api.Data;
using InnerHealth.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnerHealth.Api.Infrastructure.Data
{
    public class UserRepository
    {
        private readonly ApplicationDbContext  _ctx;

        public UserRepository(ApplicationDbContext  ctx)
        {
            _ctx = ctx;
        }

        public async Task<User?> GetByEmailAsync(string email)
            => await _ctx.Users.FirstOrDefaultAsync(x => x.Email == email);

        public async Task AddAsync(User user)
        {
            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();
        }
    }
}

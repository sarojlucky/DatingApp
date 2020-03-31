using System;
using System.Threading.Tasks;
using Datingapp.api.Models;
using Microsoft.EntityFrameworkCore;

namespace Datingapp.api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if(user == null)
            {
                return null;
            }

            if(!VerifyPasswordJHash(password, user.PasswordHash, user.PasswordSalt))
            return null;

            return user;

        }

        private bool VerifyPasswordJHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hamc = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hamc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                 if(computeHash[i] != passwordHash[i]) return false;   
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passworSalt;
            CreatePasswordHash(password, out passwordHash, out passworSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passworSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passworSalt)
        {
            using (var hamc = new System.Security.Cryptography.HMACSHA512())
            {
                passworSalt = hamc.Key;
                passwordHash = hamc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.Username == username)) 
            return true;
            
            return false;

        }
    }
}
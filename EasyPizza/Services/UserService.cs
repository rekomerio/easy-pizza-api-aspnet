using System;
using System.Collections.Generic;
using System.Linq;
using EasyPizza.DAL;
using EasyPizza.Models;
using EasyPizza.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EasyPizza.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(long id);
        Task<User> Create(User user, string password);
        Task<User> Delete(long id);
        void Update(User user, string password = null);
    }

    public class UserService : IUserService
    {
        private readonly EasyPizzaContext _context;

        public UserService(EasyPizzaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);

            // User is null if no user is registered with given email
            if (user == null)
            {
                return null;
            }

            // Check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
                
            // Authentication successful
            return user;
        }

        public async Task<User> GetById(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ApplicationException("Password is required");

            if (_context.Users.Any(x => x.Email == user.Email))
                throw new ApplicationException("Email \"" + user.Email + "\" is taken");


            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.CreatedAt = DateTime.Now;

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Delete(long id)
        {
            User user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return user;
        }

        public async void Update(User user, string password = null)
        {
            User userToUpdate = _context.Users.Find(user.Id);

            if (userToUpdate == null)
                throw new ApplicationException("User not found");

            // If user is trying to change his email, check no one else is using it
            if (!string.IsNullOrWhiteSpace(user.Email) && user.Email != userToUpdate.Email)
            {
                if (_context.Users.Any(x => x.Email == user.Email))
                    throw new ApplicationException("Email \"" + user.Email + "\" is taken");
            }

            // If user is updating his password, create new salt and hash
            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}

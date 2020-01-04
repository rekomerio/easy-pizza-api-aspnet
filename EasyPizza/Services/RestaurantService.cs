using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyPizza.Entities;
using EasyPizza.DAL;
using Microsoft.EntityFrameworkCore;

namespace EasyPizza.Services
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetAll();
        Task<Restaurant> GetById(long id);
        Task<Restaurant> Create(Restaurant restaurant, long creatorId);
        Task<Restaurant> Delete(long id);
        Task<long> GetOwner(long id);
        void Update(Restaurant restaurant);
    }
    public class RestaurantService : IRestaurantService
    {
        private readonly EasyPizzaContext _context;

        public RestaurantService(EasyPizzaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurant>> GetAll()
        {
            return await _context.Restaurants.ToListAsync();
        }
        public async Task<Restaurant> GetById(long id)
        {
            return await _context.Restaurants.FindAsync(id);
        }

        public async Task<Restaurant> Create(Restaurant restaurant, long creatorId)
        {
            User creator = await _context.Users.FindAsync(creatorId);

            if (creator == null)
                throw new ApplicationException("User with given id \"" + creatorId + "\" does not exist");

            restaurant.UserId = creatorId;
            restaurant.CreatedAt = DateTime.Now;
            restaurant.EditedAt = DateTime.Now;

            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();

            return restaurant;
        }

        public async Task<Restaurant> Delete(long id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
            }

            return restaurant;
        }

        public async void Update(Restaurant restaurant)
        {
            Restaurant restaurantToUpdate = _context.Restaurants.Find(restaurant.Id);

            if (restaurantToUpdate == null)
                throw new ApplicationException("Restaurant was not found");

            restaurant.EditedAt = DateTime.Now;

            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task<long> GetOwner(long id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            return restaurant != null ? restaurant.UserId : -1;
        }
    }
}

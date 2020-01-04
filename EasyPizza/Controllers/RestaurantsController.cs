using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyPizza.Services;
using EasyPizza.Entities;
using EasyPizza.Models.UserModels;
using EasyPizza.Models.RestaurantModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EasyPizza.Helpers;

namespace EasyPizza.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;

        public RestaurantsController(
            IRestaurantService restaurantService,
            IMapper mapper)
        {
            _restaurantService = restaurantService;
            _mapper = mapper;
        }

        // GET: api/Restaurants
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantModel>>> GetAll()
        {
            IEnumerable<Restaurant> restaurants = await _restaurantService.GetAll();
            IEnumerable<RestaurantModel> restaurantModels = _mapper.Map<IEnumerable<RestaurantModel>>(restaurants);

            return Ok(restaurantModels);
        }

        // GET: api/Restaurants/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantModel>> GetById(long id)
        {
            Restaurant restaurant = await _restaurantService.GetById(id);

            if (restaurant == null)
                return NotFound();

            return Ok(FormatForUser(restaurant));
        }

        // PUT: api/Restaurants/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurant(long id, [FromBody]UpdateModel model)
        {
            AuthorizationModel auth = UserAuth();

            long restaurantOwner = await _restaurantService.GetOwner(id);

            if (auth.IsNotOwner(restaurantOwner) && auth.IsNotAdmin())
                return BadRequest(new { message = "Insufficient privildeges" });

            Restaurant restaurant = _mapper.Map<Restaurant>(model);
            try
            {
                _restaurantService.Update(restaurant);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Restaurants
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<RestaurantModel>> Create([FromBody]CreateModel createModel)
        {
            Restaurant restaurant = _mapper.Map<Restaurant>(createModel);
            try
            {
                restaurant = await _restaurantService.Create(restaurant, UserAuth().Id);
                return CreatedAtAction("GetById", new { id = restaurant.Id }, FormatForUser(restaurant));
            } 
            catch(ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RestaurantModel>> DeleteRestaurant(long id)
        {
            AuthorizationModel auth = UserAuth();
            // Read owner of the restaurant
            long restaurantOwner = await _restaurantService.GetOwner(id);

            if (auth.IsNotOwner(restaurantOwner) && auth.IsNotAdmin())
                return BadRequest(new { message = "Insufficient privildeges" });
            // Delete the restaurant
            Restaurant deletedRestaurant = await _restaurantService.Delete(id);
            // If deleted restauraunt is null, it does not exist
            if (deletedRestaurant == null)
                return NotFound();

            return Ok(FormatForUser(deletedRestaurant));
        }
        /*
        * Map Restaurant to RestaurantModel to hide some fields that are not supposed to be 
        * shown in the api requests
        */
        private RestaurantModel FormatForUser(Restaurant restaurant)
        {
            return _mapper.Map<RestaurantModel>(restaurant);
        }
        /*
        * Reads user authorization information from JWT 
        */
        private AuthorizationModel UserAuth()
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            return identity.ReadUserDataFromJWT();
        }
    }
}

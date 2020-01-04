using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyPizza.Entities;
using EasyPizza.DAL;
using AutoMapper;
using EasyPizza.Services;
using EasyPizza.Models.MenuModels;

namespace EasyPizza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly EasyPizzaContext _context;
        private readonly IMapper _mapper;

        public MenusController(EasyPizzaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Menu> menus = await _context.Menus
                .Include(menu => menu.MenuItems)
                    .ThenInclude(menuItem => menuItem.Ingredients)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<MenuModel>>(menus));
        }

        // GET: api/Menus/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var menu = await _context.Menus
                .Include(menu => menu.MenuItems)
                    .ThenInclude(menuItem => menuItem.Ingredients)
                .SingleOrDefaultAsync(menu => menu.Id == id);

            if (menu == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MenuModel>(menu));
        }
        [HttpGet("restaurant/{id}")]
        public async Task<IActionResult> GetByRestaurantId(long id)
        {
          IEnumerable<Menu> menus = await _context.Menus
                .Where(menu => menu.RestaurantId == id)
                .Include(menu => menu.MenuItems)
                    .ThenInclude(menuItem => menuItem.Ingredients)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<MenuModel>>(menus));
        }

        // PUT: api/Menus/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody]Menu menu)
        {
            Menu menuToUpdate = await _context.Menus.FindAsync(menu.Id);

            if (menuToUpdate != null)
            {
                _context.Menus.Update(menu);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }

        // POST: api/Menus
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<IActionResult> Create(Menu menu)
        {
            await _context.Menus.AddAsync(menu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = menu.Id }, menu);
        }

        // DELETE: api/Menus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            Menu menu = await _context.Menus.FindAsync(id);

            if (menu == null)
                return NotFound();

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();

            return Ok(menu);
        }
    }
}

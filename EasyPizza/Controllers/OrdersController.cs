using AutoMapper;
using EasyPizza.DAL;
using EasyPizza.Entities;
using EasyPizza.Helpers;
using EasyPizza.Models.OrderModels;
using EasyPizza.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyPizza.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly EasyPizzaContext _context;
        private readonly IMapper _mapper;

        public OrdersController(EasyPizzaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Orders
                .Where(order => order.UserId == UserAuth().Id)
                .Include(order => order.OrderItems)
                .Include(order => order.Recipient)
                .ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(long id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody]CreateOrderModel createOrderModel)
        {
            Order order = _mapper.Map<Order>(createOrderModel);
            // User existance is checked in authorization so no need to check user exists here
            order.UserId = UserAuth().Id;
            order.CreatedAt = DateTime.Now;
            order.PaymentStatus = PaymentStatus.Pending;
            order.OrderStatus = OrderStatus.Pending;
            order.Price = 0;
            // Calculate price and save menuItem name and price to order item
            foreach (var item in order.OrderItems)
            {
                var menuItem = await _context.MenuItems.FindAsync(item.MenuItemId);
                order.Price += (menuItem.Price * item.Quantity);
                item.Name = menuItem.Name;
                item.Price = menuItem.Price;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, _mapper.Map<OrderModel>(order));
        }

        [HttpPost("{id}/set/status/{status}")]
        public async Task<IActionResult> SetOrderStatus(long id, int status)
        {
            var order = await _context.Orders.FindAsync(id);
            order.OrderStatus = (OrderStatus)status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<OrderModel>(order));
        }

        [HttpPost("{id}/set/payment/{status}")]
        public async Task<IActionResult> SetPaymentStatus(long id, int status)
        {
            var order = await _context.Orders.FindAsync(id);
            order.PaymentStatus = (PaymentStatus)status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<OrderModel>(order));
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        private AuthorizationModel UserAuth()
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            return identity.ReadUserDataFromJWT();
        }
    }
}

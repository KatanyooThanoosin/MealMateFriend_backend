using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using main_backend.Models;
using main_backend.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace main_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller{
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService){
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder(NewOrderModel newOrder){
            try{
                string userId = Request.HttpContext.User.FindFirstValue("UserId");
                await _orderService.CreateOrderAsync(newOrder,userId);
                return Ok();
            }
            catch{
                return BadRequest();
            }
        }
    }
}
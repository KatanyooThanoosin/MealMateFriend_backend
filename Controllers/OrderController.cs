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
        private readonly PostService _postService;
        private readonly UserService _userService;

        public OrderController(OrderService orderService,PostService postService,UserService userService){
            _orderService = orderService;
            _postService = postService;
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        [Route("GetOrdersByMyPost")]
        public async Task<List<RubFarkModel>> GetOrdersByMyPost(){
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            var post = await _postService.GetPostByUserIdAsync(userId);
            var orders = await _orderService.ListOrdersByPostId(post.Id);
            var rubFark = new List<RubFarkModel>();
            foreach(var order in orders){
                var user = await _userService.GetUserByIdAsync(order.Owner);
                rubFark.Add(new RubFarkModel { 
                    OrderStatus = order.Status,
                    Username = user.Username,
                    Phone = user.Phone,
                    FoodName = order.Foodname,
                    Note = order.Note
                });
            };
            return rubFark;
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
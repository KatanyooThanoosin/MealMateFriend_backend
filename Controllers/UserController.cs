using Microsoft.AspNetCore.Mvc;
using main_backend.Models;
using main_backend.Services;

namespace MongoExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller{

        private readonly UserService _userService;

        public UserController(UserService userService){
            _userService = userService;
        }

        [HttpGet]
        public async Task<List<UserModel>> Get(){
            return await _userService.GetAllUserAsync();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(NewUserModel newUser){
            try{
                await _userService.CreateUserAsync(newUser);
                return Ok();  
            }
            catch{
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel loginUser){
            var user = await _userService.LoginAsync(loginUser);
            if(user==null){
                return NotFound();
            }
            else{
                return Ok();
            }
        }
    }
}
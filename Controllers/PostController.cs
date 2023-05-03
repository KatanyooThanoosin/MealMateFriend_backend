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
    public class PostController : Controller{
        private readonly PostService _postService;
        private readonly UserService _userService;

        public PostController(PostService postService,UserService userService){
            _postService = postService;
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        [Route("ListAllPosts")]
        public async Task<List<PostModel>> ListAllPosts(){
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            var posts = await _postService.ListAllPostsAsync(userId);
            foreach (var post in posts){
                var owner = await _userService.GetUserByIdAsync(post.Owner);
                post.OwnerUserName = owner.Username;
            }
            return posts;
        }

        [Authorize]
        [HttpPost]
        [Route("CreatePost")]
        public async Task<IActionResult> CreatePost(NewPostModel newPost){
            try{
                string userId = Request.HttpContext.User.FindFirstValue("UserId");
                await _postService.CreatePostAsync(userId,newPost);
                return Ok();
            }
            catch{
                return BadRequest();
            }
            
        }

    } 
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TestAPI1.Data;
using TestAPI1.Data.Models;

namespace TestAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public const string Role_RegisteredUser = "RegisteredUser";
        public const string Role_Administrator = "Administrator";

        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtHandler _jwtHandler;

        public AuthController(ApplicationDBContext dbContext, UserManager<ApplicationUser> userManager, JwtHandler jwtHandler)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            //var user = await _userManager.FindByNameAsync("user@email.com");
            //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            //await _userManager.ResetPasswordAsync(user, code, "zcdse$#gbdfwq3458765nJ");

            var user = await _userManager.FindByNameAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                return Unauthorized(new LoginResult()
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }
            var secToken = await _jwtHandler.GetTokenAsync(user!);
            var jwt = new JwtSecurityTokenHandler().WriteToken(secToken);
            return Ok(new LoginResult()
            {
                Success = true,
                Message = "Login successful",
                Token = jwt
            });
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Email);
            if (user != null || loginRequest.Email == "" || loginRequest.Password == "")
            {
                return Unauthorized(new LoginResult()
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }

            var applicationUser = new ApplicationUser()
            {
                Email = loginRequest.Email,
                UserName = loginRequest.Email
            };
            try
            {
                var creationResult = await _userManager.CreateAsync(applicationUser, loginRequest.Password);
                var createdUser = await _userManager.FindByNameAsync(loginRequest.Email);
                if (createdUser == null)
                {
                    return Unauthorized(new LoginResult()
                    {
                        Success = false,
                        Message = "Unable to create user: possible cause: invalid password"
                    }); ;
                }
                var secToken = await _jwtHandler.GetTokenAsync(createdUser!);
                var jwt = new JwtSecurityTokenHandler().WriteToken(secToken);
                return Ok(new LoginResult()
                {
                    Success = true,
                    Message = "User creation successful",
                    Token = jwt
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new LoginResult()
                {
                    Success = false,
                    Message = "Error while creating user: " + ex.Message
                });
            }
        }
    }
}

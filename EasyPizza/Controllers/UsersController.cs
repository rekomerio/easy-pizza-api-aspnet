using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EasyPizza.Entities;
using EasyPizza.Helpers;
using EasyPizza.Models.UserModels;
using EasyPizza.Services;

namespace EasyPizza.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        /*
        * Returns all users. Accessable only for admins
        * URL: /api/users
        * Method: GET
        */
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (UserAuth().IsNotAdmin())
                return BadRequest(new { message = "Insufficient privildeges" });

            IEnumerable<User> users = await _userService.GetAll();
            IEnumerable<UserModel> userModels = _mapper.Map<IEnumerable<UserModel>>(users);

            return Ok(userModels);
        }
        /*
        * Returns single user information 
        * URL: /api/users/:id
        * Method: GET
        */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            AuthorizationModel auth = UserAuth();

            if (auth.IsNotOwner(id) && auth.IsNotAdmin())
                return BadRequest(new { message = "Insufficient privildeges" });

            User user = await _userService.GetById(id);
            UserModel model = _mapper.Map<UserModel>(user);

            return Ok(model);
        }
        /*
        * Returns single user information. User is identified from sent JWT
        * URL: /api/users/me
        * Method: GET
        */
        [HttpGet("me")]
        public async Task<IActionResult> GetUserFromJWT()
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            AuthorizationModel auth = identity.ReadUserDataFromJWT();

            if (auth != null)
                return await GetById(auth.Id);

            return NotFound();
        }
        /*
        * Returns a new user if registeration was successful
        * URL: /api/users/register
        * Method: POST
        */
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            User user = _mapper.Map<User>(model);

            try
            {
                // Create an user
                user = await _userService.Create(user, model.Password);
                // Convert user to usermodel to hide secret fields for returning the data
                UserModel userModel = _mapper.Map<UserModel>(user);
               
                return CreatedAtAction("GetById", new { id = user.Id }, userModel);
            }
            catch (ApplicationException ex)
            {
                // Most common exception is taken email
                return BadRequest(new { message = ex.Message });
            }
        }
        /*
        * Returns JWT for accessing API methods and other user information if given email and password match
        * URL: /api/users/authenticate
        * Method: POST
        */
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            // Returns user object, if email and password match, otherwise null
            User user = await _userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Invalid email or password" });

            // Create JWT if authentication was success
            string token = CreateJWT(user.Id, user.UserGroup);

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.UserGroup,
                Token = token
            });
        }
        /*
        * Updates user of the given id if the request maker is owner of the account or admin
        * URL: /api/users/:id
        * Method: PUT
        */
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody]UserUpdateModel model)
        {
            AuthorizationModel auth = UserAuth();

            if (auth.IsNotOwner(id) && auth.IsNotAdmin())
                return BadRequest(new { message = "Insufficient privildeges" });
            
            User user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                _userService.Update(user, model.Password);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                // Error can be thrown by already taken email
                return BadRequest(new { message = ex.Message });
            }
        }
        /*
        * Deletes user of the given id if the request maker is owner of the account or admin
        * URL: /api/users/:id
        * Method: DELETE
        */
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserModel>> Delete(int id)
        {
            AuthorizationModel auth = UserAuth();

            if (auth.IsNotOwner(id) && auth.IsNotAdmin())
                return BadRequest(new { message = "Insufficient privildeges" });

            User deletedUser = await _userService.Delete(id);

            if (deletedUser == null)
                return NotFound();

            UserModel userModel = _mapper.Map<UserModel>(deletedUser);

            return userModel;
        }
        /*
        * Reads user authorization information from JWT 
        */
        private AuthorizationModel UserAuth()
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            return identity.ReadUserDataFromJWT();
        }
        /*
        * Creates new JWT and returns in string format
        */
        private string CreateJWT(long id, UserGroup userGroup)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // Read secret key from app settings
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id.ToString()),                  // Save user id
                    new Claim(ClaimTypes.Role, ((int) userGroup).ToString())    // Save user usergroup
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}

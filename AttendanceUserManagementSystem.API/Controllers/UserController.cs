using AttendanceUserManagementSystem.API.Authentication;
using AttendanceUserManagementSystem.API.Repositories;
using AttendanceUserManagementSystem.API.Resources.DTO;
using AttendanceUserManagementSystem.API.Resources.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttendanceUserManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {

            _userRepository = userRepository;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]GetUsersResourceParameters parameters)
        {
            var users = await _userRepository.GetAllUsers(parameters);


            if (users.Count > 0)
            {
                return Ok(users);

            }

            return Ok("No users found");

        }

        [HttpPost]
        public async Task<IActionResult> Post(RegisterUserDto registerUserDto)
        {
            
            await _userRepository.AddUser(registerUserDto);

            return Ok("User has been created");

        }


        // GET api/<UserController>/5
        [HttpGet("id")]
        public async Task<IActionResult> Get(string id)
        {

            var user = await _userRepository.GetUserById(id);

            if (user != null)
            {


                return Ok(user);
            }

            return BadRequest("User does not exist");
        }

        [HttpGet("init")]
        public async Task<ActionResult> Init()
        {


            if (await _userRepository.InitAdmin())
            {
                return Ok("User Created Succesfully");
            }

            return BadRequest("User creation failed");


        }

        [HttpGet("All-Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _userRepository.GetAllRoles();

            return Ok(roles);
        }

        [HttpPut]
        // PUT api/<UserController>/5
        public async Task<IActionResult> Put(string id, UpdateUserDto upgradeUserDTO)
        {
            var upgradeUser = await _userRepository.GetUserById(id);

            if (upgradeUser != null)
            {
                upgradeUser.FirstName = upgradeUserDTO.FirstName;
                upgradeUser.LastName = upgradeUserDTO.LastName;

                await _userRepository.UpdateUser(upgradeUser);

                return Ok("User has been successfully upgraded");
            }

            return BadRequest("User does not exist");

        }
    }
}

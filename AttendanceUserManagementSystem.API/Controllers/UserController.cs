using AttendanceUserManagementSystem.API.Authentication;
using AttendanceUserManagementSystem.API.Repositories;
using AttendanceUserManagementSystem.API.Resources.DTO;
using AttendanceUserManagementSystem.API.Resources.Models;
using AttendanceUserManagementSystem.API.Resources.ResourceParameters;
using AttendanceUserManagementSystem.API.Resources.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttendanceUserManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {

            _userRepository = userRepository;
            _mapper = mapper;
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


        [HttpGet("Users-Without-Addresses")]
        public async Task<IActionResult> Get()
        {
            var parameters = new GetUsersResourceParameters();

            var users = await _userRepository.GetAllUsers(parameters);


            if (users.Count > 0)
            {
                var responses = new List<UsersWithoutAddressesResponse>();  

                foreach (var user in users)
                {
                    if (user.MACAddress == " " || user.IPAddress == " ")
                    {
                        var response = _mapper.Map<UserDto, UsersWithoutAddressesResponse>(user);

                        responses.Add(response);

                    }
                }

                return Ok(responses);

            }

            return Ok("No users found");

        }

        [HttpPut("Add-Addresses-Range")]
        public async Task<ActionResult> AddRange(List<AddAddressesDto> addAddresses)
        {


            var userList = new List<ApplicationUser>();   

            foreach (var addAddress in addAddresses)
            {
                var user = await _userRepository.GetUserByCode(addAddress.EmployeeCode);

                if (user != null)
                {
                    user.MACAddress = addAddress.MACAddress;
                    user.IPAddress = addAddress.IPAddress;

                    userList.Add(user);
                }
            }

            var results = await _userRepository.AddRangeAddresses(userList);

            return Ok(results);

        }

        // GET api/<UserController>/5
        [HttpGet("id")]
        public async Task<ActionResult> Get(string id)
        {

            var user = await _userRepository.GetUserById(id);

            if (user != null)
            {


                return Ok(user);
            }

            return BadRequest("User does not exist");
        }

        [HttpGet("Employee-Info")]
        public async Task<IActionResult> GetInfo(string employeeCode)
        {
            var parameter = new GetUsersResourceParameters();

            parameter.EmployeeCode = employeeCode;


            var user = await _userRepository.GetAllUsers(parameter);

            if (user.Count < 1)
            {
                return BadRequest("User does not exist");
            }

            var response = new EmployeeInfoModel();

            foreach (var item in user)
            {
                response.EmployeeCode = item.EmployeeCode;
                response.FirstName = item.FirstName;
                response.LastName = item.LastName;
                response.Email = item.Email;
            }

            

            return Ok(response);

           
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
                upgradeUser.IPAddress = upgradeUserDTO.IPAddress;
                upgradeUser.MACAddress = upgradeUserDTO.MACAddress;

                await _userRepository.UpdateUser(upgradeUser);

                return Ok("User has been successfully updated");
            }

            return BadRequest("User does not exist");

        }

        [HttpPut("Exempt-User")]
        // PUT api/<UserController>/5
        public async Task<IActionResult> Exempt(string id, ExemptUserDto userDTO)
        {
            var upgradeUser = await _userRepository.GetUserById(id);

            if (upgradeUser != null)
            {
                upgradeUser.AddressAuthenticationExemption = userDTO.IsExcempted ;

                await _userRepository.UpdateUser(upgradeUser);

                return Ok("User has been successfully upgraded");
            }

            return BadRequest("User does not exist");

        }
    }
}

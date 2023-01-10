using AttendanceUserManagementSystem.API.Enumerators;
using AttendanceUserManagementSystem.API.Repositories;
using AttendanceUserManagementSystem.API.Resources.Models;
using AttendanceUserManagementSystem.API.Resources.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.NetworkInformation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttendanceUserManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _authentication;

        // GET: api/<AuthenticationController>

        public AuthenticationController(IAuthenticationRepository authentication)
        {
            _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
        }


        // POST api/<AuthenticationController>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Post(LoginModel loginModel)
        {

            string mac = "";


            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                PhysicalAddress address = adapter.GetPhysicalAddress();
                mac = address.ToString();
            }

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();


           

            var loginResponse = await _authentication.Login(loginModel, ip, mac);

            return Ok(loginResponse);
        }


        [Authorize]
        [HttpPost("Change-Password")]
        public async Task<ActionResult> Post(ChangePasswordModel changePasswordModel)
        {
            var Result = await _authentication.ChangePassword(changePasswordModel);

            return Ok(Result);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("Change-Password-Admin")]
        public async Task<ActionResult> Post(ChangePasswordAdminModel changePassword)
        {

            var Result = await _authentication.ChangePasswordAdmin(changePassword);

            return Ok(Result);
        }

    }
}

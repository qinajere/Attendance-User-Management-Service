using AttendanceUserManagementSystem.API.Authentication;
using AttendanceUserManagementSystem.API.Resources.Models;
using AttendanceUserManagementSystem.API.Resources.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace AttendanceUserManagementSystem.API.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<LoginResponse> Login(LoginModel loginModel, string ip, string mac)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            if (user != null && user.ActivationStatus && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {


                if (!user.AddressAuthenticationExemption)
                {
                    if(user.IPAddress != ip || user.MACAddress != mac )
                    {
                        throw new Exception("Failed to Login");
                    }

                }

                var userRoles = await _userManager.GetRolesAsync(user);


                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }


                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );


                var login = new LoginResponse() { UserId = user.Id, Expiration = token.ValidTo, Token = new JwtSecurityTokenHandler().WriteToken(token), Role = userRoles[0] };

                return login;
            }

            else
            {
                throw new Exception("Failed to Login");
            }
        }

        public async Task<bool> ChangePassword(ChangePasswordModel changePasswordDto)
        {
            var userExists = await _userManager.FindByIdAsync(changePasswordDto.UserID);

            if (userExists == null)
            {
                return false;
            }


            var passwordValidator = new PasswordValidator<ApplicationUser>();
            var passwordValidatorResult = await passwordValidator.ValidateAsync(_userManager, null, changePasswordDto.CurrentPassword);


            if (passwordValidatorResult.Succeeded)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(userExists);

                var result = await _userManager.ResetPasswordAsync(userExists, token, changePasswordDto.NewPassword);

                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /*  public async Task<bool> InitiatePasswordReset(string id)
          {
              var resetPassword = new ResetPassword()
              {
                  Id = 0,
                  UserId = id,
                  Created = DateTime.Now,
                  ValidityPeriod = DateTime.Now.AddHours(2)

              };
              await _applicationDbContext.ResetPasswords.AddAsync(resetPassword);
              await _applicationDbContext.SaveChangesAsync();

              return true;
          }*/

        public async Task<bool> ChangePasswordAdmin(ChangePasswordAdminModel changePasswordAdminModel)
        {
            var userExists = await _userManager.FindByIdAsync(changePasswordAdminModel.UserId);

            if (userExists == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(userExists);

            var result = await _userManager.ResetPasswordAsync(userExists, token, changePasswordAdminModel.Password);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

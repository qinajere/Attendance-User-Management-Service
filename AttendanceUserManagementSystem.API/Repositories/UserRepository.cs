using AttendanceUserManagementSystem.API.Authentication;
using AttendanceUserManagementSystem.API.Enumerators;
using AttendanceUserManagementSystem.API.Helpers;
using AttendanceUserManagementSystem.API.Resources.DTO;
using AttendanceUserManagementSystem.API.Resources.Models;
using AttendanceUserManagementSystem.API.Resources.ResourceParameters;
using AttendanceUserManagementSystem.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics;
using System.Net.Mail;

namespace AttendanceUserManagementSystem.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IConfiguration _configuration;

        public UserRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSenderService emailSenderService,IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSenderService = emailSenderService;
            _configuration = configuration;
        }
        public async Task AddUser(RegisterUserDto user)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(user.Email);

                var username = await this.CreateUsername(user.FirstName, user.LastName);

                if (userExists != null)
                    throw new Exception("User already exists");

                ApplicationUser appUser = new ApplicationUser()
                {
                    Email = user.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmployeeCode = user.EmployeeCode,       
                    UserName = username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ActivationStatus = true,
                    CreationDate = DateTime.Now

                };


                var password = RandomPasswordGenerator.GenerateRandomPassword();
                var result = await _userManager.CreateAsync(appUser, password);
                if (!result.Succeeded)
                    throw new Exception("User creation failed! Please check user details and try again.");



                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await _roleManager.RoleExistsAsync(UserRoles.RegularUser))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.RegularUser));
                if (!await _roleManager.RoleExistsAsync(UserRoles.SuperUSer))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.SuperUSer));
               

                if (user.Role == UserRoles.RegularUser)
                {
                    await _userManager.AddToRoleAsync(appUser, UserRoles.RegularUser);
                }

                if (user.Role == UserRoles.Admin)
                {
                    await _userManager.AddToRoleAsync(appUser, UserRoles.Admin);
                }

                if (user.Role == UserRoles.SuperUSer)
                {
                    await _userManager.AddToRoleAsync(appUser, UserRoles.SuperUSer);
                }


                // send email

                var passwordModel = new PasswordEmailModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = appUser.UserName,
                    Role = user.Role,
                    Password = password
                };
                var emailContent = RegistrationEmailContent.PasswordEmailContent(passwordModel, _configuration);

                var emailMessege = new EmailMessage()
                {
                    From = "pha@nitel.mw",
                    To = user.Email,
                    Subject = "Account Registration",
                    Content = emailContent,

                };


                _emailSenderService.SendEmail(emailMessege);



                Log.Information("User email sent successfully to: " + user.Email);


            }
            catch (Exception ex)
            {

                Log.Error("An error occured when creating user: " + ex);
            }
        }

        public async Task<string> CreateUsername(string firstname, string lastname)
        {
            var username = $"{firstname[0]}{lastname}".ToLower();
            var usernameExists = await _userManager.FindByNameAsync(username);

            if (usernameExists == null)
            {
                return username;
            }

            if (usernameExists != null)
            {
                return $"{firstname[0]}{firstname[1]}{firstname[2]}{lastname}";
            }

            return null;
        }

        public async Task<List<string>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var role = new List<string>();

            foreach (var item in roles)
            {
                role.Add(item.Name);
            }

            return role;
        }

        public async Task<List<UserDto>> GetAllUsers(GetUsersResourceParameters parameters)
        {
            try
            {
                var query = _applicationDbContext.Users
                    .AsQueryable();


                if (parameters.RegistrationStartDate != null && parameters.RegistrationEndDate != null)
                {
                    parameters.RegistrationEndDate = parameters.RegistrationEndDate.Value.AddHours(23);

                    query = query.Where(u => u.CreationDate >= parameters.RegistrationStartDate && u.CreationDate <= parameters.RegistrationEndDate);
                }

                if (parameters.ActivationStatus != null)
                {
                    query = query.Where(u => u.ActivationStatus == parameters.ActivationStatus);
                }

                if (parameters.EmployeeCode != null)
                {
                    query = query.Where(u => u.EmployeeCode == parameters.EmployeeCode);
                }


                var users = await query.ToListAsync();


                var usersList = new List<UserDto>();

                foreach (var user in users)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var mapUser = new UserDto()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        ActivationStatus = user.ActivationStatus,
                        Email = user.Email,
                        CreationDate = user.CreationDate,
                        LastName = user.LastName,
                        FirstName = user.FirstName,
                        EmployeeCode = user.EmployeeCode,
                        Role = userRoles[0]

                    };


                    if (parameters.Role != null)
                    {
                        if (mapUser.Role == parameters.Role)
                        {
                            usersList.Add(mapUser);
                        }
                    }

                    else
                    {
                        usersList.Add(mapUser);
                    }

                }

                return usersList;
            }
            catch (Exception)
            {

                throw new Exception("Failed to get all users");
            }

        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            try
            {

                var user = await _applicationDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    throw new ArgumentNullException("User does not exist");
                }

                return user;

            }
            catch (Exception ex)
            {

                Log.Error("Failed to get user: " + ex);
                return null;
            }

        }

        public async Task UpdateUser(ApplicationUser user)
        {

            try
            {

                var result = await _userManager.UpdateAsync(user);
            }
            catch (Exception)
            {

                throw new Exception("Failed to update user");
            }
        }

        public async Task<bool> InitAdmin()
        {
            try
            {
                ApplicationUser adminUser = new ApplicationUser()
                {
                    Email = "admin@nitel.mw",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    FirstName = "Initial",
                    LastName = "Admin",
                    ActivationStatus = true,
                    CreationDate = DateTime.Now,
                    EmployeeCode = "1"

                };


                var adminPassword = "Winning2022*";
                var result1 = await _userManager.CreateAsync(adminUser, adminPassword);

                if (!result1.Succeeded)
                    throw new Exception("User creation failed! Please check user details and try again.");



                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await _roleManager.RoleExistsAsync(UserRoles.SuperUSer))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.SuperUSer));

                if (!await _roleManager.RoleExistsAsync(UserRoles.RegularUser))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.RegularUser));


                await _userManager.AddToRoleAsync(adminUser, UserRoles.Admin);


                return true;
            }
            catch (Exception ex)
            {

                Log.Error("An error occured when creating inital admin user: " + ex);
                return false;
            }
           
        }
    }
}

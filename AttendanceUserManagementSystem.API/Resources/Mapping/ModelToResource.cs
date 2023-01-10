using AttendanceUserManagementSystem.API.Authentication;
using AttendanceUserManagementSystem.API.Resources.DTO;
using AttendanceUserManagementSystem.API.Resources.Responses;
using AutoMapper;

namespace AttendanceUserManagementSystem.API.Resources.Mapping
{
    public class ModelToResource : Profile
    {
        public ModelToResource()
        {
            CreateMap<Branch, BranchResponse>();
            CreateMap<Department, DepartmentResponse>();
            CreateMap<UserDto, UsersWithoutAddressesResponse>();

        }
    }
}

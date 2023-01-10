using AttendanceUserManagementSystem.API.Authentication;
using AttendanceUserManagementSystem.API.Resources.DTO;
using AutoMapper;

namespace AttendanceUserManagementSystem.API.Resources.Mapping
{
    public class ResourceToModel: Profile
    {
        public ResourceToModel()
        {
            CreateMap<AddBranchDto, Branch>();
            CreateMap<AddDepartmentDto, Department>();
        }
    }
}

using AttendanceUserManagementSystem.API.Authentication;
using AttendanceUserManagementSystem.API.Repositories;
using AttendanceUserManagementSystem.API.Resources.DTO;
using AttendanceUserManagementSystem.API.Resources.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttendanceUserManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        // GET: api/<BranchController>
        [HttpGet]
        public async Task<ActionResult<DepartmentResponse>> Get()
        {
            var results = await _departmentRepository.GetAllDepartments();

            if (results.Count == 0)
                return NotFound();

            var departments = new List<DepartmentResponse>();

            foreach (var result in results)
            {
                var department = new DepartmentResponse();

                department = _mapper.Map<Department, DepartmentResponse>(result);

                departments.Add(department);
            }

            return Ok(departments);
        }

        // GET api/<BranchController>/5
        [HttpGet("id")]
        public async Task<ActionResult<DepartmentResponse>> Get(int id)
        {
            var result = await _departmentRepository.GetDepartmentByID(id);

            if (result != null)
            {
                var department = _mapper.Map<Department, DepartmentResponse>(result);

                return Ok(department);
            }

            return BadRequest("Department does not exist");

        }

        // POST api/<BranchController>
        [HttpPost]
        public async Task<ActionResult<bool>> Post(AddDepartmentDto addDepartmentDto)
        {
            var department = _mapper.Map<AddDepartmentDto, Department>(addDepartmentDto);

            var result = await _departmentRepository.AddDepartment(department);

            return Ok(result);
        }

        // PUT api/<BranchController>/5
        [HttpPut("id")]
        public async Task<ActionResult<bool>> Put(int id, [FromBody] UpdateDepartmentDto updateDepartment)
        {
            var existingDepartment = await _departmentRepository.GetDepartmentByID(id);

            if (existingDepartment == null)
            {
                return BadRequest("Branch does not exist");
            }

            existingDepartment.DepartmentName = updateDepartment.DepartmentName;

            var result = await _departmentRepository.UpdateDepartment(existingDepartment);

            return result;
        }


    }
}

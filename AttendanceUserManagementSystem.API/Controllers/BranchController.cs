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

   
    public class BranchController : ControllerBase
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public BranchController(IBranchRepository branchRepository, IMapper mapper )
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }
        // GET: api/<BranchController>
        [HttpGet]
        public async Task<ActionResult<BranchResponse>> Get()
        {
            var results = await  _branchRepository.GetAllBranches();

            if (results.Count == 0)
                return NotFound();

            var branches = new List<BranchResponse>();

            foreach (var result in results)
            {
                var branch = new BranchResponse();

                branch = _mapper.Map<Branch, BranchResponse>(result);

                branches.Add(branch);
            }

            return Ok(branches);
        }

        // GET api/<BranchController>/5
        [HttpGet("id")]
        public async Task<ActionResult<BranchResponse>> Get(int id)
        {
            var result = await _branchRepository.GetBranchById(id);

            if (result != null)
            {
                var branch = _mapper.Map<Branch, BranchResponse>(result);

                return Ok(branch);
            }

           return BadRequest("Branch does not exist");
            
        }

        // POST api/<BranchController>
        [HttpPost]
        public async Task<ActionResult<bool>> Post( AddBranchDto addBranchDto)
        {
            var branch = _mapper.Map<AddBranchDto, Branch>(addBranchDto);

            var result = await  _branchRepository.AddBranch(branch);

            return Ok(result);
        }

        // PUT api/<BranchController>/5
        [HttpPut("id")]
        public async Task<ActionResult<bool>> Put(int id, [FromBody] UpdateBranch updateBranch)
        {
            var existingBranch = await _branchRepository.GetBranchById(id);

            if (existingBranch == null)
            {
                return BadRequest("Branch does not exist");
            }

            existingBranch.BranchName = updateBranch.BranchName;

            var result = await _branchRepository.UpdateBranch(existingBranch);

            return result;
        }

       
    }
}

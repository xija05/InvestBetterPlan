
using InvestBetterPlan_RestAPI.Models;
using InvestBetterPlan_RestAPI.Models.Dto;
using InvestBetterPlan_RestAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InvestBetterPlan_RestAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class BetterPlanAPIController : ControllerBase
    {
        private readonly IUserRepository _dbUser;
        private readonly ISummaryRepository _dbSummary;
        private readonly IGoalsRepository _dbGoal;
       

        public BetterPlanAPIController(IUserRepository dbUser, ISummaryRepository dbSummary, IGoalsRepository dbGoal)
        {
            _dbUser = dbUser;     
            _dbSummary = dbSummary;
            _dbGoal = dbGoal;
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task< ActionResult<UserDTO>> GetUser(int id)
        {
            UserDTO userDTO = new UserDTO();
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var user = await _dbUser.GetUser(u => u.Id == id);

                if (user == null)
                    return NotFound();

                              
                userDTO.Id = user.Id;
                userDTO.NombreCompleto = user.ToString();
                userDTO.NombreCompletoAdvisor = _dbUser.GetAdvisorFullNameById(user.Advisorid);
                userDTO.FechaCreacion = new DateTime(user.Created.Year, user.Created.Month, user.Created.Day);

               
            }
            catch (Exception ex)
            {

            }

            return Ok(userDTO);
        }




        [HttpGet("{id:int}/summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<List<SummaryDTO>>> GetSummary(int id)
        {
            if (id == 0)
                return BadRequest();

            var summary = await _dbSummary.GetSummary(id);

            if (summary == null)
                return NotFound();

            return Ok(summary);
        }

        [HttpGet("{id:int}/goals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<List<SummaryDTO>>> GetGoals(int id)
        {
            if (id == 0)
                return BadRequest();

            var summary = await _dbGoal.GetGoals(id);

            if (summary == null)
                return NotFound();

            return Ok(summary);
        }

        //public ActionResult<UserDTO> GetUserSummary(int id)
        //{
        //    if (id == 0)
        //        return BadRequest();

        //    var user = UserStore.usersList.FirstOrDefault(user => user.Id == id);

        //    if (user == null)
        //        return NotFound();
        //    return Ok(user);
        //}



    }
}

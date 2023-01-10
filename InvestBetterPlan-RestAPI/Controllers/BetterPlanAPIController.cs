using InvestBetterPlan_RestAPI.Data;

using InvestBetterPlan_RestAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace InvestBetterPlan_RestAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class BetterPlanAPIController : ControllerBase
    {
       
        public BetterPlanAPIController()
        {

        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetUsers()
        {
            return Ok(UserStore.usersList);
        }
        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<UserDTO> GetUser(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var user = UserStore.usersList.FirstOrDefault(user => user.Id == id);

            if(user == null)
                return NotFound();
            return Ok(user);
        }
        [HttpGet("{id:int}/summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<UserDTO> GetUserSummary(int id)
        {
            if (id == 0)
                return BadRequest();

            var user = UserStore.usersList.FirstOrDefault(user => user.Id == id);

            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}

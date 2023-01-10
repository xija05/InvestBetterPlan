
using InvestBetterPlan_RestAPI.Models;
using InvestBetterPlan_RestAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace InvestBetterPlan_RestAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class BetterPlanAPIController : ControllerBase
    {
        private readonly challengeContext _db;

        public BetterPlanAPIController(challengeContext db)
        {
            _db = db;
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<User>> GetUsers()
        //{
        //    return Ok(
        //                (from u in _db.Users
        //                select u).Take(3).ToList()
        //                );
        //}


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

            //var user = _db.Users.FirstOrDefault(user => user.Id == id);
            var user = (
                        (from u in _db.Users
                        where u.Id == id
                        select new
                        {
                            Id = u.Id,
                            NombreCompleto = u.ToString(),
                            NombreCompletoAdvisor = string.Empty,
                            FechaCreacion = u.Created
                        }
                        ).ToList()
                );

            if (user == null)
                return NotFound();
            return Ok(user);
        }

        //[HttpGet("{id:int}/summary")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]

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

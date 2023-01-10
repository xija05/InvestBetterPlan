
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

        public UserDTO userDTO { get; set; }    

        public BetterPlanAPIController(IUserRepository dbUser)
        {
            _dbUser = dbUser;
            userDTO = new UserDTO();
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task< ActionResult<UserDTO>> GetUser(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var user = await _dbUser.GetUser(u => u.Id == id);

                if (user == null)
                    return NotFound();

                //var user01 = await (
                //            (from u in _db.Users
                //             where u.Id == id

                //             select new
                //             {
                //                 Id = u.Id,
                //                 NombreCompleto = u.ToString(),
                //                 AdvisorId = u.Advisorid,
                //                 FechaCreacion = u.Created
                //             }).ToListAsync()
                //    );

                //if (user01 == null)
                //    return NotFound();

                
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

using DemoAPI.Interface;
using DemoAPI.Models;
using DemoAPI.Rapository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private IMember members = new MembersRepository();

        [HttpGet]
        [Route("getallmember")]
        public ActionResult<IEnumerable<Member>> GetAllMembers()
        {
            return members.GetAllMember();
        }


        [HttpGet]
        [Route("GetByID")]
        public ActionResult<Member> GetMemberById(int id)
        {
            return members.GetMember(id);
        }

       
    }

}

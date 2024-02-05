using DemoAPI.Models;

namespace DemoAPI.Interface
{
    public interface IMember
    {

        List<Member> GetAllMember();
        Member GetMember(int id);
    }
}



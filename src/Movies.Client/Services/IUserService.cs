using Movies.Client.Models;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public interface IUserService
    {
        Task<UserInfo> GetUserInfo();
    }
}

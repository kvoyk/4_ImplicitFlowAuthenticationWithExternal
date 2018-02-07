using System.Threading.Tasks;
using MLaw.Idp.Mvc.Models;

namespace MLaw.Idp.Mvc.Services
{
    public interface ILoggedUsersStorage
    {
        Task<LoggedinUserModel> GetLoginAsync(string cacheKey);
        Task<string> SaveLoginAsync(LoggedinUserModel loggedinUserModel);
    }
}
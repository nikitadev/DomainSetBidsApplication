using System.Threading.Tasks;
using RegAPI.Library.Models;
using RegAPI.Library.Models.Autorization;

namespace RegAPI.Library.Infostructures.Interfaces
{
    public interface IAutorizationProvider
    {
        Task<Result<AutorizationAnswer>> CheckAsync(string username, string password);

        Task<Result<AutorizationAnswer>> GetUserIdAsync(string username, string password);
    }
}

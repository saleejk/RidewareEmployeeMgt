using RidewareEmployeeMgt.Models;

namespace RidewareEmployeeMgt.Interfaces
{
    public interface IUserServices
    {
        Task<string> RegisterUser(UserDto user);
        Task<string> LogIn(UserDto user);
    }
}

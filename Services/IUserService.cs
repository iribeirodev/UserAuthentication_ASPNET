using ShopAutenticacao.Models;
using ShopAutenticacao.Request;

namespace ShopAutenticacao.Services
{
    public interface IUserService
    {
        User GetByUsernameAndPassword(string username, string password);

        User GetByUsername(string username);

        User Create(CreateUserRequest createUserRequest);
    }
}
using ShopAutenticacao.Models;
using MongoDB.Driver;
using System.Linq;
using ShopAutenticacao.Request;

namespace ShopAutenticacao.Services
{
    public class UserService: IUserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IShopAuthenticationDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public User GetByUsernameAndPassword(string username, string password) {
            var user = GetByUsername(username);
            var passwordCheck = AesOperation.DecryptString(
                Settings.SymmetricKey,
                user.Password);

            if (password.Equals(passwordCheck))
                return user;

            return null;
        }

        public User GetByUsername(string username) =>
            _users.Find(u => u.Username.ToLower() == username.ToLower()).SingleOrDefault();

        public User Create(CreateUserRequest createUserRequest) 
        {
            var user = new User{
                Username = createUserRequest.Username,
                Password = AesOperation.EncryptString(Settings.SymmetricKey, createUserRequest.Password),
                Role = createUserRequest.Role
            };

            _users.InsertOne(user);

            return user;
        }
    }
}
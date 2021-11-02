using System;
namespace ShopAutenticacao.Response
{
    public class UserCreatedResponse
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.Now;
    }
}
namespace ShopAutenticacao.Response
{
    public class SignedInResponse
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
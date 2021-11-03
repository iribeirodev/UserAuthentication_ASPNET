namespace ShopAutenticacao.Services.Config
{
    public interface IDbInitializer
    {
         /// <summary>
         /// Adiciona valores default ao database.
         /// </summary>
         void SeedData();
    }
}
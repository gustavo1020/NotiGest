namespace Application.Contracts
{
    public interface IJwtService
    {
        dynamic GenerateToken(string username, IList<string> rol);
    }
}

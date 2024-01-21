namespace Application.Contracts
{
    public interface IJwtService
    {
        dynamic GenerateToken(string email, IList<string> rol, Guid id, string username);
    }
}

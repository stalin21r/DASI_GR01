namespace Backend;

public interface ICurrentUserService
{
    string? GetUserId();
    string? GetUserEmail();
    string? GetUserRole();
    bool IsAuthenticated();
}

namespace Shared;

public class UserProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public DateTime DateOfBirth { get; set; }
    public string ScoutUniqueId { get; set; } = string.Empty;
    public bool Active { get; set; }
    public decimal Balance { get; set; }
    public string Role { get; set; } = string.Empty;
    public OccupationDto? Occupation { get; set; }
}
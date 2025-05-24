namespace SubscriptionApi.Models { 
public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; }
    public bool IsSubscribed { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
}

public class LoginModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    } 
}
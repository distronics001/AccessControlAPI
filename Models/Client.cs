using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessControlAPI.Models
{
    [Table("clients")]
public class Client
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
}

}

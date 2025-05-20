using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessControlAPI.Models
{
    [Table("clients")]
    public class Client
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}

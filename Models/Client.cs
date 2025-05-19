using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessControlAPI.Models
{
    [Table("clients")]
    public class Client
    {
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("expiry_date")]
        public DateTime? ExpiryDate { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}

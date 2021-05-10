using System;
using System.ComponentModel.DataAnnotations;

namespace BankWebApi.DTOs
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Email { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
    }
}

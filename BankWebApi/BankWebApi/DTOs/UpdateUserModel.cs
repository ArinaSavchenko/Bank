using System;
using System.ComponentModel.DataAnnotations;

namespace BankWebApi.DTOs
{
    public class UpdateUserModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int OfficeId { get; set; } = 0;
        [Required]
        public string Position { get; set; }
    }
}

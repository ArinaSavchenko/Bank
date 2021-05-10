using Bank.Datalayer.Entities;
using System;

namespace BankWebApi.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public int PhoneNumber { get; set; }
        public bool Status { get; set; }
    }
}

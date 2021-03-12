using System;
using System.Collections.Generic;

namespace Bank.Datalayer.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public bool Status { get; set; }
        
        public Client Client { get; set; }
        public Worker Worker { get; set; }
    }
}
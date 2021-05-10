using System.Collections.Generic;

namespace Bank.Datalayer.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Address { get; set; }
        
        public User User { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
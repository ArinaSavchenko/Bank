using System.Collections.Generic;

namespace Bank.Datalayer.Entities
{
    public class Worker
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OfficeId { get; set; }
        public string Position { get; set; }
        
        public User User { get; set; }
        public Office Office { get; set; }
        public ICollection<Contract> Contracts { get; set; }
    }
}
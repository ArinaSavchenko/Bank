using System.Collections.Generic;

namespace Bank.Datalayer.Entities
{
    public class TypeOfAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public decimal Percent { get; set; }
        
        public ICollection<Account> Accounts { get; set; }
    }
}
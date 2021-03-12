using System;
using System.Collections.Generic;

namespace Bank.Datalayer.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public decimal Sum { get; set; }
        public int ClientId { get; set; }
        public int AccountTypeId { get; set; }
        public string Status { get; set; }
        
        public Client Client { get; set; }
        public TypeOfAccount TypeOfAccount { get; set; }
        public Contract Contract { get; set; }
    }
}
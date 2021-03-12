using System.Collections.Generic;

namespace Bank.Datalayer.Entities
{
    public class Office
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        
        public ICollection<Worker> Workers { get; set; }
    }
}
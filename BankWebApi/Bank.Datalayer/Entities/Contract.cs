namespace Bank.Datalayer.Entities
{
    public class Contract
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Type { get; set; }
        public int WorkerId { get; set; }
        
        public Account Account { get; set; }
        public Worker Worker { get; set; }
    }
}
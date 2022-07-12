namespace NipSearch.Entities
{
    public class Account
    {
        public int IdAccountNumber { get; set; }
        public int IdSubject { get; set; }
        public Subject? Subject { get; set; }
        public string? Number { get; set; }
    }
}

namespace myProject02.Models
{
    public class Candidate
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Nic { get; set; } = string.Empty;

        // Foreign key
        public int PartyId { get; set; }

        // Navigation property
        public Party Party { get; set; } = null!;
    }
}

namespace myProject02.Models
{
    public class PartyElection
    {
        public int PartyId { get; set; }
        public Party Party { get; set; } = null!;

        public int ElectionId { get; set; }
        public Election Election { get; set; } = null!;
    }
}

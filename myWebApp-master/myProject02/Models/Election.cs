namespace myProject02.Models
{
    public class Election
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Year { get; set; }
        public ICollection<PartyElection> PartyElections { get; set; } = new List<PartyElection>();
    }
}
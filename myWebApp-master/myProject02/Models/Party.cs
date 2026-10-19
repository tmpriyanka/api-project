namespace myProject02.Models
{
    public class Party
    {
        
            public int Id { get; set; }

            public string Name { get; set; } = string.Empty;

            public string Abbreviation { get; set; } = string.Empty;

            // One party can have many candidates
            public ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

            public ICollection<PartyElection> PartyElections { get; set; } = new List<PartyElection>();

    }
}

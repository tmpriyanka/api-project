namespace myProject02.Models
{
    public class Voter
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? District { get; set; }
        public DateTime CreatedAt { get; internal set; }
    }
}

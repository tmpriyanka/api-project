namespace myProject02.Dto
{
    public class ElectionDetailDto : ElectionDto
    {
        public List<PartyDTO> Parties { get; set; } = new();
    }
}

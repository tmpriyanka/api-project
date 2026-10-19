namespace myProject02.Dto
{
    public class PartyDetailDto : PartyDTO
    {
        public List<ElectionDto> Elections { get; set; } = new();
    }
}

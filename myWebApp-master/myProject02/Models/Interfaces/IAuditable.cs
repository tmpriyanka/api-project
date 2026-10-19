namespace myProject02.Models.Interfaces
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        string? UpdatedBy { get; set; }
    }
}

namespace myProject02.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string Action {  get; set; }
        public string Changes { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

namespace myProject02.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public int RoleId {  get; set; }
        public Role Role { get; set; } = null! ;
    }
}

namespace WebApi.Models
{
    public enum Role
    {
        User,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
        public string Pseudo { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public User() { }
         public User(int id, string pseudo, string password, Role role)
        {
            Id = id;
            Pseudo = pseudo;
            Password = password;
            Role = role;
        }
    }
}


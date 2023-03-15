namespace Task4.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public DateTime LastLogInTime { get; set; }

    }
}

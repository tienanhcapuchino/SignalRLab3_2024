namespace SignalRLab3
{
    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public static class UserList
    {
        public static Dictionary<string, string> UserConnections = new Dictionary<string, string>();
        public static void AddUser(string email, string connectionId)
        {
            UserConnections.TryAdd(email, connectionId);
        }
    }

}

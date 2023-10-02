namespace server.Mysql.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Account { get; set; }
        public string Pass { get; set; }
        public string Status { get; set; }
    }
}
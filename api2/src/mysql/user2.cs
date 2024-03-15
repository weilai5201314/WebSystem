namespace server.Mysql.Models
{
    // user 表
    public class user2
    {
        public int ID { get; set; }

        // public string Account { get; set; }
        public string username { get; set; }

        public string password { get; set; } // 存储哈希密码
    }
}
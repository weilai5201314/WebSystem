namespace server.Mysql.Models
{
    // user 表
    public class User
    {
        public int ID { get; set; }
        public string Account { get; set; }
        public byte[] Pass { get; set; } // 存储哈希密码
        public byte[] Salt { get; set; } // 存储盐值

        public int Status { get; set; }
    }

    // usergroup 表
    public class UserGroup
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    // UserUsergroup 表
    public class UserUsergroup
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int UserGroupID { get; set; }
    }
}
namespace server.Mysql.Models
{
    // user 表
    // public class User
    // {
    //     public int ID { get; set; }
    //     public string Account { get; set; }
    //     public byte[] Pass { get; set; } // 存储哈希密码
    //     public byte[] Salt { get; set; } // 存储盐值
    //     public int Status { get; set; } // 用户状态
    //     public byte[] RevertPass { get; set; }
    // }
    // user 表
    public class User
    {
        public int ID { get; set; }
        // public string Account { get; set; }
        public string Account { get; set; }

        public byte[] Pass { get; set; } // 存储哈希密码
        public byte[] Salt { get; set; } // 存储盐值
        public int Status { get; set; } // 用户状态
        public byte[] RevertPass { get; set; }
        public int N { get; set; } // 迭代次数
        public byte[] R { get; set; } // 随机数
        public int N2 { get; set; } // 迭代次数2
        public byte[] R2 { get; set; } // 随机数2
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

    // Log 表
    public class Log
    {
        public int ID { get; set; }

        public DateTime Timestamp { get; set; }

        public string User { get; set; }

        public string Action { get; set; }

        public bool InputResult { get; set; }

        public string InputValue { get; set; }

        public bool ReturnResult { get; set; }

        public string ReturnValue { get; set; }
    }

    // Resource 表
    public class Resource
    {
        public int ID { get; set; }
        public string FileName { get; set; }
    }

    // Permission 表
    public class Permission
    {
        public int ID { get; set; }
        public int PermissionCode { get; set; }
        public string PermissionDescription { get; set; }
    }

    // UserResourcePermission 表
    public class UserResourcePermission
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ResourceID { get; set; }
        public int PermissionID { get; set; }
    }
}
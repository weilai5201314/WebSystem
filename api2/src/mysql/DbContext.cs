﻿using Microsoft.EntityFrameworkCore;
using server.Mysql.Models; // 导入数据模型

namespace server.Mysql.Data
{
    public class MysqlDbContext : DbContext
    {
        private readonly IConfiguration _configuration; // 用于配置数据库连接的 IConfiguration

        // 构造函数，接受 DbContextOptions 和 IConfiguration 作为参数
        public MysqlDbContext(DbContextOptions<MysqlDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration; // 初始化 IConfiguration
        }

        // DbSet 属性，用于表示数据库中的  表
        public DbSet<user2> user2 { get; set; }


        // OnConfiguring 方法，用于配置数据库连接
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // 从 IConfiguration 获取数据库连接字符串
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                // 使用 MySQL 数据库提供程序，并设置连接字符串
                optionsBuilder.UseMySQL(connectionString);
            }
        }
    }
}
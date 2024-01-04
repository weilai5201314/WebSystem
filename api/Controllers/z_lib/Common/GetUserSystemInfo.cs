using Microsoft.AspNetCore.Mvc;
using server.Mysql.Data;

namespace server.Controllers;

public partial class Api
{
    // 构造注入数据库结构
    //  操作数据库的注入器，全局通用
    [FromServices] public MysqlDbContext DbContext { get; set; }

    //  存储文件的根目录路径，仅在此修改
    private const string DirectoryPath = "D:\\zzz\\school\\InfoSecurity\\WebSystem\\api\\Data";
}
using server.Mysql.Models;
using server.Time;

namespace server.Controllers;

public partial class Api
{
    // 添加记录登录操作的日志函数
    private void TypeLog(string userName, string action, bool inputResult, string inputValue, bool returnResult,
        string returnValue)
    {
        DateTime cstTime = TimeHelper.BeijingTime;
        // 创建日志实体
        var log = new Log
        {
            Timestamp = cstTime, // 北京时间
            User = userName, // 记录用户
            Action = action, // 用户行为
            InputResult = inputResult, // 输入状态
            InputValue = inputValue, // 输入值
            ReturnResult = returnResult, // 输出状态
            ReturnValue = returnValue // 输出值
        };

        // 将日志实体添加到数据库中
        DbContext.Log.Add(log);
        DbContext.SaveChanges();
    }
}
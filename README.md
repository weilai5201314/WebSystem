# WebSystem

## WebApi构建练习

大三上的信息系统安全实验，因为这个学期已经结束了，所以后面的MAC，RBAC都还没有写。


**1.目前已经完成的部分：**
- 基本的注册登录找回密码。
- 用户组区分了系统权限，并且根据身份进行基础的访问控制。
- 实现了一次性口令认证的交互过程。
- 实现了DAC的文件管理系统。
- 有一点点MAC和RBAC的基础，但是没怎么写，有点摆烂 ~~不是~~。

**2.项目依赖**

- NuGet:
  - Microsoft.AspNetCore.OpenApi 7.0.7
  - Microsoft.Extensions.Primitives 8.0.0-rc.1.23419.4
  - MySql.EntityFrameworkCore 7.0.5
  - System.IdentityModel.Tokens.Jwt 7.0.2
  - Newtonsoft.Json 13.0.3

- Mysql 8.0.32

**3.项目改进**
- 感觉自己的系统设计还是不够全面。
- 本来准备尝试一下使用芒果数据库，后来还是放弃了。
- 第一次构建webapi，很多地方没考虑到。
- 前后端的接口编写有待提高。

不过将后端api写好之后，再写前端直接调用api还是很爽的。
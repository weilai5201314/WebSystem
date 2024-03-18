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
  - System.IdentityModel.Tokens.Jwt 7.1.2
  - Newtonsoft.Json 13.0.3

- Mysql 8.0.32

**3.项目改进**
- 感觉自己的系统设计还是不够全面。
- 本来准备尝试一下使用芒果数据库，后来还是放弃了。
- 第一次构建webapi，很多地方没考虑到。
- 前后端的接口编写有待提高。


**4.mysql创建语句**
```mysql
CREATE DATABASE IF NOT EXISTS `websystem`;

USE `websystem`;

-- 用户表
CREATE TABLE IF NOT EXISTS `User`
(
  `ID`         INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `Account`    VARCHAR(255) NOT NULL,
  `Pass`       BINARY(64)   NOT NULL, -- 哈希密码，假设使用 SHA-256，长度为 64 字节
  `Salt`       BINARY(16)   NOT NULL, -- 盐值，假设长度为 16 字节
  `Status`     INT          NOT NULL,
  `RevertPass` BINARY(64),            -- 逆转哈希密码，假设使用 SHA-256，长度为 64 字节
  `N`          INT,                   -- 迭代次数
  `R`          BINARY(16),            -- 随机数，假设长度为 16 字节
  `N2`         INT,                   -- 迭代次数2
  `R2`         BINARY(16)             -- 随机数2，假设长度为 16 字节
);

-- 用户组表
CREATE TABLE IF NOT EXISTS `UserGroup`
(
  `ID`          INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `Name`        VARCHAR(255) NOT NULL,
  `Description` VARCHAR(255)
);

-- 用户-用户组关联表
CREATE TABLE IF NOT EXISTS `UserUsergroup`
(
  `ID`          INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `UserID`      INT NOT NULL,
  `UserGroupID` INT NOT NULL,
  FOREIGN KEY (`UserID`) REFERENCES `User` (`ID`),
  FOREIGN KEY (`UserGroupID`) REFERENCES `UserGroup` (`ID`)
);

-- 日志表
CREATE TABLE IF NOT EXISTS `Log`
(
  `ID`           INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `Timestamp`    DATETIME     NOT NULL,
  `User`         VARCHAR(255) NOT NULL,
  `Action`       VARCHAR(255) NOT NULL,
  `InputResult`  BOOLEAN      NOT NULL,
  `InputValue`   VARCHAR(255),
  `ReturnResult` BOOLEAN      NOT NULL,
  `ReturnValue`  VARCHAR(255)
);

-- 资源表
CREATE TABLE IF NOT EXISTS `Resource`
(
  `ID`       INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `FileName` VARCHAR(255) NOT NULL
);

-- 权限表
CREATE TABLE IF NOT EXISTS `Permission`
(
  `ID`                    INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `PermissionCode`        INT NOT NULL,
  `PermissionDescription` VARCHAR(255)
);

-- 用户-资源-权限关联表
CREATE TABLE IF NOT EXISTS `UserResourcePermission`
(
  `ID`           INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `UserID`       INT NOT NULL,
  `ResourceID`   INT NOT NULL,
  `PermissionID` INT NOT NULL,
  FOREIGN KEY (`UserID`) REFERENCES `User` (`ID`),
  FOREIGN KEY (`ResourceID`) REFERENCES `Resource` (`ID`),
  FOREIGN KEY (`PermissionID`) REFERENCES `Permission` (`ID`)
);

alter table user
  modify Account varchar(256) not null;

alter table user
  modify Pass varchar(256) not null;

alter table user
  modify Salt varchar(256) not null;

alter table user
  modify RevertPass varchar(256) null;
alter table user
  modify R binary(32) null;

alter table user
  modify R2 binary(32) null;

alter table user
  modify Pass varbinary(256) not null;

alter table user
  modify Salt varbinary(256) not null;

alter table user
  modify RevertPass varbinary(256) null;




```

不过将后端api写好之后，再写前端直接调用api还是很爽的。
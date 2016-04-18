--2015.06.25 chixiu.gong #1001模板的建立 DEV_V1.2
alter table CoreSysHotelSencond add LogoImageUrl varchar(8000) DEFAULT ''

--2015.07.02 chixiu.gong #1138酒店管理页面基本信息扩展信息的合并 DEV_V1.2
alter table AppPublish change  SeriseCode HotelId char(50) not null

--2015.07.09 doudou 备用机转换 DEV_V1.3
ALTER TABLE `YeahTVTest`.`BackupDevice` 
ADD COLUMN `IsUsed` BIT(1) NULL DEFAULT b'0' AFTER `DeviceType`;

--2015.07.13 chixiu.gong 更改表结构语句 DEV_V1.2
alter table CoreSysHotelSencond add VODAddress varchar(4000) DEFAULT ''




--2015.07.17 rick.wang 模板改版新增字段 DEV_V1.4
alter table TvDocumentAttribute add ParentId varchar(50);
alter table TvTemplateAttribute add ParentId varchar(50);
alter table TvTemplateAttribute add DataType int(11) not null;
alter table TvTemplateAttribute add Description varchar(2000);




--tianyu.xu 更改CoreSysGroup表结构   新增列apikey  DEV_V1.4
alter table YeahTVUnit.CoreSysGroup add ApiKey varchar(100);
--更改TvTemplateAttribute（模板属性）表结构   新增必填列【Required】   DEV_V1.4
alter table TvTemplateAttribute add Required bit;
update TvTemplateAttribute set Required =0;
ALTER TABLE TvDocumentAttribute  MODIFY COLUMN Value text;

--放出菜单
insert into ErpPowerResource (FuncId,Type,
Path,DisplayName,ParentFuncId,Orders,Display,Controller,Action,Logo)  values 
(
    302,'SRC','/Group','集团维护',3,2,1,'Group','Index',''
) ;

insert into ErpPowerResource (FuncId,Type,
Path,DisplayName,ParentFuncId,Orders,Display,Controller,Action,Logo)  values 
(
    303,'SRC','/Brand','品牌维护',3,3,1,'Brand','Index',''
) ;

---tianyu.xu   15/07/28 
ALTER TABLE TvTemplateAttribute  MODIFY COLUMN Value text; 


--2015.07.27 rick.wang 错误 #1271 APP-versioncode为小数时后台报错 DEV_V1.4
ALTER TABLE AppVersion  MODIFY COLUMN VersionCode int(11);
ALTER TABLE AppPublish  MODIFY COLUMN VersionCode int(11);
ALTER TABLE DeviceAppsMonitor  MODIFY COLUMN VersionCode int(11);


--2015.07.27 rick.wang 门店数据配置汇总 DEV_V1.4
insert into ErpPowerResource (FuncId,Type,
Path,DisplayName,ParentFuncId,Orders,Display,Controller,Action,Logo)  values 
(
    206,'SRC','/HotelConfigSummary','门店数据配置汇总',2,6,1,'HotelConfigSummary','Index',''
) ;


--2015.07.28 rick.wang 模板预览 DEV_V1.4
INSERT INTO `YeahTV`.`ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('50202', 'PAGE', '/Template/TemplatePreview', '模版预览', '502', '2', '1', 'Template', 'TemplatePreview', '');

--2015.07.29 roger.wu 增加电影排序功能 DEV_V1.4
ALTER TABLE `YeahTV`.`HotelMovieTrace` 
ADD COLUMN `Order` INT NULL AFTER `Active`;

UPDATE `YeahTV`.`HotelMovieTrace`
SET
`Order` = 1;


--2015.7.30 chixiu.gong CoreSysHotel添加CreateTime字段 DEV_V1.4
alter table YeahTVBind.CoreSysHotel add  CreateTime datetime default '2015-7-30';

--2015.08.05 chixiu.gong CoreSysHotel表中修改TemplateId为可空类型 DEV_V1.4
ALTER TABLE `YeahTVBind`.`CoreSysHotel` CHANGE COLUMN `TemplateId` `TemplateId` VARCHAR(50) NULL ;

--2015.08.04 roger.wu  V1.8 新版VOD功能实现
CREATE TABLE `YeahTV`.`MovieForLocalize` (
  `Id` VARCHAR(64) NOT NULL,
  `Name` VARCHAR(64) NOT NULL,
  `Director` VARCHAR(64) NULL,
  `Starred` VARCHAR(64) NULL,
  `District` VARCHAR(64) NULL,
  `Mins` VARCHAR(64) NULL,
  `Vintage` int NULL,
  `MovieReview` VARCHAR(64) NULL,
  `PosterAddress` VARCHAR(300) NULL DEFAULT NULL,
  `CoverAddress` VARCHAR(100) NULL,
  `Tags` VARCHAR(100) NULL,
  `VodUrl` VARCHAR(500) NULL,
  `MD5` VARCHAR(256) NULL,
  `Language` VARCHAR(100) NULL,
  `Rate` int NULL,
  `HotelCount` int NULL,
  `DefaultAmount`  decimal(10,2),
  `CurrencyType` VARCHAR(100) NULL,
  `Attribute` VARCHAR(100) NULL,
  `Order` int(11) DEFAULT NULL,
  `IsTop` bit,
  `CreateTime` DATETIME NOT NULL,
  `LastUpdateTime` DATETIME NOT NULL,
  `LastUpdateUser` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`));

CREATE TABLE `YeahTV`.`Tag` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `ParentId` INT NOT NULL,
  `RescorceId` VARCHAR(64) NULL,
  `Icon` VARCHAR(500) NULL,
  PRIMARY KEY (`Id`));

CREATE TABLE `HotelMovieTraceNoTemplate` (
  `HotelId` varchar(64) NOT NULL,
  `MovieId` varchar(64) NOT NULL,
  `Price` decimal(10,2) DEFAULT NULL,
  `ViewCount` double DEFAULT NULL,
  `DownloadStatus` varchar(20) DEFAULT NULL,
  `Active` bit(1) NOT NULL,
  `Order` int(11) DEFAULT NULL,
  `IsTop` bit(1) DEFAULT NULL,
  `CreateTime` datetime NOT NULL,
  `LastViewTime` datetime NOT NULL,
  `LastUpdateUser` varchar(50) NOT NULL,
  PRIMARY KEY (`HotelId`,`MovieId`)
); 


CREATE TABLE `YeahTV`.`LocalizeResource` (
  `Id` VARCHAR(64) NOT NULL,
  `Lang` VARCHAR(10) NOT NULL,
  `Content` TEXT NULL,
  PRIMARY KEY (`Id`, `Lang`));
  
  
--2015.08.05 rick.wang VOD2.0 DEV_V1.8
ALTER TABLE `VODOrder` 
ADD COLUMN `PayType` VARCHAR(50) NULL DEFAULT 'Movie' AFTER `IsDelete`;

新增系统配置项VodDailyOrderExpires，设置包天有效时间
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('VodDailyOrderExpires', '-24', 'System', 1, now());

【每次】新增酒店必须增加配置项：
INSERT INTO `TvHotelconfig` (`HotelId`, `ConfigCode`, `ConfigName`, `ConfigValue`, `Active`, `LastUpdater`, `LastUpdateTime`, `CreateTime`) VALUES ('修改为酒店ID', 'HotelPayment', '支付相关配置', '{\"PriceOfDay\":5.00,\"PaymentModels\":[\"ALIPAY\",\"WXPAY\",\"FZPAY\"],\"PayType\":\"Daily\"}', '1', 'amdmin', now(), now());

--2015.08.13 YangJin HCS接口实现 DEV_V1.81
CREATE TABLE `HCSDownloadTask` (
  `Id` varchar(64) NOT NULL,
  `TaskNo` varchar(64) NOT NULL,
  `ServerId` varchar(128) NOT NULL,
  `Status` varchar(45) NOT NULL,
  `ResultStatus` varchar(45) NOT NULL,
  `Config` text NOT NULL,
  `ErrorMessage` varchar(500) DEFAULT NULL,
  `CreateTime` datetime DEFAULT NULL,
  `UpdateTime` datetime NOT NULL,
  `LastUpdateUser` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
);

CREATE TABLE `HCSJob` (
  `Id` varchar(64) NOT NULL,
  `TaskId` varchar(64) NOT NULL,
  `Name` varchar(200) NOT NULL,
  `MD5` varchar(512) NOT NULL,
  `Priority` varchar(10) NOT NULL,
  `BizType` varchar(64) NOT NULL,
  `Url` varchar(5000) NOT NULL,
  `Status` varchar(45) NOT NULL,
  `BizNo` varchar(50) NOT NULL,
  `ErrorMessage` varchar(500) DEFAULT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateTime` datetime NOT NULL,
  `LastUpdateUser` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
);

CREATE TABLE `HCSConfig` (
  `Id` varchar(64) NOT NULL,
  `ServerId` varchar(128) NOT NULL,
  `Type` varchar(45) NOT NULL,
  `Value` text,
  `CreateTime` datetime NOT NULL,
  `UpdateTime` datetime NOT NULL,
  `UpdateUser` varchar(64) NOT NULL,
  PRIMARY KEY (`Id`)
);

--2015.08.18 YangJin 添加HCS验证公钥 DEV_V1.81
INSERT INTO `YeahTV`.`SystemConfig`
(`ConfigName`,`ConfigValue`,`ConfigType`,`Active`,`LastUpdateTime`) VALUES ('HCSPublicKey','d2b1adc163094517bc165b80e0105ac8','System','1','2015-08-18 00:00:00');

--2015.08.18 rick.wang 更改BehaviorLog表结构语句 DEV_V1.8.1
drop table BehaviorLog;
CREATE TABLE `BehaviorLog` (
  `Id` VARCHAR(50) NOT NULL,
  `HotelId` VARCHAR(45) NOT NULL,
  `DeviceSerise` VARCHAR(50) NOT NULL,
  `BehaviorType` VARCHAR(50) NOT NULL,
  `BehaviorInfo` TEXT NOT NULL,
  `CreateTime` DATETIME NOT NULL,
  PRIMARY KEY (`Id`));

  --2015.08.20 YangJin 在Task表中增加Type字段来区分Task的类型 DEV_V1.8.1
ALTER TABLE `YeahTV`.`HCSDownloadTask` 
ADD COLUMN `Type` VARCHAR(45) NOT NULL DEFAULT '' AFTER `ServerId`;



----------------------------- SystemConfig新加配置项--begin------------------------------------------------------------------------------
INSERT INTO `YeahTV`.`SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`,`LastUpdateTime`) VALUES ('VodBackground', '#000000', 'System', 1,now());

INSERT INTO `YeahTV`.`SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`,`LastUpdateTime`) VALUES ('VodColor', '#000000', 'System', 1,now());
-------------------------SystemConfig新加配置项-----end----------------------------------------------------------------------------------

--2015.08.21 YangJin 在Job表中增加Operation字段来区分上下架的类型 DEV_V1.8.1
ALTER TABLE `YeahTV`.`HCSJob` 
ADD COLUMN `Operation` VARCHAR(45) NOT NULL AFTER `Name`;



--2015.08.17 rick.wang VOD2.0 DEV_V1.8.2
ALTER TABLE `HotelMovieTraceNoTemplate` 
ADD COLUMN `IsDelete` BIT(1) NOT NULL DEFAULT 0 AFTER `CreateTime`;



新增系统配置项HotelPayment，支付全局默认配置
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('HotelPayment', '{\"PriceOfDay\":5.00,\"PaymentModels\":[\"ALIPAY\",\"WXPAY\",\"FZPAY\"],\"PayType\":\"Daily\",\"TopMoviesCount\":10}', 'System', 1, now());

--2015.08.19 rick.wang VOD2.0 菜单 DEV_V1.8.2
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('207', 'SRC', '/VOD', '电影管理2.0', '2', '7', '1', 'VOD', 'Index', '');
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('20701', 'PAGE', '/VOD/Edit', '电影信息管理', '207', '1', '1', 'VOD', 'Edit', '');
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('20702', 'PAGE', '/VOD/Distribute', '电影分发', '207', '2', '1', 'VOD', 'Distribute', '');
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('20703', 'PAGE', '/VOD/Add', '新增电影信息', '207', '3', '1', 'VOD', 'Add', '');

ALTER TABLE `MovieForLocalize` 
ADD COLUMN `DistributeAll` BIT(1) NOT NULL DEFAULT b'0' AFTER `CreateTime`;

--2015.08.19 rick.wang VOD2.0 菜单 DEV_V1.8.2
INSERT INTO `SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('HCSTaskDefaultConfig', '{\"highspeed\": \"80\",\"highspeed_start\": \"23\",\"highspeed_end\": \"06\",\"lowspeed\": \"20\",\"lowspeed_start\": \"06\",\"lowspeed_end\": \"23\"}', 'System', 1, now());





--2015.08.21 dou VOD2.0 添加电影名称首字母搜索关键字 DEV_V1.8.2
ALTER TABLE `YeahTV`.`MovieForLocalize` 
ADD COLUMN `FirstWord` VARCHAR(100) NULL DEFAULT NULL AFTER `DistributeAll`;


--2015.08.21 dou VOD2.0 修改评分的字段类型 DEV_V1.8.2
ALTER TABLE `YeahTV`.`MovieForLocalize` 
CHANGE COLUMN `Rate` `Rate` DECIMAL(3,1) NULL DEFAULT NULL ; 
 

-----------------------------------------------2015.8.22 巩威MovieForLocalize表中添加字段HotelCount--DEV_V1.8.2分支---------------------------------------
ALTER TABLE `YeahTV`.`MovieForLocalize` 
CHANGE COLUMN `HotelCount` `HotelCount` INT NOT NULL DEFAULT 0 ;--------------------------------------------------------------------------------------------------------------------------------


-----------------------------------------------2015.8.28 9:28 豆志伟 MovieForLocalize表中英文电影分类数据维护--DEV_V1.8.2分支---------------------------------------
delete from  `YeahTV`.`Tag` 

INSERT INTO `YeahTV`.`Tag`(`Id`,`RescorceId`,`ParentId`,`Icon`)  VALUES 
(1,'288eebe32cce43618e27a07f59f9c3df',0,'z.png'),
(2,'711951e442c7492eaff21b533cf1f191',0,'gengxin.png'),
(3,'dc1ec291d4dd4bafbd73281fde8a4de1',0,'s.png'),
(4,'d4ab30dec068416687fe39e72941d7e6',0,NULL),
(5,'abf786366aa9496d8b9c172a4960a25d',0,NULL),
(6,'919ee85a158945d3bb61cfdb37d472be',0,NULL),
(7,'d6d3018721864d23818d7888f259bcfc',0,NULL),
(8,'Td6d3018721864d23818d7888f259bcfc',0,NULL),
(9,'Td6d3018721864d23818d7888f259bcfd',0,NULL),
(10,'Td6d3018721864d23818d7888f259bcfF',0,NULL); 

delete   
FROM
    YeahTV.LocalizeResource
where
    id in ('d4ab30dec068416687fe39e72941d7e6',
'd6d3018721864d23818d7888f259bcfc',
'Td6d3018721864d23818d7888f259bcfE',
'Td6d3018721864d23818d7888f259bcfF',
'Td6d3018721864d23818d7888f259bcfc',
'dc1ec291d4dd4bafbd73281fde8a4de1',
'abf786366aa9496d8b9c172a4960a25d',
'288eebe32cce43618e27a07f59f9c3df',
'711951e442c7492eaff21b533cf1f191',
'919ee85a158945d3bb61cfdb37d472be',
'Td6d3018721864d23818d7888f259bcfd',
'Td6d3018721864d23818d7888f259bcfF',
'd4ab30dec068416687fe39e72941d7e6',
'd6d3018721864d23818d7888f259bcfc',
'Td6d3018721864d23818d7888f259bcfc',
'Td6d3018721864d23818d7888f259bcfd',
'711951e442c7492eaff21b533cf1f191',
'288eebe32cce43618e27a07f59f9c3df',
'abf786366aa9496d8b9c172a4960a25d',
'919ee85a158945d3bb61cfdb37d472be',
'Td6d3018721864d23818d7888f259bcfE',
'dc1ec291d4dd4bafbd73281fde8a4de1')
INSERT INTO `YeahTV`.`LocalizeResource` VALUES 
('288eebe32cce43618e27a07f59f9c3df','en-us','Hot'),
('288eebe32cce43618e27a07f59f9c3df','zh-cn','热门推荐'),
('711951e442c7492eaff21b533cf1f191','en-us','Recent'),
('711951e442c7492eaff21b533cf1f191','zh-cn','最近更新'),
('dc1ec291d4dd4bafbd73281fde8a4de1','en-us','On Sale'),
('dc1ec291d4dd4bafbd73281fde8a4de1','zh-cn','限时优惠'),
('d4ab30dec068416687fe39e72941d7e6','en-us','Action'),
('d4ab30dec068416687fe39e72941d7e6','zh-cn','动作电影'),
('abf786366aa9496d8b9c172a4960a25d','en-us','Romance'),
('abf786366aa9496d8b9c172a4960a25d','zh-cn','爱情电影'),
('919ee85a158945d3bb61cfdb37d472be','en-us','Sci-Fi'),
('919ee85a158945d3bb61cfdb37d472be','zh-cn','科幻电影'),
('d6d3018721864d23818d7888f259bcfc','en-us','Comedy'),
('d6d3018721864d23818d7888f259bcfc','zh-cn','喜剧电影'),
('Td6d3018721864d23818d7888f259bcfc','en-us','Horror'),
('Td6d3018721864d23818d7888f259bcfc','zh-cn','恐怖电影'),
('Td6d3018721864d23818d7888f259bcfd','en-us','War'),
('Td6d3018721864d23818d7888f259bcfd','zh-cn','战争电影'),
('Td6d3018721864d23818d7888f259bcfF','en-us','Drama'),
('Td6d3018721864d23818d7888f259bcfF','zh-cn','剧情电影')




  
--2015.09.01 rick.wang VOD2.0 权限 DEV_V1.7
  CREATE TABLE `HotelPermition` (
  `Id` VARCHAR(64) NOT NULL,
  `PermitionType` VARCHAR(45) NOT NULL,
  `UserId` VARCHAR(64) NOT NULL,
  `TypeId` VARCHAR(64) NOT NULL,
  PRIMARY KEY (`Id`));

--2015.09.01 rick.wang dashboard可查询天数 DEV_V1.7
  INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('DashBoardValidityDays', '180', 'System', 1, now());
  
  
--2015.09.02 rick.wang 用户权限管理页面访问权限 DEV_V1.7
 INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('40101', 'PAGE', '/User/EditeIndex', '用户信息编辑', '401', '1', '1', 'User', 'EditeIndex', '');
 
 
--2015.09.06 rick.wang DashBoard访问权限 DEV_V1.7
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('6', 'MODEL', '', '酒店用户权限', '0', '1', '0', '', '', '');
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('601', 'PAGE', '/DashBoard', '单店DashBorad', '6', '1', '0', 'DashBoard', 'Index', '');
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('602', 'PAGE', '/DashBoard/MoreHotelIndex', '多店DashBorad', '6', '2', '0', 'DashBoard', 'MoreHotelIndex', '');



  


------------2015.9.7DianPingController.cs中修改的SystemConfig表中配置文件1.7分支--------------------------------------------------
INSERT INTO `YeahTV`.`SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `AppId`, `Active`,`LastUpdateTime`) VALUES ('AppKey', '3716066064', 'System', '', 1,now());

INSERT INTO `YeahTV`.`SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `AppId`, `Active`,`LastUpdateTime`) VALUES ('AppSecret', '92b099f67fde4b439a4295f0de1b1b1d', 'System', '', 1,now());

INSERT INTO `YeahTV`.`SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `AppId`, `Active`,`LastUpdateTime`) VALUES ('RADIUS', '5000', 'System', '', 1,now());
-----------------------------------------------------------------------------------------------------------




------------2015.9.11 商城订单表 rick.wang 分支1.9.3--------------------------------------------------
CREATE TABLE `StoreOrder` (
  `OrderId` varchar(32) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `Status` int(11) NOT NULL DEFAULT '0',
  `CompleteTime` datetime NOT NULL,
  `SeriseCode` varchar(128) DEFAULT NULL,
  `RoomNo` varchar(7) DEFAULT NULL,
  `Hotelid` varchar(50) NOT NULL,
  `HotelName` varchar(1000) NOT NULL,
  `GoodsName` varchar(32) DEFAULT NULL,
  `GoodsDesc` varchar(100) DEFAULT NULL,
  `PayInfo` varchar(200) DEFAULT NULL,
  `IsDelete` bit(1) NOT NULL DEFAULT b'0',
  `DeliveryState` int(11) not null,
  `ExpirationDate` datetime NOT NULL,
  `InvoicesTitle` varchar(1000) DEFAULT NULL,
  `DeliveryType` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`OrderId`)
);


CREATE TABLE `OrderProducts` (
  `Id` varchar(64) NOT NULL,
  `OrderId` varchar(32) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UnitPrice` decimal(10,2) NOT NULL,
  `Quantity` int(11) NOT NULL ,
  `ProductName` varchar(200) NOT NULL,
  `ProductId` varchar(36) NOT NULL,
  `ProductInfo` varchar(4000)  NULL,
  PRIMARY KEY (`Id`)
);
-----------------------------------------------------------------------------------------------------------

------------2015.9.15 商城支付回调接口 rick.wang 分支1.9.3 --------------------------------------------------

INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('PaymentNotifyUrl', 'http://10.254.0.42:8099/api/PayMent/PaymentCallBack', 'System', 1, now());

-----------------------------------------------------------------------------------------------------------

------------2015.9.15 商城公钥、私钥 rick.wang 分支1.9.3 --------------------------------------------------
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('StoreSignPublicKey', '2c9837cb1b714ee28edb3002fd2e3387', 'System', 1, now());
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('StoreSignPrivateKey', 'c7e0604eb43a41749a6427c2a8982283', 'System', 1, now());
-----------------------------------------------------------------------------------------------------------


-------------------------------2015.9.17_gongwei_1.9.3分支_SystemConfig表添加商城接口配置项------------------------------
INSERT INTO `YeahTV`.`SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('ShoppingMallUrl', 'http://192.168.8.88:8080', 'System', 1, now());

----------------------------------------------------


-------------------------------2015.9.21_gongwei_1.9.3分支_systemconfig表添加订单过期时间配置项---------------------------------
INSERT INTO `YeahTV`.`SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('ExpirationDate', '30', 'System', 1, now());

---------------------------------------------------

-------------------------------2015.9.24_YangJin_1.9.3分支_为Order表添加发票抬头字段---------------------------------

ALTER TABLE `YeahTV`.`VODOrder` 
ADD COLUMN `InvoicesTitle` VARCHAR(1000) NULL AFTER `PayType`;

---------------------------------------------------


-------------------------------2015.10.8_roger.wu_1.9.3分支_SystemConfig表添加订单查看配置项------------------------------
INSERT INTO `YeahTV`.`SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('ShoppingOrderAddress', 'http://192.168.8.90:8080/hotelmall/login.do', 'System', 1, now());

----------------------------------------------------

-------------------------------2015.10.8_YangJin_1.9.3分支_添加菜单权限------------------------------

INSERT INTO `YeahTV`.`ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('20703', 'PAGE', '/DashBoard/VodOrderReportIndex', 'VOD支付订单查询', '207', '1', '1', 'DashBoard', 'VodOrderReportIndex', '');

----------------------------------------------------

----------------------------------------------------2015.11.5_尚兴宝_1.10分支IU个性化设置欢迎词字段类型修改
ALTER TABLE  `CoreSysHotelSencond` 
CHANGE COLUMN `WelcomeWord` `WelcomeWord` TEXT NULL DEFAULT NULL ;


----------------------------------------------------2015.11.6_窦志伟_1.10分支IU个性化设置权限添加-----------------------------------
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('603', 'PAGE', '/DashBoard/VodOrderReportIndex', 'IU酒店个性化配置', '6', '2', '1', 'DashBoard', 'IUConfigIndex', '');

-------------------------------2015.11.10_rick.wang_2.0分支_添加hcs缓存管理权限菜单------------------------------

INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('407', 'SRC', '/HCSCacheVersion', 'HCS缓存管理', '4', '2', '1', 'HCSCacheVersion', 'Index', '');

----------------------------------------------------


-------------------------------2015.11.23_rick.wang_2.0分支_添加hcs缓存管理表------------------------------
CREATE TABLE `HCSCacheVersion` (
  `Id` varchar(64) NOT NULL,
  `PermitionType` varchar(45) NOT NULL,
  `TypeId` varchar(64) NOT NULL,
  `Version` int(11) NOT NULL,
  `LastUpdateUser` varchar(64) NOT NULL,
  `LastUpdateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-------------------------------2015.12.03_douzhiwei2.1分支_添加AuthTVToken积分兑换Token表------------------------------
CREATE TABLE `AuthTVToken` (
  `Id` VARCHAR(64) NOT NULL,
  `AuthTicket` VARCHAR(200) NOT NULL,
  `AuthToken` VARCHAR(64) NOT NULL,
  `Code` VARCHAR(100) NOT NULL,
  `Type` INT NOT NULL,
  `CreateTime` DATETIME NOT NULL,
  `ExpireTime` DATETIME NOT NULL,
  PRIMARY KEY (`Id`));

ALTER TABLE `AuthTVToken` 
CHANGE COLUMN `AuthTicket` `AuthTicket` VARCHAR(500) NOT NULL ,
CHANGE COLUMN `Code` `Code` VARCHAR(500) NOT NULL ;

INSERT INTO `SystemConfig` (`Id`, `ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('913', 'OpenAPIAuthSignPrivateKey', '01a280ff22154718926edeecc3aa518e', 'System', 1, '2015-12-03 15:15:08');


-------------------------------2015.12.03_rick.wang_2.0.1分支_添加全局配置表------------------------------
CREATE TABLE `GlobalConfig` (
  `Id` varchar(64) NOT NULL,
  `PermitionType` varchar(45) NOT NULL,
  `TypeId` varchar(64) NOT NULL,
  `ConfigName` varchar(50) NOT NULL,
  `ConfigValue` varchar(4000) NOT NULL,
  `ConfigDescribe` varchar(4000) NOT NULL,
  `Active` bit(1) NOT NULL,
  `PriorityLevel` int(11) NOT NULL,
  `LastUpdateUser` varchar(64) NOT NULL,
  `LastUpdateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-------------------------------2015.12.09_rick.wang_2.0.1分支_添加积分兑换记录表------------------------------
CREATE TABLE `ScoreExchang` (
  `Id` varchar(64) NOT NULL,
  `OrderType` varchar(100) NOT NULL,
  `OrderId` varchar(64) NOT NULL,
  `RunningNumber` varchar(200) NOT NULL,
  `Score` int(11) NOT NULL,
  `LastUpdateTime` datetime NOT NULL,
  `ScoreRate` varchar(20) NOT NULL,
  `Productid` varchar(64) NOT NULL,
  `Reqtime` varchar(14) NOT NULL,
  `Remark` varchar(4000) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-------------------------------2015.12.14_rick.wang_2.0.1分支_添加行悦自建资源站点配置项------------------------------
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('YeahInfoResourceSiteAddress', 'http://resource.yeah-info.com/', 'System', 1, '2015-12-14 10:57:11');

-------------------------------2015.12.15_rick.wang_2.0.1分支_OpenApi私钥------------------------------
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('OpenAPIAuthSignPrivateKey', '01a280ff22154718926edeecc3aa518e', 'System', 1, '2015-12-15 10:57:11');

-------------------------------2015.12.15_rick.wang_2.0.1分支_全局配置项菜单------------------------------
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`) VALUES ('30301', 'PAGE', '/GlobalConfig', '全局配置项', '303', '4', '1', 'GlobalConfig', 'Index');

-------------------------------2015.12.17_rick.wang_2.0.1分支_添加订单取二维码记录表------------------------------
CREATE TABLE `OrderQRCodeRecord` (
  `Id` VARCHAR(64) NOT NULL,
  `OrderId` VARCHAR(64) NOT NULL,
  `OrderType` VARCHAR(100) NOT NULL,
  `Ticket` VARCHAR(1000) NOT NULL,
  `CreateTime` DATETIME NOT NULL,
  PRIMARY KEY (`Id`));




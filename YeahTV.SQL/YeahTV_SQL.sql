--2015.06.25 chixiu.gong #1001ģ��Ľ��� DEV_V1.2
alter table CoreSysHotelSencond add LogoImageUrl varchar(8000) DEFAULT ''

--2015.07.02 chixiu.gong #1138�Ƶ����ҳ�������Ϣ��չ��Ϣ�ĺϲ� DEV_V1.2
alter table AppPublish change  SeriseCode HotelId char(50) not null

--2015.07.09 doudou ���û�ת�� DEV_V1.3
ALTER TABLE `YeahTVTest`.`BackupDevice` 
ADD COLUMN `IsUsed` BIT(1) NULL DEFAULT b'0' AFTER `DeviceType`;

--2015.07.13 chixiu.gong ���ı�ṹ��� DEV_V1.2
alter table CoreSysHotelSencond add VODAddress varchar(4000) DEFAULT ''




--2015.07.17 rick.wang ģ��İ������ֶ� DEV_V1.4
alter table TvDocumentAttribute add ParentId varchar(50);
alter table TvTemplateAttribute add ParentId varchar(50);
alter table TvTemplateAttribute add DataType int(11) not null;
alter table TvTemplateAttribute add Description varchar(2000);




--tianyu.xu ����CoreSysGroup��ṹ   ������apikey  DEV_V1.4
alter table YeahTVUnit.CoreSysGroup add ApiKey varchar(100);
--����TvTemplateAttribute��ģ�����ԣ���ṹ   ���������С�Required��   DEV_V1.4
alter table TvTemplateAttribute add Required bit;
update TvTemplateAttribute set Required =0;
ALTER TABLE TvDocumentAttribute  MODIFY COLUMN Value text;

--�ų��˵�
insert into ErpPowerResource (FuncId,Type,
Path,DisplayName,ParentFuncId,Orders,Display,Controller,Action,Logo)  values 
(
    302,'SRC','/Group','����ά��',3,2,1,'Group','Index',''
) ;

insert into ErpPowerResource (FuncId,Type,
Path,DisplayName,ParentFuncId,Orders,Display,Controller,Action,Logo)  values 
(
    303,'SRC','/Brand','Ʒ��ά��',3,3,1,'Brand','Index',''
) ;

---tianyu.xu   15/07/28 
ALTER TABLE TvTemplateAttribute  MODIFY COLUMN Value text; 


--2015.07.27 rick.wang ���� #1271 APP-versioncodeΪС��ʱ��̨���� DEV_V1.4
ALTER TABLE AppVersion  MODIFY COLUMN VersionCode int(11);
ALTER TABLE AppPublish  MODIFY COLUMN VersionCode int(11);
ALTER TABLE DeviceAppsMonitor  MODIFY COLUMN VersionCode int(11);


--2015.07.27 rick.wang �ŵ��������û��� DEV_V1.4
insert into ErpPowerResource (FuncId,Type,
Path,DisplayName,ParentFuncId,Orders,Display,Controller,Action,Logo)  values 
(
    206,'SRC','/HotelConfigSummary','�ŵ��������û���',2,6,1,'HotelConfigSummary','Index',''
) ;


--2015.07.28 rick.wang ģ��Ԥ�� DEV_V1.4
INSERT INTO `YeahTV`.`ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('50202', 'PAGE', '/Template/TemplatePreview', 'ģ��Ԥ��', '502', '2', '1', 'Template', 'TemplatePreview', '');

--2015.07.29 roger.wu ���ӵ�Ӱ������ DEV_V1.4
ALTER TABLE `YeahTV`.`HotelMovieTrace` 
ADD COLUMN `Order` INT NULL AFTER `Active`;

UPDATE `YeahTV`.`HotelMovieTrace`
SET
`Order` = 1;


--2015.7.30 chixiu.gong CoreSysHotel���CreateTime�ֶ� DEV_V1.4
alter table YeahTVBind.CoreSysHotel add  CreateTime datetime default '2015-7-30';

--2015.08.05 chixiu.gong CoreSysHotel�����޸�TemplateIdΪ�ɿ����� DEV_V1.4
ALTER TABLE `YeahTVBind`.`CoreSysHotel` CHANGE COLUMN `TemplateId` `TemplateId` VARCHAR(50) NULL ;

--2015.08.04 roger.wu  V1.8 �°�VOD����ʵ��
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

����ϵͳ������VodDailyOrderExpires�����ð�����Чʱ��
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('VodDailyOrderExpires', '-24', 'System', 1, now());

��ÿ�Ρ������Ƶ�������������
INSERT INTO `TvHotelconfig` (`HotelId`, `ConfigCode`, `ConfigName`, `ConfigValue`, `Active`, `LastUpdater`, `LastUpdateTime`, `CreateTime`) VALUES ('�޸�Ϊ�Ƶ�ID', 'HotelPayment', '֧���������', '{\"PriceOfDay\":5.00,\"PaymentModels\":[\"ALIPAY\",\"WXPAY\",\"FZPAY\"],\"PayType\":\"Daily\"}', '1', 'amdmin', now(), now());

--2015.08.13 YangJin HCS�ӿ�ʵ�� DEV_V1.81
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

--2015.08.18 YangJin ���HCS��֤��Կ DEV_V1.81
INSERT INTO `YeahTV`.`SystemConfig`
(`ConfigName`,`ConfigValue`,`ConfigType`,`Active`,`LastUpdateTime`) VALUES ('HCSPublicKey','d2b1adc163094517bc165b80e0105ac8','System','1','2015-08-18 00:00:00');

--2015.08.18 rick.wang ����BehaviorLog��ṹ��� DEV_V1.8.1
drop table BehaviorLog;
CREATE TABLE `BehaviorLog` (
  `Id` VARCHAR(50) NOT NULL,
  `HotelId` VARCHAR(45) NOT NULL,
  `DeviceSerise` VARCHAR(50) NOT NULL,
  `BehaviorType` VARCHAR(50) NOT NULL,
  `BehaviorInfo` TEXT NOT NULL,
  `CreateTime` DATETIME NOT NULL,
  PRIMARY KEY (`Id`));

  --2015.08.20 YangJin ��Task��������Type�ֶ�������Task������ DEV_V1.8.1
ALTER TABLE `YeahTV`.`HCSDownloadTask` 
ADD COLUMN `Type` VARCHAR(45) NOT NULL DEFAULT '' AFTER `ServerId`;



----------------------------- SystemConfig�¼�������--begin------------------------------------------------------------------------------
INSERT INTO `YeahTV`.`SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`,`LastUpdateTime`) VALUES ('VodBackground', '#000000', 'System', 1,now());

INSERT INTO `YeahTV`.`SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`,`LastUpdateTime`) VALUES ('VodColor', '#000000', 'System', 1,now());
-------------------------SystemConfig�¼�������-----end----------------------------------------------------------------------------------

--2015.08.21 YangJin ��Job��������Operation�ֶ����������¼ܵ����� DEV_V1.8.1
ALTER TABLE `YeahTV`.`HCSJob` 
ADD COLUMN `Operation` VARCHAR(45) NOT NULL AFTER `Name`;



--2015.08.17 rick.wang VOD2.0 DEV_V1.8.2
ALTER TABLE `HotelMovieTraceNoTemplate` 
ADD COLUMN `IsDelete` BIT(1) NOT NULL DEFAULT 0 AFTER `CreateTime`;



����ϵͳ������HotelPayment��֧��ȫ��Ĭ������
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('HotelPayment', '{\"PriceOfDay\":5.00,\"PaymentModels\":[\"ALIPAY\",\"WXPAY\",\"FZPAY\"],\"PayType\":\"Daily\",\"TopMoviesCount\":10}', 'System', 1, now());

--2015.08.19 rick.wang VOD2.0 �˵� DEV_V1.8.2
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('207', 'SRC', '/VOD', '��Ӱ����2.0', '2', '7', '1', 'VOD', 'Index', '');
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('20701', 'PAGE', '/VOD/Edit', '��Ӱ��Ϣ����', '207', '1', '1', 'VOD', 'Edit', '');
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('20702', 'PAGE', '/VOD/Distribute', '��Ӱ�ַ�', '207', '2', '1', 'VOD', 'Distribute', '');
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('20703', 'PAGE', '/VOD/Add', '������Ӱ��Ϣ', '207', '3', '1', 'VOD', 'Add', '');

ALTER TABLE `MovieForLocalize` 
ADD COLUMN `DistributeAll` BIT(1) NOT NULL DEFAULT b'0' AFTER `CreateTime`;

--2015.08.19 rick.wang VOD2.0 �˵� DEV_V1.8.2
INSERT INTO `SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('HCSTaskDefaultConfig', '{\"highspeed\": \"80\",\"highspeed_start\": \"23\",\"highspeed_end\": \"06\",\"lowspeed\": \"20\",\"lowspeed_start\": \"06\",\"lowspeed_end\": \"23\"}', 'System', 1, now());





--2015.08.21 dou VOD2.0 ��ӵ�Ӱ��������ĸ�����ؼ��� DEV_V1.8.2
ALTER TABLE `YeahTV`.`MovieForLocalize` 
ADD COLUMN `FirstWord` VARCHAR(100) NULL DEFAULT NULL AFTER `DistributeAll`;


--2015.08.21 dou VOD2.0 �޸����ֵ��ֶ����� DEV_V1.8.2
ALTER TABLE `YeahTV`.`MovieForLocalize` 
CHANGE COLUMN `Rate` `Rate` DECIMAL(3,1) NULL DEFAULT NULL ; 
 

-----------------------------------------------2015.8.22 ����MovieForLocalize��������ֶ�HotelCount--DEV_V1.8.2��֧---------------------------------------
ALTER TABLE `YeahTV`.`MovieForLocalize` 
CHANGE COLUMN `HotelCount` `HotelCount` INT NOT NULL DEFAULT 0 ;--------------------------------------------------------------------------------------------------------------------------------


-----------------------------------------------2015.8.28 9:28 ��־ΰ MovieForLocalize����Ӣ�ĵ�Ӱ��������ά��--DEV_V1.8.2��֧---------------------------------------
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
('288eebe32cce43618e27a07f59f9c3df','zh-cn','�����Ƽ�'),
('711951e442c7492eaff21b533cf1f191','en-us','Recent'),
('711951e442c7492eaff21b533cf1f191','zh-cn','�������'),
('dc1ec291d4dd4bafbd73281fde8a4de1','en-us','On Sale'),
('dc1ec291d4dd4bafbd73281fde8a4de1','zh-cn','��ʱ�Ż�'),
('d4ab30dec068416687fe39e72941d7e6','en-us','Action'),
('d4ab30dec068416687fe39e72941d7e6','zh-cn','������Ӱ'),
('abf786366aa9496d8b9c172a4960a25d','en-us','Romance'),
('abf786366aa9496d8b9c172a4960a25d','zh-cn','�����Ӱ'),
('919ee85a158945d3bb61cfdb37d472be','en-us','Sci-Fi'),
('919ee85a158945d3bb61cfdb37d472be','zh-cn','�ƻõ�Ӱ'),
('d6d3018721864d23818d7888f259bcfc','en-us','Comedy'),
('d6d3018721864d23818d7888f259bcfc','zh-cn','ϲ���Ӱ'),
('Td6d3018721864d23818d7888f259bcfc','en-us','Horror'),
('Td6d3018721864d23818d7888f259bcfc','zh-cn','�ֲ���Ӱ'),
('Td6d3018721864d23818d7888f259bcfd','en-us','War'),
('Td6d3018721864d23818d7888f259bcfd','zh-cn','ս����Ӱ'),
('Td6d3018721864d23818d7888f259bcfF','en-us','Drama'),
('Td6d3018721864d23818d7888f259bcfF','zh-cn','�����Ӱ')




  
--2015.09.01 rick.wang VOD2.0 Ȩ�� DEV_V1.7
  CREATE TABLE `HotelPermition` (
  `Id` VARCHAR(64) NOT NULL,
  `PermitionType` VARCHAR(45) NOT NULL,
  `UserId` VARCHAR(64) NOT NULL,
  `TypeId` VARCHAR(64) NOT NULL,
  PRIMARY KEY (`Id`));

--2015.09.01 rick.wang dashboard�ɲ�ѯ���� DEV_V1.7
  INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('DashBoardValidityDays', '180', 'System', 1, now());
  
  
--2015.09.02 rick.wang �û�Ȩ�޹���ҳ�����Ȩ�� DEV_V1.7
 INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('40101', 'PAGE', '/User/EditeIndex', '�û���Ϣ�༭', '401', '1', '1', 'User', 'EditeIndex', '');
 
 
--2015.09.06 rick.wang DashBoard����Ȩ�� DEV_V1.7
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('6', 'MODEL', '', '�Ƶ��û�Ȩ��', '0', '1', '0', '', '', '');
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('601', 'PAGE', '/DashBoard', '����DashBorad', '6', '1', '0', 'DashBoard', 'Index', '');
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('602', 'PAGE', '/DashBoard/MoreHotelIndex', '���DashBorad', '6', '2', '0', 'DashBoard', 'MoreHotelIndex', '');



  


------------2015.9.7DianPingController.cs���޸ĵ�SystemConfig���������ļ�1.7��֧--------------------------------------------------
INSERT INTO `YeahTV`.`SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `AppId`, `Active`,`LastUpdateTime`) VALUES ('AppKey', '3716066064', 'System', '', 1,now());

INSERT INTO `YeahTV`.`SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `AppId`, `Active`,`LastUpdateTime`) VALUES ('AppSecret', '92b099f67fde4b439a4295f0de1b1b1d', 'System', '', 1,now());

INSERT INTO `YeahTV`.`SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `AppId`, `Active`,`LastUpdateTime`) VALUES ('RADIUS', '5000', 'System', '', 1,now());
-----------------------------------------------------------------------------------------------------------




------------2015.9.11 �̳Ƕ����� rick.wang ��֧1.9.3--------------------------------------------------
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

------------2015.9.15 �̳�֧���ص��ӿ� rick.wang ��֧1.9.3 --------------------------------------------------

INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('PaymentNotifyUrl', 'http://10.254.0.42:8099/api/PayMent/PaymentCallBack', 'System', 1, now());

-----------------------------------------------------------------------------------------------------------

------------2015.9.15 �̳ǹ�Կ��˽Կ rick.wang ��֧1.9.3 --------------------------------------------------
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('StoreSignPublicKey', '2c9837cb1b714ee28edb3002fd2e3387', 'System', 1, now());
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('StoreSignPrivateKey', 'c7e0604eb43a41749a6427c2a8982283', 'System', 1, now());
-----------------------------------------------------------------------------------------------------------


-------------------------------2015.9.17_gongwei_1.9.3��֧_SystemConfig������̳ǽӿ�������------------------------------
INSERT INTO `YeahTV`.`SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('ShoppingMallUrl', 'http://192.168.8.88:8080', 'System', 1, now());

----------------------------------------------------


-------------------------------2015.9.21_gongwei_1.9.3��֧_systemconfig����Ӷ�������ʱ��������---------------------------------
INSERT INTO `YeahTV`.`SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('ExpirationDate', '30', 'System', 1, now());

---------------------------------------------------

-------------------------------2015.9.24_YangJin_1.9.3��֧_ΪOrder����ӷ�Ʊ̧ͷ�ֶ�---------------------------------

ALTER TABLE `YeahTV`.`VODOrder` 
ADD COLUMN `InvoicesTitle` VARCHAR(1000) NULL AFTER `PayType`;

---------------------------------------------------


-------------------------------2015.10.8_roger.wu_1.9.3��֧_SystemConfig����Ӷ����鿴������------------------------------
INSERT INTO `YeahTV`.`SystemConfig` ( `ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('ShoppingOrderAddress', 'http://192.168.8.90:8080/hotelmall/login.do', 'System', 1, now());

----------------------------------------------------

-------------------------------2015.10.8_YangJin_1.9.3��֧_��Ӳ˵�Ȩ��------------------------------

INSERT INTO `YeahTV`.`ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('20703', 'PAGE', '/DashBoard/VodOrderReportIndex', 'VOD֧��������ѯ', '207', '1', '1', 'DashBoard', 'VodOrderReportIndex', '');

----------------------------------------------------

----------------------------------------------------2015.11.5_���˱�_1.10��֧IU���Ի����û�ӭ���ֶ������޸�
ALTER TABLE  `CoreSysHotelSencond` 
CHANGE COLUMN `WelcomeWord` `WelcomeWord` TEXT NULL DEFAULT NULL ;


----------------------------------------------------2015.11.6_�־ΰ_1.10��֧IU���Ի�����Ȩ�����-----------------------------------
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('603', 'PAGE', '/DashBoard/VodOrderReportIndex', 'IU�Ƶ���Ի�����', '6', '2', '1', 'DashBoard', 'IUConfigIndex', '');

-------------------------------2015.11.10_rick.wang_2.0��֧_���hcs�������Ȩ�޲˵�------------------------------

INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`, `Logo`) VALUES ('407', 'SRC', '/HCSCacheVersion', 'HCS�������', '4', '2', '1', 'HCSCacheVersion', 'Index', '');

----------------------------------------------------


-------------------------------2015.11.23_rick.wang_2.0��֧_���hcs��������------------------------------
CREATE TABLE `HCSCacheVersion` (
  `Id` varchar(64) NOT NULL,
  `PermitionType` varchar(45) NOT NULL,
  `TypeId` varchar(64) NOT NULL,
  `Version` int(11) NOT NULL,
  `LastUpdateUser` varchar(64) NOT NULL,
  `LastUpdateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-------------------------------2015.12.03_douzhiwei2.1��֧_���AuthTVToken���ֶһ�Token��------------------------------
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


-------------------------------2015.12.03_rick.wang_2.0.1��֧_���ȫ�����ñ�------------------------------
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

-------------------------------2015.12.09_rick.wang_2.0.1��֧_��ӻ��ֶһ���¼��------------------------------
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

-------------------------------2015.12.14_rick.wang_2.0.1��֧_��������Խ���Դվ��������------------------------------
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('YeahInfoResourceSiteAddress', 'http://resource.yeah-info.com/', 'System', 1, '2015-12-14 10:57:11');

-------------------------------2015.12.15_rick.wang_2.0.1��֧_OpenApi˽Կ------------------------------
INSERT INTO `SystemConfig` (`ConfigName`, `ConfigValue`, `ConfigType`, `Active`, `LastUpdateTime`) VALUES ('OpenAPIAuthSignPrivateKey', '01a280ff22154718926edeecc3aa518e', 'System', 1, '2015-12-15 10:57:11');

-------------------------------2015.12.15_rick.wang_2.0.1��֧_ȫ��������˵�------------------------------
INSERT INTO `ErpPowerResource` (`FuncId`, `Type`, `Path`, `DisplayName`, `ParentFuncId`, `Orders`, `Display`, `Controller`, `Action`) VALUES ('30301', 'PAGE', '/GlobalConfig', 'ȫ��������', '303', '4', '1', 'GlobalConfig', 'Index');

-------------------------------2015.12.17_rick.wang_2.0.1��֧_��Ӷ���ȡ��ά���¼��------------------------------
CREATE TABLE `OrderQRCodeRecord` (
  `Id` VARCHAR(64) NOT NULL,
  `OrderId` VARCHAR(64) NOT NULL,
  `OrderType` VARCHAR(100) NOT NULL,
  `Ticket` VARCHAR(1000) NOT NULL,
  `CreateTime` DATETIME NOT NULL,
  PRIMARY KEY (`Id`));




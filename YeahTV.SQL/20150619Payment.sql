-- MySQL dump 10.13  Distrib 5.6.17, for Win32 (x86)
--
-- Host: 10.4.31.65    Database: YeahTV
-- ------------------------------------------------------
-- Server version	5.5.24-ucloudrel1-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `VODOrder`
--

DROP TABLE IF EXISTS `VODOrder`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `VODOrder` (
  `OrderId` varchar(20) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `State` int(11) NOT NULL DEFAULT '0',
  `CompleteTime` datetime DEFAULT NULL,
  `VodRequestId` varchar(64) DEFAULT NULL,
  `MovieId` varchar(64) NOT NULL,
  `SeriseCode` varchar(128) DEFAULT NULL,
  `RoomNo` varchar(7) DEFAULT NULL,
  `Hotelid` varchar(50) NOT NULL,
  `GoodsName` varchar(32) DEFAULT NULL,
  `GoodsDesc` varchar(100) DEFAULT NULL,
  `PayInfo` varchar(200) DEFAULT NULL,
  `IsDelete` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`OrderId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `VODPaymentRequest`
--

DROP TABLE IF EXISTS `VODPaymentRequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `VODPaymentRequest` (
  `Id` varchar(64) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `OrderId` varchar(20) NOT NULL,
  `OrderAmount` varchar(10) NOT NULL,
  `PayInfo` varchar(200) DEFAULT NULL,
  `GoodsId` varchar(32) DEFAULT NULL,
  `GoodsName` varchar(32) DEFAULT NULL,
  `GoodsDesc` varchar(100) DEFAULT NULL,
  `BizHotle` varchar(50) DEFAULT NULL,
  `BizRoom` varchar(50) DEFAULT NULL,
  `BizDevice` varchar(50) DEFAULT NULL,
  `BizMember` varchar(50) DEFAULT NULL,
  `NotifyUrl` varchar(128) DEFAULT NULL,
  `Memo` varchar(20) DEFAULT NULL,
  `Pid` varchar(64) DEFAULT NULL,
  `RequestSign` varchar(64) DEFAULT NULL,
  `ResultSign` varchar(64) DEFAULT NULL,
  `ResultMessage` varchar(4000) DEFAULT NULL,
  `ResultCode` varchar(50) DEFAULT NULL,
  `ResultQrcodeUrl` varchar(4000) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `VODPaymentResult`
--

DROP TABLE IF EXISTS `VODPaymentResult`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `VODPaymentResult` (
  `Id` varchar(64) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `ResultSign` varchar(64) DEFAULT NULL,
  `ResultMessage` varchar(4000) DEFAULT NULL,
  `ResultCode` varchar(50) DEFAULT NULL,
  `OrderId` varchar(20) DEFAULT NULL,
  `NotifyTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `VODRequest`
--

DROP TABLE IF EXISTS `VODRequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `VODRequest` (
  `Id` varchar(64) NOT NULL,
  `MovieId` varchar(64) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `SeriseCode` varchar(128) NOT NULL,
  `ResultType` int(11) DEFAULT NULL,
  `ResultMessage` varchar(4000) DEFAULT NULL,
  `PayInfo` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-06-19  9:44:34



--
-- Dumping data for table `SystemConfig`
--

LOCK TABLES `SystemConfig` WRITE;
/*!40000 ALTER TABLE `SystemConfig` DISABLE KEYS */;
INSERT INTO `SystemConfig` VALUES (871,'VodOrderExpires','-24','System',NULL,'','2015-06-17 15:59:00'),(872,'VodPaymentSignKey','sstyxmh3erotgsjpbgukdz2y5ddedfg3','System',NULL,'','2015-06-17 15:59:00'),(873,'VodPaymentPid','1d4b534e-f0f3-493b-9142-f778973b19e2','System',NULL,'','2015-06-17 15:59:00'),(874,'VodDefaultPayInfo','ALIPAY','System',NULL,'','2015-06-17 15:59:00'),(875,'VodPaymentRequestUrl','https://115.239.196.15:8080/PayGateway/charge.do','System',NULL,'','2015-06-17 16:45:29'),(876,'VodPaymentNotifyUrl','http://10.254.0.10:8003/PayMent/VodPaymentCallBack','System',NULL,'','2015-06-17 15:59:00');
/*!40000 ALTER TABLE `SystemConfig` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-06-19  9:45:30



ALTER TABLE `MovieTemplateRelation` 
CHANGE COLUMN `Price` `Price` DECIMAL(10,2) NOT NULL ;

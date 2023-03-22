CREATE DATABASE  IF NOT EXISTS `db` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `db`;
-- MySQL dump 10.13  Distrib 8.0.30, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: trade
-- ------------------------------------------------------
-- Server version	5.6.31

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Category`
--

DROP TABLE IF EXISTS `Category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Category` (
  `CategoryID` int(11) NOT NULL AUTO_INCREMENT,
  `CategoryName` varchar(100) NOT NULL,
  PRIMARY KEY (`CategoryID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Category`
--

LOCK TABLES `Category` WRITE;
/*!40000 ALTER TABLE `Category` DISABLE KEYS */;
INSERT INTO `Category` VALUES (1,'Струнные'),(2,'Духовые'),(3,'Ударные'),(4,'Язычковые'),(5,'Клавишные'),(6,'Электронные '),(7,'Механические');
/*!40000 ALTER TABLE `Category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Order`
--

DROP TABLE IF EXISTS `Order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Order` (
  `OrderID` int(11) NOT NULL AUTO_INCREMENT,
  `OrderStatus` int(11) NOT NULL,
  `OrderDeliveryDate` date NOT NULL,
  `OrderPickupPoint` int(11) NOT NULL,
  `OrderCode` int(11) NOT NULL,
  `OrderUser` int(11) NOT NULL,
  `OrderDate` date NOT NULL,
  PRIMARY KEY (`OrderID`),
  KEY `opp_idx` (`OrderPickupPoint`),
  KEY `ou_idx` (`OrderUser`),
  KEY `os_idx` (`OrderStatus`),
  CONSTRAINT `opp1` FOREIGN KEY (`OrderPickupPoint`) REFERENCES `PickupPoint` (`PickupPointID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `os` FOREIGN KEY (`OrderStatus`) REFERENCES `Status` (`StatusID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `ou` FOREIGN KEY (`OrderUser`) REFERENCES `User` (`UserID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Order`
--

LOCK TABLES `Order` WRITE;
/*!40000 ALTER TABLE `Order` DISABLE KEYS */;
INSERT INTO `Order` VALUES (1,1,'2022-05-20',1,801,1,'2022-05-20'),(2,2,'2022-05-20',14,802,1,'2022-05-20'),(3,2,'2023-05-20',2,803,2,'2023-05-20'),(4,2,'2023-05-20',22,804,2,'2023-05-20'),(5,2,'2025-05-20',2,805,3,'2025-05-20');
/*!40000 ALTER TABLE `Order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `OrderProduct`
--

DROP TABLE IF EXISTS `OrderProduct`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `OrderProduct` (
  `OrderProductID` int(11) NOT NULL AUTO_INCREMENT,
  `OrderID` int(11) NOT NULL,
  `ProductArticleNumber` varchar(100) NOT NULL,
  `OrderProductCount` int(11) NOT NULL,
  PRIMARY KEY (`OrderProductID`),
  KEY `opp_idx` (`ProductArticleNumber`),
  KEY `opo_idx` (`OrderID`),
  CONSTRAINT `opo` FOREIGN KEY (`OrderID`) REFERENCES `Order` (`OrderID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `opp` FOREIGN KEY (`ProductArticleNumber`) REFERENCES `Product` (`ProductArticleNumber`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=75 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `OrderProduct`
--

LOCK TABLES `OrderProduct` WRITE;
/*!40000 ALTER TABLE `OrderProduct` DISABLE KEYS */;
INSERT INTO `OrderProduct` VALUES (1,1,'B736H6',2),(2,2,'B963H5',2),(3,3,'C430T4',1),(4,4,'C635Y6',1),(5,5,'C943G5',1);
/*!40000 ALTER TABLE `OrderProduct` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `PickupPoint`
--

DROP TABLE IF EXISTS `PickupPoint`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `PickupPoint` (
  `PickupPointID` int(11) NOT NULL AUTO_INCREMENT,
  `PickupPointName` varchar(100) NOT NULL,
  PRIMARY KEY (`PickupPointID`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `PickupPoint`
--

LOCK TABLES `PickupPoint` WRITE;
/*!40000 ALTER TABLE `PickupPoint` DISABLE KEYS */;
INSERT INTO `PickupPoint` VALUES (1,'344288, г. Талнах, ул. Чехова, 1'),(2,'614164, г.Талнах,  ул. Степная, 30'),(3,'394242, г. Талнах, ул. Коммунистическая, 43'),(4,'660540, г. Талнах, ул. Солнечная, 25'),(5,'125837, г. Талнах, ул. Шоссейная, 40'),(6,'125703, г. Талнах, ул. Партизанская, 49'),(7,'625283, г. Талнах, ул. Победы, 46'),(8,'614611, г. Талнах, ул. Молодежная, 50'),(9,'454311, г.Талнах, ул. Новая, 19'),(10,'660007, г.Талнах, ул. Октябрьская, 19'),(11,'603036, г. Талнах, ул. Садовая, 4'),(12,'450983, г.Талнах, ул. Комсомольская, 26'),(13,'394782, г. Талнах, ул. Чехова, 3'),(14,'603002, г. Талнах, ул. Дзержинского, 28'),(15,'450558, г. Талнах, ул. Набережная, 30'),(16,'394060, г.Талнах, ул. Фрунзе, 43'),(17,'410661, г. Талнах, ул. Школьная, 50'),(18,'625590, г. Талнах, ул. Коммунистическая, 20'),(19,'625683, г. Талнах, ул. 8 Марта'),(20,'400562, г. Талнах, ул. Зеленая, 32'),(21,'614510, г. Талнах, ул. Маяковского, 47'),(22,'410542, г. Талнах, ул. Светлая, 46'),(23,'620839, г. Талнах, ул. Цветочная, 8'),(24,'443890, г. Талнах, ул. Коммунистическая, 1'),(25,'603379, г. Талнах, ул. Спортивная, 46'),(26,'603721, г. Талнах, ул. Гоголя, 41'),(27,'410172, г. Талнах, ул. Северная, 13'),(28,'420151, г. Талнах, ул. Вишневая, 32'),(29,'125061, г. Талнах, ул. Подгорная, 8'),(30,'630370, г. Талнах, ул. Шоссейная, 24'),(31,'614753, г. Талнах, ул. Полевая, 35'),(32,'426030, г. Талнах, ул. Маяковского, 44'),(33,'450375, г. Талнах ул. Клубная, 44'),(34,'625560, г. Талнах, ул. Некрасова, 12'),(35,'630201, г. Талнах, ул. Комсомольская, 17'),(36,'190949, г. Талнах, ул. Мичурина, 26');
/*!40000 ALTER TABLE `PickupPoint` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Product`
--

DROP TABLE IF EXISTS `Product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Product` (
  `ProductArticleNumber` varchar(100) NOT NULL,
  `ProductName` text NOT NULL,
  `ProductDescription` text NOT NULL,
  `ProductCategory` int(11) NOT NULL,
  `ProductPhoto` varchar(100) DEFAULT NULL,
  `ProductManufacturer` text NOT NULL,
  `ProductCost` decimal(19,4) NOT NULL,
  `ProductDiscountAmount` int(11) DEFAULT '0',
  `ProductQuantityInStock` int(11) NOT NULL,
  `ProductDiscount` int(11) DEFAULT '0',
  `ProductSupplier` int(11) NOT NULL,
  `ProductUnit` int(11) NOT NULL,
  PRIMARY KEY (`ProductArticleNumber`),
  KEY `pu_idx` (`ProductUnit`),
  KEY `pc_idx` (`ProductCategory`),
  KEY `ps_idx` (`ProductSupplier`),
  CONSTRAINT `pc` FOREIGN KEY (`ProductCategory`) REFERENCES `Category` (`CategoryID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `ps` FOREIGN KEY (`ProductSupplier`) REFERENCES `Supplier` (`SupplierID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `pu` FOREIGN KEY (`ProductUnit`) REFERENCES `Unit` (`UnitID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Product`
--

LOCK TABLES `Product` WRITE;
/*!40000 ALTER TABLE `Product` DISABLE KEYS */;
INSERT INTO `Product` VALUES ('B736H6','Классическая гитара HORA N1226-4/4 Student','Гитара Hora N1226-4/4 Student Классическая 4/4',1,'picture.png','Румыния',18220.0000,3,4,5,2,1),('B963H5','Цифровое пианино ARTESIA PE-88 White','Цифровое фортепиано имеет глубокое и выразительное звучание,',5,NULL,'Китай',31800.0000,3,8,5,2,1),('C430T4','Классическая гитара HOMAGE LC-3400 1/2','Гитара Homage LC-3400 Классическая уменьшенная 1/2 34\"',1,NULL,'Китай',4600.0000,3,6,30,2,1),('C635Y6','Классическая гитара HOMAGE LC-3911-BK','LC-3911-BK Классическая 6-струнная гитара 39\"',1,'HOMAGE.jpg','Китай',5800.0000,4,12,15,1,1),('C730R7','BASIX OX109-BK','Ударная установка BASIX OX109',3,'basix.jpg','BASIX',49300.0000,3,6,5,2,1),('C943G5','DIXON PODSP422ADG Spark','Ударная установка Dixon PODSP422ACG Spark. Обечайка изготовлена из красного дерева, фурнитура: хром.',3,'DIXON.jpg','DIXON',56200.0000,4,4,5,1,1);
/*!40000 ALTER TABLE `Product` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Role`
--

DROP TABLE IF EXISTS `Role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Role` (
  `RoleID` int(11) NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(100) NOT NULL,
  PRIMARY KEY (`RoleID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Role`
--

LOCK TABLES `Role` WRITE;
/*!40000 ALTER TABLE `Role` DISABLE KEYS */;
INSERT INTO `Role` VALUES (1,'Клиент'),(2,'Менеджер'),(3,'Администратор');
/*!40000 ALTER TABLE `Role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Status`
--

DROP TABLE IF EXISTS `Status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Status` (
  `StatusID` int(11) NOT NULL AUTO_INCREMENT,
  `StatusName` varchar(100) NOT NULL,
  PRIMARY KEY (`StatusID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Status`
--

LOCK TABLES `Status` WRITE;
/*!40000 ALTER TABLE `Status` DISABLE KEYS */;
INSERT INTO `Status` VALUES (1,'Завершен'),(2,'Новый ');
/*!40000 ALTER TABLE `Status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Supplier`
--

DROP TABLE IF EXISTS `Supplier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Supplier` (
  `SupplierID` int(11) NOT NULL AUTO_INCREMENT,
  `SupplierName` varchar(100) NOT NULL,
  PRIMARY KEY (`SupplierID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Supplier`
--

LOCK TABLES `Supplier` WRITE;
/*!40000 ALTER TABLE `Supplier` DISABLE KEYS */;
INSERT INTO `Supplier` VALUES (1,'ООО Струны'),(2,'Мы поём');
/*!40000 ALTER TABLE `Supplier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Unit`
--

DROP TABLE IF EXISTS `Unit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Unit` (
  `UnitID` int(11) NOT NULL AUTO_INCREMENT,
  `UnitName` varchar(100) NOT NULL,
  PRIMARY KEY (`UnitID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Unit`
--

LOCK TABLES `Unit` WRITE;
/*!40000 ALTER TABLE `Unit` DISABLE KEYS */;
INSERT INTO `Unit` VALUES (1,'шт.');
/*!40000 ALTER TABLE `Unit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `User`
--

DROP TABLE IF EXISTS `User`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `User` (
  `UserID` int(11) NOT NULL AUTO_INCREMENT,
  `UserSurname` varchar(100) NOT NULL,
  `UserName` varchar(100) NOT NULL,
  `UserPatronymic` varchar(100) NOT NULL,
  `UserLogin` text NOT NULL,
  `UserPassword` text NOT NULL,
  `UserRole` int(11) NOT NULL,
  PRIMARY KEY (`UserID`),
  KEY `UserRole` (`UserRole`),
  CONSTRAINT `user_ibfk_1` FOREIGN KEY (`UserRole`) REFERENCES `Role` (`RoleID`)
) ENGINE=InnoDB AUTO_INCREMENT=72 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `User`
--

LOCK TABLES `User` WRITE;
/*!40000 ALTER TABLE `User` DISABLE KEYS */;
INSERT INTO `User` VALUES (1,'Ефремов','Сергей','Пантелеймонович','loginDEppn2018','6}i+FD',1),(2,'Родионова','Тамара','Валентиновна','loginDElqb2018','RNynil',1),(3,'Миронова','Галина','Улебовна','loginDEydn2018','34I}X9',2),(4,'Сидоров','Роман','Иринеевич','loginDEijg2018','4QlKJW',2),(5,'Ситников','Парфений','Всеволодович','loginDEdpy2018','MJ0W|f',2),(6,'Никонов','Роман','Геласьевич','loginDEwdm2018','&PynqU',2),(7,'Щербаков','Владимир','Матвеевич','loginDEdup2018','JM+2{s',1),(8,'Кулаков','Мартын','Михаилович','loginDEhbm2018','9aObu4',2),(9,'Сазонова','Оксана','Лаврентьевна','loginDExvq2018','hX0wJz',3),(10,'Архипов','Варлам','Мэлорович','loginDErks2018','LQNSjo',2),(11,'Устинова','Ираида','Мэлоровна','loginDErvb2018','ceAf&R',3),(12,'Лукин','Георгий','Альбертович','loginDEulo2018','#ИМЯ?',3),(13,'Кононов','Эдуард','Валентинович','loginDEgfw2018','3c2Ic1',1),(14,'Орехова','Клавдия','Альбертовна','loginDEmxb2018','ZPXcRS',2),(15,'Яковлев','Яков','Эдуардович','loginDEgeq2018','&&Eim0',2),(16,'Воронов','Мэлс','Семёнович','loginDEkhj2018','Pbc0t{',1),(17,'Вишнякова','Ия','Данииловна','loginDEliu2018','32FyTl',1),(18,'Третьяков','Фёдор','Вадимович','loginDEsmf2018','{{O2QG',1),(19,'Макаров','Максим','Ильяович','loginDEutd2018','GbcJvC',2),(20,'Шубина','Маргарита','Анатольевна','loginDEpgh2018','YV2lvh',2),(21,'Блинова','Ангелина','Владленовна','loginDEvop2018','pBP8rO',2),(22,'Воробьёв','Владлен','Фролович','loginDEwjo2018','EQaD|d',1),(23,'Сорокина','Прасковья','Фёдоровна','loginDEbur2018','aZKGeI',2),(24,'Давыдов','Яков','Антонович','loginDEszw2018','EGU{YE',1),(25,'Рыбакова','Евдокия','Анатольевна','loginDExsu2018','*2RMsp',1),(26,'Маслов','Геннадий','Фролович','loginDEztn2018','nJBZpU',1),(27,'Цветкова','Элеонора','Аристарховна','loginDEtmn2018','UObB}N',1),(28,'Евдокимов','Ростислав','Александрович','loginDEhep2018','SwRicr',1),(29,'Никонова','Венера','Станиславовна','loginDEevr2018','zO5l}l',1),(30,'Громов','Егор','Антонович','loginDEnpa2018','M*QLjf',1),(31,'Суворова','Валерия','Борисовна','loginDEgyt2018','Pav+GP',3),(32,'Мишина','Елизавета','Романовна','loginDEbrr2018','Z7L|+i',1),(33,'Зимина','Ольга','Аркадьевна','loginDEyoo2018','UG1BjP',3),(34,'Игнатьев','Игнатий','Антонинович','loginDEaob2018','3fy+3I',3),(35,'Пахомова','Зинаида','Витальевна','loginDEwtz2018','&GxSST',1),(36,'Устинов','Владимир','Федосеевич','loginDEctf2018','sjt*3N',3),(37,'Кулаков','Мэлор','Вячеславович','loginDEipm2018','MAZl6|',2),(38,'Сазонов','Авксентий','Брониславович','loginDEjoi2018','o}C4jv',1),(39,'Бурова','Наина','Брониславовна','loginDEwap2018','4hny7k',2),(40,'Фадеев','Демьян','Федосеевич','loginDEaxm2018','BEc3xq',1),(41,'Бобылёва','Дарья','Якуновна','loginDEsmq2018','ATVmM7',1),(42,'Виноградов','Созон','Арсеньевич','loginDEeur2018','n4V{wP',1),(43,'Гордеев','Владлен','Ефимович','loginDEvke2018','WQLXSl',1),(44,'Иванова','Зинаида','Валерьевна','loginDEvod2018','0EW93v',2),(45,'Гусев','Руслан','Дамирович','loginDEjaw2018','h6z&Ky',1),(46,'Маслов','Дмитрий','Иванович','loginDEpdp2018','8NvRfC',2),(47,'Антонова','Ульяна','Семёновна','loginDEjpp2018','oMOQq3',1),(48,'Орехова','Людмила','Владимировна','loginDEkiy2018','BQzsts',2),(49,'Авдеева','Жанна','Куприяновна','loginDEhmn2018','a|Iz|7',2),(50,'Кузнецов','Фрол','Варламович','loginDEfmn2018','cw3|03',1),(51,'гг','гг','гг','user','user',1),(52,'оо','оо','оо','admin','admin',3),(53,'мм','мм','мм','men','men',2),(54,'rrt','try','try','loginTt0m','Tt0m',1),(55,'3','324','324','loginrg5S','rg5S',1),(56,'mnjbhv','bhvgcf','mnbvg','login82kj','82kj',1),(57,'тим','орп','рпк','loginKSGL','KSGL',1),(58,'j','y','g','loginodw7','odw7',1),(59,'e','ds','cfa','loginNX6d','NX6d',1),(60,'gr','rg','egr','loginNZ21','NZ21',1),(61,'g','gf','gfd','loginl0PC','l0PC',1),(62,'dcd','ds','s','loginn0AW','n0AW',1),(63,'мс','см ','м с','login4nxl','4nxl',1),(64,'vc','cv','vc','loginDTaa','DTaa',1),(65,'fd','dfg','dggd','logindHAY','dHAY',1),(66,'ggrrrg','gr','retre','login9I1w','9I1w',1),(67,'123','1233','123','login43gi','43gi',1),(68,'reg','ert','retre','loginquqt','quqt',1),(69,'ttrh','try','tyt','loginITXt','ITXt',1),(70,'11','11','11','loginuPKF','uPKF',1),(71,'Иван','Иванов','Иванович','login9q8d','9q8d',1);
/*!40000 ALTER TABLE `User` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-03-17 22:29:14

-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: tmp
-- ------------------------------------------------------
-- Server version	8.4.1

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
-- Table structure for table `clear_info`
--

DROP TABLE IF EXISTS `clear_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clear_info` (
  `clear_info_id` int NOT NULL AUTO_INCREMENT,
  `room_id` int NOT NULL,
  `stage_number` tinyint DEFAULT NULL,
  `clear_date` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`clear_info_id`),
  KEY `fk_clear_info_room_id_idx` (`room_id`),
  CONSTRAINT `fk_clear_info_room_id` FOREIGN KEY (`room_id`) REFERENCES `room` (`room_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `easter_egg`
--

DROP TABLE IF EXISTS `easter_egg`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `easter_egg` (
  `easter_egg_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `description` varchar(500) DEFAULT NULL,
  `stage` tinyint NOT NULL,
  `nft_image_hash` varchar(255) NOT NULL,
  `nft_metadata_hash` varchar(255) NOT NULL,
  PRIMARY KEY (`easter_egg_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `friend`
--

DROP TABLE IF EXISTS `friend`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `friend` (
  `friend_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `friend_user_id` int NOT NULL,
  `is_active` tinyint(1) DEFAULT '0',
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`friend_id`),
  UNIQUE KEY `user_id_UNIQUE` (`user_id`,`friend_user_id`),
  KEY `fk_friend_user_id_idx` (`user_id`),
  KEY `fk_friend_friend_user_id_idx` (`friend_user_id`),
  CONSTRAINT `fk_friend_friend_user_id` FOREIGN KEY (`friend_user_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `fk_friend_user_id` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `friend_request`
--

DROP TABLE IF EXISTS `friend_request`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `friend_request` (
  `id` int NOT NULL AUTO_INCREMENT,
  `request_user_id` int NOT NULL,
  `receive_user_id` int NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '0',
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_fr_req_id_idx` (`request_user_id`),
  KEY `fk_fr_rec_id_idx` (`receive_user_id`),
  CONSTRAINT `fk_fr_rec_id` FOREIGN KEY (`receive_user_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `fk_fr_req_id` FOREIGN KEY (`request_user_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `metadata`
--

DROP TABLE IF EXISTS `metadata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `metadata` (
  `id` int NOT NULL AUTO_INCREMENT,
  `metadata_uri` varchar(255) NOT NULL,
  `image_url` varchar(255) NOT NULL,
  `description` text NOT NULL,
  `attributes` json NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `metadata_uri_UNIQUE` (`metadata_uri`),
  UNIQUE KEY `image_url_UNIQUE` (`image_url`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `nft`
--

DROP TABLE IF EXISTS `nft`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nft` (
  `nft_id` int NOT NULL AUTO_INCREMENT,
  `token_id` int NOT NULL,
  `contract_address` varchar(255) NOT NULL,
  `owner_address` varchar(255) NOT NULL,
  `metadata_id` int NOT NULL,
  `token_type` enum('ERC-721','ERC-1155') NOT NULL,
  `user_id` int NOT NULL,
  `easter_egg_id` int NOT NULL,
  `is_active` tinyint(1) DEFAULT '0',
  `minted_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`nft_id`),
  UNIQUE KEY `token_id_UNIQUE` (`token_id`),
  KEY `nfts_ibfk_1` (`metadata_id`),
  KEY `fk_nft_user_id_idx` (`user_id`),
  KEY `fk_nft_easter_egg_id_idx` (`easter_egg_id`),
  CONSTRAINT `fk_nft_easter_egg_id` FOREIGN KEY (`easter_egg_id`) REFERENCES `easter_egg` (`easter_egg_id`),
  CONSTRAINT `fk_nft_user_id` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `nfts_ibfk_1` FOREIGN KEY (`metadata_id`) REFERENCES `metadata` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `room`
--

DROP TABLE IF EXISTS `room`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `room` (
  `room_id` int NOT NULL AUTO_INCREMENT,
  `host_id` int NOT NULL,
  `participant_id` int NOT NULL,
  `character_host` tinyint DEFAULT NULL,
  `character_participant` tinyint DEFAULT NULL,
  `is_cleared` tinyint(1) DEFAULT '0',
  `start_date` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `clear_date` timestamp NULL DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`room_id`),
  KEY `room_host_id_idx` (`host_id`),
  KEY `room_participant_id_idx` (`participant_id`),
  CONSTRAINT `room_host_id` FOREIGN KEY (`host_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `room_participant_id` FOREIGN KEY (`participant_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(255) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `wallet_address` varchar(255) DEFAULT NULL,
  `last_stage` tinyint DEFAULT '0',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT NULL,
  `last_login` timestamp NULL DEFAULT NULL,
  `is_active` tinyint DEFAULT '1',
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `email_UNIQUE` (`email`),
  UNIQUE KEY `username_UNIQUE` (`username`),
  UNIQUE KEY `wallet_address` (`wallet_address`),
  KEY `idx_user_username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-09-20 14:36:09

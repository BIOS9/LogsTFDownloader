-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 19, 2021 at 01:34 AM
-- Server version: 10.4.21-MariaDB
-- PHP Version: 8.0.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `test`
--

-- --------------------------------------------------------

--
-- Table structure for table `logsraw`
--

CREATE TABLE `logsraw` (
  `id` int(11) NOT NULL,
  `ImportStatus` enum('ToImport','Succeeded','Failed','NotFound') NOT NULL,
  `FailureMessage` text DEFAULT NULL,
  `Hash` varbinary(32) DEFAULT NULL,
  `DuplicateId` int(11) DEFAULT NULL,
  `Time` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `logsraw`
--
ALTER TABLE `logsraw`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_logsraw_logsrawduplicates_id` (`DuplicateId`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `logsraw`
--
ALTER TABLE `logsraw`
  ADD CONSTRAINT `fk_logsraw_logsrawduplicates_id` FOREIGN KEY (`DuplicateId`) REFERENCES `logsrawduplicates` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

-- phpMyAdmin SQL Dump
-- version 4.9.7
-- https://www.phpmyadmin.net/
--
-- Хост: localhost
-- Время создания: Май 25 2025 г., 11:57
-- Версия сервера: 8.0.34-26-beget-1-1
-- Версия PHP: 5.6.40

SET NOCOUNT ON;
BEGIN TRANSACTION;


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: "ivangaog_hospita"
--

-- --------------------------------------------------------

--
-- Структура таблицы "patients"
--
-- Создание: Май 25 2025 г., 08:56
--

DROP TABLE IF EXISTS "patients";
CREATE TABLE "patients" (
  "snils" int DEFAULT NULL,
  "surname" text NOT NULL,
  "name" text NOT NULL,
  "gender" text NOT NULL,
  "age" int DEFAULT NULL,
  "diagnosis" text NOT NULL,
  "status" text NOT NULL,
  "doctor" text NOT NULL,
  "department" text NOT NULL,
  "days_in_hospital" int DEFAULT NULL
);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

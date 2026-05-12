-- ============================================================
--  Portfolio App — MySQL Schema
--  Run this ONCE to set up the database + user
-- ============================================================

CREATE DATABASE IF NOT EXISTS portfolio_db
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

CREATE USER IF NOT EXISTS 'portfolio_user'@'localhost'
  IDENTIFIED BY 'YourStrongPassword123!';

GRANT ALL PRIVILEGES ON portfolio_db.* TO 'portfolio_user'@'localhost';
FLUSH PRIVILEGES;

USE portfolio_db;

-- EF Core migrations will create the actual tables.
-- Use this file only to bootstrap the DB and user.
-- After setup, run: dotnet ef database update

-- ── Manual table definitions (alternative to EF migrations) ───────────────

CREATE TABLE IF NOT EXISTS `Projects` (
  `Id`          INT          NOT NULL AUTO_INCREMENT,
  `Title`       VARCHAR(200) NOT NULL,
  `Description` TEXT         NOT NULL,
  `TechStack`   VARCHAR(500) NOT NULL,
  `GithubUrl`   VARCHAR(500) NULL,
  `LiveUrl`     VARCHAR(500) NULL,
  `ImageUrl`    VARCHAR(500) NULL,
  `IsFeatured`  TINYINT(1)   NOT NULL DEFAULT 0,
  `CreatedAt`   DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS `Skills` (
  `Id`          INT          NOT NULL AUTO_INCREMENT,
  `Name`        VARCHAR(100) NOT NULL,
  `Category`    VARCHAR(100) NOT NULL,
  `Proficiency` INT          NOT NULL DEFAULT 50,
  `IconClass`   VARCHAR(100) NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS `Experiences` (
  `Id`          INT          NOT NULL AUTO_INCREMENT,
  `Company`     VARCHAR(200) NOT NULL,
  `Role`        VARCHAR(200) NOT NULL,
  `Description` TEXT         NOT NULL,
  `StartDate`   DATETIME(6)  NOT NULL,
  `EndDate`     DATETIME(6)  NULL,
  `IsCurrent`   TINYINT(1)   NOT NULL DEFAULT 0,
  `Location`    VARCHAR(200) NULL,
  `CompanyUrl`  VARCHAR(500) NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS `ContactMessages` (
  `Id`      INT          NOT NULL AUTO_INCREMENT,
  `Name`    VARCHAR(200) NOT NULL,
  `Email`   VARCHAR(200) NOT NULL,
  `Subject` VARCHAR(300) NOT NULL,
  `Message` TEXT         NOT NULL,
  `IsRead`  TINYINT(1)   NOT NULL DEFAULT 0,
  `SentAt`  DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

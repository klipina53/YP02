-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Апр 30 2025 г., 20:40
-- Версия сервера: 8.0.30
-- Версия PHP: 8.1.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `electronic_journal`
--

-- --------------------------------------------------------

--
-- Структура таблицы `Consultations`
--

CREATE TABLE `Consultations` (
  `ConsultationID` int NOT NULL,
  `Date` datetime NOT NULL,
  `StudentFullName` varchar(255) NOT NULL,
  `PracticeSubmitted` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `Consultations`
--

INSERT INTO `Consultations` (`ConsultationID`, `Date`, `StudentFullName`, `PracticeSubmitted`) VALUES
(1, '2025-04-24 00:00:00', 'Kbgbf eem fcefe', '2,3,4'),
(2, '2025-04-25 00:00:00', 'Липина Кристина Александровна', 'УП.02 УП.01');

-- --------------------------------------------------------

--
-- Структура таблицы `DisciplinePrograms`
--

CREATE TABLE `DisciplinePrograms` (
  `ProgramID` int NOT NULL,
  `DisciplineID` int DEFAULT NULL,
  `Topic` varchar(100) NOT NULL,
  `Type` enum('Лекция','Практика','Консультация','Курсовой проект','Экзамен') NOT NULL,
  `Hours` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `DisciplinePrograms`
--

INSERT INTO `DisciplinePrograms` (`ProgramID`, `DisciplineID`, `Topic`, `Type`, `Hours`) VALUES
(1, 1, 'Практическая работа 1', 'Практика', 4),
(3, 2, 'Практическая работа №6', 'Практика', 4),
(4, 3, 'Лекция 1', 'Лекция', 2);

-- --------------------------------------------------------

--
-- Структура таблицы `Disciplines`
--

CREATE TABLE `Disciplines` (
  `DisciplineID` int NOT NULL,
  `Name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `Disciplines`
--

INSERT INTO `Disciplines` (`DisciplineID`, `Name`) VALUES
(1, 'МДК 01.03'),
(2, 'МДК 01.04'),
(3, 'Информатика');

-- --------------------------------------------------------

--
-- Структура таблицы `Groups`
--

CREATE TABLE `Groups` (
  `GroupID` int NOT NULL,
  `Name` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `Groups`
--

INSERT INTO `Groups` (`GroupID`, `Name`) VALUES
(1, 'ИСП-21-4'),
(2, 'ИСП-21-2');

-- --------------------------------------------------------

--
-- Структура таблицы `InstructorLoads`
--

CREATE TABLE `InstructorLoads` (
  `LoadID` int NOT NULL,
  `DisciplineID` int DEFAULT NULL,
  `GroupID` int DEFAULT NULL,
  `LectureHours` int DEFAULT NULL,
  `PracticalHours` int DEFAULT NULL,
  `ConsultationHours` int DEFAULT NULL,
  `ProjectHours` int DEFAULT NULL,
  `ExamHours` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `InstructorLoads`
--

INSERT INTO `InstructorLoads` (`LoadID`, `DisciplineID`, `GroupID`, `LectureHours`, `PracticalHours`, `ConsultationHours`, `ProjectHours`, `ExamHours`) VALUES
(1, 2, 2, 12, 12, 12, 12, 12);

-- --------------------------------------------------------

--
-- Структура таблицы `Instructors`
--

CREATE TABLE `Instructors` (
  `InstructorID` int NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `Patronymic` varchar(50) DEFAULT NULL,
  `Login` varchar(50) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `Instructors`
--

INSERT INTO `Instructors` (`InstructorID`, `LastName`, `FirstName`, `Patronymic`, `Login`, `PasswordHash`) VALUES
(2, 'Ощепков', 'Александр', 'Олегович', 'pupik', 'pupik');

-- --------------------------------------------------------

--
-- Структура таблицы `Lessons`
--

CREATE TABLE `Lessons` (
  `ID` int NOT NULL,
  `Name` varchar(100) DEFAULT NULL,
  `GroupId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `TotalClasses` int DEFAULT NULL,
  `ConductedHours` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `Lessons`
--

INSERT INTO `Lessons` (`ID`, `Name`, `GroupId`, `TotalClasses`, `ConductedHours`) VALUES
(5, 'se', 'eds', 152, 23),
(6, 'МДК.01.01', 'ИСП-21-4', 6, 12);

-- --------------------------------------------------------

--
-- Структура таблицы `Marks`
--

CREATE TABLE `Marks` (
  `Id` int NOT NULL,
  `Mark` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DisciplineProgramId` int NOT NULL,
  `StudentId` int NOT NULL,
  `Description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Структура таблицы `Students`
--

CREATE TABLE `Students` (
  `StudentID` int NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `Patronymic` varchar(50) NOT NULL,
  `DismissalDate` date DEFAULT NULL,
  `GroupID` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Дамп данных таблицы `Students`
--

INSERT INTO `Students` (`StudentID`, `LastName`, `FirstName`, `Patronymic`, `DismissalDate`, `GroupID`) VALUES
(1, 'Липина', 'Кристина', 'Александровна', NULL, 1),
(2, 'Мохов', 'Артём', 'Викторович', NULL, 2),
(5, 'сав', 'всывсаы', 'ЫВФЫЦ', NULL, 1);

-- --------------------------------------------------------

--
-- Структура таблицы `Zaniyatie`
--

CREATE TABLE `Zaniyatie` (
  `Id` int NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `MinutesMissed` int NOT NULL,
  `ExplanationText` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `Consultations`
--
ALTER TABLE `Consultations`
  ADD PRIMARY KEY (`ConsultationID`);

--
-- Индексы таблицы `DisciplinePrograms`
--
ALTER TABLE `DisciplinePrograms`
  ADD PRIMARY KEY (`ProgramID`),
  ADD KEY `DisciplineID` (`DisciplineID`);

--
-- Индексы таблицы `Disciplines`
--
ALTER TABLE `Disciplines`
  ADD PRIMARY KEY (`DisciplineID`);

--
-- Индексы таблицы `Groups`
--
ALTER TABLE `Groups`
  ADD PRIMARY KEY (`GroupID`);

--
-- Индексы таблицы `InstructorLoads`
--
ALTER TABLE `InstructorLoads`
  ADD PRIMARY KEY (`LoadID`),
  ADD KEY `DisciplineID` (`DisciplineID`),
  ADD KEY `GroupID` (`GroupID`);

--
-- Индексы таблицы `Instructors`
--
ALTER TABLE `Instructors`
  ADD PRIMARY KEY (`InstructorID`),
  ADD UNIQUE KEY `Login` (`Login`);

--
-- Индексы таблицы `Lessons`
--
ALTER TABLE `Lessons`
  ADD PRIMARY KEY (`ID`);

--
-- Индексы таблицы `Marks`
--
ALTER TABLE `Marks`
  ADD PRIMARY KEY (`Id`);

--
-- Индексы таблицы `Students`
--
ALTER TABLE `Students`
  ADD PRIMARY KEY (`StudentID`);

--
-- Индексы таблицы `Zaniyatie`
--
ALTER TABLE `Zaniyatie`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `Consultations`
--
ALTER TABLE `Consultations`
  MODIFY `ConsultationID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT для таблицы `DisciplinePrograms`
--
ALTER TABLE `DisciplinePrograms`
  MODIFY `ProgramID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `Disciplines`
--
ALTER TABLE `Disciplines`
  MODIFY `DisciplineID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT для таблицы `Groups`
--
ALTER TABLE `Groups`
  MODIFY `GroupID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT для таблицы `InstructorLoads`
--
ALTER TABLE `InstructorLoads`
  MODIFY `LoadID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT для таблицы `Instructors`
--
ALTER TABLE `Instructors`
  MODIFY `InstructorID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT для таблицы `Lessons`
--
ALTER TABLE `Lessons`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT для таблицы `Marks`
--
ALTER TABLE `Marks`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT для таблицы `Students`
--
ALTER TABLE `Students`
  MODIFY `StudentID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `Zaniyatie`
--
ALTER TABLE `Zaniyatie`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `DisciplinePrograms`
--
ALTER TABLE `DisciplinePrograms`
  ADD CONSTRAINT `disciplineprograms_ibfk_1` FOREIGN KEY (`DisciplineID`) REFERENCES `Disciplines` (`DisciplineID`);

--
-- Ограничения внешнего ключа таблицы `InstructorLoads`
--
ALTER TABLE `InstructorLoads`
  ADD CONSTRAINT `instructorloads_ibfk_1` FOREIGN KEY (`DisciplineID`) REFERENCES `Disciplines` (`DisciplineID`),
  ADD CONSTRAINT `instructorloads_ibfk_2` FOREIGN KEY (`GroupID`) REFERENCES `Groups` (`GroupID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

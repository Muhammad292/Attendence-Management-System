create database sample;
use sample;


-- Step 2: Create the Student table
CREATE TABLE Students (
    StudentID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
	RegNo NVARCHAR(50) NOT NULL,
	ProfileImage VARBINARY(MAX) NUll ,
    Password NVARCHAR(50) NOT NULL
);
GO
ALTER TABLE Students
ADD CONSTRAINT DF_ProfileImage DEFAULT 
    0xFFD8FFE000104A46494600010100000100010000FFDB0043000B090907090907090909090B0909090909090B090B0B0C0B0B0B0C0D100C110E0D0E0C121912251A1D251D191F1C292916253735361A2A323E2D2930193B2113FFDB0043010708080B090B150B0B152C1D191D2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2CFFC000110800BC00BC03012200021101031101FFC4001C0001000300030101000000000000000000000607080204050103
FOR ProfileImage;

ALTER TABLE Students
ALTER COLUMN ProfileImage VARBINARY(MAX) NULL;

CREATE TABLE Challan (
    ChallanID INT PRIMARY KEY IDENTITY(1,1),
    StudentID INT NOT NULL,
    StudentName NVARCHAR(100) NOT NULL,
    CourseID INT NOT NULL,
    CourseName NVARCHAR(100) NOT NULL,
    TeacherID INT NOT NULL,
    TeacherName NVARCHAR(100) NOT NULL,
    Price INT NOT NULL,
	ChallanStatus NVARCHAR(50) NOT NULL DEFAULT 'Unpaid', 
    EnrollmentDate DATE NOT NULL
);
GO

CREATE OR ALTER TRIGGER trg_PopulateChallan
ON Enrollments
AFTER INSERT
AS
BEGIN
    INSERT INTO Challan (StudentID, StudentName, CourseID, CourseName, TeacherID, TeacherName, Price, EnrollmentDate, ChallanStatus)
    SELECT 
        e.StudentID,
        s.Name AS StudentName,
        e.CourseID,
        c.Name AS CourseName,
        c.TeacherID,
        t.Name AS TeacherName,
        c.Price,
        e.EnrollmentDate,
        'Unpaid' AS ChallanStatus
    FROM 
        Inserted e
    INNER JOIN 
        Students s ON e.StudentID = s.StudentID
    INNER JOIN 
        Courses c ON e.CourseID = c.CourseID
    INNER JOIN 
        Teachers t ON c.TeacherID = t.TeacherID;
END;
GO

ALTER TABLE Teachers
ADD CONSTRAINT DF_TtProfileImage DEFAULT 
    0xFFD8FFE000104A46494600010100000100010000FFDB0043000B090907090907090909090B0909090909090B090B0B0C0B0B0B0C0D100C110E0D0E0C121912251A1D251D191F1C292916253735361A2A323E2D2930193B2113FFDB0043010708080B090B150B0B152C1D191D2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2C2CFFC000110800BC00BC03012200021101031101FFC4001C0001000300030101000000000000000000000607080204050103
FOR ProfileImage;

-- Step 3: Create the Teacher table
CREATE TABLE Teachers (
    TeacherID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
	ProfileImage VARBINARY(MAX),
	Role NVARCHAR(50) NOT NULL ,
    Password NVARCHAR(50) NOT NULL
);
GO

-- Step 4: Create the Course table
CREATE TABLE Courses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    TeacherID INT NOT NULL,
	Price INT NOT NULL DEFAULT 0 CHECK (Price >= 0),
    TotalStudentLimit INT NOT NULL DEFAULT 1 CHECK (TotalStudentLimit >= 1),
    FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID) ON DELETE CASCADE
);
GO

-- Step 5: Create the Enrollment table
CREATE TABLE Enrollments (
    CourseID INT NOT NULL,
    StudentID INT NOT NULL,
    EnrollmentDate DATE NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY (CourseID, StudentID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID) ON DELETE CASCADE,
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID) ON DELETE CASCADE
);
GO
---- Drop the existing Attendance table if it exists
--IF OBJECT_ID('Attendance', 'U') IS NOT NULL
--    DROP TABLE Attendance;
--GO
-- Step 6: Create the Attendance table
CREATE TABLE Attendance (
    AttendanceID INT PRIMARY KEY IDENTITY(1,1),   
    StudentID INT NOT NULL,
    CourseID INT NOT NULL, 
    AttendanceDate DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    StudentName NVARCHAR(100), -- New column for Student Name
    TeacherName NVARCHAR(100), -- New column for Teacher Name
    CourseName NVARCHAR(100), -- New column for Course Name   
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID) ON DELETE cascade,
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID) ON DELETE  cascade 
);

-- Create a trigger to insert or update StudentName, CourseName, and TeacherName in Attendance table
CREATE TRIGGER trg_InsertUpdateAttendance
ON Attendance
AFTER INSERT, UPDATE
AS
BEGIN
    -- Declare variables to hold the names
    DECLARE @StudentName NVARCHAR(100), @CourseName NVARCHAR(100), @TeacherName NVARCHAR(100);

    -- Get the StudentName based on StudentID
    SELECT @StudentName = Name FROM Students WHERE StudentID IN (SELECT StudentID FROM inserted);

    -- Get the CourseName based on CourseID
    SELECT @CourseName = Name FROM Courses WHERE CourseID IN (SELECT CourseID FROM inserted);

    -- Get the TeacherName based on TeacherID of the Course
    SELECT @TeacherName = T.Name
    FROM Teachers T
    JOIN Courses C ON T.TeacherID = C.TeacherID
    WHERE C.CourseID IN (SELECT CourseID FROM inserted);

    -- Update the Attendance table with the student name, course name, and teacher name
    UPDATE Attendance
    SET 
        StudentName = @StudentName,
        CourseName = @CourseName,
        TeacherName = @TeacherName
    FROM Attendance A
    JOIN inserted I ON A.AttendanceID = I.AttendanceID;
END;
GO


--drop table Attendance

-- Step 7: Populate the tables with sample data

-- Insert data into Students table
INSERT INTO Students (Name, RegNo , Email, Password)
VALUES 
('Bob Smith','y', 'bob.smith@student.com', '456'),
('Charlie Brown','o', 'charlie.brown@student.com', '789');
GO

-- Insert data into Teachers table
--INSERT INTO Teachers (Name, Email, Password, Role)
--VALUES 
--('Dr. John Doe', 'john.doe@teacher.com', '123','i'),
--('Dr. Jane Roe', 'jane.roe@teacher.com', '456','j');
--GO

INSERT INTO Teachers (Name, Email, Password, Role)
VALUES 
('Danish', 'Danish@teacher.com', '123','admin'),
('Usman', 'Usman@teacher.com', '456','teacher');
GO

-- Insert data into Courses table
INSERT INTO Courses (Name, TeacherID, TotalStudentLimit)
VALUES 
('Mathematics', 3, 30),
('Physics', 3, 25),
('Chemistry', 2, 20);
GO

-- Insert data into Enrollments table

INSERT INTO Enrollments (CourseID, StudentID, EnrollmentDate)
VALUES 
(3, 1, GETDATE()),
(3, 2, GETDATE()),
(4, 1, GETDATE()),
(4, 2, GETDATE()),
(5, 1, GETDATE())

GO

-- Insert data into Attendance table
INSERT INTO Attendance ( StudentID, CourseID, AttendanceDate, Status)
VALUES 
--( 1, 1, GETDATE(), 'Present'),
--( 2, 1, GETDATE(), 'Present'),
--( 2, 3, GETDATE(), 'Present'),
--( 1, 2, GETDATE(), 'Absent'),
( 2, 7, GETDATE(), 'Present');
GO


SELECT * FROM Attendance;
SELECT * FROM Enrollments;
select * from Challan;

select * from Courses;

select * from Teachers;

select * from Students;

DELETE FROM Students WHERE StudentID=7;
DELETE FROM Teachers WHERE TeacherID=1
--DELETE FROM Courses WHERE CourseID=1





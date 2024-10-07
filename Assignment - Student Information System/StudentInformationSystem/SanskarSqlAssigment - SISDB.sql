--***** TASK - 1: Database Design *****

--CREATING DATABASE SCHEMA 

Create DATABASE SISDB;
USE SISDB;

--Creating Tables and populating them

-- #STUDENTS TABLE
CREATE TABLE Students(
    student_id INT NOT NULL PRIMARY KEY,               
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) ,
    date_of_birth DATE NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    phone_number VARCHAR(15) NOT NULL UNIQUE
);
-- #TEACHER TABLE
CREATE TABLE Teacher(
    teacher_id INT NOT NULL PRIMARY KEY,                
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) ,
    email VARCHAR(100) NOT NULL UNIQUE
);
-- #PAYMENTS TABLE
CREATE TABLE Payments(
    payment_id INT PRIMARY KEY,                
    student_id INT NOT NULL,                            
    amount INT NOT NULL,
    payment_date DATE NOT NULL,
    CONSTRAINT FK_Student_Payments FOREIGN KEY (student_id) 
        REFERENCES Students(student_id)
        ON DELETE CASCADE ON UPDATE CASCADE    
);
-- #COURSES TABLE
CREATE TABLE Courses(
    course_id INT PRIMARY KEY,                 
    course_name VARCHAR(50) NOT NULL,
    credits INT NOT NULL,
    teacher_id INT ,                            
    CONSTRAINT FK_Teacher_Courses FOREIGN KEY (teacher_id) 
        REFERENCES Teacher(teacher_id)
        ON DELETE SET NULL ON UPDATE CASCADE  
);
-- #ENROLLMENTS TABLE
CREATE TABLE Enrollments(
    enrollment_id INT PRIMARY KEY,             
    student_id INT,                            
    course_id INT,                             
    enrollment_date DATE NOT NULL,
    CONSTRAINT FK_Student_Enrollments FOREIGN KEY (student_id) 
        REFERENCES Students(student_id)
        ON DELETE CASCADE ON UPDATE CASCADE,   
    CONSTRAINT FK_Course_Enrollments FOREIGN KEY (course_id) 
        REFERENCES Courses(course_id)
        ON DELETE CASCADE ON UPDATE CASCADE    
);

--INSERTING 10 SAMPLE VALUES IN EACH TABLE (i.e, populating the empty schema)

--Adding Data To Students Table
INSERT INTO Students (student_id, first_name, last_name, date_of_birth, email, phone_number) VALUES
(1, 'Sanskar', 'Kakde', '2002-02-14', 'sanskarkakde13@gmail.com', '6267609084'),
(2, 'Priya', 'Verma', '1999-11-22', 'priya.verma@yahoo.com', '9823456789'),
(3, 'Karan', 'Mehta', '2001-01-17', 'karan.mehta@outlook.com', '9934567891'),
(4, 'Aisha', 'Khan', '2000-03-25', 'aisha.khan@gmail.com', '9845671234'),
(5, 'Vikas', 'Patel', '1998-07-07', 'vikas.patel@gmail.com', '9876512345'),
(6, 'Neha', 'Singh', '2001-09-09', 'neha.singh@yahoo.com', '9812345678'),
(7, 'Sanjay', 'Reddy', '1999-10-30', 'sanjay.reddy@outlook.com', '9823456710'),
(8, 'Pooja', 'Desai', '2000-12-18', 'pooja.desai@gmail.com', '9876543120'),
(9, 'Amit', 'Bose', '2001-06-22', 'amit.bose@yahoo.com', '9812456789'),
(10, 'Raj', 'Kapoor', '1998-08-15', 'raj.kapoor@gmail.com', '9876512346');


--Adding Data To Teacher Table
INSERT INTO Teacher (teacher_id, first_name, last_name, email) VALUES
(1, 'Anil', 'Kumar', 'anil@svvv.edu.in'),
(2, 'Sunita', 'Gupta', 'sunita.gupta@vit.edu'),
(3, 'Rahul', 'Saxena', 'rahul.saxena@IIT.edu'),
(4, 'Geeta', 'Chopra', 'geeta.chopra@school.edu'),
(5, 'Ajay', 'Mishra', 'ajay.mishra@harvards.edu'),
(6, 'Neeraj', 'Pandey', 'neeraj.pandey@school.edu'),
(7, 'Sarika', 'Joshi', 'sarika.joshi@lnct.edu'),
(8, 'Vikram', 'Thakur', 'vikram.thakur@school.edu'),
(9, 'Renu', 'Singh', 'renu.singh@MIT.edu'),
(10, 'Ashok', 'Nair', 'ashok.nair@school.edu');

--Adding Data To Payments Table
INSERT INTO Payments (payment_id, student_id, amount, payment_date) VALUES
(1, 1, 15000, '2024-01-10'),
(2, 2, 18000, '2024-02-14'),
(3, 3, 12000, '2024-03-01'),
(4, 4, 13000, '2024-04-15'),
(5, 5, 14000, '2024-05-10'),
(6, 6, 15000, '2024-06-18'),
(7, 7, 16000, '2024-07-12'),
(8, 8, 12000, '2024-08-01'),
(9, 9, 11000, '2024-09-05'),
(10, 10, 17000, '2024-10-02');

--Adding Data To Courses Table
INSERT INTO Courses (course_id, course_name, credits, teacher_id) VALUES
(1, 'Mathematics', 4, 1),
(2, 'Physics', 3, 2),
(3, 'Chemistry', 3, 3),
(4, 'Biology', 4, 4),
(5, 'Computer Science', 5, 5),
(6, 'English', 2, 6),
(7, 'History', 3, 7),
(8, 'Geography', 2, 8),
(9, 'Political Science', 3, 9),
(10, 'Economics', 4, 10);

--Adding Data To Enrollments Table
INSERT INTO Enrollments (enrollment_id, student_id, course_id, enrollment_date) VALUES
(1, 1, 1, '2024-01-15'),
(2, 2, 2, '2024-02-18'),
(3, 3, 3, '2024-03-02'),
(4, 4, 4, '2024-04-16'),
(5, 5, 5, '2024-05-11'),
(6, 6, 6, '2024-06-19'),
(7, 7, 7, '2024-07-13'),
(8, 8, 8, '2024-08-02'),
(9, 9, 9, '2024-09-06'),
(10, 10, 10, '2024-10-03');

-----------------------------------------------------------------
--***** TASK - 2: Select, Where, Between, and Like *****

-- 1. Write an SQL query to insert a new student into the "Students" table with the following details:
INSERT INTO Students VALUES(11,'TONY','stark','1995-08-15','mark42@ironmail.com','1234567890')

-- 2. Write an SQL query to enroll a student in a course. Choose an existing student and course and insert a record into the "Enrollments" table with the enrollment date
INSERT INTO Enrollments VALUES(11, 1, 5, '2024-09-19');

-- 3. Update the email address of a specific teacher in the "Teacher" table. Choose any teacher and modify their email address.
UPDATE Teacher
SET email = 'facultyrahul@DPS.edu'
WHERE teacher_id = 3;

-- 4. Write an SQL query to delete a specific enrollment record from the "Enrollments" table. Select an enrollment record based on the student and course.
DELETE FROM Enrollments
WHERE student_id = 5 AND course_id = 5;

-- 5. Update the "Courses" table to assign a specific teacher to a course. Choose any course and teacher from the respective tables.
UPDATE Courses
SET teacher_id=5
WHERE course_id=8

-- 6. Delete a specific student from the "Students" table and remove all their enrollment records from the "Enrollments" table. Be sure to maintain referential integrity.
DELETE FROM Students
WHERE student_id=2

-- 7. Update the payment amount for a specific payment record in the "Payments" table. Choose any payment record and modify the payment amount.
UPDATE Payments
SET amount=20000
WHERE payment_id=1

-----------------------------------------------------------------

--***** Task - 3: Aggregate functions, Having, Order By, GroupBy and Joins *****

-- 1. Write an SQL query to calculate the total payments made by a specific student. You will need to join the "Payments" table with the "Students" table based on the student's ID
SELECT S.student_id, S.first_name, S.last_name, SUM(P.amount) AS total_payments
FROM Students S
INNER JOIN Payments P ON S.student_id = P.student_id
WHERE S.student_id = 5 
GROUP BY S.student_id, S.first_name, S.last_name;

-- 2. Write an SQL query to retrieve a list of courses along with the count of students enrolled in each course. Use a JOIN operation between the "Courses" table and the "Enrollments" table.
SELECT C.course_id, C.course_name,COUNT(E.student_id) AS enrolled_students_count
FROM Courses C
LEFT JOIN Enrollments E ON C.course_id = E.course_id
GROUP BY C.course_id, C.course_name;
   
-- 3. Write an SQL query to find the names of students who have not enrolled in any course. Use a LEFT JOIN between the "Students" table and the "Enrollments" table to identify students without enrollments.
SELECT S.first_name,S.last_name
FROM Students S
LEFT JOIN Enrollments E ON S.student_id = E.student_id
WHERE E.course_id IS NULL;

-- 4. Write an SQL query to retrieve the first name, last name of students, and the names of the courses they are enrolled in. Use JOIN operations between the "Students" table and the "Enrollments" and "Courses" tables.
SELECT S.first_name,S.last_name,C.course_name
FROM Students S
INNER JOIN Enrollments E ON S.student_id = E.student_id
INNER JOIN Courses C ON E.course_id = C.course_id;

-- 5. Create a query to list the names of teachers and the courses they are assigned to. Join the "Teacher" table with the "Courses" table.
SELECT T.first_name AS TeacherName, C.course_name AS CourseName 
From Teacher T
INNER JOIN Courses C ON T.teacher_id=C.course_id;

-- 6. Retrieve a list of students and their enrollment dates for a specific course. You'll need to join the "Students" table with the "Enrollments" and "Courses" tables.
SELECT S.first_name AS StudentName, E.enrollment_date, C.course_name
FROM Students S
INNER JOIN Enrollments E ON S.student_id=E.enrollment_id
INNER JOIN Courses C on E.enrollment_id=C.course_id 
WHERE C.course_name='Chemistry';

-- 7. Find the names of students who have not made any payments. Use a LEFT JOIN between the "Students" table and the "Payments" table and filter for students with NULL payment records.
SELECT S.student_id, S.first_name, S.last_name
FROM Students S
LEFT JOIN Payments P ON S.student_id = P.student_id
WHERE P.payment_id IS NULL;

-- 8. Write a query to identify courses that have no enrollments. You'll need to use a LEFT JOIN between the "Courses" table and the "Enrollments" table and filter for courses with NULL enrollment records.
SELECT C.course_id, C.course_name
FROM Courses C
LEFT JOIN Enrollments E ON C.course_id = E.course_id
WHERE E.enrollment_id IS NULL;

-- 9. Identify students who are enrolled in more than one course. Use a self-join on the "Enrollments" table to find students with multiple enrollment records.
SELECT S.first_name, S.last_name, COUNT(E.course_id) AS course_count
FROM Students S
INNER JOIN Enrollments E ON S.student_id = E.student_id
GROUP BY S.student_id, S.first_name, S.last_name
HAVING COUNT(E.course_id) > 1;

-- 10. Find teachers who are not assigned to any courses. Use a LEFT JOIN between the "Teacher" table and the "Courses" table and filter for teachers with NULL course assignments.
SELECT T.first_name, T.last_name
FROM Teacher T
LEFT JOIN Courses C ON T.teacher_id = C.teacher_id
WHERE C.course_id IS NULL;

------------------------------------------------------------------------

--***** Task - 4: Subquery and its type *****

-- 1. Write an SQL query to calculate the average number of students enrolled in each course. Use aggregate functions and subqueries to achieve this.
SELECT AVG(student_count) AS avg_students_per_course
FROM (SELECT course_id, COUNT(student_id) AS student_count FROM Enrollments GROUP BY course_id) AS course_enrollments;

-- 2. Identify the student(s) who made the highest payment. Use a subquery to find the maximum payment amount and then retrieve the student(s) associated with that amount.
SELECT S.student_id, S.first_name, S.last_name, P.amount
FROM Students S
INNER JOIN Payments P ON S.student_id = P.student_id
WHERE P.amount = (SELECT MAX(amount) FROM Payments);

-- 3. Retrieve a list of courses with the highest number of enrollments. Use subqueries to find the course(s) with the maximum enrollment count.
SELECT C.course_id, C.course_name, COUNT(E.student_id) AS enrollment_count
FROM Courses C
INNER JOIN Enrollments E ON C.course_id = E.course_id
GROUP BY C.course_id, C.course_name
HAVING COUNT(E.student_id) = (
    SELECT MAX(enrollment_count)
    FROM (SELECT COUNT(student_id) AS enrollment_count FROM Enrollments GROUP BY course_id) AS MaxEnrollments);

-- 4. Calculate the total payments made to courses taught by each teacher. Use subqueries to sum payments for each teacher's courses.
SELECT T.teacher_id, T.first_name, T.last_name, 
       (SELECT SUM(P.amount)
        FROM Payments P
        INNER JOIN Enrollments E ON P.student_id = E.student_id
        INNER JOIN Courses C ON E.course_id = C.course_id
        WHERE C.teacher_id = T.teacher_id
       ) AS total_payments FROM Teacher T;

-- 5. Identify students who are enrolled in all available courses. Use subqueries to compare a student's enrollments with the total number of courses.
SELECT S.first_name, S.last_name
FROM Students S
WHERE (SELECT COUNT(E.course_id) 
FROM Enrollments E 
WHERE E.student_id = S.student_id) = (SELECT COUNT(*) FROM Courses);

-- 6. Retrieve the names of teachers who have not been assigned to any courses. Use subqueries to find teachers with no course assignments.
SELECT T.first_name, T.last_name
FROM Teacher T
WHERE T.teacher_id NOT IN (SELECT C.teacher_id FROM Courses C);

-- 7. Calculate the average age of all students. Use subqueries to calculate the age of each student based on their date of birth.
SELECT AVG(age) AS average_age
FROM ( SELECT FLOOR(DATEDIFF(YEAR, date_of_birth, GETDATE())) AS age FROM Students) AS AgeCalculation;

-- 8. Identify courses with no enrollments. Use subqueries to find courses without enrollment records.
SELECT course_name
FROM Courses
WHERE course_id NOT IN (SELECT DISTINCT course_id FROM Enrollments);

-- 9. Calculate the total payments made by each student for each course they are enrolled in. Use subqueries and aggregate functions to sum payments.
SELECT S.student_id, S.first_name, S.last_name, C.course_name, 
       (SELECT SUM(P.amount) 
        FROM Payments P 
        INNER JOIN Enrollments E2 ON P.student_id = E2.student_id 
        WHERE E2.course_id = C.course_id AND E2.student_id = S.student_id) AS total_payments
FROM Students S
INNER JOIN Enrollments E ON S.student_id = E.student_id
INNER JOIN Courses C ON E.course_id = C.course_id
ORDER BY S.student_id, C.course_name;

-- 10. Identify students who have made more than one payment. Use subqueries and aggregate functions to count payments per student and filter for those with counts greater than one.
SELECT S.student_id, S.first_name, S.last_name
FROM Students S
WHERE (SELECT COUNT(*) FROM Payments P WHERE P.student_id = S.student_id) > 1;

-- 11. Write an SQL query to calculate the total payments made by each student. Join the "Students" table with the "Payments" table and use GROUP BY to calculate the sum of payments for each student.
SELECT S.student_id, S.first_name, S.last_name, 
(SELECT SUM(P.amount) FROM Payments P WHERE P.student_id = S.student_id) AS total_payments
FROM Students S;

-- 12. Retrieve a list of course names along with the count of students enrolled in each course. Use JOIN operations between the "Courses" table and the "Enrollments" table and GROUP BY to count enrollments.
SELECT C.course_name, (SELECT COUNT(E.student_id) FROM Enrollments E WHERE E.course_id = C.course_id) AS student_count
FROM Courses C;

-- 13. Calculate the average payment amount made by students. Use JOIN operations between the "Students" table and the "Payments" table and GROUP BY to calculate the average.
SELECT S.student_id, S.first_name, S.last_name, AVG(P.amount) AS average_payment
FROM Students S
JOIN Payments P ON S.student_id = P.student_id
GROUP BY S.student_id, S.first_name, S.last_name;

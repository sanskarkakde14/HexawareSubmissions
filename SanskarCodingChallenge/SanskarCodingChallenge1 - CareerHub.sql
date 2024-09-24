--Creating And Initializing The DB

CREATE DATABASE CareerHub;
USE CareerHub;

--Laying Down The Schema

-- Table: Companies
CREATE TABLE Companies (
    CompanyID INT PRIMARY KEY IDENTITY(1,1),
    CompanyName VARCHAR(255) NOT NULL,
    Location VARCHAR(255) NOT NULL
);

-- Table: Jobs
CREATE TABLE Jobs (
    JobID INT PRIMARY KEY IDENTITY(1,1),
    CompanyID INT NOT NULL,
    JobTitle VARCHAR(255) NOT NULL,
    JobDescription TEXT,
    JobLocation VARCHAR(255) NOT NULL,
    Salary DECIMAL(18, 2),
    JobType VARCHAR(50) NOT NULL,
    PostedDate DATETIME NOT NULL,
    CONSTRAINT FK_Jobs_Companies FOREIGN KEY (CompanyID) 
        REFERENCES Companies(CompanyID) 
        ON UPDATE CASCADE 
        ON DELETE CASCADE
);

-- Table: Applicants
CREATE TABLE Applicants (
    ApplicantID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(50) NOT NULL UNIQUE,
    Phone VARCHAR(15) NOT NULL UNIQUE,
    Resume TEXT,
    Experience INT,
    City VARCHAR(100),
    State VARCHAR(100)
);

-- Table: Applications
CREATE TABLE Applications (
    ApplicationID INT PRIMARY KEY IDENTITY(1,1),
    JobID INT NOT NULL,
    ApplicantID INT NOT NULL,
    ApplicationDate DATETIME NOT NULL,
    CoverLetter TEXT,
    CONSTRAINT FK_Applications_Jobs FOREIGN KEY (JobID) 
        REFERENCES Jobs(JobID) 
        ON UPDATE CASCADE 
        ON DELETE CASCADE,
    CONSTRAINT FK_Applications_Applicants FOREIGN KEY (ApplicantID) 
        REFERENCES Applicants(ApplicantID) 
        ON UPDATE CASCADE 
        ON DELETE CASCADE
);

--Populating The Schema With Values

-- Insert into Companies
INSERT INTO Companies (CompanyName, Location)
VALUES 
('Hexaware', 'Pune'),
('Microsoft', 'Bangalore'),
('At&t', 'London'),
('JIO', 'Mumbai'),
('Rakuten', 'Tokyo'),
('Nike', 'USA');

-- Insert into Jobs
INSERT INTO Jobs (CompanyID, JobTitle, JobDescription, JobLocation, Salary, JobType, PostedDate) VALUES
(1, 'Software Engineer', 'Develop and maintain software applications.', 'Mumbai', 700000.00, 'Full-time', '2023-09-01'),
(2, 'Data Analyst', 'Analyze datasets to uncover insights.', 'Bangalore', 550000.00, 'Full-time', '2023-08-25'),
(3, 'UI/UX Designer', 'Design user interfaces and experiences.', 'Hyderabad', 500000.00, 'Contract', '2023-08-15'),
(4, 'DevOps Engineer', 'Implement and maintain CI/CD pipelines.', 'Noida', 800000.00, 'Full-time', '2023-07-30'),
(5, 'Cloud Architect', 'Design scalable cloud architectures.', 'Pune', 950000.00, 'Full-time', '2023-09-05'),
(6, 'Business Analyst', 'Analyze business operations and suggest improvements.', 'Gurgaon', 600000.00, 'Full-time', '2023-09-10');

-- Insert into Applicants
INSERT INTO Applicants (FirstName, LastName, Email, Phone, Resume) VALUES
('Sanskar', 'Kakde', 'sanskarkakde13@gmail.com', '6267609084', 'Experienced software engineer with expertise in Cloud and Python.'),
('Priya', 'Kumar', 'priya.kumar@outlook.com', '9876543211', 'Skilled data analyst with 3 years of experience in SQL and Excel.'),
('Amit', 'Gupta', 'amit.gupta@wipro.com', '9876543212', 'Creative UI/UX designer with a focus on mobile apps.'),
('Sneha', 'Mehta', 'sneha.mehta@yahoo.com', '9876543213', 'Experienced DevOps engineer with hands-on experience in AWS.'),
('Vikram', 'Patel', 'vikram.patel@infosys.com', '9876543214', 'Certified cloud architect with expertise in Azure and GCP.'),
('Sunita', 'Reddy', 'sunita.reddy@gmail.com', '9876543215', 'Business analyst with strong analytical skills and domain expertise.');

-- Insert into Applications
INSERT INTO Applications (JobID, ApplicantID, ApplicationDate, CoverLetter) VALUES
(1, 1, '2023-09-02', 'I am excited about the opportunity to work as a Software Engineer.'),
(2, 2, '2023-08-26', 'I would love to apply my data analysis skills at your company.'),
(3, 3, '2023-08-16', 'I have a passion for UI/UX design and would be a great fit.'),
(4, 4, '2023-07-31', 'My experience in DevOps makes me an ideal candidate for this role.'),
(5, 5, '2023-09-06', 'I have extensive experience as a cloud architect.'),
(6, 6, '2023-09-11', 'I have strong skills in business analysis and would be excited to join your team.');

-- 5. Write an SQL query to count the number of applications received for each job listing in the "Jobs" table. Display the job title and the corresponding application count. Ensure that it lists all jobs, even if they have no applications.
SELECT j.JobTitle,COUNT(a.ApplicationID) AS ApplicationCount
FROM Jobs j
LEFT JOIN Applications a ON j.JobID = a.JobID
GROUP BY j.JobTitle
ORDER BY ApplicationCount DESC;

-- 6. Develop an SQL query that retrieves job listings from the "Jobs" table within a specified salary range. Allow parameters for the minimum and maximum salary values. Display the job title, company name, location, and salary for each matching job.
DECLARE @MinSalary DECIMAL(10, 2) = 700000.00;  
DECLARE @MaxSalary DECIMAL(10, 2) = 1000000.00;
SELECT j.JobTitle,c.CompanyName,j.JobLocation,j.Salary
FROM Jobs j
JOIN Companies c ON j.CompanyID = c.CompanyID
WHERE j.Salary BETWEEN @MinSalary AND @MaxSalary;

-- 7. Write an SQL query that retrieves the job application history for a specific applicant. Allow a parameter for the ApplicantID, and return a result set with the job titles, company names, and application dates for all the jobs the applicant has applied to.
DECLARE @ApplicantID INT = 1;  
SELECT j.JobTitle,c.CompanyName,a.ApplicationDate
FROM Applications a
JOIN Jobs j ON a.JobID = j.JobID
JOIN Companies c ON j.CompanyID = c.CompanyID
WHERE a.ApplicantID = @ApplicantID;

-- 8. Create an SQL query that calculates and displays the average salary offered by all companies for job listings in the "Jobs" table. Ensure that the query filters out jobs with a salary of zero
SELECT c.CompanyName, AVG(j.Salary) AS AverageSalary
FROM Companies c
INNER JOIN Jobs j ON c.CompanyID = j.CompanyID
WHERE j.Salary > 0
GROUP BY c.CompanyName
HAVING AVG(j.Salary) > 0;

-- 9. Write an SQL query to identify the company that has posted the most job listings. Display the company name along with the count of job listings they have posted. Handle ties if multiple companies have the same maximum count.
SELECT c.CompanyName, COUNT(j.JobID) AS JobCount
FROM Companies c
LEFT JOIN Jobs j ON c.CompanyID = j.CompanyID
GROUP BY c.CompanyName
HAVING COUNT(j.JobID) = (SELECT MAX(JobCount) FROM (SELECT COUNT(JobID) AS JobCount FROM Jobs GROUP BY CompanyID) AS CompanyJobCounts
);

-- 10. Find the applicants who have applied for positions in companies located in 'CityX' and have at least 3 years of experience.
SELECT a.FirstName, a.LastName
FROM Applicants a
JOIN Applications ap ON a.ApplicantID = ap.ApplicantID
JOIN Jobs j ON ap.JobID = j.JobID
JOIN Companies c ON j.CompanyID = c.CompanyID
WHERE c.Location = 'Pune' AND a.Experience >= 3;

-- 11. Retrieve a list of distinct job titles with salaries between $60,000 and $80,000
SELECT DISTINCT JobTitle, Salary FROM JOBS WHERE SALARY >=60000 AND SALARY<=80000;

-- 12.  Find the jobs that have not received any applications.
SELECT j.*
FROM Jobs j
LEFT JOIN Applications a ON j.JobID = a.JobID
WHERE a.ApplicationID IS NULL;

-- 13. Retrieve a list of job applicants along with the companies they have applied to and the positions they have applied for.
SELECT a.FirstName, a.LastName, c.CompanyName, j.JobTitle
FROM Applicants a
INNER JOIN Applications ap ON a.ApplicantID = ap.ApplicantID
INNER JOIN Jobs j ON ap.JobID = j.JobID
INNER JOIN Companies c ON j.CompanyID = c.CompanyID;

-- 14. Retrieve a list of companies along with the count of jobs they have posted, even if they have not received any applications.
SELECT c.CompanyName, COUNT(j.JobID) AS JobCount
FROM Companies c
LEFT JOIN Jobs j ON c.CompanyID = j.CompanyID
GROUP BY c.CompanyName;

-- 15. List all applicants along with the companies and positions they have applied for, including those who have not applied.
SELECT a.FirstName, a.LastName, c.CompanyName, j.JobTitle
FROM Applicants a
LEFT JOIN Applications ap ON a.ApplicantID = ap.ApplicantID
LEFT JOIN Jobs j ON ap.JobID = j.JobID
LEFT JOIN Companies c ON j.CompanyID = c.CompanyID;

-- 16. Find companies that have posted jobs with a salary higher than the average salary of all jobs:
SELECT DISTINCT c.CompanyName, j.salary
FROM Companies c
JOIN Jobs j ON c.CompanyID = j.CompanyID
WHERE j.Salary > (SELECT AVG(Salary) FROM Jobs);

-- 17. Display a list of applicants with their names and a concatenated string of their city and state.
SELECT FirstName, LastName, CONCAT(City, ', ', State) AS Location
FROM Applicants;

-- 18. Retrieve a list of jobs with titles containing either 'Developer' or 'Engineer'.
SELECT JobTitle
FROM Jobs
WHERE JobTitle LIKE '%Developer%' OR JobTitle LIKE '%Engineer%';

-- 19. Retrieve a list of applicants and the jobs they have applied for, including those who have not applied and jobs without applicants.
SELECT a.ApplicantID, a.FirstName, a.LastName, j.JobID, j.JobTitle
FROM Applicants AS a
FULL OUTER JOIN Applications AS a1 ON a.ApplicantID= a1.ApplicantID
FULL OUTER JOIN Jobs AS j ON a1.JobID = j.JobID

-- 20. List all combinations of applicants and companies where the company is in a specific city and the applicant has more than 2 years of experience. For example: city=Chennai
SELECT a. FirstName, a. LastName, c. CompanyName
FROM Applicants a
JOIN Applications ap ON a.ApplicantID = ap.ApplicantID
JOIN Jobs j ON ap.JobID = j.JobID
JOIN Companies c ON j. CompanyID = c. CompanyID
WHERE c.Location='Pune' AND a.experience > 2;

using System;
namespace StudentInformationSystem.Model
{
    internal class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Payment> Payments { get; set; }

        public Student(int studentId, string firstName, string lastName, DateTime dob, string email, string phoneNumber)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dob;
            Email = email;
            PhoneNumber = phoneNumber;
            Enrollments = new List<Enrollment>();
            Payments = new List<Payment>();
        }
        public Student( string firstName, string lastName, DateTime dob, string email, string phoneNumber, int studentId=0)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dob;
            Email = email;
            PhoneNumber = phoneNumber;
            Enrollments = new List<Enrollment>();
            Payments = new List<Payment>();
        }
        public Student(int studentId, string firstName, string lastName)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
        }
        public override string ToString()
        {
            return $"Student ID: {StudentId}, Name: {FirstName} {LastName}, Email: {Email}, Phone: {PhoneNumber}";
        }
    }

}


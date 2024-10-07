using System;
namespace StudentInformationSystem.Model
{
    internal class Teacher
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Course> AssignedCourses { get; set; }

        public Teacher(int teacherId, string firstName, string lastName, string email)
        {
            TeacherId = teacherId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            AssignedCourses = new List<Course>();
        }
        //public Teacher() { }
        public override string ToString()
        {
            return $"Teacher ID: {TeacherId}, Name: {FirstName} {LastName}, Email: {Email}";
        }
    }

}


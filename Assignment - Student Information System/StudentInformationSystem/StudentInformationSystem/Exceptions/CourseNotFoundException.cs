using System;
namespace StudentInformationSystem.Exceptions
{
    public class CourseNotFoundException : System.Exception
    {
        public CourseNotFoundException(string message) : base(message) { }
    }
}


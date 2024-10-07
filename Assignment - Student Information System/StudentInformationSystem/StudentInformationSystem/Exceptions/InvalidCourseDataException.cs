using System;
namespace StudentInformationSystem.Exceptions
{
    public class InvalidCourseDataException : System.Exception
    {
        public InvalidCourseDataException(string message) : base(message) { }
    }
}


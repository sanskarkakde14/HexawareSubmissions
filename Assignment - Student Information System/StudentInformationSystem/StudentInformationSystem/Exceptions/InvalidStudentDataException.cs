using System;
namespace StudentInformationSystem.Exceptions
{
    public class InvalidStudentDataException : System.Exception
    {
        public InvalidStudentDataException(string message) : base(message) { }
    }
}


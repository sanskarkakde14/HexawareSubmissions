using System;
namespace StudentInformationSystem.Exceptions
{
    public class InvalidTeacherDataException : System.Exception
    {
        public InvalidTeacherDataException(string message) : base(message) { }
    }
}


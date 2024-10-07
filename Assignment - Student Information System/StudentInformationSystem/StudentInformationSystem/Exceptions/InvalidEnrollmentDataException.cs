using System;
namespace StudentInformationSystem.Exceptions
{
    public class InvalidEnrollmentDataException : System.Exception
    {
        public InvalidEnrollmentDataException(string message) : base(message) { }
    }
}


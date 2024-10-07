using System;
namespace StudentInformationSystem.Exceptions
{
	public class DuplicateEnrollmentException : System.Exception
    {
        public DuplicateEnrollmentException(string message) : base(message) { }
    }

}


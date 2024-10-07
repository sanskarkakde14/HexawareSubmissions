using System;
namespace StudentInformationSystem.Exceptions
{
    public class StudentNotFoundException : System.Exception
    {
        public StudentNotFoundException(string message) : base(message) { }
    }
}


using System;
namespace StudentInformationSystem.Exceptions
{
    public class TeacherNotFoundException : System.Exception
    {
        public TeacherNotFoundException(string message) : base(message) { }
    }
}


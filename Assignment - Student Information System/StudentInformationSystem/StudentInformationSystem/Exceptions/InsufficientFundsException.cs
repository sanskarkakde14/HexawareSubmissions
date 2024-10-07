using System;
namespace StudentInformationSystem.Exceptions
{
    public class InsufficientFundsException : System.Exception
    {
        public InsufficientFundsException(string message) : base(message) { }
    }
}


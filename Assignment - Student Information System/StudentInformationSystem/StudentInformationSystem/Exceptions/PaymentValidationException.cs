using System;
namespace StudentInformationSystem.Exceptions
{
    public class PaymentValidationException : System.Exception
    {
        public PaymentValidationException(string message) : base(message) { }
    }
}


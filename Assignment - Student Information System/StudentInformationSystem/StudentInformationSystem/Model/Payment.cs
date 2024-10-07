using System;
namespace StudentInformationSystem.Model
{
    internal class Payment
    {
        public int PaymentId { get; set; }
        public Student Student { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public Payment(int paymentId,Student student, decimal amount, DateTime paymentDate)
        {
            PaymentId = paymentId;
            Student = student;
            Amount = amount;
            PaymentDate = paymentDate;
        }
        public Payment( Student student, decimal amount, DateTime paymentDate, int paymentId=0)
        {
            PaymentId = paymentId;
            Student = student;
            Amount = amount;
            PaymentDate = paymentDate;
        }

        public override string ToString()
        {
            return $"Payment ID: {PaymentId}, Amount: {Amount}, Date: {PaymentDate}";
        }
    }

}


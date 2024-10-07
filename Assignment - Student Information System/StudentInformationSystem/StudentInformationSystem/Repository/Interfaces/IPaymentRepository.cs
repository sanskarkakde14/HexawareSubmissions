using System;
using StudentInformationSystem.Model;
namespace StudentInformationSystem.Repository
{
	internal interface IPaymentRepository
	{
        Payment GetPaymentById(int id);
        IEnumerable<Payment> GetAllPayments();
        int AddPayment(Payment payment);
        void UpdatePayment(Payment payment);
        void DeletePayment(int id);
    }
}


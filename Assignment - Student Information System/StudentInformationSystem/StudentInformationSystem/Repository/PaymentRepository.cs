using System;
using StudentInformationSystem.Model;
using System.Data.SqlClient;
using StudentInformationSystem.Utils;

namespace StudentInformationSystem.Repository
{
	internal class PaymentRepository : IPaymentRepository
	{
        private readonly string _connectionString;

        public PaymentRepository()
		{
            _connectionString = DbConnUtil.GetConnString();
        }

        // Get a single payment by ID
        public Payment GetPaymentById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "SELECT Payments.payment_id, Payments.amount, Payments.payment_date, " +
                    "Students.student_id, Students.first_name, Students.last_name " +
                    "FROM Payments " +
                    "JOIN Students ON Payments.student_id = Students.student_id " +
                    "WHERE Payments.payment_id = @PaymentId", connection))
                {
                    command.Parameters.AddWithValue("@PaymentId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Create the Student object
                            Student student = new Student(
                                reader.GetInt32(reader.GetOrdinal("student_id")),
                                reader.GetString(reader.GetOrdinal("first_name")),
                                reader.GetString(reader.GetOrdinal("last_name"))
                            );

                            // Create and return the Payment object
                            return new Payment(
                                reader.GetInt32(reader.GetOrdinal("payment_id")),
                                student, // First parameter: Student object
                                reader.GetDecimal(reader.GetOrdinal("amount")), // Second parameter: Amount
                                reader.GetDateTime(reader.GetOrdinal("payment_date")) // Third parameter: PaymentDate
                                
                            );
                        }
                    }
                }
            }

            return null; // Return null if no payment found
        }

        // Get all payments
        public IEnumerable<Payment> GetAllPayments()
        {
            var payments = new List<Payment>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Use a JOIN to get student details
                using (var command = new SqlCommand(@"SELECT p.payment_id, p.student_id, p.amount, p.payment_date, s.first_name, s.last_name FROM Payments p JOIN Students s ON p.student_id = s.student_id", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment(
                                reader.GetInt32(reader.GetOrdinal("payment_id")),
                                new Student(
                                    reader.GetInt32(reader.GetOrdinal("student_id")), // Student ID
                                    reader.GetString(reader.GetOrdinal("first_name")), // First Name
                                    reader.GetString(reader.GetOrdinal("last_name"))  // Last Name
                                ),
                                reader.GetInt32(reader.GetOrdinal("amount")),
                                reader.GetDateTime(reader.GetOrdinal("payment_date"))
                            ));
                        }
                    }
                }
            }
            return payments;
        }

        // Add a new payment
        public int AddPayment(Payment payment)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        connection.Open();
        using (var command = new SqlCommand(
            "INSERT INTO Payments (student_id, amount, payment_date) " +
            "VALUES (@StudentId, @Amount, @PaymentDate); " +
            "SELECT SCOPE_IDENTITY();", connection))
        {
            command.Parameters.AddWithValue("@StudentId", payment.Student.StudentId);
            command.Parameters.AddWithValue("@Amount", payment.Amount);
            command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
            
            // ExecuteScalar returns the last identity value generated
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}

        // Update an existing payment
        public void UpdatePayment(Payment payment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "UPDATE Payments SET student_id = @StudentId, amount = @Amount, payment_date = @PaymentDate " +
                    "WHERE payment_id = @PaymentId", connection))
                {
                    command.Parameters.AddWithValue("@PaymentId", payment.PaymentId);
                    command.Parameters.AddWithValue("@StudentId", payment.Student.StudentId);
                    command.Parameters.AddWithValue("@Amount", payment.Amount);
                    command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Delete a payment by ID
        public void DeletePayment(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Payments WHERE payment_id = @PaymentId", connection))
                {
                    command.Parameters.AddWithValue("@PaymentId", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}


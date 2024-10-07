using System;
using StudentInformationSystem.Model;
using System.Data.SqlClient;
using StudentInformationSystem.Utils;

namespace StudentInformationSystem.Repository
{
	internal class EnrollmentRespository : IEnrollmentRepository
	{
        private readonly string _connectionString;

        public EnrollmentRespository()
		{
            _connectionString = DbConnUtil.GetConnString();
        }
        public bool Exists(int studentId, int courseId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Prepare the SQL query to check for existing enrollment
                var command = new SqlCommand(
                    "SELECT COUNT(*) FROM Enrollments WHERE student_id = @studentId AND course_id = @courseId",
                    connection);
                command.Parameters.AddWithValue("@studentId", studentId);
                command.Parameters.AddWithValue("@courseId", courseId);

                // Execute the query and check the count
                int count = (int)command.ExecuteScalar();
                return count > 0; // Return true if there's at least one enrollment
            }
        }
        // Method to get an enrollment by its ID
        public Enrollment GetEnrollmentById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"SELECT Enrollments.enrollment_id, Enrollments.enrollment_date,
                                                  Students.student_id, Students.first_name, Students.last_name, 
                                                  Courses.course_id, Courses.course_name, Courses.credits
                                                  FROM Enrollments
                                                  JOIN Students ON Enrollments.student_id = Students.student_id
                                                  JOIN Courses ON Enrollments.course_id = Courses.course_id
                                                  WHERE Enrollments.enrollment_id = @EnrollmentId", connection))
                {
                    command.Parameters.AddWithValue("@EnrollmentId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve Student object
                            var student = new Student(
                                reader.GetInt32(reader.GetOrdinal("student_id")),
                                reader.GetString(reader.GetOrdinal("first_name")),
                                reader.GetString(reader.GetOrdinal("last_name"))
                            );

                            // Retrieve Course object
                            var course = new Course(
                                reader.GetInt32(reader.GetOrdinal("course_id")),
                                reader.GetString(reader.GetOrdinal("course_name"))
                            );

                            // Create and return the Enrollment object
                            return new Enrollment(
                                //reader.GetInt32(reader.GetOrdinal("enrollment_id")),
                                 student,
                                course,
                                reader.GetDateTime(reader.GetOrdinal("enrollment_date"))
                                
                            );
                        }
                    }
                }
            }
            return null;
        }

        // Method to get all enrollments
        public IEnumerable<Enrollment> GetAllEnrollments()
        {
            var enrollments = new List<Enrollment>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"SELECT Enrollments.enrollment_id, Enrollments.enrollment_date,
                                                  Students.student_id, Students.first_name, Students.last_name, 
                                                  Courses.course_id, Courses.course_name, Courses.credits
                                                  FROM Enrollments
                                                  JOIN Students ON Enrollments.student_id = Students.student_id
                                                  JOIN Courses ON Enrollments.course_id = Courses.course_id", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Retrieve Student object
                            var student = new Student(
                                reader.GetInt32(reader.GetOrdinal("student_id")),
                                reader.GetString(reader.GetOrdinal("first_name")),
                                reader.GetString(reader.GetOrdinal("last_name"))
                            );

                            // Retrieve Course object
                            var course = new Course(
                                reader.GetInt32(reader.GetOrdinal("course_id")),
                                reader.GetString(reader.GetOrdinal("course_name"))
                            );

                            // Create and add the Enrollment object to the list
                            var enrollment = new Enrollment(
                                //reader.GetInt32(reader.GetOrdinal("enrollment_id")),
                                student,
                                course,
                                reader.GetDateTime(reader.GetOrdinal("enrollment_date"))
                                
                            );

                            enrollments.Add(enrollment);
                        }
                    }
                }
            }

            return enrollments;
        }

        // Method to add a new enrollment
        public int AddEnrollment(Enrollment enrollment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"INSERT INTO Enrollments (student_id, course_id, enrollment_date) VALUES (@StudentId, @CourseId, @EnrollmentDate); SELECT SCOPE_IDENTITY();", connection)) // This retrieves the last inserted ID
                {
                    command.Parameters.AddWithValue("@StudentId", enrollment.Student.StudentId);
                    command.Parameters.AddWithValue("@CourseId", enrollment.Course.CourseId);
                    command.Parameters.AddWithValue("@EnrollmentDate", enrollment.EnrollmentDate);

                    // Execute the command and retrieve the new EnrollmentId
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        // Method to update an existing enrollment
        public void UpdateEnrollment(Enrollment enrollment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Enrollments SET student_id = @StudentId, course_id = @CourseId, enrollment_date = @EnrollmentDate WHERE enrollment_id = @EnrollmentId", connection))
                {
                    command.Parameters.AddWithValue("@EnrollmentId", enrollment.EnrollmentId);
                    command.Parameters.AddWithValue("@StudentId", enrollment.Student.StudentId);
                    command.Parameters.AddWithValue("@CourseId", enrollment.Course.CourseId);
                    command.Parameters.AddWithValue("@EnrollmentDate", enrollment.EnrollmentDate);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to delete an enrollment by its ID
        public void DeleteEnrollment(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Enrollments WHERE enrollment_id = @EnrollmentId", connection))
                {
                    command.Parameters.AddWithValue("@EnrollmentId", id);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}


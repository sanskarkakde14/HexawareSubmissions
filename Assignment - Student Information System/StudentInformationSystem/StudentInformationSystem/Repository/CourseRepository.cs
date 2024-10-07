using System;
using StudentInformationSystem.Model;
using StudentInformationSystem.Repository;
using StudentInformationSystem.Utils;
using System.Data.SqlClient;

namespace StudentInformationSystem.Repository
{
    internal class CourseRepository : ICourseRepository
    {
        private readonly string _connectionString;

        public CourseRepository()
        {
            _connectionString = DbConnUtil.GetConnString();
        }

        // Method to get a course by its ID
        public Course GetCourseById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
    SELECT 
        c.course_id, 
        c.course_name, 
        c.credits,
        t.teacher_id,
        t.first_name AS teacher_first_name,
        t.last_name AS teacher_last_name,
        t.email AS teacher_email,
        e.enrollment_id,
        e.enrollment_date,
        s.student_id,
        s.first_name AS student_first_name,
        s.last_name AS student_last_name,
        p.payment_id,
        p.amount AS payment_amount,
        p.payment_date
    FROM 
        Courses c
    LEFT JOIN 
        Teacher t ON c.teacher_id = t.teacher_id
    LEFT JOIN
        Enrollments e ON c.course_id = e.course_id
    LEFT JOIN
        Students s ON e.student_id = s.student_id
    LEFT JOIN
        Payments p ON s.student_id = p.student_id
    WHERE 
        c.course_id = @CourseId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        Course course = null;
                        var enrollments = new List<Enrollment>();
                        var payments = new Dictionary<int, List<Payment>>();

                        while (reader.Read())
                        {
                            if (course == null)
                            {
                                // Create the Course object
                                Teacher instructor = null;
                                if (!reader.IsDBNull(reader.GetOrdinal("teacher_id")))
                                {
                                    instructor = new Teacher(
                                        reader.GetInt32(reader.GetOrdinal("teacher_id")),
                                        reader.GetString(reader.GetOrdinal("teacher_first_name")),
                                        reader.GetString(reader.GetOrdinal("teacher_last_name")),
                                        reader.GetString(reader.GetOrdinal("teacher_email"))
                                    );
                                }
                                course = new Course(
                                    reader.GetInt32(reader.GetOrdinal("course_id")),
                                    reader.GetString(reader.GetOrdinal("course_name")),
                                    reader.GetInt32(reader.GetOrdinal("credits")),
                                    instructor
                                );
                            }

                            // Add enrollment if it exists
                            if (!reader.IsDBNull(reader.GetOrdinal("enrollment_id")))
                            {
                                var student = new Student(
                                    reader.GetInt32(reader.GetOrdinal("student_id")),
                                    reader.GetString(reader.GetOrdinal("student_first_name")),
                                    reader.GetString(reader.GetOrdinal("student_last_name"))
                                );

                                var enrollment = new Enrollment(
                                    student,
                                    course,
                                    reader.GetDateTime(reader.GetOrdinal("enrollment_date")),
                                    reader.GetInt32(reader.GetOrdinal("enrollment_id"))
                                );
                                enrollments.Add(enrollment);

                                // Add payment if it exists
                                if (!reader.IsDBNull(reader.GetOrdinal("payment_id")))
                                {
                                    var payment = new Payment(
                                        reader.GetInt32(reader.GetOrdinal("payment_id")),
                                        student,
                                        reader.GetInt32(reader.GetOrdinal("payment_amount")),
                                        reader.GetDateTime(reader.GetOrdinal("payment_date"))
                                    );
                                    if (!payments.ContainsKey(student.StudentId))
                                    {
                                        payments[student.StudentId] = new List<Payment>();
                                    }
                                    payments[student.StudentId].Add(payment);
                                }
                            }
                        }

                        // Assign enrollments and payments to the course
                        if (course != null)
                        {
                            course.Enrollments = enrollments;
                            foreach (var enrollment in course.Enrollments)
                            {
                                if (payments.ContainsKey(enrollment.Student.StudentId))
                                {
                                    enrollment.Student.Payments = payments[enrollment.Student.StudentId];
                                }
                            }
                        }

                        return course;
                    }
                }
            }
        }


        // Method to get all courses
        public IEnumerable<Course> GetAllCourses()
        {
            var courses = new List<Course>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT Courses.course_id, Courses.course_name, Courses.credits, Teacher.teacher_id, Teacher.first_name, Teacher.last_name, Teacher.email FROM Courses LEFT JOIN Teacher ON Courses.teacher_id = Teacher.teacher_id", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Retrieve teacherId safely
                            int? teacherId = reader["teacher_id"] != DBNull.Value
                                ? reader.GetInt32(reader.GetOrdinal("teacher_id"))
                                : (int?)null;

                            // Create the Teacher object if teacherId is available
                            Teacher assignedTeacher = null;
                            if (teacherId.HasValue)
                            {
                                assignedTeacher = new Teacher(
                                    teacherId.Value,
                                    reader.GetString(reader.GetOrdinal("first_name")),
                                    reader.GetString(reader.GetOrdinal("last_name")),
                                    reader.GetString(reader.GetOrdinal("email"))
                                );
                            }

                            // Create and add the Course object to the list
                            var course = new Course(
                                reader.GetInt32(reader.GetOrdinal("course_id")),
                                reader.GetString(reader.GetOrdinal("course_name")),
                                reader.GetInt32(reader.GetOrdinal("credits")),
                                assignedTeacher
                            );

                            courses.Add(course);
                        }
                    }
                }
            }
            return courses; // Return the full list of courses
        }

        // Method to add a new course
        public void AddCourse(Course course)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Courses (course_name, credits, teacher_id) VALUES (@CourseName, @Credits, @TeacherId)", connection))
                {
                    command.Parameters.AddWithValue("@CourseName", course.CourseName);
                    command.Parameters.AddWithValue("@Credits", course.Credits);
                    command.Parameters.AddWithValue("@TeacherId", course.AssignedTeacher?.TeacherId ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to update an existing course
        public void UpdateCourse(Course course)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Courses SET course_name = @CourseName, credits = @Credits, teacher_id = @TeacherId WHERE course_id = @CourseId", connection))
                {
                    command.Parameters.AddWithValue("@CourseId", course.CourseId);
                    command.Parameters.AddWithValue("@CourseName", course.CourseName);
                    command.Parameters.AddWithValue("@Credits", course.Credits);
                    command.Parameters.AddWithValue("@TeacherId", course.AssignedTeacher?.TeacherId ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to delete a course by its ID
        public void DeleteCourse(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Courses WHERE course_id = @CourseId", connection))
                {
                    command.Parameters.AddWithValue("@CourseId", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        //// Method to GetCourseEnrollments
        public IEnumerable<Enrollment> GetCourseEnrollments(int courseId)
        {
            return null;
        }
        public IEnumerable<Course> GetCoursesByTeacherId(int teacherId)
        {
            var courses = new List<Course>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Courses WHERE teacher_id = @TeacherId", connection))
                {
                    command.Parameters.AddWithValue("@TeacherId", teacherId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new Course(
                                reader.GetInt32(reader.GetOrdinal("course_id")),
                                reader.GetString(reader.GetOrdinal("course_name")) // Adjust property name as necessary
                            ));
                        }
                    }
                }
            }
            return courses;
        }
    }
}

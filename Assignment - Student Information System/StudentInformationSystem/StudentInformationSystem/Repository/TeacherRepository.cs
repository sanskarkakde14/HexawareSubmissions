using System;
using StudentInformationSystem.Model;
using System.Data.SqlClient;
using StudentInformationSystem.Utils;

namespace StudentInformationSystem.Repository
{
	internal class TeacherRepository : ITeacherRepository
	{
        private readonly string _connectionString;

        public TeacherRepository()
        {
            _connectionString = DbConnUtil.GetConnString();
        }

        public Teacher GetTeacherById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Teacher WHERE teacher_id = @TeacherId", connection))
                {
                    command.Parameters.AddWithValue("@TeacherId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Teacher(
                                reader.GetInt32(reader.GetOrdinal("teacher_id")),
                                reader.GetString(reader.GetOrdinal("first_name")),
                                reader.GetString(reader.GetOrdinal("last_name")),
                                reader.GetString(reader.GetOrdinal("email"))
                            );
                        }
                    }
                }
            }
            return null;
        }

        public IEnumerable<Teacher> GetAllTeachers()
        {
            var teachers = new List<Teacher>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Teacher", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teachers.Add(new Teacher(
                                reader.GetInt32(reader.GetOrdinal("teacher_id")),
                                reader.GetString(reader.GetOrdinal("first_name")),
                                reader.GetString(reader.GetOrdinal("last_name")),
                                reader.GetString(reader.GetOrdinal("email"))
                            ));
                        }
                    }
                }
            }
            return teachers;
        }

        public void AddTeacher(Teacher teacher)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Teacher (first_name, last_name, email) VALUES (@FirstName, @LastName, @Email)", connection))
                {
                    command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                    command.Parameters.AddWithValue("@LastName", teacher.LastName);
                    command.Parameters.AddWithValue("@Email", teacher.Email);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTeacher(Teacher teacher)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Teacher SET first_name = @FirstName, last_name = @LastName, email = @Email WHERE teacher_id = @TeacherId", connection))
                {
                    command.Parameters.AddWithValue("@TeacherId", teacher.TeacherId);
                    command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                    command.Parameters.AddWithValue("@LastName", teacher.LastName);
                    command.Parameters.AddWithValue("@Email", teacher.Email);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTeacher(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Students WHERE student_id = @TeacherId", connection))
                {
                    command.Parameters.AddWithValue("@TeacherId", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

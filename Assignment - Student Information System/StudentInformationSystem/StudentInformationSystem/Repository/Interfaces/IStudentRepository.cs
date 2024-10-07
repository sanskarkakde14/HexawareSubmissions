using System;
using StudentInformationSystem.Model;

namespace StudentInformationSystem.Repository
{
	internal interface IStudentRepository
	{
        Student GetStudentById(int id);
        IEnumerable<Student> GetAllStudents();
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
    }
}


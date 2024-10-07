using System;
using StudentInformationSystem.Model;

namespace StudentInformationSystem.Repository
{
	internal interface ITeacherRepository
	{
        Teacher GetTeacherById(int id);
        IEnumerable<Teacher> GetAllTeachers();
        void AddTeacher(Teacher teacher);
        void UpdateTeacher(Teacher teacher);
        void DeleteTeacher(int id);
    }
}


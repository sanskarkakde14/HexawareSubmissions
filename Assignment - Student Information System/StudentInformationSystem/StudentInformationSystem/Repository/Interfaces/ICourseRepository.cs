using System;
using StudentInformationSystem.Model;

namespace StudentInformationSystem.Repository
{
	internal interface ICourseRepository
	{
        Course GetCourseById(int courseId);
        IEnumerable<Course> GetAllCourses();
        void AddCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(int courseId);
        IEnumerable<Course> GetCoursesByTeacherId(int teacherId);
    }
}



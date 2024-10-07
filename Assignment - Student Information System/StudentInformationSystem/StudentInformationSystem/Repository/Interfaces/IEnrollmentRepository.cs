using System;
using StudentInformationSystem.Model;

namespace StudentInformationSystem.Repository
{
	internal interface IEnrollmentRepository
	{
        Enrollment GetEnrollmentById(int id);
        IEnumerable<Enrollment> GetAllEnrollments();
        int AddEnrollment(Enrollment enrollment);
        void UpdateEnrollment(Enrollment enrollment);
        void DeleteEnrollment(int id);
        bool Exists(int studentId, int courseId);
    }
}


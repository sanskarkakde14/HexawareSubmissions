using System;
using StudentInformationSystem.Model;

namespace StudentInformationSystem.Repository
{
    internal interface ISISservice
    {
        void EnrollStudentInCourse(int studentId, int courseId);
        void AssignTeacherToCourse(int teacherId, int courseId);
        void RecordPayment(int studentId, decimal amount, DateTime paymentDate);
        void GenerateEnrollmentReport(int courseId);
        void GeneratePaymentReport(int studentId);
        void CalculateCourseStatistics(int courseId);
    }
}


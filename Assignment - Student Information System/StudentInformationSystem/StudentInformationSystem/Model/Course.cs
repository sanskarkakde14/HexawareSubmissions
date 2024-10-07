using System;
namespace StudentInformationSystem.Model
{
    internal class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }
        public Teacher AssignedTeacher { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public Course(int courseId, string courseName,int credits,Teacher instructorname)
        {
            CourseId = courseId;
            CourseName = courseName;
            Credits = credits;
            AssignedTeacher = instructorname;
            Enrollments = new List<Enrollment>();
        }
        public Course( string courseName, int credits, int courseId=0 )
        {
            CourseId = courseId;
            CourseName = courseName;
            Credits = credits;
        }
        public Course(int courseId, string courseName)
        {
            CourseId = courseId;
            CourseName = courseName;
            Enrollments = new List<Enrollment>();
        }

        public override string ToString()
        {
            return $"Course ID: {CourseId}, Name: {CourseName}, credits: {Credits}, Enrollments: {Enrollments}";
        }
    }
}


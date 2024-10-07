using System;
using StudentInformationSystem.Model;
using static StudentInformationSystem.Repository.ServiceImplementation;
using StudentInformationSystem.Exceptions;
namespace StudentInformationSystem.Repository
{

    internal class ServiceImplementation : ISISservice
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IPaymentRepository _paymentRepository;

        public ServiceImplementation(
        IStudentRepository studentRepository,
        ICourseRepository courseRepository,
        ITeacherRepository teacherRepository,
        IEnrollmentRepository enrollmentRepository,
        IPaymentRepository paymentRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        }
        // Enrollment methods
        public void EnrollStudentInCourse(int studentId, int courseId)
        {
            var student = _studentRepository.GetStudentById(studentId);
            var course = _courseRepository.GetCourseById(courseId);

            if (student == null)
            {
                throw new StudentNotFoundException("Student not found.");
            }
             
            if (course == null)
            {
                throw new CourseNotFoundException("Course not found."); // This will catch a non-existing course
            }

            if (_enrollmentRepository.Exists(studentId, courseId))
            {
                throw new DuplicateEnrollmentException("Student is already enrolled in this course.");
            }

            // Create the Enrollment object
            var enrollment = new Enrollment(student, course, DateTime.Now);

            // Add the enrollment to the repository
            int enrollmentId = _enrollmentRepository.AddEnrollment(enrollment);

            // Update the EnrollmentId property
            enrollment.EnrollmentId = enrollmentId;

            // Update collections
            student.Enrollments.Add(enrollment);
            course.Enrollments.Add(enrollment);

            _studentRepository.UpdateStudent(student);
            _courseRepository.UpdateCourse(course);
        }


        public void AssignTeacherToCourse(int teacherId, int courseId)
        {
            
            var teacher = _teacherRepository.GetTeacherById(teacherId);
            var course = _courseRepository.GetCourseById(courseId);

            if (teacher == null || course == null)
            {
                throw new TeacherNotFoundException("Teacher or course not found.");
            }

            course.AssignedTeacher=teacher;
            teacher.AssignedCourses.Add(course);

            _courseRepository.UpdateCourse(course);
            _teacherRepository.UpdateTeacher(teacher);
        }

        // Payment methods
        public void RecordPayment(int studentId, decimal amount, DateTime paymentDate)
        {
            var student = _studentRepository.GetStudentById(studentId);
            if (student == null)
            {
                throw new StudentNotFoundException($"Student with ID {studentId} not found.");
            }

            // Call the constructor without the PaymentId since it's optional
            var payment = new Payment(student, amount, paymentDate); // No ID needed here

            // Add the payment to the database, assuming it handles ID generation
            int paymentId = _paymentRepository.AddPayment(payment);
            payment.PaymentId = paymentId; // Set the generated ID
            student.Payments.Add(payment);
            _studentRepository.UpdateStudent(student);
        }


        // Reporting methods
        public void GenerateEnrollmentReport(int courseId)
        {
            var course = _courseRepository.GetCourseById(courseId);
            if (course == null)
            {
                throw new CourseNotFoundException($"Course with ID {courseId} not found.");
            }

            Console.WriteLine("================================");
            Console.WriteLine($"Enrollment Report for {course.CourseName}");
            Console.WriteLine("================================");
            Console.WriteLine($"Course ID: {course.CourseId}");
            Console.WriteLine($"Credits: {course.Credits}");
            Console.WriteLine($"Instructor: {course.AssignedTeacher?.FirstName} {course.AssignedTeacher?.LastName}");
            Console.WriteLine($"Instructor Email: {course.AssignedTeacher?.Email}");
            Console.WriteLine("--------------------------------");

            int totalEnrollments = course.Enrollments.Count;
            Console.WriteLine($"Total Enrollments: {totalEnrollments}");

            if (totalEnrollments == 0)
            {
                Console.WriteLine("No enrollments found for this course.");
                return;
            }

            var earliestEnrollment = course.Enrollments.Min(e => e.EnrollmentDate);
            var latestEnrollment = course.Enrollments.Max(e => e.EnrollmentDate);
            var averageEnrollmentsPerDay = (double)totalEnrollments / (latestEnrollment - earliestEnrollment).Days;

            Console.WriteLine($"Earliest Enrollment: {earliestEnrollment:d}");
            Console.WriteLine($"Latest Enrollment: {latestEnrollment:d}");
            Console.WriteLine($"Average Enrollments per Day: {averageEnrollmentsPerDay:F2}");

            var enrollmentsByMonth = course.Enrollments
                .GroupBy(e => new { e.EnrollmentDate.Year, e.EnrollmentDate.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new {
                    YearMonth = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Count = g.Count()
                });

            Console.WriteLine("\nEnrollments by Month:");
            foreach (var month in enrollmentsByMonth)
            {
                Console.WriteLine($"  {month.YearMonth}: {month.Count}");
            }


            Console.WriteLine("--------------------------------");
            Console.WriteLine("Enrollment Details:");

            foreach (var enrollment in course.Enrollments.OrderBy(e => e.EnrollmentDate))
            {
                Console.WriteLine($"  Student: {enrollment.Student.FirstName} {enrollment.Student.LastName}");
                Console.WriteLine($"  Student ID: {enrollment.Student.StudentId}");
                Console.WriteLine($"  Enrollment Date: {enrollment.EnrollmentDate:d}");
                Console.WriteLine($"  Email: {enrollment.Student.Email}");
                Console.WriteLine($"  Phone: {enrollment.Student.PhoneNumber}");

                var studentPayments = enrollment.Student.Payments;
                var totalStudentPayment = studentPayments.Sum(p => p.Amount);
                Console.WriteLine($"    Total Payments: ${totalStudentPayment:F2}");
                Console.WriteLine($"    Number of Payments: {studentPayments.Count}");
                if (studentPayments.Any())
                {
                    var lastPayment = studentPayments.OrderByDescending(p => p.PaymentDate).First();
                    Console.WriteLine($"    Last Payment: ${lastPayment.Amount:F2} on {lastPayment.PaymentDate:d}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("================================");
        }

        public void GeneratePaymentReport(int studentId)
        {
            var student = _studentRepository.GetStudentById(studentId);
            if (student == null)
            {
                throw new StudentNotFoundException($"Student with ID {studentId} not found.");
            }

            Console.WriteLine("================================");
            Console.WriteLine($"Payment Report for {student.FirstName} {student.LastName}");
            Console.WriteLine("================================");
            Console.WriteLine($"Student ID: {student.StudentId}");
            Console.WriteLine($"Email: {student.Email}");
            Console.WriteLine($"Phone: {student.PhoneNumber}");
            Console.WriteLine($"Date of Birth: {student.DateOfBirth:d}");
            Console.WriteLine("--------------------------------");

            if (student.Payments.Any())
            {
                var totalPayments = student.Payments.Sum(p => p.Amount);
                var averagePayment = student.Payments.Average(p => p.Amount);
                var firstPayment = student.Payments.OrderBy(p => p.PaymentDate).First();
                var lastPayment = student.Payments.OrderByDescending(p => p.PaymentDate).First();

                Console.WriteLine($"Total Payments: ${totalPayments:F2}");
                Console.WriteLine($"Number of Payments: {student.Payments.Count}");
                Console.WriteLine($"Average Payment: ${averagePayment:F2}");
                Console.WriteLine($"First Payment: ${firstPayment.Amount:F2} on {firstPayment.PaymentDate:d}");
                Console.WriteLine($"Last Payment: ${lastPayment.Amount:F2} on {lastPayment.PaymentDate:d}");
                Console.WriteLine("--------------------------------");
                Console.WriteLine("Payment History:");

                foreach (var payment in student.Payments.OrderByDescending(p => p.PaymentDate))
                {
                    Console.WriteLine($"- Date: {payment.PaymentDate:d}, Amount: ${payment.Amount:F2}");
                }
            }
            else
            {
                Console.WriteLine("No payments found for this student.");
            }

            Console.WriteLine("================================");
        }

        public void CalculateCourseStatistics(int courseId)
        {
            var course = _courseRepository.GetCourseById(courseId);
            if (course == null)
            {
                throw new CourseNotFoundException($"Course with ID {courseId} not found.");
            }

            int enrollmentCount = course.Enrollments.Count;
            decimal totalPayments = course.Enrollments.Sum(e => e.Student.Payments.Sum(p => p.Amount));
            var earliestEnrollment = course.Enrollments.Min(e => e.EnrollmentDate);
            var latestEnrollment = course.Enrollments.Max(e => e.EnrollmentDate);
            var averagePayment = enrollmentCount > 0 ? totalPayments / enrollmentCount : 0;

            Console.WriteLine("================================");
            Console.WriteLine($"Detailed Course Statistics for {course.CourseName}");
            Console.WriteLine("================================");
            Console.WriteLine($"Course ID: {course.CourseId}");
            Console.WriteLine($"Credits: {course.Credits}");
            Console.WriteLine($"Instructor: {course.AssignedTeacher?.FirstName} {course.AssignedTeacher?.LastName}");
            Console.WriteLine($"Instructor Email: {course.AssignedTeacher?.Email}");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Number of Enrollments: {enrollmentCount}");
            Console.WriteLine($"Earliest Enrollment: {earliestEnrollment:d}");
            Console.WriteLine($"Latest Enrollment: {latestEnrollment:d}");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Total Payments: ${totalPayments:F2}");
            Console.WriteLine($"Average Payment per Student: ${averagePayment:F2}");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Enrollment Details:");

            foreach (var enrollment in course.Enrollments.OrderBy(e => e.EnrollmentDate))
            {
                var student = enrollment.Student;
                var studentPayments = student.Payments;
                var totalStudentPayment = studentPayments.Sum(p => p.Amount);
                Console.WriteLine($"  Student: {student.FirstName} {student.LastName}");
                Console.WriteLine($"  Enrolled on: {enrollment.EnrollmentDate:d}");
                Console.WriteLine($"  Total Payments: ${totalStudentPayment:F2}");
                Console.WriteLine($"  Number of Payments: {studentPayments.Count}");
                if (studentPayments.Any())
                {
                    Console.WriteLine($"    Last Payment: ${studentPayments.Max(p => p.Amount):F2} on {studentPayments.OrderByDescending(p => p.PaymentDate).First().PaymentDate:d}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("================================");
        }

        //Extra methods
        public void AddEnrollment(int studentId, int courseId, DateTime enrollmentDate)
        {
            var student = _studentRepository.GetStudentById(studentId);
            var course = _courseRepository.GetCourseById(courseId);

            if (student == null || course == null)
            {
                throw new Exception("Student or course not found.");
            }

            var enrollment = new Enrollment(student, course, enrollmentDate);
            _enrollmentRepository.AddEnrollment(enrollment);

            // Add the enrollment to both the student's and course's lists
            student.Enrollments.Add(enrollment);
            course.Enrollments.Add(enrollment);

            _studentRepository.UpdateStudent(student);
            _courseRepository.UpdateCourse(course);
        }

        // Method to assign a course to a teacher
        public void AssignCourseToTeacher(int courseId, int teacherId)
        {
            var course = _courseRepository.GetCourseById(courseId);
            var teacher = _teacherRepository.GetTeacherById(teacherId);

            if (course == null || teacher == null)
            {
                throw new Exception("Course or teacher not found.");
            }

            course.AssignedTeacher = teacher; // Assuming AssignedTeacher is a property
            teacher.AssignedCourses.Add(course);

            _courseRepository.UpdateCourse(course);
            _teacherRepository.UpdateTeacher(teacher);
        }

        // Method to add a payment
        public void AddPayment(int studentId, decimal amount, DateTime paymentDate)
        {
            var student = _studentRepository.GetStudentById(studentId);

            if (student == null)
            {
                throw new Exception("Student not found.");
            }

            var payment = new Payment(student, amount, paymentDate); // No ID needed here

            // Add the payment to the database, assuming it handles ID generation
            int paymentId = _paymentRepository.AddPayment(payment);
            payment.PaymentId = paymentId;
            student.Payments.Add(payment);
            _studentRepository.UpdateStudent(student);
        }

        // Method to retrieve enrollments for a specific student
        public IEnumerable<Enrollment> GetEnrollmentsForStudent(int studentId)
        {
            var student = _studentRepository.GetStudentById(studentId);

            if (student == null)
            {
                throw new Exception("Student not found.");
            }

            return student.Enrollments; // Return the list of enrollments
        }

        // Method to retrieve courses for a specific teacher
        public IEnumerable<Course> GetCoursesForTeacher(int teacherId)
        {
            var teacher = _teacherRepository.GetTeacherById(teacherId);

            if (teacher == null)
            {
                throw new Exception("Teacher not found.");
            }

            return teacher.AssignedCourses; // Return the list of assigned courses
        }
        public int AddStudentFromInput()
        {
            
            Console.WriteLine("Enter first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter last name:");
            string lastName = Console.ReadLine();
            Console.WriteLine("Enter date of birth (yyyy-mm-dd):");
            DateTime dateOfBirth;
            while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth))
            {
                Console.WriteLine("Invalid date format. Please enter again (yyyy-mm-dd):");
            }
            Console.WriteLine("Enter email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter phone number:");
            string phoneNumber = Console.ReadLine();
            // Create a new Student object
            var student = new Student
            (
                firstName,
                lastName,
                dateOfBirth,
                email,
                phoneNumber
            );

            // Add the student to the repository
            _studentRepository.AddStudent(student);
            Console.WriteLine("Student added successfully.");
            return student.StudentId;
        }

        public IEnumerable<Teacher> GetAllTeachers()
        {
            return _teacherRepository.GetAllTeachers();
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _courseRepository.GetAllCourses();
        }

        public IEnumerable<(Teacher Teacher, IEnumerable<Course> Courses)> GetTeachersWithCourses()
        {
            var teachers = _teacherRepository.GetAllTeachers();
            var result = new List<(Teacher, IEnumerable<Course>)>();

            foreach (var teacher in teachers)
            {
                var courses = _courseRepository.GetCoursesByTeacherId(teacher.TeacherId); // Assume this method exists
                result.Add((teacher, courses));
            }

            return result;
        }
        public IEnumerable<Payment> GetAllPayments()
        {
            return _paymentRepository.GetAllPayments(); // This method should be implemented in your PaymentRepository
        }

    }
}


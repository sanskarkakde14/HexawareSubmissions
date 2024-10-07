using SIS;
using StudentInformationSystem.Exceptions;
using StudentInformationSystem.Model;
using StudentInformationSystem.Repository;
using StudentInformationSystem;
namespace StudentInformationSystem;

    class Program
    {
        static void Main(string[] args)
        {
            // Create instances of your repositories
            IStudentRepository studentRepository = new StudentRepository();
            ICourseRepository courseRepository = new CourseRepository();
            ITeacherRepository teacherRepository = new TeacherRepository();
            IEnrollmentRepository enrollmentRepository = new EnrollmentRespository();
            IPaymentRepository paymentRepository = new PaymentRepository();

            // Create an instance of MainModule
            MainModule mainModule = new MainModule(
                studentRepository,
                courseRepository,
                teacherRepository,
                enrollmentRepository,
                paymentRepository
            );

            // Run the main menu
            mainModule.ShowMenu();
        }
    }
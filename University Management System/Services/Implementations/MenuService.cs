using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Reflection;
using University_Management_System.Data;
using University_Management_System.Entities;
using University_Management_System.PaymentGateways;
using University_Management_System.Services.Interfaces;
using static University_Management_System.Validations.InputValidator;


namespace University_Management_System.Services.Implementations
{
    public class MenuService : IMenuService
    {
        private readonly IStudentService studentService;
        private readonly IFacultyService facultyService;
        private readonly ICourseService courseService;
        private readonly IPaymentService paymentService;
        private readonly ILogger<MenuService> logger;


        public MenuService(IStudentService studentService, IFacultyService facultyService, ICourseService courseService, IPaymentService paymentService, ILogger<MenuService> logger)
        {
            this.studentService = studentService;
            this.facultyService = facultyService;
            this.courseService = courseService;
            this.paymentService = paymentService;
            this.logger = logger;
            //SeedDataAsync().Wait();
        }

        public async Task ShowMenuAsync()
        {
            bool exit = false;
            while (!exit)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write($"{"1. Add Student",-30}");
                    Console.Write($"{"2. Remove Student",-30}");
                    Console.WriteLine("3. Display Students");
                    Console.Write($"{"4. Add Course",-30}");
                    Console.Write($"{"5. Remove Course",-30}");
                    Console.WriteLine("6. Display Courses");
                    Console.Write($"{"7. Add Faculty",-30}");
                    Console.Write($"{"8. Remove Faculty",-30}");
                    Console.WriteLine("9. Display Faculty");
                    Console.Write($"{"10. Calculate Student Fees",-30}");
                    Console.Write($"{"11. Process Payment",-30}");
                    Console.WriteLine("12. Exit");
                    Console.Write("Select an option: ");

                    var choice = Console.ReadLine();
                    Console.Clear();

                    switch (choice)
                    {
                        case "1":
                            await AddStudentAsync();
                            break;
                        case "2":
                            await RemoveStudentAsync();
                            break;
                        case "3":
                            await DisplayStudentsAsync();
                            break;
                        case "4":
                            await AddCourseAsync();
                            break;
                        case "5":
                            await RemoveCourseAsync();
                            break;
                        case "6":
                            await DisplayCourseAsync();
                            break;
                        case "7":
                            await AddFacultyAsync();
                            break;
                        case "8":
                            await RemoveFacultyAsync();
                            break;
                        case "9":
                            await DisplayFacultyAsync();
                            break;
                        case "10":
                            await CalculateStudentFeesAsync();
                            break;
                        case "11":
                            await ProcessPaymentAsync();
                            break;
                        case "12":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(String.Format("An error occurred while showing the menu: {0}",ex.Message));
                }

            }
        }

        private async Task ProcessPaymentAsync()
        {
            try
            {
                int id = PromptForValidInt("Enter Student ID: ");
                Student? student = await studentService.GetStudentByIdAsync(id);
                decimal fee;
                if (student == null)
                {
                    Console.WriteLine("Student not found.");
                    return;
                }
                if (student.Fees == null || student.Fees == 0)
                {
                    Console.WriteLine("Fee is Not caluculated yet.Please calculate the fee Before Payment.");
                    return;
                }
                fee = student.Fees.Value;
                if (student.PaymentStatus == true)
                {
                    Console.WriteLine("Payment is already done.");
                    return;
                }
                bool status = false;
                Console.WriteLine(student?.ToString());
                Console.WriteLine();

                Console.WriteLine($"Total Fee to be Paid: {student?.Fees} Rupees");
                Console.WriteLine("How would you like to pay?");
                Console.WriteLine("1. Credit Card");
                Console.WriteLine("2. Bank Account");
                int choice = PromptForValidInt("Enter your choice: ");
                if (choice == 1)
                {
                    PaymentGateway gateway = new CreditCardPaymentGateway(paymentService);
                    status = await gateway.ProcessPaymentAsync(fee, id);
                }
                else if (choice == 2)
                {
                    PaymentGateway gateway = new BankTransferPaymentGateway(paymentService);
                    status = await gateway.ProcessPaymentAsync(fee, id);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Payment failed.");
                }
                student.PaymentStatus = status;
                await studentService.UpdateStudentAsync(student);
            }
            catch (Exception ex)
            {
                logger.LogError(String.Format("An error occurred while processing payment: {0}", ex.Message));
            }
        }

        private async Task CalculateStudentFeesAsync()
        {
            int courseCount = 0;
            int id = PromptForValidInt("Enter Student ID: ");
            Student student = await studentService.GetStudentByIdAsync(id);
            string query = @"Select Count(*) From StudentCourses Where StudentId = @id";
            SqlConnection connection = await ConnectionManager.GetConnAsync();
            try
            {
                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    courseCount = (int)await command.ExecuteScalarAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Calculating Student Fees: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            if (courseCount == 0)
            {
                Console.WriteLine("No courses found for the student.");
                return;
            }
            else if (courseCount <= 3)
            {
                student = new PartTimeStudent
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    DateOfBirth = student.DateOfBirth,
                    EnrollmentDate = student.EnrollmentDate,
                    Address = student.Address,
                    StudentCourses = student.StudentCourses

                };
            }
            else
            {
                student = new FullTimeStudent
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    DateOfBirth = student.DateOfBirth,
                    EnrollmentDate = student.EnrollmentDate,
                    Address = student.Address,
                    StudentCourses = student.StudentCourses
                };
            }
            decimal? fee = student?.CalculateFees();
            student.Fees = fee;
            await studentService.UpdateStudentAsync(student);


        }

        private async Task DisplayFacultyAsync()
        {
            var faculties = await facultyService.GetAllFacultiesAsync();
            //DisplayEntities(faculties);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"ID",-3}| {"Name",-20}| {"Email",-30}| {"Department",-20}| {"Courses Taught",-30}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('-', 110));
            foreach (var faculty in faculties)
            {
                var courses = faculty.CoursesTaught.Any() ? string.Join(", ", faculty.CoursesTaught.Select(c => c.Name)) : "None";
                Console.WriteLine($"{faculty.Id,-3}| {faculty.Name,-20}| {faculty.Email,-30}| {faculty.Department,-20}| {courses,-30}");
            }

            Console.WriteLine(new string('-', 110));
            Console.WriteLine();


        }

        private async Task RemoveFacultyAsync()
        {
            var id = PromptForValidInt("Enter Faculty ID to remove: ");
            await facultyService.RemoveFacultyAsync(id);
        }

        private async Task AddFacultyAsync()
        {
            var name = PromptForValidString("Enter Name: ");
            var email = PromptForValidString("Enter Email: ");
            var deparment = PromptForValidString("Enter Department: ");
            var street = PromptForValidString("Enter Street: ");
            var city = PromptForValidString("Enter City: ");
            var state = PromptForValidString("Enter State: ");
            var pinCode = PromptForValidString("Enter PinCode: ");

            Faculty faculty = new Faculty
            {
                Name = name,
                Email = email,
                Department = deparment,
                Address = new Address
                {
                    Street = street,
                    City = city,
                    State = state,
                    PinCode = pinCode
                }
            };
            await facultyService.AddFacultyAsync(faculty);
            Console.WriteLine("Faculty added successfully.");

        }

        private async Task DisplayCourseAsync()
        {
            //var courses = await courseService.GetAllCoursesAsync();
            //DisplayEntities(courses);

            //Using ADO.NET
            string query = @"
                              SELECT c.ID,c.Name, c.Credits,c.FacultyId,f.Name as FacultyName 
                              From Courses c 
                              Join Faculties f on c.FacultyId = f.I";
            SqlConnection connection = await ConnectionManager.GetConnAsync();
            try
            {
                using (SqlCommand command = new(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{"ID",-3}| {"Name",-35}| {"Credits",-7}| {"Faculty ID",-10}| {"Faculty Name",-20}|");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(new string('-', 80));

                        while (await reader.ReadAsync())
                        {
                            var id = reader.GetInt32(0);
                            var name = reader.GetString(1);
                            var credits = reader.GetInt32(2);
                            var facultyId = reader.GetInt32(3);
                            var facultyName = reader.IsDBNull(4) ? "Unknown" : reader.GetString(4);

                            Console.WriteLine($"{id,-3}| {name,-35}| {credits,-7}| {facultyId,-10}| {facultyName,-20}|");
                        }

                        Console.WriteLine(new string('-', 80));
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(String.Format("An error occurred while displaying courses: {0}", ex.Message));
            }
            finally
            {
                connection.Close();
            }

        }

        private async Task RemoveCourseAsync()
        {
            var id = PromptForValidInt("Enter Course ID to remove: ");
            await courseService.DeleteCourseAsync(id);
        }

        private async Task AddCourseAsync()
        {
            var faculties = await facultyService.GetAllFacultiesAsync();
            if (faculties == null || !faculties.Any())
            {
                Console.WriteLine("No faculties available. Cannot add course.");
                return;
            }
            var name = PromptForValidString("Enter Course Name: ");
            var credits = PromptForValidInt("Enter Credits: ");

            Console.WriteLine("Available Faculties:");
            foreach (var faculty in faculties)
            {
                Console.WriteLine($"ID: {faculty.Id}, Name: {faculty.Name}");
            }
            var facultyId = PromptForValidFacultyId(faculties);

            var course = new Course
            {
                Name = name,
                Credits = credits,
                FacultyId = facultyId
            };

            await courseService.AddCourseAsync(course);
            Console.WriteLine("Course added successfully.");
        }

        private async Task AddStudentAsync()
        {
            var courses = await courseService.GetAllCoursesAsync();
            if (courses == null || !courses.Any())
            {
                Console.WriteLine("No courses available. Cannot add student.");
                return;
            }
            var name = PromptForValidString("Enter Name: ", 50);
            var email = PromtAndValidateEmail("Enter Email: ");
            var dob = PromptForValidDate("Enter Date of Birth (yyyy-mm-dd): ");
            var enrollmentDate = PromptForValidDate("Enter Enrollment Date (yyyy-mm-dd): ");
            var street = PromptForValidString("Enter Street: ", 50);
            var city = PromptForValidString("Enter City: ", 50);
            var state = PromptForValidString("Enter State: ", 50);
            var pinCode = PromptForValidString("Enter PinCode: ", 50);

            var student = new Student
            {
                Name = name,
                Email = email,
                DateOfBirth = dob,
                EnrollmentDate = enrollmentDate,
                Address = new Address
                {
                    Street = street,
                    City = city,
                    State = state,
                    PinCode = pinCode
                },
                Fees = 0

            };

            Console.WriteLine("Available Courses:");
            foreach (var course in courses)
            {
                Console.WriteLine($"ID: {course.Id}, Name: {course.Name}");
            }
            while (true)
            {
                var courseId = PromptForValidInt("Enter Course ID to assign to student (or 0 to finish): ");
                if (courseId == 0) break;

                var course = courses.FirstOrDefault(c => c.Id == courseId);
                if (course != null)
                {
                    student.StudentCourses.Add(new StudentCourse { Student = student, Course = course });
                }
                else
                {
                    Console.WriteLine("Invalid Course ID. Please try again.");
                }
            }

            await studentService.AddStudentAsync(student);
            Console.WriteLine("Student added successfully.");
        }
        private async Task RemoveStudentAsync()
        {
            int id = PromptForValidInt("Enter Student ID to remove: ");
            await studentService.RemoveStudentAsync(id);
        }

        private async Task DisplayStudentsAsync()
        {

            var students = await studentService.GetAllStudentsAsync();
            //DisplayEntities(students);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"ID",-3}| {"Name",-12}| {"Email",-30}| {"Date of Birth",-14}| {"Enrollment Date",-15}| {"Street",-13}| {"City",-10}| {"State",-10}| {"PinCode",-10}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('-', 130));

            foreach (var student in students)
            {
                Console.WriteLine($"{student.Id,-3}| {student.Name,-12}| {student.Email,-30}| {student.DateOfBirth.ToShortDateString(),-14}| {student.EnrollmentDate.ToShortDateString(),-15}| {student.Address?.Street,-13}| {student.Address?.City,-10}| {student.Address?.State,-10}| {student.Address?.PinCode,-10}");
            }
            Console.WriteLine(new string('-', 130));
            Console.WriteLine();


        }
        private int PromptForValidFacultyId(List<Faculty> faculties)
        {
            int facultyId;
            while (true)
            {
                facultyId = PromptForValidInt("Enter Faculty ID: ");
                if (faculties.Any(f => f.Id == facultyId))
                {
                    break;
                }
                Console.WriteLine("Invalid Faculty ID. Please enter a valid Faculty ID from the list.");
            }
            return facultyId;
        }
        public async Task SeedDataAsync()
        {
            // Add Faculties
            var faculties = new List<Faculty>
    {
        new Faculty { Name = "Dr. John Smith", Email = "john.smith@university.com", Department = "Computer Science", Address = new Address { Street = "123 Main St", City = "CityA", State = "StateA", PinCode = "12345" } },
        new Faculty { Name = "Dr. Jane Doe", Email = "jane.doe@university.com", Department = "Mathematics", Address = new Address { Street = "456 Elm St", City = "CityB", State = "StateB", PinCode = "67890" } },
        new Faculty { Name = "Dr. Emily Johnson", Email = "emily.johnson@university.com", Department = "Physics", Address = new Address { Street = "789 Oak St", City = "CityC", State = "StateC", PinCode = "11223" } },
        new Faculty { Name = "Dr. Michael Brown", Email = "michael.brown@university.com", Department = "Chemistry", Address = new Address { Street = "101 Pine St", City = "CityD", State = "StateD", PinCode = "44556" } },
        new Faculty { Name = "Dr. Sarah Davis", Email = "sarah.davis@university.com", Department = "Biology", Address = new Address { Street = "202 Maple St", City = "CityE", State = "StateE", PinCode = "77889" } },
        new Faculty { Name = "Dr. David Wilson", Email = "david.wilson@university.com", Department = "History", Address = new Address { Street = "303 Birch St", City = "CityF", State = "StateF", PinCode = "99001" } }
    };

            foreach (var faculty in faculties)
            {
                await facultyService.AddFacultyAsync(faculty);
            }

            // Add Courses
            var courses = new List<Course>
    {
        new Course { Name = "Introduction to Computer Science", Credits = 3, FacultyId = faculties[0].Id },
        new Course { Name = "Data Structures and Algorithms", Credits = 4, FacultyId = faculties[0].Id },
        new Course { Name = "Calculus I", Credits = 3, FacultyId = faculties[1].Id },
        new Course { Name = "Linear Algebra", Credits = 3, FacultyId = faculties[1].Id },
        new Course { Name = "General Physics I", Credits = 4, FacultyId = faculties[2].Id },
        new Course { Name = "Quantum Mechanics", Credits = 4, FacultyId = faculties[2].Id },
        new Course { Name = "Organic Chemistry", Credits = 4, FacultyId = faculties[3].Id },
        new Course { Name = "Inorganic Chemistry", Credits = 4, FacultyId = faculties[3].Id },
        new Course { Name = "Cell Biology", Credits = 3, FacultyId = faculties[4].Id },
        new Course { Name = "Genetics", Credits = 3, FacultyId = faculties[4].Id },
        new Course { Name = "World History", Credits = 3, FacultyId = faculties[5].Id },
        new Course { Name = "Modern History", Credits = 3, FacultyId = faculties[5].Id }
    };

            foreach (var course in courses)
            {
                await courseService.AddCourseAsync(course);
            }

            // Add Students
            var students = new List<Student>
    {
        new Student { Name = "Alice Johnson", Email = "alice.johnson@university.com", DateOfBirth = new DateTime(2000, 1, 1), EnrollmentDate = DateTime.Now, Address = new Address { Street = "1st Ave", City = "CityA", State = "StateA", PinCode = "12345" } },
        new Student { Name = "Bob Smith", Email = "bob.smith@university.com", DateOfBirth = new DateTime(2001, 2, 2), EnrollmentDate = DateTime.Now, Address = new Address { Street = "2nd Ave", City = "CityB", State = "StateB", PinCode = "67890" } },
        new Student { Name = "Charlie Brown", Email = "charlie.brown@university.com", DateOfBirth = new DateTime(2002, 3, 3), EnrollmentDate = DateTime.Now, Address = new Address { Street = "3rd Ave", City = "CityC", State = "StateC", PinCode = "11223" } },
        new Student { Name = "David Wilson", Email = "david.wilson@university.com", DateOfBirth = new DateTime(2003, 4, 4), EnrollmentDate = DateTime.Now, Address = new Address { Street = "4th Ave", City = "CityD", State = "StateD", PinCode = "44556" } },
        new Student { Name = "Eva Green", Email = "eva.green@university.com", DateOfBirth = new DateTime(2004, 5, 5), EnrollmentDate = DateTime.Now, Address = new Address { Street = "5th Ave", City = "CityE", State = "StateE", PinCode = "77889" } },
        new Student { Name = "Frank White", Email = "frank.white@university.com", DateOfBirth = new DateTime(2005, 6, 6), EnrollmentDate = DateTime.Now, Address = new Address { Street = "6th Ave", City = "CityF", State = "StateF", PinCode = "99001" } },
        new Student { Name = "Grace Black", Email = "grace.black@university.com", DateOfBirth = new DateTime(2006, 7, 7), EnrollmentDate = DateTime.Now, Address = new Address { Street = "7th Ave", City = "CityG", State = "StateG", PinCode = "22334" } },
        new Student { Name = "Henry Blue", Email = "henry.blue@university.com", DateOfBirth = new DateTime(2007, 8, 8), EnrollmentDate = DateTime.Now, Address = new Address { Street = "8th Ave", City = "CityH", State = "StateH", PinCode = "55667" } },
        new Student { Name = "Ivy Red", Email = "ivy.red@university.com", DateOfBirth = new DateTime(2008, 9, 9), EnrollmentDate = DateTime.Now, Address = new Address { Street = "9th Ave", City = "CityI", State = "StateI", PinCode = "88990" } },
        new Student { Name = "Jack Yellow", Email = "jack.yellow@university.com", DateOfBirth = new DateTime(2009, 10, 10), EnrollmentDate = DateTime.Now, Address = new Address { Street = "10th Ave", City = "CityJ", State = "StateJ", PinCode = "11212" } }
    };

            var random = new Random();
            foreach (var student in students)
            {
                var courseCount = student.Name.StartsWith("A") || student.Name.StartsWith("B") || student.Name.StartsWith("C") || student.Name.StartsWith("D") || student.Name.StartsWith("E") ? random.Next(4, 6) : random.Next(1, 3);
                var assignedCourses = new HashSet<int>();
                for (int i = 0; i < courseCount; i++)
                {
                    Course course;
                    do
                    {
                        course = courses[random.Next(courses.Count)];
                    } while (assignedCourses.Contains(course.Id));
                    assignedCourses.Add(course.Id);
                    student.StudentCourses.Add(new StudentCourse { StudentId = student.Id, CourseId = course.Id });
                }
                await studentService.AddStudentAsync(student);


            }
            Console.WriteLine("Data seeding completed successfully.");
        }

    }
}


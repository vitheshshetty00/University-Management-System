using University_Management_System.Entities;

namespace University_Management_System.Services
{
    public class MenuService : IMenuService
    {
        private readonly IStudentService _studentService;

        public MenuService(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task ShowMenuAsync()
        {

            bool exit = false;
            while (!exit)
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
                    case "12":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private async Task AddStudentAsync()
        {
            Console.Write("Enter Name: ");
            var name = Console.ReadLine();
            Console.Write("Enter Email: ");
            var email = Console.ReadLine();
            Console.Write("Enter Date of Birth (yyyy-mm-dd): ");
            var dob = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter Enrollment Date (yyyy-mm-dd): ");
            var enrollmentDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter Street: ");
            var street = Console.ReadLine();
            Console.Write("Enter City: ");
            var city = Console.ReadLine();
            Console.Write("Enter State: ");
            var state = Console.ReadLine();
            Console.Write("Enter PinCode: ");
            var pinCode = Console.ReadLine();

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
                }
            };

            await _studentService.AddStudentAsync(student);
            Console.WriteLine("Student added successfully.");
        }

        private async Task RemoveStudentAsync()
        {
            Console.Write("Enter Student ID to remove: ");
            var id = int.Parse(Console.ReadLine());
            await _studentService.RemoveStudentAsync(id);
            
        }

        private async Task DisplayStudentsAsync()
        {
            var students = await _studentService.GetAllStudentsAsync();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"ID",-3}| {"Name",-12}| {"Email",-20}| {"Date of Birth",-14}| {"Enrollment Date",-15}| {"Street",-13}| {"City",-10}| {"State",-10}| {"PinCode",-10}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('-', 120));

            foreach (var student in students)
            {
                Console.WriteLine($"{student.Id,-3}| {student.Name,-12}| {student.Email,-20}| {student.DateOfBirth.ToShortDateString(),-14}| {student.EnrollmentDate.ToShortDateString(),-15}| {student.Address?.Street,-13}| {student.Address?.City,-10}| {student.Address?.State,-10}| {student.Address?.PinCode,-10}");
            }
            Console.WriteLine(new string('-', 120));
            Console.WriteLine();
        }
    }
}


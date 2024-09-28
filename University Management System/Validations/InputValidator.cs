using System.Text.RegularExpressions;
using University_Management_System.Entities;

namespace University_Management_System.Validations
{
    public static class InputValidator
    {
        public static DateTime PromptForValidDate(string message)
        {
            DateTime date;
            string? input;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (!DateTime.TryParse(input, out date))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" - Invalid date format. Please enter a valid date.");
                    Console.ResetColor();
                }
            } while (!DateTime.TryParse(input, out date));

            return date;
        }
        public static string PromtAndValidateEmail(string message)
        {
            string? email;
            do
            {
                Console.Write(message);
                email = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(email) || !ValidateEmail(email))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   - Invalid email address format.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(email) || !ValidateEmail(email));

            return email;
        }

        private static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        public static string PromptForValidString(string message, int maxLength = 256)
        {
            string? input;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) || input.Length > maxLength)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("   - Input must be a non-empty string "));
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(input) || input.Length > maxLength);

            return input;
        }

        public static int PromptForValidInt(string message)
        {
            int number;
            string? input;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (!int.TryParse(input, out number))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   - Input must be an Integer.");
                    Console.ResetColor();
                }
            } while (!int.TryParse(input, out number));

            return number;
        }

        public static string PromptForValidCVV(string message)
        {
            string? cvv;
            do
            {
                cvv = PromptForValidString(message, 3);
                if (!ValidateCVV(cvv))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   - Invalid CVV.");
                    Console.ResetColor();
                }
            } while (!ValidateCVV(cvv));

            return cvv;
        }

        private static bool ValidateCVV(string cvv)
        {
            if (string.IsNullOrWhiteSpace(cvv))
                return false;
            string cvvPattern = @"^\d{3}$";
            return Regex.IsMatch(cvv, cvvPattern);
        }



        public static string PromptForValidCCNumber(string message)
        {
            string? ccNumber;
            do
            {
                ccNumber = PromptForValidString(message);
                if (!ValidateCCNumber(ccNumber))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   - Invalid Credit Card Number.");
                    Console.ResetColor();
                }
            } while (!ValidateCCNumber(ccNumber));

            return ccNumber;
        }

        private static bool ValidateCCNumber(string ccNumber)
        {
            if (string.IsNullOrWhiteSpace(ccNumber))
                return false;
            string ccPattern = @"^\d{16}$";
            return Regex.IsMatch(ccNumber, ccPattern);
        }

        public static int PromptForValidCourseId(List<Course> courses)
        {
            int courseId;
            while (true)
            {
                courseId = PromptForValidInt("Enter Course Id to Add:");
                if (courses.Any(c => c.Id == courseId))
                {
                    break;
                }
                Console.WriteLine("Invalid Course ID. Please enter a valid Course ID from the list.");
            }
            return courseId;
        }

        public static string PromptForValidBankAccountNumber(string message)
        {
            string? accountNumber;
            do
            {
                accountNumber = PromptForValidString(message);
                if (!ValidateBankAccountNumber(accountNumber))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   - Invalid Bank Account Number.");
                    Console.ResetColor();
                }
            } while (!ValidateBankAccountNumber(accountNumber));

            return accountNumber;

        }

        private static bool ValidateBankAccountNumber(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return false;
            string accountPattern = @"^\d{10}$";
            return Regex.IsMatch(accountNumber, accountPattern);
        }

        public static string PromptForValidIFSCCode(string message)
        {
            string? ifscCode;
            do
            {
                ifscCode = PromptForValidString(message);
                if (!ValidateIFSCCode(ifscCode))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   - Invalid IFSC Code.");
                    Console.ResetColor();
                }
            } while (!ValidateIFSCCode(ifscCode));

            return ifscCode;
        }

        private static bool ValidateIFSCCode(string ifscCode)
        {
            if (string.IsNullOrWhiteSpace(ifscCode))
                return false;
            string ifscPattern = @"^[A-Z]{4}0[A-Z0-9]{6}$";
            return Regex.IsMatch(ifscCode, ifscPattern);
        }

    }
}

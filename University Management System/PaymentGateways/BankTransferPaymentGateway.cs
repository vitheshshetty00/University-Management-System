using University_Management_System.Entities;
using University_Management_System.Services.Interfaces;
using static University_Management_System.Validations.InputValidator;


namespace University_Management_System.PaymentGateways
{
    class BankTransferPaymentGateway : PaymentGateway
    {
        private BankTransferPayment Payment;
        public BankTransferPaymentGateway(IPaymentService dbPaymentService) : base(dbPaymentService)
        {
        }
        public override async Task<bool> ProcessPaymentAsync(decimal amount, int studentId)
        {
            try
            {
                string bankName = PromptForValidString("Enter the Bank Name:");
                string bankAccountNumber = PromptForValidBankAccountNumber("Enter the Account number:");
                string ifscCode = PromptForValidIFSCCode("Enter the IFSC Code:");
                string accountHolderName = PromptForValidString("Enter the Account Holder Name:");

                Payment = new BankTransferPayment
                {
                    Amount = amount,
                    StudentId = studentId,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = PaymentMethod.BankTransfer.ToString(),
                    Status = PaymentStatus.Completed,
                    BankName = bankName,
                    AccountNumber = bankAccountNumber,
                    IFSCCode = ifscCode,
                    AccountHolderName = accountHolderName
                };
                await _dbPaymentService.AddPaymentAsync(Payment);

                Console.WriteLine($"{amount} has been paid using Using Bank Account.");

                return true;

            }
            catch (Exception ex)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Credit Card Payment: An error occurred while processing the payment : {ex.Message}");
                Console.ResetColor();
                return false;
            }

        }
    }
}

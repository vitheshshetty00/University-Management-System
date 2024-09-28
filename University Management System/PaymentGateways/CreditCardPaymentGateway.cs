using University_Management_System.Entities;
using University_Management_System.Services.Interfaces;
using static University_Management_System.Validations.InputValidator;


namespace University_Management_System.PaymentGateways
{
    class CreditCardPaymentGateway : PaymentGateway
    {
        private CreditCardPayment Payment { get; set; }
        public CreditCardPaymentGateway(IPaymentService dbPaymentService) : base(dbPaymentService)
        {
        }
        public override async Task<bool> ProcessPaymentAsync(decimal amount, int studentId)
        {
            try
            {
                string cardHolderName = PromptForValidString("Enter Card Holder Name: ");
                string cardNumber = PromptForValidCCNumber("Enter Credit Card Number: ");
                DateTime expiryDate = PromptForValidDate("Enter Expiry Date: ");
                if (expiryDate < DateTime.Now)
                {
                    throw new Exception("Expiry date cannot be in the past");
                }
                string cvv = PromptForValidCVV("Enter CVV: ");

                Payment = new CreditCardPayment
                {
                    Amount = amount,
                    StudentId = studentId,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = PaymentMethod.CreditCard.ToString(),
                    Status = PaymentStatus.Completed,
                    CardHolderName = cardHolderName,
                    CardNumber = cardNumber,
                    ExpiryDate = expiryDate,
                    CVV = cvv
                };

                await _dbPaymentService.AddPaymentAsync(Payment);

                Console.WriteLine($"{amount} has been paid using credit card");
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

using University_Management_System.Data;
using University_Management_System.Entities;
using University_Management_System.Services.Interfaces;

namespace University_Management_System.Services.Implementations
{
    public class DbPaymentService : IPaymentService
    {
        private readonly UniversityDbContext _dbContext;
        public DbPaymentService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddPaymentAsync(Payment payment)
        {
            try
            {
                _dbContext.Payments.Add(payment);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred while adding the payment: {ex.Message}");
                Console.ResetColor();

            }
        }
    }
}

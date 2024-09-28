using University_Management_System.Entities;

namespace University_Management_System.Services.Interfaces
{
    public interface IPaymentService
    {
        Task AddPaymentAsync(Payment payment);
    }
}

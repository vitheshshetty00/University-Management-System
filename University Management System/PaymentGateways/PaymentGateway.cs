using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University_Management_System.Entities;
using University_Management_System.Services.Interfaces;

namespace University_Management_System.PaymentGateways
{
    public abstract class PaymentGateway
    {
        protected readonly IPaymentService _dbPaymentService;
        public PaymentGateway(IPaymentService dbPaymentService)
        {
            _dbPaymentService = dbPaymentService;
        }
        public abstract Task<bool> ProcessPaymentAsync(decimal amount, int studentId);
    }
   
}

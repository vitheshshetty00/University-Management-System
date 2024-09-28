using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_Management_System.Entities
{
    public abstract class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [ForeignKey("StudentId")]
        public int StudentId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string? PaymentMethod { get; set; } //Discriminator column 

        [Required]
        public PaymentStatus? Status { get; set; }

        public Student Student { get; set; }
    }

    public class CreditCardPayment : Payment
    {
        [Required]
        [StringLength(16)]
        public string? CardNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string? CardHolderName { get; set; }

        [Required]
        public DateTime? ExpiryDate { get; set; }

        [Required]
        [StringLength(4)]
        public string? CVV { get; set; }
    }

    public class BankTransferPayment : Payment
    {
        [Required]
        [StringLength(50)]
        public string? BankName { get; set; }

        [Required]
        [StringLength(20)]
        public string? AccountNumber { get; set; }

        [Required]
        [StringLength(15)]
        public string? IFSCCode { get; set; }

        [Required]
        [StringLength(50)]
        public string? AccountHolderName { get; set; }
    }
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }
    enum PaymentMethod
    {
        CreditCard,
        BankTransfer
    }
}

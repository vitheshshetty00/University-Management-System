﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using University_Management_System.Data;

#nullable disable

namespace University_Management_System.Migrations
{
    [DbContext(typeof(UniversityDbContext))]
    [Migration("20240928010859_AddPayments1")]
    partial class AddPayments1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("University_Management_System.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<int>("FacultyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("University_Management_System.Entities.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("University_Management_System.Entities.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("PaymentId");

                    b.HasIndex("StudentId");

                    b.ToTable("Payments");

                    b.HasDiscriminator<string>("PaymentMethod").HasValue("Payment");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("University_Management_System.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Fees")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("PaymentStatus")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("University_Management_System.Entities.StudentCourse", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.HasKey("StudentId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("StudentCourses");
                });

            modelBuilder.Entity("University_Management_System.Entities.BankTransferPayment", b =>
                {
                    b.HasBaseType("University_Management_System.Entities.Payment");

                    b.Property<string>("AccountHolderName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IFSCCode")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasDiscriminator().HasValue("BankTransfer");
                });

            modelBuilder.Entity("University_Management_System.Entities.CreditCardPayment", b =>
                {
                    b.HasBaseType("University_Management_System.Entities.Payment");

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("CardHolderName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("CreditCard");
                });

            modelBuilder.Entity("University_Management_System.Entities.Course", b =>
                {
                    b.HasOne("University_Management_System.Entities.Faculty", "Faculty")
                        .WithMany("CoursesTaught")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("University_Management_System.Entities.Faculty", b =>
                {
                    b.OwnsOne("University_Management_System.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<int>("FacultyId")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PinCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("FacultyId");

                            b1.ToTable("Faculties");

                            b1.WithOwner()
                                .HasForeignKey("FacultyId");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("University_Management_System.Entities.Payment", b =>
                {
                    b.HasOne("University_Management_System.Entities.Student", "Student")
                        .WithMany("Payments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("University_Management_System.Entities.Student", b =>
                {
                    b.OwnsOne("University_Management_System.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<int>("StudentId")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PinCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("StudentId");

                            b1.ToTable("Students");

                            b1.WithOwner()
                                .HasForeignKey("StudentId");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("University_Management_System.Entities.StudentCourse", b =>
                {
                    b.HasOne("University_Management_System.Entities.Course", "Course")
                        .WithMany("StudentCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("University_Management_System.Entities.Student", "Student")
                        .WithMany("StudentCourses")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("University_Management_System.Entities.Course", b =>
                {
                    b.Navigation("StudentCourses");
                });

            modelBuilder.Entity("University_Management_System.Entities.Faculty", b =>
                {
                    b.Navigation("CoursesTaught");
                });

            modelBuilder.Entity("University_Management_System.Entities.Student", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("StudentCourses");
                });
#pragma warning restore 612, 618
        }
    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Models
{
    public class AppointmentModel
    {
        [DisplayName("Patient's Full Name")]
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string PatientName { get; set; }

        [DisplayName("Appointment Request Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        [DisplayName("Patient's approximate net worth")]
        [Required]
        [DataType(DataType.Currency)]
        [Range(90000, double.MaxValue, ErrorMessage = "Doctors refuse to see patients unless their net worth is more than $90,000.")]
        public decimal PatientNetWorth { get; set; }

        [DisplayName("Primary Doctor's Last Name")]
        public string DoctorName { get; set; }

        [DisplayName("Patient's perceived level of pain (1 low to 10 high)")]
        [Required]
        [Range(6, 10, ErrorMessage = "Doctors refuse to see patients unless their pain level is above a 5.")]
        public int PainLevel { get; set; }

        [DisplayName("Street Address")]
        [Required]
        public string Street { get; set; }

        [DisplayName("City")]
        [Required]
        public string City { get; set; }

        [DisplayName("ZIP Code")]
        [Required]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid ZIP Code.")]
        public string ZIP { get; set; }

        [DisplayName("Email Address")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        [Required]
        [Phone]
        public string Phone { get; set; }

        // Updated Parameterized Constructor to Include All Properties
        public AppointmentModel(string patientName, DateTime dateTime, decimal patientNetWorth, string doctorName, int painLevel, string street, string city, string zip, string email, string phone)
        {
            PatientName = patientName;
            DateTime = dateTime;
            PatientNetWorth = patientNetWorth;
            DoctorName = doctorName;
            PainLevel = painLevel;
            Street = street;
            City = city;
            ZIP = zip;
            Email = email;
            Phone = phone;
        }

        // Empty constructor
        public AppointmentModel() { }
    }
}

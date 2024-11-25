using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Garage_3._0.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "The Personal Number must be exactly 12 characters long.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "The Personal Number must be exactly 12 digits.")]
        public string PersonalNumber { get; set; }

        public ICollection<ParkedVehicle> ParkedVehicles { get; set; }

        public int Age
        {
            get
            {
                if (!string.IsNullOrEmpty(PersonalNumber) && PersonalNumber.Length == 12)
                {
                    if (DateTime.TryParseExact(PersonalNumber.Substring(0, 8), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var birthDate))
                    {
                        var today = DateTime.Today;
                        var age = today.Year - birthDate.Year;

                        // Check if the birthday has occurred this year
                        if (birthDate.Date > today.AddYears(-age)) age--;

                        return age;
                    }
                }
                return 0;
            }
        }
    }

}
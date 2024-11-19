using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Garage_3._0.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Personal number is required.")]
        [RegularExpression(@"^\d{8}-\d{4}$", ErrorMessage = "Personal number format is incorrect (e.g., YYYYMMDD-XXXX).")]
        public string PersonalNumber { get; set; }

        public ICollection<ParkedVehicle> ParkedVehicles { get; set; }
        public int Age
        {
            get
            {
                if (!string.IsNullOrEmpty(PersonalNumber) && PersonalNumber.Length >= 8)
                {
                    if (DateTime.TryParseExact(PersonalNumber.Substring(0, 8), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var birthDate))
                    {
                        var today = DateTime.Today;
                        var age = today.Year - birthDate.Year;
                        if (birthDate.Date > today.AddYears(-age)) age--;
                        return age;
                    }
                }
                return 0; 
            }
        }
    }

}

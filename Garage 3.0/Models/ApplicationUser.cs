﻿using Microsoft.AspNetCore.Identity;

namespace Garage_3._0.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }

        public ICollection<ParkedVehicle> ParkedVehicles { get; set; }
        public (int Years, int Days) Age
        {
            get
            {
                if (!string.IsNullOrEmpty(PersonalNumber) && PersonalNumber.Length >= 8)
                {
                    if (DateTime.TryParseExact(PersonalNumber.Substring(0, 8), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var birthDate))
                    {
                        var today = DateTime.Today;
                        var age = today.Year - birthDate.Year;
                        var lastBirthday = birthDate.AddYears(age);

                        if (lastBirthday > today)
                        {
                            age--;
                            lastBirthday = lastBirthday.AddYears(-1);
                        }

                        var days = (today - lastBirthday).Days;
                        return (age, days);
                    }
                }
                return (0, 0);
            }
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Garage_3._0.Models
{
    public class ValidVehicleTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is VehicleType vehicleType)
            {
                // Add your valid vehicle types here
                string[] validTypes = { "Car", "Motorcycle", "Bus", "Truck" };
                if (Array.Exists(validTypes, t => t.Equals(vehicleType.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(ErrorMessage);
        }
    }

    public class ValidVehicleBrandAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string brand)
            {
                string[] validBrands = { "Toyota", "Ford", "Honda", "Chevrolet", "Volkswagen", "BMW", "Mercedes-Benz", "Audi", "Nissan", "Hyundai", "Kia", "Volvo", "Mazda", "Subaru", "Porsche" };

                if (Array.Exists(validBrands, b => b.Equals(brand, StringComparison.OrdinalIgnoreCase)))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(ErrorMessage);
        }
    }

    public class ValidVehicleColorAttribute : ValidationAttribute
    {
        private static readonly string[] ValidColors = new[]
        {
        "White", "Black", "Silver", "Gray", "Red",
        "Blue", "Green", "Yellow", "Orange", "Brown"
        };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string color)
            {
                // Trim and normalize the color input
                color = color.Trim();

                // Check for exact match (case-insensitive)
                if (ValidColors.Any(c => c.Equals(color, StringComparison.OrdinalIgnoreCase)))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}

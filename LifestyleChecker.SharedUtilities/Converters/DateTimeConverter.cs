namespace LifestyleChecker.SharedUtilities.Converters
{
    public static class DateTimeConverter
    {
        public static int ToAge(this DateTime dateOfBirth)
        {
            if (dateOfBirth == default)
                throw new ArgumentException("Date of birth cannot be default value.", nameof(dateOfBirth));
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age)) age--;
            return (int)age;
        }
    }
}

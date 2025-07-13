namespace LifestyleChecker.Services.PatientService.Structs
{
    public struct PatientLoginReturn
    {
        public bool valid;
        public string message;

        public PatientLoginReturn(bool valid = false, string message = "")
        {
            this.valid = valid;
            this.message = message;
        }
    }
}

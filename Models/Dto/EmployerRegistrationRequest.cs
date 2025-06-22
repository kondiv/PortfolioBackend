namespace Models.Dto
{
    public class EmployerRegistrationRequest : RegistrationRequest
    {
        public override string Role => "Employer";
    }
}

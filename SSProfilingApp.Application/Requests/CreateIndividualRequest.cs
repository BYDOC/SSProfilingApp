namespace SSProfilingApp.Application.Requests;
public class CreateIndividualRequest
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? BirthPlace { get; set; }
    public string? BirthDate { get; set; }
    public string? Nationality { get; set; }
    public string? IdentityNumber { get; set; }
}

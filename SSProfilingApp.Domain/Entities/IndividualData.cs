namespace SSProfilingApp.Domain.Entities;

public class IndividualData
{
    public int Id { get; set; }  // Primary Key

    public string? FirstName { get; set; }   
    public string? MiddleName { get; set; } 
    public string? LastName { get; set; } 
    public string? BirthPlace { get; set; } 
    public string? BirthDate { get; set; } 
    public string? Nationality { get; set; } 
    public string? IdentityNumber { get; set; } 
}

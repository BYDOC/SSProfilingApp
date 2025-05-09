namespace SSProfilingApp.Domain.Entities;

public class DataProfile
{
    public int Id { get; set; }  
    public int ProfileId { get; set; }  
    public int IndividualDataId { get; set; }
    public IndividualData? IndividualData { get; set; }
}

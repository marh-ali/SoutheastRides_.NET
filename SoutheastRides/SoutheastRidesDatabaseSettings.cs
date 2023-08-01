public class SoutheastRidesDatabaseSettings : ISoutheastRidesDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string UserCollectionName { get; set; }
    public string RideCollectionName { get; set; }
    public string RsvpCollectionName { get; set; }
}

public interface ISoutheastRidesDatabaseSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
    string UserCollectionName { get; set; }
    string RideCollectionName { get; set; }
    string RsvpCollectionName { get; set; }
}

namespace Kvadratai.Model
{
    /// <summary>
    /// Represents the settings required to connect to a MongoDB database.
    /// </summary>
    public class MongoDbSettings
    {
        /// <summary>
        /// Gets or sets the connection string for the MongoDB database.
        /// </summary>
        public string ConnectionString { get; set; } = null!;

        /// <summary>
        /// Gets or sets the connection string for the MongoDB database.
        /// </summary>
        public string DatabaseName { get; set; } = null!;
    }
}

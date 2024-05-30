using Kvadratai.Model;
using MongoDB.Driver;

namespace Kvadratai.Services
{
    /// <summary>
    /// Provides access to the MongoDB database and collections.
    /// </summary>
    public class PointsContext
    {
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="PointsContext"/> class.
        /// </summary>
        /// <param name="database">The MongoDB database instance.</param>
        public PointsContext(IMongoDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Gets the MongoDB collection of points.
        /// </summary>
        public IMongoCollection<Point> Points => _database.GetCollection<Point>("PointsCollection");
    }
}

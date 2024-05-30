using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Kvadratai.Model
{
    /// <summary>
    /// Represents a point in a 2D space.
    /// </summary>
    public class Point
    {
        /// <summary>
        /// Gets or sets the identifier for the point.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        /// <summary>
        /// Gets or sets the X coordinate of the point.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the point.
        /// </summary>
        public int Y { get; set; }

        public override string ToString()
        { 
            return this.X.ToString() +" " +this.Y.ToString();
        }

    }
}

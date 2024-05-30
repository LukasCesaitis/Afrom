
using Kvadratai.Model;

namespace Kvadratai.Services.Interfaces
{
    /// <summary>
    /// Defines the operations for managing points in the system.
    /// </summary>
    public interface IPointsService
    {
        /// <summary>
        /// Retrieves all points from the database.
        /// </summary>
        /// <returns>A list of points.</returns>
        Task<List<Point>> GetPointsAsync();

        /// <summary>
        /// Adds a new point to the database.
        /// </summary>
        /// <param name="point">The point to add.</param>
        /// <returns>The added point.</returns>
        Task<Point> AddPointAsync(Point point);

        /// <summary>
        /// Adds multiple points to the database.
        /// </summary>
        /// <param name="points">The list of points to add.</param>
        /// <returns>The added points.</returns>
        Task<List<Point>> AddPointsAsync(List<Point> points);

        /// <summary>
        /// Removes a point from the database.
        /// </summary>
        /// <param name="point">The point to remove.</param>
        /// <returns>True if the point was successfully removed; otherwise, false.</returns>
        Task<bool> RemovePointAsync(Point point);

        /// <summary>
        /// Counts the number of squares that can be formed with the given points.
        /// </summary>
        /// <param name="points">The list of points.</param>
        /// <returns>The number of squares.</returns>
        int CountSquares(List<Point> points);
    }
}

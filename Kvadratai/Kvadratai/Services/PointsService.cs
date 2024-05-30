using Kvadratai.Model;
using Kvadratai.Services.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Kvadratai.Services
{
    public class PointsService : IPointsService
    {
        private readonly PointsContext _context;
        private readonly ILogger<PointsService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PointsService"/> class.
        /// </summary>
        /// <param name="context">The PointsContext instance.</param>
        /// <param name="logger">The logger instance.</param>
        public PointsService(PointsContext context, ILogger<PointsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all points from the database.
        /// </summary>
        /// <returns>A list of points.</returns>
        public async Task<List<Point>> GetPointsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all points from the database.");
                var points = await _context.Points.Find(_ => true).ToListAsync();
                _logger.LogInformation("Successfully retrieved {Count} points.", points.Count);
                return points;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving points.");
                throw;
            }
        }

        /// <summary>
        /// Adds a new point to the database.
        /// </summary>
        /// <param name="point">The point to add.</param>
        /// <returns>The added point.</returns>
        public async Task<Point> AddPointAsync(Point point)
        {
            try
            {
                _logger.LogInformation("Adding a new point to the database: {Point}", point);
                await _context.Points.InsertOneAsync(point);
                _logger.LogInformation("Successfully added point: {Point}", point);
                return point;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a point.");
                throw;
            }
        }

        /// <summary>
        /// Adds a list of points to the database.
        /// </summary>
        /// <param name="points">The points to add.</param>
        /// <returns>The added points.</returns>
        public async Task<List<Point>> AddPointsAsync(List<Point> points)
        {
            try
            {
                _logger.LogInformation("Adding a list of points to the database.");
                await _context.Points.InsertManyAsync(points);
                _logger.LogInformation("Successfully added points to the database.");
                return points;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding points.");
                throw;
            }
        }

        /// <summary>
        /// Removes a point from the database.
        /// </summary>
        /// <param name="point">The point to remove.</param>
        /// <returns>True if the point was successfully removed; otherwise, false.</returns>
        public async Task<bool> RemovePointAsync(Point point)
        {
            try
            {
                _logger.LogInformation("Removing point from the database: {Point}", point);
                var result = await _context.Points.DeleteOneAsync(p => p.X == point.X && p.Y == point.Y);
                bool success = result.DeletedCount > 0;
                _logger.LogInformation("Successfully removed point: {Success}", success);
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing a point.");
                throw;
            }
        }

        /// <summary>
        /// Counts the number of squares that can be formed with the given points.
        /// </summary>
        /// <param name="points">The list of points.</param>
        /// <returns>The number of squares.</returns>
        public int CountSquares(List<Point> points)
        {
            var pointSet = new HashSet<(int, int)>(points.Select(p => (p.X, p.Y)));
            int count = 0;

            foreach (var p1 in points)
            {
                foreach (var p2 in points)
                {
                    if (p1.X == p2.X || p1.Y == p2.Y) continue;

                    var dx = Math.Abs(p1.X - p2.X);
                    var dy = Math.Abs(p1.Y - p2.Y);

                    if (dx != dy) continue;

                    if (pointSet.Contains((p1.X, p2.Y)) && pointSet.Contains((p2.X, p1.Y)))
                    {
                        count++;
                    }
                }
            }

            return count / 4;
        }
    }
}

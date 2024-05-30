using Kvadratai.Model;
using Kvadratai.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kvadratai.Controllers
{
    /// <summary>
    /// Controller for managing points.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly IPointsService _pointsService;
        private readonly ILogger<PointsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PointsController"/> class.
        /// </summary>
        /// <param name="pointsService">The points service.</param>
        /// <param name="logger">The logger.</param>
        public PointsController(IPointsService pointsService, ILogger<PointsController> logger)
        {
            _pointsService = pointsService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all points.
        /// </summary>
        /// <returns>A list of points.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Point>>> GetPoints()
        {
            return await _pointsService.GetPointsAsync();
        }

        /// <summary>
        /// Adds a new point.
        /// </summary>
        /// <param name="point">The point to add.</param>
        /// <returns>The added point.</returns>
        [HttpPost]
        public async Task<ActionResult<Point>> AddPoint(Point point)
        {
            var createdPoint = await _pointsService.AddPointAsync(point);
            return CreatedAtAction(nameof(GetPoints), new { x = createdPoint.X, y = createdPoint.Y }, createdPoint);
        }

        /// <summary>
        /// Adds multiple points.
        /// </summary>
        /// <param name="points">The list of points to add.</param>
        /// <returns>The added points.</returns>
        [HttpPost("AddPoints")]
        public async Task<ActionResult<List<Point>>> AddPoints(List<Point> points)
        {
            var createdPoints = await _pointsService.AddPointsAsync(points);
            return CreatedAtAction(nameof(GetPoints), createdPoints);
        }

        /// <summary>
        /// Removes a point.
        /// </summary>
        /// <param name="point">The point to remove.</param>
        /// <returns>A status indicating the result of the operation.</returns
        [HttpDelete]
        public async Task<IActionResult> RemovePoint(Point point)
        {
            var success = await _pointsService.RemovePointAsync(point);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Counts the number of squares that can be formed with the given points.
        /// </summary>
        /// <returns>The number of squares.</returns>
        [HttpGet("squares")]
        public async Task<ActionResult<int>> GetSquareCount()
        {
            var points = await _pointsService.GetPointsAsync();
            var squareCount = _pointsService.CountSquares(points);
            return squareCount;
        }
    }
}

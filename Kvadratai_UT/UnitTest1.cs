using Kvadratai.Model;
using Kvadratai.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kvadratai_UT
{
    /// <summary>
    /// Unit tests for the PointsService class.
    /// </summary>
    [TestClass]
    public class PointsServiceTests
    {
        private Mock<IMongoCollection<Point>> _mockCollection;
        private Mock<IMongoDatabase> _mockDatabase;
        private Mock<PointsContext> _mockContext;
        private Mock<ILogger<PointsService>> _mockLogger;
        private PointsService _pointsService;

        /// <summary>
        /// Initializes the test environment before each test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockCollection = new Mock<IMongoCollection<Point>>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockDatabase.Setup(db => db.GetCollection<Point>("PointsCollection", null))
                .Returns(_mockCollection.Object);
            _mockContext = new Mock<PointsContext>(_mockDatabase.Object);
            _mockLogger = new Mock<ILogger<PointsService>>();
            _pointsService = new PointsService(_mockContext.Object, _mockLogger.Object);
        }

        /// <summary>
        /// Tests that GetPointsAsync retrieves a list of points.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetPointsAsync_ReturnsListOfPoints()
        {
            // Arrange
            var points = new List<Point> { new Point { X = 1, Y = 2 }, new Point { X = 3, Y = 4 } };
            var mockCursor = new Mock<IAsyncCursor<Point>>();
            mockCursor.SetupSequence(_ => _.MoveNext(default))
                .Returns(true)
                .Returns(false);
            mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));
            mockCursor.Setup(_ => _.Current).Returns(points);

            _mockCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Point>>(), It.IsAny<FindOptions<Point, Point>>(), default))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _pointsService.GetPointsAsync();

            // Assert
            Assert.AreEqual(points.Count, result.Count);
        }

        /// <summary>
        /// Tests that AddPointAsync inserts a point.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AddPointAsync_InsertsPoint()
        {
            // Arrange
            var point = new Point { X = 1, Y = 2 };

            // Act
            var result = await _pointsService.AddPointAsync(point);

            // Assert
            _mockCollection.Verify(c => c.InsertOneAsync(point, null, default), Times.Once);
            Assert.AreEqual(point, result);
        }

        /// <summary>
        /// Tests that AddPointsAsync inserts multiple points.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AddPointsAsync_InsertsPoints()
        {
            // Arrange
            var points = new List<Point> { new Point { X = 1, Y = 2 }, new Point { X = 3, Y = 4 } };

            // Act
            var result = await _pointsService.AddPointsAsync(points);

            // Assert
            _mockCollection.Verify(c => c.InsertManyAsync(points, null, default), Times.Once);
            Assert.AreEqual(points, result);
        }

        /// <summary>
        /// Tests that RemovePointAsync removes a point.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task RemovePointAsync_RemovesPoint()
        {
            // Arrange
            var point = new Point { X = 1, Y = 2 };
            _mockCollection.Setup(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<Point>>(), default))
                .ReturnsAsync(new DeleteResult.Acknowledged(1));

            // Act
            var result = await _pointsService.RemovePointAsync(point);

            // Assert
            _mockCollection.Verify(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<Point>>(), default), Times.Once);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests that CountSquares returns the number of squares that can be formed with the given points.
        /// </summary>
        [TestMethod]
        public void CountSquares_ReturnsNumberOfSquares()
        {
            // Arrange
            var points = new List<Point>
            {
                /*
                new Point { X = 0, Y = 0 },
                new Point { X = 1, Y = 1 },
                new Point { X = 2, Y = 2 },
                new Point { X = 3, Y = 3 },
                new Point { X = 4, Y = 4 },
                new Point { X = 0, Y = 1 },
                new Point { X = 1, Y = 0 },
                new Point { X = 2, Y = 1 },
                new Point { X = 1, Y = 2 },
                new Point { X = 3, Y = 0 },
                new Point { X = 0, Y = 3 },
                new Point { X = 3, Y = 1 },
                new Point { X = 1, Y = 3 },
                new Point { X = 4, Y = 0 },
                new Point { X = 0, Y = 4 },
                new Point { X = 4, Y = 1 },
                new Point { X = 1, Y = 4 },
                new Point { X = 2, Y = 0 },
                new Point { X = 0, Y = 2 },
                new Point { X = 2, Y = 3 },
                new Point { X = 3, Y = 2 },
                new Point { X = 4, Y = 2 },
                new Point { X = 2, Y = 4 },
                new Point { X = 3, Y = 4 },
                new Point { X = 4, Y = 3 }
                */

                
                new Point { X = 1, Y = 1 },
                new Point { X = -1, Y = 1 },
                new Point { X = 1, Y = -1 },
                new Point { X = -1, Y = -1 },
                new Point { X = 2, Y = 2 },
                new Point { X = -2, Y = 2 },
                new Point { X = 2, Y = -2 },
                new Point { X = -2, Y = -2 },
                
            };

            // Act
            var result = _pointsService.CountSquares(points);

            // Assert
            Assert.AreEqual(2, result);
        }
    }
}

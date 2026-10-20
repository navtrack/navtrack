using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Trips;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Filters;
using Navtrack.Database.Services.Devices;
using Xunit;

namespace Navtrack.Api.Services.Tests.Trips;

public class TripServiceTests
{
    [Theory]
    [InlineData(0, false)] // Same start and end point.
    [InlineData(0.00045, false)] // Approximately 50 m apart.
    [InlineData(0.000895, false)] // 99 m apart in the distance calculator.
    [InlineData(0.0009, false)] // 100 m apart in the distance calculator.
    [InlineData(0.00091, true)] // 101 m apart in the distance calculator.
    [InlineData(0.00135, true)] // Approximately 150 m apart.
    public async Task GetTrips_FiltersByStartToEndDistance(double endLongitude, bool expectTrip)
    {
        DateTime date = new(2026, 10, 21, 1, 0, 0, DateTimeKind.Utc);
        List<DeviceMessageEntity> messages = [];

        // Travel over 300 m before returning towards the start, so the existing
        // minimum total distance filter cannot account for an ignored trip.
        for (int i = 0; i < 11; i++)
        {
            messages.Add(new DeviceMessageEntity
            {
                Date = date.AddSeconds(i * 30),
                Latitude = 0,
                Longitude = i == 10 ? endLongitude : i * 0.0005,
                Speed = 36
            });
        }

        Mock<IDeviceMessageRepository> repository = new();
        repository.Setup(x => x.GetMessages(It.IsAny<GetMessagesOptions>()))
            .ReturnsAsync(new GetMessagesResult
            {
                TotalCount = messages.Count,
                Messages = messages
            });
        TripService service = new(repository.Object);

        TripListModel result = await service.GetTrips(new AssetEntity { Id = Guid.NewGuid() },
            new TripFilterModel { Date = date.Date });

        if (expectTrip)
        {
            TripModel trip = Assert.Single(result.Items);
            Assert.Equal(endLongitude, trip.EndPosition.Coordinates.Longitude);
            Assert.True(trip.Distance > 300);
        }
        else
        {
            Assert.Empty(result.Items);
        }
    }
}

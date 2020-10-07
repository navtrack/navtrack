using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Util;

namespace Navtrack.DataAccess.Services
{
    [Service(typeof(ITripDataService))]
    public class TripDataService : ITripDataService
    {
        private readonly IRepository repository;

        public TripDataService(IRepository repository)
        {
            this.repository = repository;
        }

        // public async Task<int> GetMedianTimeSpanBetweenLocations(int assetId)
        // {
        //     List<LocationEntity> locations = await repository.GetEntities<LocationEntity>()
        //         .Where(x => x.AssetId == assetId)
        //         .OrderByDescending(x => x.DateTime)
        //         .Take(100)
        //         .ToListAsync();
        //
        //     List<double> seconds = new List<double>();
        //
        //     for (int i = 1; i < locations.Count; i++)
        //     {
        //         seconds.Add((locations[i - 1].DateTime - locations[i].DateTime).TotalSeconds);
        //     }
        //
        //     seconds = seconds.OrderBy(x => x).ToList();
        //
        //     return (int) seconds[seconds.Count / 2] * 2;
        // }
        //
        // public async Task<List<TripWithLocations>> GetTrips(int assetId)
        // {

        // }
        //
        // public async Task UpdateTrips(int assetId)
        // {
        //     using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
        //     
        //     TripEntity lastTrip = await unitOfWork.GetEntities<TripEntity>()
        //         .OrderByDescending(x => x.Number)
        //         .FirstOrDefaultAsync(x => x.AssetId == assetId);
        //     
        //     List<TripEntity> newTrips = new List<TripEntity>();
        //     List<LocationEntity> locations;
        //     
        //     do
        //     {
        //         DateTime lastDate = lastTrip?.EndDateTime ?? DateTime.MinValue;
        //
        //         locations = await unitOfWork.GetEntities<LocationEntity>()
        //             .OrderBy(x => x.DateTime)
        //             .Where(x => x.AssetId == assetId && x.DateTime > lastDate)
        //             .Take(1000)
        //             .ToListAsync();
        //         
        //         foreach (LocationEntity location in locations)
        //         {
        //             lastDate = lastTrip?.EndDateTime ?? DateTime.MinValue;
        //
        //             TimeSpan timeSpan = location.DateTime - lastDate;
        //
        //             if (timeSpan.TotalMinutes > 10)
        //             {
        //                 lastTrip = new TripEntity
        //                 {
        //                     AssetId = assetId,
        //                     StartDateTime = location.DateTime,
        //                     EndDateTime = location.DateTime,
        //                     Number = (lastTrip?.Number).GetValueOrDefault()+1
        //                 };
        //
        //                 newTrips.Add(lastTrip);
        //             }
        //             else
        //             {
        //                 if (lastTrip == null)
        //                 {
        //                     lastTrip = new TripEntity
        //                     {
        //                         AssetId = assetId,
        //                         StartDateTime = location.DateTime,
        //                         EndDateTime = location.DateTime,
        //                         Number = 1
        //                     };
        //
        //                     newTrips.Add(lastTrip);
        //                 }
        //                 else
        //                 {
        //                     lastTrip.EndDateTime = location.DateTime;
        //                 }
        //             }
        //         }
        //     } while (locations.Count > 0);
        //
        //     unitOfWork.AddRange(newTrips);
        //
        //     await unitOfWork.SaveChanges();
        // }
        //
        // private IEnumerable<TripWithLocations> MapTrips(IEnumerable<LocationEntity> locations,
        //     int medianTimeSpanBetweenLocations)
        // {
        //     List<TripWithLocations> trips = new List<TripWithLocations>();
        //
        //     TripWithLocations lastTripWithLocations = null;
        //     LocationEntity lastLocation = null;
        //
        //     foreach (LocationEntity location in locations)
        //     {
        //         TimeSpan? timeSpan = TimeSpan.Zero;;// GetTimeSpan(lastTrip, location);
        //
        //         if ((lastTripWithLocations == null || timeSpan == null ||
        //              timeSpan.Value.TotalSeconds > medianTimeSpanBetweenLocations) &&
        //             DifferentConnectionId(lastLocation, location))
        //         {
        //             lastTripWithLocations = new TripWithLocations();
        //             trips.Add(lastTripWithLocations);
        //         }
        //
        //         lastTripWithLocations?.Locations.Add(location);
        //         lastLocation = location;
        //     }
        //
        //     return trips;
        // }
        //
        // private static bool DifferentConnectionId(LocationEntity lastLocation, LocationEntity location)
        // {
        //     if (lastLocation?.ConnectionMessage == null)
        //     {
        //         return true;
        //     }
        //
        //     if (location?.ConnectionMessage == null)
        //     {
        //         return true;
        //     }
        //
        //     return lastLocation.ConnectionMessage.DeviceConnectionId != location.ConnectionMessage.DeviceConnectionId;
        // }

        // private static TimeSpan? GetTimeSpan(Trip lastTrip, LocationEntity nextLocation)
        // {
        //     LocationModel lastLocation = lastTrip?.Locations.LastOrDefault();
        //
        //     if (lastLocation != null)
        //     {
        //         TimeSpan timeSpan = nextLocation.DateTime - lastLocation.DateTime;
        //
        //         return timeSpan;
        //     }
        //
        //     return null;
        // }
        public async Task UpdateTrips(int assetId)
        {
            TripEntity lastTrip = await repository.GetEntities<TripEntity>()
                .Include(x => x.EndLocation)
                .Where(x => x.AssetId == assetId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            List<LocationEntity> locations;
            List<TripEntity> trips = InitTripsList(lastTrip);

            do
            {
                DateTime lastLocationDate = GetLastLocationDate(trips);

                locations = await repository.GetEntities<LocationEntity>()
                    .Where(x => x.AssetId == assetId && x.DateTime > lastLocationDate)
                    .OrderBy(x => x.DateTime)
                    .Take(1000)
                    .ToListAsync();

                foreach (LocationEntity location in locations)
                {
                    AddLocationToTrip(trips, location);
                }
            } while (locations.Count > 0);

            await SaveTrips(assetId, trips);
        }

        private async Task SaveTrips(int assetId, List<TripEntity> trips)
        {
            List<TripEntity> newTrips = trips.Where(x => x.Id == 0).ToList();
            
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            newTrips.ForEach(x =>
            {
                x.StartLocation = null;
                x.EndLocation = null;
                x.AssetId = assetId;
            });
            unitOfWork.AddRange(newTrips);
            await unitOfWork.SaveChanges();
        }

        public async Task<List<TripWithLocations>> GetTrips(int assetId)
        {
            List<TripEntity> trips = await repository.GetEntities<TripEntity>()
                .Include(x => x.StartLocation)
                .Include(x => x.EndLocation)
                .Where(x => x.AssetId == assetId)
                .OrderByDescending(x => x.Id)
                .Take(500)
                .ToListAsync();

            DateTime startDate = trips.Min(x => x.StartLocation.DateTime);
            DateTime endDate = trips.Max(x => x.EndLocation.DateTime);

            List<LocationEntity> locations = await repository.GetEntities<LocationEntity>()
                .Include(x => x.ConnectionMessage)
                .Where(x => x.AssetId == assetId && x.DateTime >= startDate && x.DateTime <= endDate)
                .OrderBy(x => x.DateTime)
                .ToListAsync();

            List<TripWithLocations> tripWithLocationses = trips.Select(x => new TripWithLocations
            {
                Trip = x,
                Locations = locations.Where(y =>
                    y.DateTime >= x.StartLocation.DateTime && y.DateTime <= x.EndLocation.DateTime).ToList()
            }).ToList();

            return tripWithLocationses;
        }

        private static void AddLocationToTrip(List<TripEntity> trips, LocationEntity location)
        {
            TripEntity tripEntity = trips.LastOrDefault();

            if (tripEntity == null)
            {
                trips.Add(CreateTrip(location, 1));
            }
            else
            {
                TimeSpan timeSpan = location.DateTime - tripEntity.EndLocation.DateTime;

                if (timeSpan.TotalMinutes > 10)
                {
                    trips.Add(CreateTrip(location, tripEntity.Number + 1));
                }
                else
                {
                    tripEntity.Distance += DistanceCalculator.CalculateDistance((tripEntity.EndLocation.Latitude,
                            tripEntity.EndLocation.Longitude, tripEntity.EndLocation.Odometer),
                        (location.Latitude, location.Longitude, location.Odometer));
                    tripEntity.EndLocation = location;
                    tripEntity.EndLocationId = location.Id;
                }
            }
        }

        private static TripEntity CreateTrip(LocationEntity location, int number)
        {
            return new TripEntity
            {
                Number = number,
                StartLocation = location,
                StartLocationId = location.Id,
                EndLocation = location,
                EndLocationId = location.Id
            };
        }

        private static DateTime GetLastLocationDate(List<TripEntity> trips)
        {
            TripEntity tripEntity = trips.LastOrDefault();

            return tripEntity?.EndLocation.DateTime ?? DateTime.MinValue;
        }

        private static List<TripEntity> InitTripsList(TripEntity lastTrip)
        {
            return lastTrip != null ? new List<TripEntity> {lastTrip} : new List<TripEntity>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services.TaskQueue;
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

        private static readonly CategorizedTaskQueue<int> UpdateTripsQueue = new CategorizedTaskQueue<int>();

        public async Task UpdateTrips(int assetId)
        {
            await UpdateTripsQueue.Enqueue(assetId, () => ExecuteUpdateTrips(assetId));
        }

        private async Task ExecuteUpdateTrips(int assetId)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            TripEntity lastTrip = await unitOfWork.GetEntities<TripEntity>()
                .Include(x => x.EndLocation)
                .Where(x => x.AssetId == assetId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            List<LocationEntity> locations;
            List<TripEntity> trips = InitTripsList(lastTrip);

            do
            {
                DateTime lastLocationDate = GetLastLocationDate(trips);

                locations = await unitOfWork.GetEntities<LocationEntity>()
                    .Where(x => x.AssetId == assetId && x.DateTime > lastLocationDate)
                    .OrderBy(x => x.DateTime)
                    .Take(1000)
                    .ToListAsync();

                foreach (LocationEntity location in locations)
                {
                    AddLocationToTrip(trips, location);
                }
            } while (locations.Count > 0);

            await SaveTrips(assetId, trips, unitOfWork);
        }

        private static async Task SaveTrips(int assetId, List<TripEntity> trips, IUnitOfWork unitOfWork)
        {
            List<TripEntity> newTrips = trips.Where(x => x.Id == 0).ToList();

            newTrips.ForEach(x =>
            {
                x.StartLocation = null;
                x.EndLocation = null;
                x.AssetId = assetId;
            });
            unitOfWork.AddRange(newTrips);
            await unitOfWork.SaveChanges();
        }

        public async Task<List<TripWithLocations>> GetTrips(int assetId, LocationFilter locationFilter)
        {
            IQueryable<TripEntity> queryable = repository.GetEntities<TripEntity>()
                .OrderByDescending(x => x.Id)
                .Include(x => x.StartLocation)
                .Include(x => x.EndLocation)
                .Where(x => x.AssetId == assetId);

            queryable = ApplyFiltering(queryable, locationFilter);
            queryable = queryable.Take(100);

            List<TripEntity> trips = await queryable
                .ToListAsync();

            if (trips.Any())
            {
                DateTime startDate = trips.Min(x => x.StartLocation.DateTime);
                DateTime endDate = trips.Max(x => x.EndLocation.DateTime);

                List<LocationEntity> locations = await repository.GetEntities<LocationEntity>()
                    .Include(x => x.ConnectionMessage)
                    .Where(x => x.AssetId == assetId && x.DateTime >= startDate && x.DateTime <= endDate)
                    .OrderBy(x => x.DateTime)
                    .ToListAsync();

                List<TripWithLocations> tripWithLocations = trips.Select(x => new TripWithLocations
                {
                    Trip = x,
                    Locations = locations.Where(y =>
                        y.DateTime >= x.StartLocation.DateTime && y.DateTime <= x.EndLocation.DateTime).ToList()
                }).ToList();

                return tripWithLocations;
            }

            return Enumerable.Empty<TripWithLocations>().ToList();
        }

        private IQueryable<TripEntity> ApplyFiltering(IQueryable<TripEntity> queryable, LocationFilter locationFilter)
        {
            if (locationFilter.StartDate.HasValue)
            {
                queryable = queryable.Where(x => x.StartLocation.DateTime >= locationFilter.StartDate.Value);
            }

            if (locationFilter.EndDate.HasValue)
            {
                queryable = queryable.Where(x => x.EndLocation.DateTime <= locationFilter.EndDate.Value);
            }

            // TODO 
            // if (locationFilter.MinSpeed.HasValue)
            // {
            //     queryable = queryable.Where(x => x.Speed >= locationFilter.MinSpeed.Value);
            // }
            //
            // if (locationFilter.MaxSpeed.HasValue)
            // {
            //     queryable = queryable.Where(x => x.Speed <= locationFilter.MaxSpeed.Value);
            // }
            //
            // if (locationFilter.MinAltitude.HasValue)
            // {
            //     queryable = queryable.Where(x => x.Altitude >= locationFilter.MinAltitude.Value);
            // }
            //
            // if (locationFilter.MaxAltitude.HasValue)
            // {
            //     queryable = queryable.Where(x => x.Altitude <= locationFilter.MaxAltitude.Value);
            // }

            return queryable;
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
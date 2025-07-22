using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Reports;

[Service(typeof(IReportRepository))]
public class ReportRepository(IRepository repository) : IReportRepository
{
    public async Task<List<DistanceReportItem>> GetDistanceReportItems(ObjectId assetId, DateTime startDate,
        DateTime endDate)
    {
        PipelineDefinition<DeviceMessageDocument, DistanceReportItem> pipeline = new[]
        {
            new BsonDocument("$match",
                new BsonDocument
                {
                    { "md.aid", assetId },
                    {
                        "pos.d", new BsonDocument
                        {
                            { "$gte", startDate },
                            { "$lt", endDate }
                        }
                    }
                }),
            new BsonDocument("$sort", new BsonDocument("pos.d", 1)),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", new BsonDocument("$dayOfYear", "$pos.d") },
                { "maxSpeed", new BsonDocument("$max", "$pos.spd") },
                {
                    "averageSpeed", new BsonDocument("$avg", new BsonDocument("$cond", new BsonDocument
                    {
                        { "if", new BsonDocument("$gt", new BsonArray { "$pos.spd", 0 }) },
                        { "then", "$pos.spd" },
                        { "else", BsonNull.Value }
                    }))
                },
                { "first", new BsonDocument("$first", "$$ROOT") },
                { "last", new BsonDocument("$last", "$$ROOT") }
            }),
            new BsonDocument("$project",
                new BsonDocument
                {
                    { "date", "$first.pos.d" },
                    { "maxSpeed", "$maxSpeed" },
                    { "averageSpeed", "$averageSpeed" },
                    {
                        "duration", new BsonDocument("$subtract",
                            new BsonArray
                            {
                                "$last.veh.ignd",
                                "$first.veh.ignd"
                            })
                    },
                    {
                        "distance",
                        new BsonDocument("$subtract",
                            new BsonArray
                            {
                                "$last.dev.odo",
                                "$first.dev.odo"
                            })
                    }
                })
        };

        List<DistanceReportItem> result = await repository
            .GetCollection<DeviceMessageDocument>()
            .Aggregate(pipeline)
            .ToListAsync();

        return result;
    }

    public async Task<List<FuelConsumptionReportItem>> GetFuelConsumptionReportItems(ObjectId assetId, DateTime startDate,
        DateTime endDate)
    {
        PipelineDefinition<DeviceMessageDocument, FuelConsumptionReportItem> pipeline = new[]
        {
            new BsonDocument("$match",
                new BsonDocument
                {
                    { "md.aid", assetId },
                    {
                        "pos.d",
                        new BsonDocument
                        {
                            { "$gte", startDate },
                            { "$lte", endDate }
                        }
                    },
                    { "veh.fuelc", new BsonDocument("$exists", true) }
                }),
            new BsonDocument("$sort", new BsonDocument("pos.d", 1)),
            new BsonDocument("$group",
                new BsonDocument
                {
                    { "_id", new BsonDocument("$dayOfYear", "$pos.d") },
                    {
                        "averageSpeed", new BsonDocument("$avg", new BsonDocument("$cond", new BsonDocument
                        {
                            { "if", new BsonDocument("$gt", new BsonArray { "$pos.spd", 0 }) },
                            { "then", "$pos.spd" },
                            { "else", BsonNull.Value }
                        }))
                    },
                    {
                        "first",
                        new BsonDocument("$first", "$$ROOT")
                    },
                    {
                        "last",
                        new BsonDocument("$last", "$$ROOT")
                    }
                }),
            new BsonDocument("$project",
                new BsonDocument
                {
                    { "date", "$first.pos.d" },
                    { "averageSpeed", "$averageSpeed" },
                    {
                        "duration", new BsonDocument("$subtract",
                            new BsonArray
                            {
                                "$last.veh.ignd",
                                "$first.veh.ignd"
                            })
                    },
                    {
                        "distance",
                        new BsonDocument("$subtract",
                            new BsonArray
                            {
                                "$last.dev.odo",
                                "$first.dev.odo"
                            })
                    },
                    {
                        "fuelConsumption",
                        new BsonDocument("$subtract",
                            new BsonArray
                            {
                                "$last.veh.fuelc",
                                "$first.veh.fuelc"
                            })
                    }
                })
        };

        List<FuelConsumptionReportItem> result = await repository
            .GetCollection<DeviceMessageDocument>()
            .Aggregate(pipeline)
            .ToListAsync();

        return result;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Navtrack.Api.Services.Common.Mappers;

public static class ListMapper
{
    public static Model.Common.ListModel<TOut> Map<TIn,TOut>(IEnumerable<TIn> items, Func<TIn, TOut> mapper)
    {
        Model.Common.ListModel<TOut> list = new()
        {
            Items = items.Select(mapper).ToList()
        };

        return list;
    }
}
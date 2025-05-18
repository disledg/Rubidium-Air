using System;
using System.Collections.Generic;

namespace Rubidium
{
    public interface IAircraftMetricsRepo : IRepository<AircraftMetric>
    {
        List<AircraftMetric> GetByAircraftId(int aircraftId);
        List<AircraftMetric> GetByDateRange(int aircraftId, DateTime startDate, DateTime endDate);
        AircraftMetric GetLatestMetrics(int aircraftId);
    }
} 
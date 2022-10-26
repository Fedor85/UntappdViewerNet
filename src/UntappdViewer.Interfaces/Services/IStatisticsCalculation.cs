﻿using System.Collections.Generic;
using UntappdViewer.Models.Different;

namespace UntappdViewer.Interfaces.Services
{
    public interface IStatisticsCalculation
    {
        int BeerTypeCountByOther { get; }

        string DefaultServingType { get; }

        int GetCountrysCount();

        List<KeyValue<double, int>> GetChekinsRatingByCount();

        List<KeyValue<double, int>> GetBeersRatingByCount();

        List<KeyValue<string, int>> GetDateChekinsByCount();

        List<KeyValue<string, List<long>>> GetBeerTypesByCheckinIdsGroupByCount(int countByOther);

        List<KeyValue<string, List<long>>> GetCountrysByCheckinIds();

        List<KeyValue<string, List<long>>> GetServingTypeByCheckinIds(string defaultServingType);

        List<KeyValue<string, double>> GetAverageRatingByCheckinIds(List<KeyValue<string, List<long>>> checkinIds);

        List<KeyValue<double, double>> GetABVToIBU();

        List<KeyValueParam<string, int>> GetRangeABVByCount(double range, double maxValue);

        List<KeyValueParam<string, int>> GetRangeIBUByCount(double range, double maxValue);
    }
}
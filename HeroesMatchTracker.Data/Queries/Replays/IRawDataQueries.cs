using HeroesMatchTracker.Data.Models;
using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    /// <summary>
    /// Raw data queries
    /// </summary>
    /// <typeparam name="T">IReplayModelDataTable</typeparam>
    public interface IRawDataQueries<T>
        where T : IRawDataDisplay
    {
        List<T> ReadAllRecords();
        List<T> ReadTopRecords(int amount);
        List<T> ReadLastRecords(int amount);
        List<T> ReadRecordsCustomTop(int amount, string columnName, string orderBy);
        List<T> ReadRecordsWhere(string columnName, string operand, string input);
    }
}

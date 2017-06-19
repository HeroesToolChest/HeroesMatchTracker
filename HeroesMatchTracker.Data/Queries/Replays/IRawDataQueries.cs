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
        IEnumerable<T> ReadAllRecords();
        IEnumerable<T> ReadTopRecords(int amount);
        IEnumerable<T> ReadLastRecords(int amount);
        IEnumerable<T> ReadRecordsCustomTop(int amount, string columnName, string orderBy);
        IEnumerable<T> ReadRecordsWhere(string columnName, string operand, string input);
    }
}

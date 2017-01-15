using HeroesStatTracker.Data.Models;
using System.Collections.Generic;

namespace HeroesStatTracker.Data.Queries.Replays
{
    /// <summary>
    /// Raw data queries
    /// </summary>
    /// <typeparam name="T">IReplayModelDataTable</typeparam>
    internal interface IRawQueries<T> where T : IRawDataDisplay
    {
        List<T> ReadTopRecords(int amount);
        List<T> ReadLastRecords(int amount);
        List<T> ReadRecordsCustomTop(int amount, string columnName, string orderBy);
        List<T> ReadRecordsWhere(string columnName, string operand, string input);
    }
}

using HeroesStatTracker.Data.Replays.Models;
using System.Collections.Generic;

namespace HeroesStatTracker.Data.Replays.Queries
{
    /// <summary>
    /// Raw data queries
    /// </summary>
    /// <typeparam name="T">IReplayModelDataTable</typeparam>
    internal interface IRawQueries<T> where T : IReplayModelDataTable
    {
        List<T> ReadTopRecords(int amount);
        List<T> ReadLastRecords(int amount);
        List<T> ReadRecordsCustomTop(int amount, string columnName, string orderBy);
        List<T> ReadRecordsWhere(string columnName, string operand, string input);
    }
}

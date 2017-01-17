using GalaSoft.MvvmLight.CommandWpf;
using HeroesStatTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public abstract class RawDatabase<T> : HstViewModel
        where T : IRawDataDisplay
    {
        private long _rowsReturned;
        private string _queryStatus;

        private Dictionary<ButtonCommands, Action> ButtonCommandActions = new Dictionary<ButtonCommands, Action>();

        /// <summary>
        /// Constructor
        /// </summary>
        protected RawDatabase()
        {
            QueryStatus = "Awaiting query...";
            RowsReturned = 0;
        }

        private enum ButtonCommands
        {
            SelectAll,
            SelectTop100,
            SelectBottom100,
            SelectCustomTop,
            SelectCustomWhere,
        }

        /*public RelayCommand SelectAllCommand => new RelayCommand());*/

        public long RowsReturned
        {
            get { return _rowsReturned; }
            set
            {
                _rowsReturned = value;
                RaisePropertyChanged();
            }
        }

        public string QueryStatus
        {
            get { return _queryStatus; }
            set
            {
                _queryStatus = value;
                RaisePropertyChanged();
            }
        }

        private void AddButtonCommandActions()
        {
            // ButtonCommandActions.Add(ButtonCommands.SelectAll, )
        }
    }
}

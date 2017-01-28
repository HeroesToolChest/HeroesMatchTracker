using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Icons;
using HeroesStatTracker.Data.Models;
using HeroesStatTracker.Data.Queries.Replays;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public abstract class RawDataBase<T> : HstViewModel
        where T : IRawDataDisplay, new()
    {
        private bool _isQueryActive;
        private long _rowsReturned;
        private string _queryStatus;
        private int _selectedCustomTopCount;
        private string _selectedTopColumnName;
        private string _selectedTopOrderBy;
        private string _selectedWhereColumnName;
        private string _selectedWhereOperand;
        private string _selectedWhereTextBoxInput;

        private List<string> _columnNamesList = new List<string>();
        private List<string> _orderByList = new List<string>();
        private List<string> _conditionalOperandsList = new List<string>();

        private IRawDataQueries<T> IRawDataQueries;
        private Dictionary<ButtonCommands, Action> ButtonCommandActions;

        private ObservableCollection<T> _rawDataCollection = new ObservableCollection<T>();

        /// <summary>
        /// Contstructor
        /// </summary>
        /// <param name="iRawDataQueries"></param>
        protected RawDataBase(IRawDataQueries<T> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(heroesIcons)
        {
            AddColumnNamesList();
            AddOrderByList();
            AddConditionalOperandsList();
            AddButtonCommandsActions();

            IRawDataQueries = iRawDataQueries;
            IsQueryActive = true;
            QueryStatus = "Awaiting query...";
            RowsReturned = 0;
        }

        internal enum ButtonCommands
        {
            SelectAll,
            SelectTop100,
            SelectBottom100,
            SelectCustomTop,
            SelectCustomWhere,
        }

        public RelayCommand SelectAllCommand => new RelayCommand(() => ExecuteQueryCommand(ButtonCommands.SelectAll));
        public RelayCommand SelectTop100Command => new RelayCommand(() => ExecuteQueryCommand(ButtonCommands.SelectTop100));
        public RelayCommand SelectBottom100Command => new RelayCommand(() => ExecuteQueryCommand(ButtonCommands.SelectBottom100));
        public RelayCommand SelectCustomTopCommand => new RelayCommand(() => ExecuteQueryCommand(ButtonCommands.SelectCustomTop));
        public RelayCommand SelectCustomWhereCommand => new RelayCommand(() => ExecuteQueryCommand(ButtonCommands.SelectCustomWhere));

        public bool IsQueryActive
        {
            get { return _isQueryActive; }
            set
            {
                _isQueryActive = value;
                RaisePropertyChanged();
            }
        }

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

        public int SelectedCustomTopCount
        {
            get { return _selectedCustomTopCount; }
            set
            {
                _selectedCustomTopCount = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedTopColumnName
        {
            get { return _selectedTopColumnName; }
            set
            {
                _selectedTopColumnName = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedTopOrderBy
        {
            get { return _selectedTopOrderBy; }
            set
            {
                _selectedTopOrderBy = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedWhereColumnName
        {
            get { return _selectedWhereColumnName; }
            set
            {
                _selectedWhereColumnName = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedWhereOperand
        {
            get { return _selectedWhereOperand; }
            set
            {
                _selectedWhereOperand = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedWhereTextBoxInput
        {
            get { return _selectedWhereTextBoxInput; }
            set
            {
                _selectedWhereTextBoxInput = value;
                RaisePropertyChanged();
            }
        }

        public List<string> ColumnNamesList
        {
            get { return _columnNamesList; }
            set
            {
                _columnNamesList = value;
                RaisePropertyChanged();
            }
        }

        public List<string> OrderByList
        {
            get { return _orderByList; }
            set
            {
                _orderByList = value;
                RaisePropertyChanged();
            }
        }

        public List<string> ConditionalOperandsList
        {
            get { return _conditionalOperandsList; }
            set
            {
                _conditionalOperandsList = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<T> RawDataCollection
        {
            get { return _rawDataCollection; }
            set
            {
                _rawDataCollection = value;
                RaisePropertyChanged();
            }
        }

        private void AddColumnNamesList()
        {
            foreach (var prop in new T().GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                {
                    string columnName = prop.Name.Split('_')[1];
                    if (!columnName.Contains("Ticks"))
                        ColumnNamesList.Add(columnName);
                }
            }
        }

        private void AddOrderByList()
        {
            OrderByList.Add("asc");
            OrderByList.Add("desc");
        }

        private void AddConditionalOperandsList()
        {
            ConditionalOperandsList.Add("=");
            ConditionalOperandsList.Add("!=");
            ConditionalOperandsList.Add(">");
            ConditionalOperandsList.Add(">=");
            ConditionalOperandsList.Add("<");
            ConditionalOperandsList.Add("<=");
            ConditionalOperandsList.Add("LIKE");
        }

        private void AddButtonCommandsActions()
        {
            ButtonCommandActions = new Dictionary<ButtonCommands, Action>();
            ButtonCommandActions.Add(ButtonCommands.SelectAll, SelectAll);
            ButtonCommandActions.Add(ButtonCommands.SelectTop100, SelectTop100);
            ButtonCommandActions.Add(ButtonCommands.SelectBottom100, SelectBottom100);
            ButtonCommandActions.Add(ButtonCommands.SelectCustomTop, SelectCustomTop);
            ButtonCommandActions.Add(ButtonCommands.SelectCustomWhere, SelectCustomWhere);
        }

        private void ExecuteQueryCommand(ButtonCommands commands)
        {
            IsQueryActive = false;
            QueryStatus = "Executing query...";
            RawDataCollection.Clear();

            Task.Run(() =>
            {
                try
                {
                    ButtonCommandActions[commands]();
                    QueryStatus = "Query executed successfully";
                }
                catch (Exception ex)
                {
                    QueryStatus = "Query failed";
                    ExceptionLog.Log(LogLevel.Error, ex);
                }

                IsQueryActive = true;
            });
        }

        private void SelectAll()
        {
            RawDataCollection = new ObservableCollection<T>(IRawDataQueries.ReadAllRecords());
            RowsReturned = RawDataCollection.Count;
        }

        private void SelectTop100()
        {
            RawDataCollection = new ObservableCollection<T>(IRawDataQueries.ReadTopRecords(100));
            RowsReturned = RawDataCollection.Count;
        }

        private void SelectBottom100()
        {
            RawDataCollection = new ObservableCollection<T>(IRawDataQueries.ReadLastRecords(100));
            RowsReturned = RawDataCollection.Count;
        }

        private void SelectCustomTop()
        {
           RawDataCollection = new ObservableCollection<T>(IRawDataQueries.ReadRecordsCustomTop(SelectedCustomTopCount, SelectedTopColumnName, SelectedTopOrderBy));
           RowsReturned = RawDataCollection.Count;
        }

        private void SelectCustomWhere()
        {
           RawDataCollection = new ObservableCollection<T>(IRawDataQueries.ReadRecordsWhere(SelectedWhereColumnName, SelectedWhereOperand, SelectedWhereTextBoxInput));
           RowsReturned = RawDataCollection.Count;
        }
    }
}

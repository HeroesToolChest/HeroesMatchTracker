using HeroesParserData.Models.DbModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Data
{
    public abstract class RawDataContext : ViewModelBase
    {
        #region private properties
        private bool _isQueryActive;
        private int _selectedNumber;
        private string _selectedWhereColumnName;
        private string _selectedTopColumnName;
        private string _selectedOperand;
        private string _textBoxSelectWhere;
        private string _selectedTopOrderBy;
        private List<string> _columnNames = new List<string>();
        private List<string> _conditionalOperands = new List<string>();
        private List<string> _orderBy = new List<string>();

        private int _rowsReturned;
        private string _queryStatus;
        private Dictionary<ButtonCommands, Action> ButtonCommandActions;
        #endregion private properties

        #region properties
        public bool IsQueryActive
        {
            get
            {
                return !_isQueryActive;
            }
            set
            {
                _isQueryActive = value;
                RaisePropertyChangedEvent(nameof(IsQueryActive));
            }
        }

        public int SelectedNumber
        {
            get { return _selectedNumber; }
            set
            {
                _selectedNumber = value;
                RaisePropertyChangedEvent(nameof(SelectedNumber));
            }
        }

        public string SelectedWhereColumnName
        {
            get { return _selectedWhereColumnName; }
            set
            {
                _selectedWhereColumnName = value;
                RaisePropertyChangedEvent(nameof(SelectedWhereColumnName));
            }
        }

        public string SelectedTopColumnName
        {
            get { return _selectedTopColumnName; }
            set
            {
                _selectedTopColumnName = value;
                RaisePropertyChangedEvent(nameof(SelectedTopColumnName));
            }
        }

        public string SelectedOperand
        {
            get { return _selectedOperand; }
            set
            {
                _selectedOperand = value;
                RaisePropertyChangedEvent(nameof(SelectedOperand));
            }
        }

        public string TextBoxSelectWhere
        {
            get { return _textBoxSelectWhere; }
            set
            {
                _textBoxSelectWhere = value;
                RaisePropertyChangedEvent(nameof(TextBoxSelectWhere));
            }
        }

        public string SelectedTopOrderBy
        {
            get { return _selectedTopOrderBy; }
            set
            {
                _selectedTopOrderBy = value;
                RaisePropertyChangedEvent(nameof(SelectedTopOrderBy));
            }
        }

        public List<string> ColumnNames
        {
            get { return _columnNames; }
            set
            {
                _columnNames = value;
                RaisePropertyChangedEvent(nameof(ColumnNames));
            }
        }

        public List<string> ConditionalOperands
        {
            get { return _conditionalOperands; }
            set
            {
                _conditionalOperands = value;
                RaisePropertyChangedEvent(nameof(ConditionalOperands));
            }
        }

        public List<string> OrderBy
        {
            get { return _orderBy; }
            set
            {
                _orderBy = value;
                RaisePropertyChangedEvent(nameof(OrderBy));
            }
        }

        public int RowsReturned
        {
            get { return _rowsReturned; }
            set
            {
                _rowsReturned = value;
                RaisePropertyChangedEvent(nameof(RowsReturned));
            }
        }

        public string QueryStatus
        {
            get { return _queryStatus; }
            set
            {
                _queryStatus = value;
                RaisePropertyChangedEvent(nameof(QueryStatus));
            }
        }
        #endregion properties

        #region Button Commands
        public ICommand SelectTop100
        {
            get { return new DelegateCommand(() => PerformButtonCommand(ButtonCommands.ReadDataTop)); }
        }

        public ICommand SelectLast100
        {
            get { return new DelegateCommand(() => PerformButtonCommand(ButtonCommands.ReadDataLast)); }
        }

        public ICommand SelectCustomTop
        {
            get { return new DelegateCommand(() => PerformButtonCommand(ButtonCommands.ReadDataCustomTop)); }
        }

        public ICommand SelectWhere
        {
            get { return new DelegateCommand(() => PerformButtonCommand(ButtonCommands.ReadDataWhere)); }
        }
        #endregion Button Commands

        protected RawDataContext()
            : base()
        {
            AddButtonCommandsActions();
            AddListConditionalOperators();
            AddListOrderBy();   
        }

        #region Abstract Query Methods
        protected abstract void ReadDataTop();
        protected abstract void ReadDataLast();
        protected abstract void ReadDataCustomTop();
        protected abstract void ReadDataWhere();
        #endregion Abstract Query Methods

        protected void AddListColumnNames(IReplayDataTable replayDataTable)
        {
            foreach (var prop in replayDataTable.GetType().GetMethods())
            {
                if (prop.IsVirtual == false && prop.ReturnType.Name == "Void")
                {
                    string columnName = prop.Name.Split('_')[1];
                    if (!columnName.Contains("Ticks"))
                        ColumnNames.Add(columnName);
                }
            }
        }

        private void AddButtonCommandsActions()
        {
            ButtonCommandActions = new Dictionary<ButtonCommands, Action>();
            ButtonCommandActions.Add(ButtonCommands.ReadDataTop, () => ReadDataTop());
            ButtonCommandActions.Add(ButtonCommands.ReadDataLast, () => ReadDataLast());
            ButtonCommandActions.Add(ButtonCommands.ReadDataCustomTop, () => ReadDataCustomTop());
            ButtonCommandActions.Add(ButtonCommands.ReadDataWhere, () => ReadDataWhere());
        }

        private void AddListConditionalOperators()
        {
            ConditionalOperands.Add("=");
            ConditionalOperands.Add("!=");
            ConditionalOperands.Add(">");
            ConditionalOperands.Add(">=");
            ConditionalOperands.Add("<");
            ConditionalOperands.Add("<=");
            ConditionalOperands.Add("LIKE");
        }

        private void AddListOrderBy()
        {
            OrderBy.Add("asc");
            OrderBy.Add("desc");
        }

        private void PerformButtonCommand(ButtonCommands commands)
        {
            IsQueryActive = true;
            QueryStatus = "Executing query...";
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

            IsQueryActive = false;
        }
    }

    enum ButtonCommands
    {
        ReadDataTop,
        ReadDataLast,
        ReadDataCustomTop,
        ReadDataWhere,
    }
}

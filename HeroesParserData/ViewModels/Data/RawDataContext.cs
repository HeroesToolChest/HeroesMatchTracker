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
        private Dictionary<ButtonCommands, Func<Task>> _buttonCommandActions;
        #endregion

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
                RaisePropertyChangedEvent("IsQueryActive");
            }
        }

        public int SelectedNumber
        {
            get { return _selectedNumber; }
            set
            {
                _selectedNumber = value;
                RaisePropertyChangedEvent("SelectedNumber");
            }
        }

        public string SelectedWhereColumnName
        {
            get { return _selectedWhereColumnName; }
            set
            {
                _selectedWhereColumnName = value;
                RaisePropertyChangedEvent("SelectedWhereColumnName");
            }
        }

        public string SelectedTopColumnName
        {
            get { return _selectedTopColumnName; }
            set
            {
                _selectedTopColumnName = value;
                RaisePropertyChangedEvent("SelectedTopColumnName");
            }
        }

        public string SelectedOperand
        {
            get { return _selectedOperand; }
            set
            {
                _selectedOperand = value;
                RaisePropertyChangedEvent("SelectedOperand");
            }
        }

        public string TextBoxSelectWhere
        {
            get { return _textBoxSelectWhere; }
            set
            {
                _textBoxSelectWhere = value;
                RaisePropertyChangedEvent("TextBoxSelectWhere");
            }
        }

        public string SelectedTopOrderBy
        {
            get { return _selectedTopOrderBy; }
            set
            {
                _selectedTopOrderBy = value;
                RaisePropertyChangedEvent("SelectedTopOrderBy");
            }
        }

        public List<string> ColumnNames
        {
            get { return _columnNames; }
            set
            {
                _columnNames = value;
                RaisePropertyChangedEvent("ColumnNames");
            }
        }

        public List<string> ConditionalOperands
        {
            get { return _conditionalOperands; }
            set
            {
                _conditionalOperands = value;
                RaisePropertyChangedEvent("ConditionalOperands");
            }
        }

        public List<string> OrderBy
        {
            get { return _orderBy; }
            set
            {
                _orderBy = value;
                RaisePropertyChangedEvent("OrderBy");
            }
        }

        public int RowsReturned
        {
            get { return _rowsReturned; }
            set
            {
                _rowsReturned = value;
                RaisePropertyChangedEvent("RowsReturned");
            }
        }

        public string QueryStatus
        {
            get { return _queryStatus; }
            set
            {
                _queryStatus = value;
                RaisePropertyChangedEvent("QueryStatus");
            }
        }
        #endregion properties

        #region Button Commands
        public ICommand SelectTop100
        {
            get { return new DelegateCommand(async() => await PerformButtonCommand(ButtonCommands.ReadDataTop)); }
        }

        public ICommand SelectLast100
        {
            get { return new DelegateCommand(async () => await PerformButtonCommand(ButtonCommands.ReadDataLast)); }
        }

        public ICommand SelectCustomTop
        {
            get { return new DelegateCommand(async () => await PerformButtonCommand(ButtonCommands.ReadDataCustomTop)); }
        }

        public ICommand SelectWhere
        {
            get { return new DelegateCommand(async () => await PerformButtonCommand(ButtonCommands.ReadDataWhere)); }
        }
        #endregion

        protected RawDataContext()
            : base()
        {
            AddButtonCommandsActions();
            AddListConditionalOperators();
            AddListOrderBy();   
        }

        #region Abstract Query Methods
        protected abstract Task ReadDataTop();
        protected abstract Task ReadDataLast();
        protected abstract Task ReadDataCustomTop();
        protected abstract Task ReadDataWhere();
        #endregion

        private void AddButtonCommandsActions()
        {
            _buttonCommandActions = new Dictionary<ButtonCommands, Func<Task>>();
            _buttonCommandActions.Add(ButtonCommands.ReadDataTop, () => ReadDataTop());
            _buttonCommandActions.Add(ButtonCommands.ReadDataLast, () => ReadDataLast());
            _buttonCommandActions.Add(ButtonCommands.ReadDataCustomTop, () => ReadDataCustomTop());
            _buttonCommandActions.Add(ButtonCommands.ReadDataWhere, () => ReadDataWhere());
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

        private async Task PerformButtonCommand(ButtonCommands commands)
        {
            IsQueryActive = true;
            QueryStatus = "Executing query...";
            try
            {
                 await _buttonCommandActions[commands]();
                QueryStatus = "Query executed successfully";
            }
            catch (Exception ex)
            {
                QueryStatus = "Query failed";
                Logger.Log(LogLevel.Error, ex);
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.Wrappers
{
    public class EntityWrapper<T> : INotifyPropertyChanged, IDataErrorInfo
        where T : class, INotifyPropertyChanged
    {
        #region Private fields

        /// <summary>
        /// Contains key: property name, value: original property value
        /// </summary>
        private Dictionary<string, object> propertyOriginalValues;
        private bool hasErrors;

        #endregion

        #region Initialization

        public EntityWrapper(T entity)
        {
            Entity = entity;
            InitializePropertyOriginalValues(entity);
            IsChanged = false;
            Entity.PropertyChanged += (sender, args) =>
            {
                IsChanged = true;
                Validate();
            };
        }

        private void InitializePropertyOriginalValues(T entity)
        {
            propertyOriginalValues = new Dictionary<string, object>();

            foreach (var propertyInfo in typeof(T).GetProperties().Where(item => item.CanRead && item.CanWrite))
            {
                propertyOriginalValues.Add(propertyInfo.Name, propertyInfo.GetValue(entity));
            }
        }

        #endregion

        #region Public properties

        public bool IsChanged { get; set; }

        public T Entity { get; private set; }

        public bool HasErrors
        {
            get
            {
                // validate to determine if the object has errors
                if (!hasErrors)
                    hasErrors = (Validate(null) != null);

                return hasErrors;
            }
            protected set
            {
                if (hasErrors != value)
                {
                    hasErrors = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Public methods

        public void RevertChanges()
        {
            if (IsChanged)
            {
                foreach (var propertyInfo in typeof(T).GetProperties())
                {
                    if (propertyOriginalValues.ContainsKey(propertyInfo.Name) && propertyInfo.CanWrite)
                        propertyInfo.SetValue(Entity, propertyOriginalValues[propertyInfo.Name]);
                }
            }
        }

        public void Validate()
        {
            HasErrors = Validate(null) != null;
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErrorInfo implementation

        string IDataErrorInfo.Error => null;
        string IDataErrorInfo.this[string columnName] => Validate(columnName);

        /// <summary>
        /// Returns first error message for columnName property or for the whole object if columnName is null.
        /// Returns null if subject is valid.
        /// Note for overriders: always call base.Validate because it keeps HasErrors in actual state.
        /// </summary>
        protected virtual string Validate(string columnName)
        {
            if (columnName == null)
            {
                // validate the whole object
                var validationContext = new ValidationContext(Entity);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(Entity, validationContext, validationResults, true))
                {
                    if (validationResults.Count > 0)
                    {
                        return validationResults[0].ErrorMessage;
                    }
                }

                return null;
            }
            else
            {
                // validate the whole object to check if it has errors
                HasErrors = (Validate(null) != null);

                // DataAnnotations validation
                var validationContext = new ValidationContext(Entity) { MemberName = columnName };
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateProperty(Entity.GetType().GetProperty(columnName).GetValue(Entity), validationContext, validationResults))
                {
                    if (validationResults.Count > 0)
                    {
                        return validationResults[0].ErrorMessage;
                    }
                }

                return null;
            }
        }

        #endregion
    }
}

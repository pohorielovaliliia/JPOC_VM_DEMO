using System;

namespace JPOC_VM_DEMO.Services.JpocData.T_JP_SiteSetting
{
    [Serializable]
    public class T_JP_SiteSetting : IEquatable<T_JP_SiteSetting>
    {
        #region Construction

        /// <summary>
        /// Initializes a new (no-args) instance of the T_JP_SiteSetting class.
        /// </summary>
        public T_JP_SiteSetting()
        {
        }

        /// <summary>
        /// Initializes a new instance of the T_JP_SiteSetting class.
        /// </summary>
        public T_JP_SiteSetting(string parameter, string value, int modifiedBy, DateTime modifiedDate)
        {
            Parameter = parameter;
            Value = value;
            modified_by = modifiedBy;
            modified_date = modifiedDate;
        }

        #endregion

        #region Properties

        private string m_Parameter;
        /// <summary>
        /// Gets or sets the siteInformation_id value.
        /// </summary>
        public string Parameter
        {
            get => m_Parameter;
            set => m_Parameter = value;
        }

        private string m_Value;
        /// <summary>
        /// Gets or sets the role_id value.
        /// </summary>
        public string Value
        {
            get => m_Value;
            set => m_Value = value;
        }

        private int m_modified_by;
        /// <summary>
        /// Gets or sets the created_by value.
        /// </summary>
        public int modified_by
        {
            get => m_modified_by;
            set => m_modified_by = value;
        }

        private DateTime m_modified_date;
        /// <summary>
        /// Gets or sets the created_date value.
        /// </summary>
        public DateTime modified_date
        {
            get => m_modified_date;
            set => m_modified_date = value;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns the Primary Key of the object.
        /// </summary>
        public override string ToString()
        {
            return $"[T_JP_SiteSetting] {Parameter}";
        }

        /// <summary>
        /// Returns true if the Ids of the two instances are equal.
        /// </summary>
        /// <param name="obj">The other object instance.</param>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is not T_JP_SiteSetting other) return false;
            return Equals(other);
        }

        /// <summary>
        /// Implements IEquatable<T_JP_SiteSetting>
        /// </summary>
        public bool Equals(T_JP_SiteSetting other)
        {
            if (other is null) return false;
            return string.Equals(Parameter, other.Parameter, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the GetHashCode() method of the Primary Key member.
        /// </summary>
        public override int GetHashCode()
        {
            return -1;
        }

        #endregion

        #region CRUD Methods

        /// <summary>
        /// Load a single record.
        /// </summary>
        public void Load()
        {
            throw new NotImplementedException(); //TODO:
        }

        /// <summary>
        /// Load all records.
        /// </summary>
        public void LoadAll()
        {
            throw new NotImplementedException(); //TODO:
        }

        /// <summary>
        /// Insert a new record.
        /// </summary>
        public void Insert()
        {
            throw new NotImplementedException(); //TODO:
        }

        /// <summary>
        /// Update existing record.
        /// </summary>
        public void Update()
        {
            throw new NotImplementedException(); //TODO:
        }

        /// <summary>
        /// Delete existing record.
        /// </summary>
        public void Delete()
        {
            throw new NotImplementedException(); //TODO:
        }

        #endregion
    }
}

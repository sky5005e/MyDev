using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.BE
{
   public class AppSettingBE
    {
        #region Fields
        private string sOperation;
        private byte iSettingId;
        private string sSettingName;
        private string sSettingValue;
        private string sDescription;
        private string sControl;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the SOperation value.
        /// </summary>
        public virtual string SOperation
        {
            get { return sOperation; }
            set { sOperation = value; }
        }

        /// <summary>
        /// Gets or sets the ISettingId value.
        /// </summary>
        public virtual byte ISettingId
        {
            get { return iSettingId; }
            set { iSettingId = value; }
        }

        /// <summary>
        /// Gets or sets the SSettingName value.
        /// </summary>
        public virtual string SSettingName
        {
            get { return sSettingName; }
            set { sSettingName = value; }
        }

        /// <summary>
        /// Gets or sets the SSettingValue value.
        /// </summary>
        public virtual string SSettingValue
        {
            get { return sSettingValue; }
            set { sSettingValue = value; }
        }

        /// <summary>
        /// Gets or sets the SDescription value.
        /// </summary>
        public virtual string SDescription
        {
            get { return sDescription; }
            set { sDescription = value; }
        }

        /// <summary>
        /// Gets or sets the SControl value.
        /// </summary>
        public virtual string SControl
        {
            get { return sControl; }
            set { sControl = value; }
        }
        

        #endregion
    }
   
}

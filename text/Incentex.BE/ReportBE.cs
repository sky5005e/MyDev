using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.BE
{
    public class ReportBE
    {
       
        #region Property
        private string StoreiD;
        private DateTime FromDate;
        private DateTime ToDate;
        private string ItemNumber;
        private  string MasterItemNumber;
        private string Workgroup;
       
        private string sOperation;
        #endregion
        #region Properties
        
        
        /// <summary>
        /// Gets or sets the STemplateName value.
        /// </summary>
        public virtual string SStoreiD
        {
            get { return StoreiD; }
            set { StoreiD = value; }
        }
        /// <summary>
        /// Gets or sets the SOperation value.
        /// </summary>
        public virtual string SOperation
        {
            get { return sOperation; }
            set { sOperation = value; }
        }


        /// <summary>
        /// Gets or sets the SFromAddress value.
        /// </summary>
        public virtual string SItemNumber
        {
            get { return ItemNumber; }
            set { ItemNumber = value; }
        }


        /// <summary>
        /// Gets or sets the SSubject value.
        /// </summary>
        public virtual string SMasterItemNumber
        {
            get { return MasterItemNumber; }
            set { MasterItemNumber = value; }
        }

        /// <summary>
        /// Gets or sets the STemplateContent value.
        /// </summary>
        public virtual string SWorkgroup
        {
            get { return Workgroup; }
            set { Workgroup = value; }
        }

        /// <summary>
        /// Gets or sets the DCreateDate value.
        /// </summary>
        public virtual DateTime SFromDate
        {
            get { return FromDate; }
            set { FromDate = value; }
        }
        public virtual DateTime SToDate
        {
            get { return ToDate; }
            set { ToDate = value; }
        }

        #endregion
    }
}

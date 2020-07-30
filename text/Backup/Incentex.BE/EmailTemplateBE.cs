using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.BE
{
    public class EmailTemplateBE
    {
        #region Property
        private byte iTemplateID;
        private string sTemplateName;
        private string sFromAddress;
        private string sFromName;
        private string sSubject;
        private string sTemplateContent;
        private DateTime dCreateDate;
        private string sOperation;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the ITemplateID value.
        /// </summary>
        public virtual byte ITemplateID
        {
            get { return iTemplateID; }
            set { iTemplateID = value; }
        }

        /// <summary>
        /// Gets or sets the STemplateName value.
        /// </summary>
        public virtual string STemplateName
        {
            get { return sTemplateName; }
            set { sTemplateName = value; }
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
        public virtual string SFromAddress
        {
            get { return sFromAddress; }
            set { sFromAddress = value; }
        }

        /// <summary>
        /// Gets or sets the SFromName value.
        /// </summary>
        public virtual string SFromName
        {
            get { return sFromName; }
            set { sFromName = value; }
        }

        /// <summary>
        /// Gets or sets the SSubject value.
        /// </summary>
        public virtual string SSubject
        {
            get { return sSubject; }
            set { sSubject = value; }
        }

        /// <summary>
        /// Gets or sets the STemplateContent value.
        /// </summary>
        public virtual string STemplateContent
        {
            get { return sTemplateContent; }
            set { sTemplateContent = value; }
        }

        /// <summary>
        /// Gets or sets the DCreateDate value.
        /// </summary>
        public virtual DateTime DCreateDate
        {
            get { return dCreateDate; }
            set { dCreateDate = value; }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.BE
{
    public class UserManagementMenuBE
    {
       
             #region Fields

        private int iMenudetailsID;
        private int iMenuID;
        private int iManageID;
        private string sMenuSubName;
        private string sMenuName;
        #endregion
        #region Properties
        

        /// <summary>
        /// Gets or sets the IMenudetailsID value.
        /// </summary>
        public virtual int IMenudetailsID
        {
            get { return iMenudetailsID; }
            set { iMenudetailsID = value; }
        }

        /// <summary>
        /// Gets or sets the IMenuID value.
        /// </summary>
        public virtual int IMenuID
        {
            get { return iMenuID; }
            set { iMenuID = value; }
        }

        /// <summary>
        /// Gets or sets the IManageID value.
        /// </summary>
        public virtual int IManageID
        {
            get { return iManageID; }
            set { iManageID = value; }
        }

        /// <summary>
        /// Gets or sets the SMenuSubName value.
        /// </summary>
        public virtual string SMenuSubName
        {
            get { return sMenuSubName; }
            set { sMenuSubName = value; }
        }

        /// <summary>
        /// Gets or sets the SMenuName value.
        /// </summary>
        public virtual string SMenuName
        {
            get { return sMenuName; }
            set { sMenuName = value; }
        }

        #endregion
    }
}

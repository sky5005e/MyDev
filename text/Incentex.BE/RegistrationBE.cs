using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.BE
{
    public class RegistrationBE
    {
        #region Fields

        private long iRegistraionID;
        private string sCompanyName;
        private string sFirstName;
        private string sLastName;
        private string sAddress1;
        private string sAddress2;
        private long iCountryId;
        private long iStateId;
        private long iCityId;
        private string sZipCode;
        private string sEmailAddress;
        private string sTelephoneNumber;
        private string sMobileNumber;
        private string sEmployeeId;
        private long iWorkgroupId;
        private long iBasestationId;
        private int iGender;
        private long? idepartmentId;
        private string sPhoto;
        public string sOperation { get; set; }
        public Int64 iCompanyName { get; set; }
        public string status { get; set; }
        public DateTime dateRequestSubmitted { get; set; }
        public DateTime DOH { get; set; }
        public Int64? iEmployeeTypeID { get; set; }
        #endregion

		#region Properties
		/// <summary>
		/// Gets or sets the IRegistraionID value.
		/// </summary>
		public virtual long IRegistraionID
		{
			get { return iRegistraionID; }
			set { iRegistraionID = value; }
		}

		/// <summary>
		/// Gets or sets the SCompanyName value.
		/// </summary>
		public virtual string SCompanyName
		{
			get { return sCompanyName; }
			set { sCompanyName = value; }
		}

		/// <summary>
		/// Gets or sets the SFirstName value.
		/// </summary>
		public virtual string SFirstName
		{
			get { return sFirstName; }
			set { sFirstName = value; }
		}

		/// <summary>
		/// Gets or sets the SLastName value.
		/// </summary>
		public virtual string SLastName
		{
			get { return sLastName; }
			set { sLastName = value; }
		}

		/// <summary>
		/// Gets or sets the SAddress1 value.
		/// </summary>
		public virtual string SAddress1
		{
			get { return sAddress1; }
			set { sAddress1 = value; }
		}

		/// <summary>
		/// Gets or sets the SAddress2 value.
		/// </summary>
		public virtual string SAddress2
		{
			get { return sAddress2; }
			set { sAddress2 = value; }
		}

		/// <summary>
		/// Gets or sets the ICountryId value.
		/// </summary>
		public virtual long ICountryId
		{
			get { return iCountryId; }
			set { iCountryId = value; }
		}

		/// <summary>
		/// Gets or sets the IStateId value.
		/// </summary>
		public virtual long IStateId
		{
			get { return iStateId; }
			set { iStateId = value; }
		}

		/// <summary>
		/// Gets or sets the ICityId value.
		/// </summary>
		public virtual long ICityId
		{
			get { return iCityId; }
			set { iCityId = value; }
		}

		/// <summary>
		/// Gets or sets the SZipCode value.
		/// </summary>
		public virtual string SZipCode
		{
			get { return sZipCode; }
			set { sZipCode = value; }
		}

		/// <summary>
		/// Gets or sets the SEmailAddress value.
		/// </summary>
		public virtual string SEmailAddress
		{
			get { return sEmailAddress; }
			set { sEmailAddress = value; }
		}

		/// <summary>
		/// Gets or sets the STelephoneNumber value.
		/// </summary>
		public virtual string STelephoneNumber
		{
			get { return sTelephoneNumber; }
			set { sTelephoneNumber = value; }
		}

		/// <summary>
		/// Gets or sets the SMobileNumber value.
		/// </summary>
		public virtual string SMobileNumber
		{
			get { return sMobileNumber; }
			set { sMobileNumber = value; }
		}

		/// <summary>
		/// Gets or sets the SEmployeeId value.
		/// </summary>
		public virtual string SEmployeeId
		{
			get { return sEmployeeId; }
			set { sEmployeeId = value; }
		}

		/// <summary>
		/// Gets or sets the IWorkgroupId value.
		/// </summary>
		public virtual long IWorkgroupId
		{
			get { return iWorkgroupId; }
			set { iWorkgroupId = value; }
		}

		/// <summary>
		/// Gets or sets the IBasestationId value.
		/// </summary>
		public virtual long IBasestationId
		{
			get { return iBasestationId; }
			set { iBasestationId = value; }
		}

		/// <summary>
		/// Gets or sets the IGender value.
		/// </summary>
		public virtual int IGender
		{
			get { return iGender; }
			set { iGender = value; }
		}

        /// <summary>
        /// Gets or sets the sPhoto value.
        /// </summary>
        public virtual string SPhoto
        {
            get { return sPhoto; }
            set { sPhoto = value; }
        }
        /// <summary>
        /// Gets or sets the Idepartment value.
        /// </summary>
        public virtual long? IDepartmentId
        {
            get { return idepartmentId; }
            set { idepartmentId = value; }
        }

        /// <summary>
        /// Gets or sets the status value.
        /// </summary>
        public virtual string Status
        {
            get { return status; }
            set { status = value; }
        }

        public virtual DateTime DateRequestSubmitted
        {
            get { return dateRequestSubmitted; }
            set { dateRequestSubmitted = value; }
        }
        
        public virtual DateTime DateOfHired
        {
            get
            {
                return this.DOH;
            }
            set
            {
                this.DOH = value;
            }
        }
		#endregion
    }
}

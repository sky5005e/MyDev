using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.SqlClient;
using System.Text;
using Incentex.DAL.Common;
using System.Data.Linq.Mapping;

namespace Incentex.DAL.SqlRepository
{
    public class LookupRepository : RepositoryBase
    {

        //public IQueryable<INC_Lookup> GetAllQuery()
        //{
        //    IQueryable<INC_Lookup> qry = from q in db.INC_Lookups
        //                                 select q;

        //    return qry;
        //}


        public Int64? GetIdByLookupName(string LookupName)
        {
            Int64? LookupId = (from q in db.INC_Lookups
                               where SqlMethods.Like(q.sLookupName, LookupName)
                               select q.iLookupID).FirstOrDefault();

            return LookupId;
        }


        public Int64? GetIdByLookupNameAndCode(string LookupName)
        {
            Int64? LookupId = (from q in db.INC_Lookups
                               where q.sLookupName == LookupName && q.iLookupCode == "MasterItemNumber"
                               select q.iLookupID).FirstOrDefault();

            return LookupId;
        }


        public Int64 GetIdByLookupNameforBulk(string LookupName)
        {
            Int64 LookupId = (from q in db.INC_Lookups
                              where SqlMethods.Like(q.sLookupName, LookupName)
                              select q.iLookupID).FirstOrDefault();

            return LookupId;
        }

        public Int64? GetIdByLookupNameNLookUpCode(String LookupName, String LookupCode)
        {
            Int64? LookupId = (from q in db.INC_Lookups
                               where q.sLookupName == LookupName && q.iLookupCode == LookupCode
                               select q.iLookupID).FirstOrDefault();

            return LookupId;
        }

        public string GetIconByLookupName(string LookupName)
        {
            return (from q in db.INC_Lookups
                    where SqlMethods.Like(q.sLookupName, LookupName)
                    select q.sLookupIcon).FirstOrDefault();


        }

        public List<INC_Lookup> GetByLookupCode(DAEnums.LookupCodeType Code)
        {
            return this.GetByLookup(DAEnums.GetLookupCodeName(Code));
        }

        public List<INC_Lookup> GetByLookup(String LookCode)
        {
            return db.INC_Lookups.Where(C => C.iLookupCode == LookCode).OrderBy(le => le.sLookupName).ToList();
        }

        public List<INC_Lookup> GetWorkgroupWithIssuance()
        {
            List<INC_Lookup> objList = (from LU in db.INC_Lookups
                                        join UIP in db.UniformIssuancePolicies on LU.iLookupID equals UIP.WorkgroupID
                                        where LU.iLookupCode == "Workgroup"
                                        orderby LU.sLookupName
                                        select LU)
                                        .Distinct()
                                        .OrderBy(x => x.sLookupName)
                                        .ToList();

            return objList;
        }        

        public List<INC_Lookup> GetByLookupWorkgroupName(String LookCode, Int32 iLookupID)
        {
            return db.INC_Lookups.Where(C => C.iLookupCode == LookCode && C.iLookupID != iLookupID).ToList();
        }

        public List<INC_Lookup> GetWorkgroupByCompany(Int64 CompanyID)
        {
            List<INC_Lookup> objList = (from lu in db.INC_Lookups
                                        join sw in db.StoreWorkGroups on lu.iLookupID equals sw.WorkGroupID
                                        join cs in db.CompanyStores on sw.StoreID equals cs.StoreID
                                        where lu.iLookupCode == "Workgroup" && cs.CompanyID == CompanyID
                                        select lu)
                                        .OrderBy(x => x.sLookupName)
                                        .ToList();
            return objList;
        }

        public List<INC_Lookup> GetByLookupWorkgroupList(string LookCode, List<Int64> iLookupID)
        {
            List<INC_Lookup> objLookList = db.INC_Lookups.Where(C => C.iLookupCode == LookCode && iLookupID.Contains(C.iLookupID)).ToList();

            return objLookList;
        }

        public INC_Lookup GetById(Int64 iLookupID)
        {
            //IQueryable<INC_Lookup> qry = GetAllQuery().Where(l => l.iLookupID == iLookupID);
            //INC_Lookup obj = GetSingle(qry.ToList());

            //return obj;
            //change by mayur on 3-feb-2012
            return db.INC_Lookups.SingleOrDefault(i => i.iLookupID == iLookupID);
        }

        public void DeleteLooupById(Int64 iLookupID, string iLookupCode)
        {
            INC_Lookup objLookup = GetById(iLookupID);
            if (objLookup != null)
            {
                if (iLookupCode == "DocumentStorageType")
                {
                    List<DocumentStorageCentre> objdocument = db.DocumentStorageCentres.Where(t => t.DocumentType == iLookupID).ToList();
                    if (objdocument != null)
                    {
                        db.DocumentStorageCentres.DeleteAllOnSubmit(objdocument);
                        base.SubmitChanges();
                    }
                }
                else if (iLookupCode == "EmployeeTrainingType")
                {
                    List<EmployeeTrainingCenter> objEmployeeTraining = db.EmployeeTrainingCenters.Where(t => t.EmployeeTrainingType == iLookupID).ToList();
                    if (objEmployeeTraining != null)
                    {
                        db.EmployeeTrainingCenters.DeleteAllOnSubmit(objEmployeeTraining);
                        base.SubmitChanges();
                    }
                }

                db.INC_Lookups.DeleteOnSubmit(objLookup);
                base.SubmitChanges();
            }
        }

        public void UpdateLooupById(Int64 iLookupID, string LookupName, string password, string createby)
        {
            INC_Lookup objLookup = GetById(iLookupID);
            if (objLookup != null)
            {
                objLookup.sLookupName = LookupName;
                objLookup.Val1 = password;
                objLookup.sLookupIcon = !string.IsNullOrEmpty(createby) ? createby : objLookup.sLookupIcon;

                if (string.IsNullOrEmpty(password))
                    objLookup.sLookupIcon = null;

                db.SubmitChanges();
            }
        }

        #region Supplier Documents

        public enum SupplierDocumentsType
        {
            SupplierQualityCertificates,/*lookup table has same name for sLookupName column  */
            SupplierListVacationAndSupplierClosing,
            SupplierMasterPriceOfferList
        }

        /// <summary>
        /// Get look up id for supplier documents
        /// </summary>
        /// <param name="DocumentsType"></param>
        /// <returns></returns>
        public Int64 GetSupplierDocumentLookUpId(SupplierDocumentsType DocumentsType)
        {
            Int64 LookUpId = 0;
            string CodeName = DAEnums.GetLookupCodeName(DAEnums.LookupCodeType.SupplierDocuments);

            //IQueryable<INC_Lookup> qry = GetAllQuery().Where(l => l.iLookupCode.Equals(CodeName));
            //qry = qry.Where(d => d.sLookupName.Equals(DocumentsType.ToString()));

            //INC_Lookup obj = GetSingle(qry.ToList());

            //if(obj != null)
            //{
            //    LookUpId = obj.iLookupID;
            //}

            //Update by Prashant - Feb 2013
            INC_Lookup obj = db.INC_Lookups.FirstOrDefault(l => l.iLookupCode.Equals(CodeName) && l.sLookupName.Equals(DocumentsType.ToString()));
            if (obj != null)
            {
                LookUpId = obj.iLookupID;
            }
            //Update by Prashant - Feb 2013

            return LookUpId;
        }

        #endregion


        #region Get Lookup ID
        /// <summary>
        /// Get ID bbased on type
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public String GetValueID(String Type)
        {
            String valueID = String.Empty;
            switch (Type)
            {
                case "Both Supply and Uniform Items":
                    String[] arrcatID = db.Categories.Where(q => q.CategoryID == 1 || q.CategoryID == 2).Select(s => Convert.ToString(s.CategoryID)).ToArray();// Employee Uniforms, Aviation Supplies
                    valueID = String.Join(",", arrcatID);
                    break;
                case "Supply Items":
                    valueID = db.Categories.Where(q => q.CategoryID == 2).Select(s => Convert.ToString(s.CategoryID)).FirstOrDefault(); // Aviation Supplies
                    break;
                case "Uniform Items":
                    valueID = db.Categories.Where(q => q.CategoryID == 1).Select(s => Convert.ToString(s.CategoryID)).FirstOrDefault(); // Employee Uniforms
                    break;
                case "Unisex":
                    String[] arrSexID = db.INC_Lookups.Where(q => q.iLookupCode == "GarmentType").Select(s => Convert.ToString(s.iLookupID)).ToArray();// all Garment Type
                    valueID = String.Join(",", arrSexID);
                    break;
                case "Male":
                    valueID = db.INC_Lookups.Where(q => q.iLookupCode == "GarmentType" && q.sLookupName == "Male").Select(s => Convert.ToString(s.iLookupID)).FirstOrDefault();// Male Garment Type
                    break;
                case "Female":
                    valueID = db.INC_Lookups.Where(q => q.iLookupCode == "GarmentType" && q.sLookupName == "Female").Select(s => Convert.ToString(s.iLookupID)).FirstOrDefault();// Female Garment Type
                    break;
                default:
                    valueID = String.Empty;
                    break;
            }
            return valueID;
        }

        #endregion

        #region IncentexEmployeeDocument

        /// <summary>
        /// Document type
        /// </summary>
        public enum IncentexEmployeeSetupDocumentType
        {
            SocialSecurityCard, /*lookup table has same name for sLookupName column  */
            W4I9Documents,
            ValidPhotoID,
            SignedNDAAgreement
        }

        public enum MiscellaneousIssuanceSetup
        {
            BothSupplyandUniformItems,
            SupplyItems,
            UniformItems,
            GarmentTypeUnisex,
            GarmentTypeMale,
            GarmentTypeFemale
        }
        public enum IncentexEmployeeAdditionalDocumentType
        {
            AnnualReview,
            OtherDocuments
        }

        public static string GetIncentexEmployeeSetupDocumentTypeName(IncentexEmployeeSetupDocumentType type)
        {
            string Name = "";
            switch (type)
            {
                case IncentexEmployeeSetupDocumentType.SignedNDAAgreement:
                    Name = "Signed NDA Agreement";
                    break;
                case IncentexEmployeeSetupDocumentType.SocialSecurityCard:
                    Name = "Social Security Card";
                    break;
                case IncentexEmployeeSetupDocumentType.ValidPhotoID:
                    Name = "Valid Photo ID";
                    break;
                case IncentexEmployeeSetupDocumentType.W4I9Documents:
                    Name = "W4I9 Documents";
                    break;
            }
            return Name;
        }

        public static string GetIncentexEmployeeAdditionalDocumentTypeName(IncentexEmployeeAdditionalDocumentType type)
        {
            string Name = "";
            switch (type)
            {
                case IncentexEmployeeAdditionalDocumentType.AnnualReview:
                    Name = "Annual Review";
                    break;
                case IncentexEmployeeAdditionalDocumentType.OtherDocuments:
                    Name = "Other Documents";
                    break;
            }
            return Name;
        }


        /// <summary>
        /// Get look up id for incentex employee documents
        /// </summary>
        /// <param name="DocumentsType"></param>
        /// <returns></returns>
        public Int64 GetIncentexEmployeeDocumentLookUpId(IncentexEmployeeSetupDocumentType DocumentsType)
        {
            Int64 LookUpId = 0;
            string CodeName = DAEnums.GetLookupCodeName(DAEnums.LookupCodeType.IncentexEmployeeSetupDocument);

            //IQueryable<INC_Lookup> qry = GetAllQuery().Where(l => l.iLookupCode.Equals(CodeName));

            //qry = qry.Where(d => d.sLookupName.Equals(GetIncentexEmployeeSetupDocumentTypeName(DocumentsType)));

            //INC_Lookup obj = GetSingle(qry.ToList());

            //if (obj != null)
            //{
            //    LookUpId = obj.iLookupID;
            //}

            //Update by Prashant - Feb 2013
            INC_Lookup obj = db.INC_Lookups.FirstOrDefault(l => l.iLookupCode.Equals(CodeName) && l.sLookupName.Equals(GetIncentexEmployeeSetupDocumentTypeName(DocumentsType)));
            if (obj != null)
            {
                LookUpId = obj.iLookupID;
            }
            //Update by Prashant - Feb 2013

            return LookUpId;
        }


        /// <summary>
        /// Get look up id for incentex employee documents
        /// </summary>
        /// <param name="DocumentsType"></param>
        /// <returns></returns>
        public Int64 GetIncentexEmployeeDocumentLookUpId(IncentexEmployeeAdditionalDocumentType DocumentsType)
        {
            Int64 LookUpId = 0;
            string CodeName = DAEnums.GetLookupCodeName(DAEnums.LookupCodeType.IncentexEmployeeAdditionalDocument);

            //IQueryable<INC_Lookup> qry = GetAllQuery().Where(l => l.iLookupCode.Equals(CodeName));

            //qry = qry.Where(d => d.sLookupName.Equals(GetIncentexEmployeeAdditionalDocumentTypeName( DocumentsType)));

            //INC_Lookup obj = GetSingle(qry.ToList());

            //if (obj != null)
            //{
            //    LookUpId = obj.iLookupID;
            //}
            //Update by Prashant - Feb 2013
            INC_Lookup obj = db.INC_Lookups.FirstOrDefault(l => l.iLookupCode.Equals(CodeName) && l.sLookupName.Equals(GetIncentexEmployeeAdditionalDocumentTypeName(DocumentsType)));
            if (obj != null)
            {
                LookUpId = obj.iLookupID;
            }
            //Update by Prashant - Feb 2013
            return LookUpId;
        }
        /// <summary>
        ///  It will return List which will Kepp "Other" as a Last..
        /// </summary>
        /// <param name="LookCode"></param>
        /// <returns></returns>
        public List<INC_Lookup> GetShipperByShipperType(string LookCode)
        {
            List<INC_Lookup> objLookList = db.INC_Lookups.Where(C => C.iLookupCode == LookCode).ToList();
            List<INC_Lookup> a = new List<INC_Lookup>();
            List<INC_Lookup> b = new List<INC_Lookup>();
            foreach (INC_Lookup Main in objLookList)
            {
                if (Main.sLookupName == "Other")
                {
                    a.Add(Main);
                }
                else
                {
                    b.Add(Main);
                }
            }
            b.AddRange(a);

            return b;
        }



        #endregion
    }
}

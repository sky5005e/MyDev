using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class AdditionalWorkgroupRepository : RepositoryBase
    {
        IQueryable<AdditionalWorkgroup> GetAllQuery()
        {
            IQueryable<AdditionalWorkgroup> qry = from a in db.AdditionalWorkgroups
                                                  select a;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CompanyiD"></param>
        /// <returns></returns>
        public List<AdditionalWorkgroup> GetManageDetailName(int EmployeeID, int CompanyiD)
        {
            //IQueryable<AdditionalWorkgroup> qry = GetAllQuery().Where(c => c.CompanyEmployeeID == EmployeeID && c.CompanyID == CompanyiD);
            //List<AdditionalWorkgroup> objList = qry.ToList();

            List<AdditionalWorkgroup> objList = (from a in db.AdditionalWorkgroups
                                                 where a.CompanyEmployeeID == EmployeeID && a.CompanyID == CompanyiD
                                                 select a).ToList();
            return objList;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserinfoID"></param>
        /// <param name="CompanyiD"></param>
        /// <returns></returns>
        public List<AdditionalWorkgroup> GetWorkgroupName(int UserinfoID, int CompanyiD)
        {
            //IQueryable<AdditionalWorkgroup> qry = GetAllQuery().Where(c => c.UserInfoID == UserinfoID && c.CompanyID == CompanyiD);
            //List<AdditionalWorkgroup> objList = qry.ToList();

            List<AdditionalWorkgroup> objList = (from a in db.AdditionalWorkgroups
                                                 where a.UserInfoID == UserinfoID && a.CompanyID == CompanyiD
                                                 select a).ToList();

            return objList;
        }

        /// <summary>
        /// Update BY : Gaurang Pathak
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AdditionalWorkgroup GetById(Int64 ID)
        {
            //AdditionalWorkgroup obj = GetSingle(GetAllQuery().Where(C => C.ID == ID).ToList());
            AdditionalWorkgroup obj = (from a in db.AdditionalWorkgroups
                                       where a.ID == ID
                                       select a).SingleOrDefault();
            return obj;
        }
       
        /// <summary>
        /// Delete a AdditionalWorkgroup Record by  ID
        /// </summary> 
        /// <param name="Company ID"></param>
        public void Delete(int EmployeeID, int CompanyID)
        {
            var matchedNote = (from c in db.GetTable<AdditionalWorkgroup>()
                               where c.CompanyEmployeeID == (EmployeeID) && c.CompanyID == (CompanyID)
                               select c);
            try
            {
                if (matchedNote != null)
                {
                    foreach (var notedetail in matchedNote)
                    {
                        db.AdditionalWorkgroups.DeleteOnSubmit(notedetail);
                    }

                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Deletes the by employee ID company ID and workgroup ID.
        /// Created by mayur for delete workgroup if available in aditional workgroup
        /// Create on 12-jan-2012
        /// </summary>
        /// <param name="EmployeeID">The employee ID.</param>
        /// <param name="CompanyID">The company ID.</param>
        /// <param name="WorkgroupID">The workgroup ID.</param>
        public void DeleteByEmployeeIDCompanyIDAndWorkgroupID(Int64 EmployeeID, Int64 CompanyID, Int64 WorkgroupID)
        {
            var matchedNote = (from c in db.GetTable<AdditionalWorkgroup>()
                               where c.CompanyEmployeeID == EmployeeID && c.CompanyID == CompanyID && c.WorkgroupID == WorkgroupID
                               select c);
            try
            {
                if (matchedNote != null)
                {
                    foreach (var notedetail in matchedNote)
                    {
                        db.AdditionalWorkgroups.DeleteOnSubmit(notedetail);
                    }

                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}

/* Project Name : Incentex 
 * Module Name : Uniform Isuance Program 
 * Description : This page is for data access logic for UniformIssuancePolicy table
 * ----------------------------------------------------------------------------------------- 
 * DATE | ID/ISSUE| AUTHOR | REMARKS 
 * ----------------------------------------------------------------------------------------- 
 * 23-Oct-2010 | 1 | Amit Trivedi | Design and Coding
 * ----------------------------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class UniformIssuancePolicyRepository : RepositoryBase
    {
        /// <summary>
        /// Get all record linq query 
        /// </summary>
        /// <returns></returns>
        IQueryable<UniformIssuancePolicy> GetAllQuery()
        {
            IQueryable<UniformIssuancePolicy> Query = from m in db.UniformIssuancePolicies
                                                      select m;
            return Query;
        }

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="UniformIssuancePolicyID"></param>
        /// <returns></returns>
        public UniformIssuancePolicy GetById(Int64 UniformIssuancePolicyID)
        {
            UniformIssuancePolicy objUniformIssuancePolicy = GetSingle( GetAllQuery().Where(m => m.UniformIssuancePolicyID == UniformIssuancePolicyID).ToList());
            return objUniformIssuancePolicy;
        }

        /// <summary>
        /// Get By UniformIssuance Policy ID
        /// </summary>
        /// <param name="UniformIssuancePolicyID"></param>
        /// <returns></returns>
        public UniformIssuancePolicy GetByUniformIssuancePolicyID(Int64 UniformIssuancePolicyID)
        {
            return db.UniformIssuancePolicies.Where(q=>q.UniformIssuancePolicyID == UniformIssuancePolicyID).FirstOrDefault();
        }

        public List<UniformIssuancePolicyItem> CoutRecord(Int64 uniforissuancePolicyId)
        {
            var query = from m in db.UniformIssuancePolicyItems
                        where m.UniformIssuancePolicyID == uniforissuancePolicyId && m.AssociationbudgetAmt != null
                        select m;
            return query.ToList();
        }

        public List<SelectTotaoIssuanceBudgetPolicyResult> CoutRecord1(Int64 uniforissuancePolicyId)
        {
            var query = db.SelectTotaoIssuanceBudgetPolicy(Convert.ToInt32(uniforissuancePolicyId));
            return query.ToList();
        }
        public List<UniformIssuancePolicyItem> CoutGroupRecord(Int64 uniforissuancePolicyId)
        {
            var query = from m in db.UniformIssuancePolicyItems
                        where m.UniformIssuancePolicyID == uniforissuancePolicyId && m.ISGROUPASSOCIATION == 'Y' 
                        select m;
            return query.ToList();
        }
        //public UniformIssuancePolicyItem GetByIdAndNewGroup(Int64 UniformIssuancePolicyID,string NewGroup)
        //{
        //    UniformIssuancePolicyItem objUniformIssuancePolicy = GetSingle( GetAllQuery().Where(m => m.UniformIssuancePolicyID == UniformIssuancePolicyID && m.new).ToList());
        //    return objUniformIssuancePolicy;
        //}
        

        /// <summary>
        /// Get By StoreId
        /// </summary>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public List<UniformIssuancePolicy> GetByStoreId(Int64 StoreId)
        {
            return db.UniformIssuancePolicies.Where(q => q.StoreId == StoreId).ToList();//List<UniformIssuancePolicy> objUniformIssuancePolicyList = GetAllQuery().Where(u => u.StoreId == StoreId).ToList();
            //return objUniformIssuancePolicyList;
        }
        public Int64 GetUniformIssuancePolicyIDbyName(String IssuanceProgramName)
        {
            return db.UniformIssuancePolicies.Where(q => q.IssuanceProgramName == IssuanceProgramName).FirstOrDefault().UniformIssuancePolicyID;
        }
        public List<GetAllIssuancePolicyCountResult> GetAllIssuancePolicyCount()
        {
            return db.GetAllIssuancePolicyCount().ToList();
        }

        public List<GetIssuancePoliciesForReportResult> GetIssuancePoliciesForReport(Int64 StoreID)
        {
            return db.GetIssuancePoliciesForReport(StoreID).ToList();
        }
        public List<UniformIssuancePolicy> GetByCompanyIdandWorkgroupId(Int64 CompanyId, Int64 WorkgroupId, Int64 Gendertype, Int64? EmployeeTypeID)
        {
            List<UniformIssuancePolicy> objList;
            
                var Query = from m in db.UniformIssuancePolicies
                            join c in db.CompanyStores on m.StoreId equals c.StoreID
                            join s in db.INC_Lookups on m.Status equals s.iLookupID
                            where c.CompanyID == CompanyId && m.WorkgroupID == WorkgroupId
                            && s.sLookupName == "Active" && m.GenderType == Gendertype
                            && (db.UniformIssuancePolicyItems.Where(item=>item.UniformIssuancePolicyID==m.UniformIssuancePolicyID && (item.EmployeeTypeid == 0 || item.EmployeeTypeid == EmployeeTypeID))).Count()>0
                           select m;   

                 return  objList=Query.ToList();;
        }
        /// <summary>
        /// This method is used to update Group name of Uniform Issuance Policy
        /// </summary>
        /// <param name="UniformIssuancePolicyID"></param>
        /// <param name="groupName"></param>
        public void UpdateUniformIssuancePolicyGroupNamebyID(Int64 UniformIssuancePolicyID, String groupName)
        {
            UniformIssuancePolicy obj = new UniformIssuancePolicy();
            obj = GetById(UniformIssuancePolicyID);
            if (obj != null)
            {
                if (!String.IsNullOrEmpty(groupName))
                    obj.GroupName = groupName;
                else
                    obj.GroupName = null;
                SubmitChanges();
            }
        }
        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="UniformIssuancePolicyID"></param>
        public void Delete(Int64 UniformIssuancePolicyID)
        {
            IssuanceCompanyAddressRepository objIssuanceCompanyAddressRepository = new IssuanceCompanyAddressRepository();
            objIssuanceCompanyAddressRepository.DeleteAddress(UniformIssuancePolicyID);

            // delete children from UniformIssuancePolicyItem
            UniformIssuancePolicyItemRepository objUniformIssuancePolicyItemRepository = new UniformIssuancePolicyItemRepository();
            objUniformIssuancePolicyItemRepository.DeleteByUniformIssuancePolicyID(UniformIssuancePolicyID);

            //delete record
            UniformIssuancePolicy objUniformIssuancePolicy = GetById(UniformIssuancePolicyID);
            Delete(objUniformIssuancePolicy);
            SubmitChanges();
        }
        /// <summary>
        /// Nagmani 21/01/2011
        /// Thismethod is used to bind image from table.
        /// </summary>
        /// <returns></returns>
        public List<SelectUniformIssuanceImageResult> GetIssuanceProductImage(int associationType,long MasteritemNo,int PolicyID )
        {
            List<SelectUniformIssuanceImageResult> obj = new List<SelectUniformIssuanceImageResult>();
            return obj = db.SelectUniformIssuanceImage(associationType,MasteritemNo, PolicyID).ToList<SelectUniformIssuanceImageResult>();
        }
        //public List<SelectPolicyDateAndBeforeAfterResult> GetPolicyDateAfterBefore(int CompanyId, int WorkgroupId, int Gendertype)
        //{
        //    List<SelectPolicyDateAndBeforeAfterResult> obj = new List<SelectPolicyDateAndBeforeAfterResult>();
        //    return obj = db.SelectPolicyDateAndBeforeAfter(CompanyId, WorkgroupId, Gendertype).ToList<SelectPolicyDateAndBeforeAfterResult>();
        //}


        //public List<SelectIssuancePolicyNameResult> GetIssuancePolicyName(int CompanyId, int WorkgroupId, int Gendertype,string BeforeAfter,DateTime ? HireDAte,int UniformIssuanceId)
        //{
        //    List<SelectIssuancePolicyNameResult> obj = new List<SelectIssuancePolicyNameResult>();
        //    return obj = db.SelectIssuancePolicyName(CompanyId, WorkgroupId, Gendertype, BeforeAfter, HireDAte, UniformIssuanceId).ToList<SelectIssuancePolicyNameResult>();
           
        //}

        public string UpdateMOASUserid(Int64 uniformIssuancePolicyId)
        {
            db.UpdateMOASUSERID(uniformIssuancePolicyId);
            db.SubmitChanges();
            return "Updated";
        }

        public List<GetIssuancePoliciesByUserInfoIDResult> GetIssuancePolicyByUserInfoID(Int64 UserInfoID)
        {
            return db.GetIssuancePoliciesByUserInfoID(UserInfoID).ToList();
        }       
    }
}
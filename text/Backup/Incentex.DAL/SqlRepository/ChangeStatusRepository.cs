using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
   public class ChangeStatusRepository : RepositoryBase
    {

       public void UpdateUserStatus(Int64 userInfoID, Char IssuancePolicyStatus, Int64 employeeTypeID, Int64 wLSStatusID, Int64 createdBy, Int64 workgroupID, String EmployeeUnifomrAccess)
       {
           ChangeStatusRepository objChangeStatusRepo = new ChangeStatusRepository();
           var objUserinformation = (from u in db.GetTable<UserInformation>()
                                     where u.UserInfoID == userInfoID
                                     select u).SingleOrDefault();
           if (objUserinformation != null)
           {
               objUserinformation.WLSStatusId = wLSStatusID;
           }
           var objCompanyEmployee = (from c in db.GetTable<CompanyEmployee>()
                                     where c.UserInfoID == userInfoID
                                     select c).SingleOrDefault();
           if (objCompanyEmployee != null)
           {
               //Update Company Employee
               objCompanyEmployee.IssuancePolicyStatus = IssuancePolicyStatus;
               objCompanyEmployee.EmployeeTypeID = employeeTypeID;
               objCompanyEmployee.WorkgroupID = workgroupID;
               if (!String.IsNullOrEmpty(EmployeeUnifomrAccess))
                   objCompanyEmployee.EmployeeUniformAccess = EmployeeUnifomrAccess;

               //Insert Record in Note Detail
               NoteDetail objNoteDetail = new NoteDetail();
               objNoteDetail.Notecontents = "User Status Changed";
               objNoteDetail.NoteFor = Convert.ToString(Incentex.DAL.Common.DAEnums.NoteForType.CompanyEmployee);
               objNoteDetail.ForeignKey = objCompanyEmployee.CompanyEmployeeID;
               objNoteDetail.CreatedBy = createdBy;
               objNoteDetail.CreateDate = DateTime.Now;
               objNoteDetail.UpdatedBy = createdBy;
               objNoteDetail.UpdateDate = DateTime.Now;
               objChangeStatusRepo.Insert(objNoteDetail);
               objChangeStatusRepo.SubmitChanges();
           }
           db.SubmitChanges();

       }
       /// <summary>
       /// Save records as per user level Reactive issuance policy
       /// </summary>
       /// <param name="userID"></param>
       /// <param name="policyID"></param>
       /// <param name="createdBy"></param>
       public void SaveRecordsForReActivatedPolicy(Int64 userID, Int64 policyID, Int64 createdBy)
       {
           UserReActivatedIssuancePolicy objReAct = new UserReActivatedIssuancePolicy();
           objReAct.UserInfoID = userID;
           objReAct.UniformIssuancePolicyID = policyID;
           objReAct.CreatedDate = DateTime.Now;
           objReAct.CreatedBy = createdBy;
           objReAct.IsOrder = false;
           this.Insert(objReAct);
           this.SubmitChanges();
       }
       /// <summary>
       /// Check is user issuance policy is active or not
       /// </summary>
       /// <param name="userID"></param>
       /// <param name="policyID"></param>
       /// <returns></returns>
       public Boolean IsUserReActivatedPolicyExist(Int64 userID, Int64 policyID)
       {
           Boolean IsActivated = false;
           var qry = db.UserReActivatedIssuancePolicies.Where(q => q.UserInfoID == userID && q.UniformIssuancePolicyID == policyID && q.IsOrder == false).Select(s => s.IsOrder);
           if (qry.Count() >0)
               IsActivated = true;

           return IsActivated;
       }

       /// <summary>
       /// This method is used to update Group name of Uniform Issuance Policy
       /// </summary>
       /// <param name="UniformIssuancePolicyID"></param>
       /// <param name="groupName"></param>
       public void UpdateUserReActivatedIssuancePolicy(Int64 userID, Int64 policyID)
       {
           UserReActivatedIssuancePolicy obj = new UserReActivatedIssuancePolicy();
           obj = (from s in db.UserReActivatedIssuancePolicies
                  where s.UserInfoID == userID && s.UniformIssuancePolicyID == policyID && s.IsOrder == false
                  select s).FirstOrDefault();
           if (obj != null)
           {
              
               obj.IsOrder = true;
               this.SubmitChanges();
           }
       }
       /// <summary>
       /// Save records as per user level publish issuance policy
       /// </summary>
       /// <param name="userID"></param>
       /// <param name="policyID"></param>
       /// <param name="createdBy"></param>
       public void SaveRecordsForUserPublishIssuancePolicy(Int64 userID, Int64 policyID, Int64 createdBy)
       {
           UserPublishIssuancePolicy objPubPolicy = new UserPublishIssuancePolicy();
           objPubPolicy.UserInfoID = userID;
           objPubPolicy.UniformIssuancePolicyID = policyID;
           objPubPolicy.CreatedDate = DateTime.Now;
           objPubPolicy.CreatedBy = createdBy;
           objPubPolicy.IsPublish = true;
           this.Insert(objPubPolicy);
           this.SubmitChanges();
       }
       /// <summary>
       /// Update Existing Publish Issuance Policy 
       /// </summary>
       /// <param name="userID"></param>
       public void UpdateExistingPublishIssuancePolicy(Int64 userID)
       {
           var qry = (from s in db.UserPublishIssuancePolicies
                      where s.UserInfoID == userID && s.IsPublish == true
                      select s).ToList();
           UserPublishIssuancePolicy objPubPolicy = new UserPublishIssuancePolicy();
           foreach (var item in qry)
           {
              
               objPubPolicy = db.UserPublishIssuancePolicies.FirstOrDefault(q => q.PublishID == item.PublishID);
               objPubPolicy.IsPublish = false;
               this.SubmitChanges();
           }
       }
       /// <summary>
       /// get UserPublishIssuancePolicies
       /// </summary>
       /// <param name="userID"></param>
       /// <returns></returns>
       public List<UserPublishIssuancePolicy> GetUserPublishIssuancePolicies(Int64 userID)
       {
           return db.UserPublishIssuancePolicies.Where(q => q.UserInfoID == userID && q.IsPublish == true).ToList();
       }
       /// <summary>
       ///  Get all SubCategory Name
       /// </summary>
       /// <param name="departmentId"></param>
       /// <returns></returns>
       public String GetAlldepartmentList(String departmentId)
       {
           String SubCategory = String.Empty;
           string [] ids = departmentId.Split(',');

           foreach (var item in ids)
           {
               if (!String.IsNullOrEmpty(SubCategory) && !String.IsNullOrEmpty(item))
                   SubCategory = SubCategory + "," + db.SubCategories.Where(q => q.SubCategoryID == Convert.ToInt64(item) && q.CategoryID == 1).Select(s => s.SubCategoryName).FirstOrDefault().ToString();
               else if (!String.IsNullOrEmpty(item))
                   SubCategory = db.SubCategories.Where(q => q.SubCategoryID == Convert.ToInt64(item) && q.CategoryID == 1).Select(s => s.SubCategoryName).FirstOrDefault().ToString();
           }
           return SubCategory;
       }
    }
}

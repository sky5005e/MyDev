using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class EmployeeTrainingCenterRepository : RepositoryBase
    {
        #region Common Functions
        /// <summary>
        /// Get all Employee Training Center by Search 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="uplodedby"></param>
        /// <param name="searchkeyword"></param>
        /// <returns></returns>
        public List<EmployeeTrainingCentreCustom> GetEmployeeTrainingcenterBySearch(string filename, long uplodedby, string searchkeyword, long? EmployeeTrainingType)
        {
            List<EmployeeTrainingCentreCustom> qry = (from e in db.EmployeeTrainingCenters
                                                      join u in db.UserInformations on e.UplodedBy equals u.UserInfoID
                                                      from objFinaljoinLastViewBy in db.UserInformations.Where(t => t.UserInfoID == e.LastViewBy).DefaultIfEmpty()
                                                      where (!string.IsNullOrEmpty(filename) ? e.FileName.Contains(filename) : true) &&
                                                            (!string.IsNullOrEmpty(searchkeyword) ? e.Searchkeyword.Contains(searchkeyword) : true) &&
                                                            (uplodedby != 0 ? e.UplodedBy == uplodedby : true) &&
                                                            (EmployeeTrainingType != 0 ? e.EmployeeTrainingType == EmployeeTrainingType : true)
                                                      select new EmployeeTrainingCentreCustom
                                                      {
                                                          EmployeeTrainingCenterID = e.EmployeeTrainingCenterID,
                                                          FileName = e.FileName,
                                                          UplodedDate = e.UplodedDate,
                                                          UplodedBy = u.LastName != null ? (u.FirstName + " " + u.LastName) : u.FirstName,
                                                          FileSize = e.FileSize.HasValue ? (e.FileSize.Value / 1024) / 1024 : 0,    //Convert Bytes to MB
                                                          OriginalFileName = e.OriginalFileName,
                                                          YouTubeVideoID = e.YouTubeVideoID,
                                                          LastViewBy = (objFinaljoinLastViewBy != null && objFinaljoinLastViewBy.FirstName != null && objFinaljoinLastViewBy.LastName != null) ? (objFinaljoinLastViewBy.FirstName + " " + objFinaljoinLastViewBy.LastName) : string.Empty
                                                      }).ToList<EmployeeTrainingCentreCustom>();

            return qry;
        }

        public EmployeeTrainingCenter GetEmployeeTrainingCenterById(long EmployeeTrainingID)
        {
            return (from e in db.EmployeeTrainingCenters
                    where e.EmployeeTrainingCenterID == EmployeeTrainingID
                    select e).SingleOrDefault();
        }

        public void UpdateEmployeeTrainingCenter(long UpdateBy, long EmployeeTrainingID)
        {
            EmployeeTrainingCenter objEmpTraining = GetEmployeeTrainingCenterById(EmployeeTrainingID);
            if (objEmpTraining != null)
            {
                objEmpTraining.LastViewBy = UpdateBy;
                base.SubmitChanges();
            }
        }

        public int CheckEmployeeTrainingExistence(string EmployeeTrainingType, long TrainingTypeID)
        {
            var result = (from l in db.INC_Lookups
                          where l.sLookupName.ToLower() == EmployeeTrainingType.ToLower() && l.iLookupCode.Contains("EmployeeTrainingType")
                                && l.iLookupID != TrainingTypeID
                          select l.iLookupID).Count();

            return result;
        }

        public void DeleteEmployeeTrainingCenter(long EmployeeTrainingID)
        {
            EmployeeTrainingCenter obj = GetEmployeeTrainingCenterById(EmployeeTrainingID);
            if (obj != null)
            {
                db.EmployeeTrainingCenters.DeleteOnSubmit(obj);
                base.SubmitChanges();
            }
        }
        #endregion

        #region Extension Class
        public class EmployeeTrainingCentreCustom
        {
            public long EmployeeTrainingCenterID { get; set; }

            public string FileName { get; set; }

            public string SearchKeyWord { get; set; }

            public string Email { get; set; }

            public decimal FileSize { get; set; }

            public string OriginalFileName { get; set; }

            public long EmployeeTrainingType { get; set; }

            public System.DateTime UplodedDate { get; set; }

            public string UplodedBy { get; set; }

            public string LastViewBy { get; set; }

            public string YouTubeVideoID { get; set; }
        }
        #endregion
    }


}

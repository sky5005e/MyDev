using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class AccessRightRepository : RepositoryBase
    {
        public List<AccessMenu> GetAccessMenu()
        {
            return db.AccessMenus.ToList();
        }

        public List<GetAccessRightsByUserInfoIDResult> GetAccessRights(Int64? UserInfoID)
        {
            return db.GetAccessRightsByUserInfoID(UserInfoID).ToList();
        }

        public AccessRight GetAccessRight(Int64 AccessRightID)
        {
            return db.AccessRights.FirstOrDefault(le => le.AccessRightID == AccessRightID);
        }

        public List<GetAccessRightsMenuWiseResult> CheckAcessRight(Int64? UserInfoID, String MenuItem, Int64 ParentMenuID)
        {
            return db.GetAccessRightsMenuWise(UserInfoID, MenuItem, ParentMenuID).ToList();
        }

        public List<GetMainMenuForIEByUserInfoIDResult> GetMainMenuForIE(Int64? UserInfoID)
        {
            return db.GetMainMenuForIEByUserInfoID(UserInfoID).ToList();
        }
    }
}
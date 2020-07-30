using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
   public class UserTypeRepository : RepositoryBase
   {
       public List<UserType> GetUserTypes()
       {
           return db.UserTypes.Where(u => u.UserTypeID != Convert.ToInt64(DAL.Common.DAEnums.UserTypes.SuperAdmin)).ToList();
       }
   }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class Page404ErrorRepository : RepositoryBase
    {
        public List<Get404ErrorDetailsResult> GetErrorDetails(Int64? CompanyID, Int64? UserID, string ModuleName, string FromDate, string ToDate, string keyWord, Int32 PageSize, Int32 PageIndex, string SortColumn, string SortDirection)
        {
            return db.Get404ErrorDetails(CompanyID, UserID, ModuleName, FromDate, ToDate, keyWord, PageSize, PageIndex, SortColumn, SortDirection).ToList();
        }

        public List<Get404ErrorNotesResult> GetNotesForError(Int32? UserInfoID)
        {
            return db.Get404ErrorNotes(UserInfoID).ToList();
        }
    }
}

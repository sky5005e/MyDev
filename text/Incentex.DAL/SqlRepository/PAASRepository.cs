using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class PAASRepository : RepositoryBase
    {
        public List<SelectPAASOrderCSVFieldsResult> SelectPAASOrderCSVLines()
        {
            return db.SelectPAASOrderCSVFields().ToList();
        }

        public Int32 UpdatePAASOrdersAsSent(String OrderIDs, String DisplayName)
        {
            return db.UpdatePAASOrdersAsSent(OrderIDs, DisplayName);
        }

        public void DeleteDownloadedPAASOrder(Int64 ID, Boolean Delete)
        {
            DownloadedPAASOrder objDownload = db.DownloadedPAASOrders.FirstOrDefault(le => le.ID == ID);
            if (objDownload != null)
            {
                if (Delete)
                {
                    db.DownloadedPAASOrders.DeleteOnSubmit(objDownload);
                }
                else
                {
                    objDownload.IsDeleted = true;
                }
                db.SubmitChanges();
            }
        }

        public List<vw_DownloadedPAASOrder> SelectDownloadedOrders()
        {
            return db.vw_DownloadedPAASOrders.ToList();
        }

        public List<SelectCSVFilesByDatesResult> SelectCSVFilesByDate(String FromDate, String ToDate)
        {
            return db.SelectCSVFilesByDates(FromDate, ToDate).ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class ServiceTicketAttachmentRepository : RepositoryBase
    {
        public List<ServiceTicketAttachment> GetByServiceTicektId(Int64 ServiceTicketID)
        {
            return db.ServiceTicketAttachments.Where(le => le.ServiceTicketID == ServiceTicketID && le.IsDeleted == false).ToList();
        }

        //Delete By Support Ticket Id
        public void DeleteByServiceTicketID(Int64 ServiceTicketID, Int64? UserInfoID)
        {
            List<ServiceTicketAttachment> lstServiceTicketAttachments = db.ServiceTicketAttachments.Where(le => le.ServiceTicketID == ServiceTicketID).ToList();
            try
            {
                if (lstServiceTicketAttachments != null)
                {
                    foreach (ServiceTicketAttachment objAttachment in lstServiceTicketAttachments)
                    {
                        objAttachment.IsDeleted = true;
                        objAttachment.DeletedBy = UserInfoID;
                        objAttachment.DeletedDate = DateTime.Now;
                    }
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete By Attachment Id
        public void DeleteByAttachmentID(Int64 AttachmentID, Int64? UserInfoID)
        {
            ServiceTicketAttachment objAttachment = db.ServiceTicketAttachments.FirstOrDefault(le => le.AttachmentID == AttachmentID);
            try
            {
                if (objAttachment != null)
                {
                    objAttachment.IsDeleted = true;
                    objAttachment.DeletedBy = UserInfoID;
                    objAttachment.DeletedDate = DateTime.Now;
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

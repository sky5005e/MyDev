using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;
using System.Collections;

namespace Incentex.DAL.SqlRepository
{
    public class SupplierPartnerRepo : RepositoryBase
    {
        IQueryable<SupplierPartner> GetAllQuery()
        {
            IQueryable<SupplierPartner> qry = from s in db.SupplierPartners
                                       select s;
            return qry;
        }
        public enum SupplierPartnerSortExpType
        {
            Name,
            URL,
            LoginName,
            Password,
            CratedDate,
            Status
        }

        public List<SupplierPartner> GetSupplierPartnerList(SupplierPartnerSortExpType SortExp, DAEnums.SortOrderType SortOrder)
        {
            List<SupplierPartner> qry = GetAllQuery().ToList<SupplierPartner>();
            var List = qry.ToList<SupplierPartner>();
            switch (SortExp)
            {
                case SupplierPartnerSortExpType.Name :
                    List = qry.OrderBy(s => s.Name).ToList();
                    break;
                case SupplierPartnerSortExpType.URL:
                    List = qry.OrderBy(s => s.URL).ToList();
                    break;
                case SupplierPartnerSortExpType.Password:
                    List = qry.OrderBy(s => s.Password).ToList();
                    break;
                case SupplierPartnerSortExpType.CratedDate :
                    List = qry.OrderBy(s => s.CratedDate).ToList();
                    break;
            }

            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                List.Reverse();
            }

            return List;
        }
        public int GetCountSupplierId(Int64 SupplierPartnerID)
        {
            IQueryable<SupplierPartner> qry = GetAllQuery().Where(e => e.SupplierPartnerID == SupplierPartnerID).OrderBy(e => e.Name);
            return qry.Count();
        }
        public void Delete(Int64 SupplierPartnerID)
        {
            var matched = (from c in db.GetTable<SupplierPartner>()
                           where c.SupplierPartnerID == Convert.ToInt64(SupplierPartnerID)
                               select c).SingleOrDefault();
            try
            {
                if (matched != null)
                {
                    db.SupplierPartners.DeleteOnSubmit(matched);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public IQueryable<SupplierPartner> GetAllSupplier()
        {
            var lst = (from s in db.SupplierPartners
                       select s);
            return lst;
        }

        public IQueryable<SupplierPartner> GetAllSupplierActive()
        {
            var lst = (from s in db.SupplierPartners
                       where s.Status==true
                       select s);
            return lst;
        }
        public void UpdateStatusForSupplier(int Sid, bool Flag)
        {

            {
                var obj = db.Update_StatusForSupplier(Sid, Flag);

            }
        }
        public SupplierPartner GetById(Int64 SupplierPartnerID)
        {
            IQueryable<SupplierPartner> qry = GetAllQuery().Where(s => s.SupplierPartnerID == SupplierPartnerID);
            SupplierPartner obj = GetSingle(qry.ToList());
            return obj;
        }

    }
}

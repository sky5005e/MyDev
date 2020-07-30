using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
  public  class SupplierDocumentRepository:RepositoryBase
    {
      IQueryable<SupplierDocument> GetAllQuery()
      {
          IQueryable<SupplierDocument> qry = from d in db.SupplierDocuments
                                     select d;
          return qry;
      }

      public List<SupplierDocument> GetByDocumentTypeId(Int64 DocumentTypeID, Int64 ForeignKey)
      {
          IQueryable<SupplierDocument> qry = GetAllQuery().Where(d => d.DocumentTypeID == DocumentTypeID);
          qry = qry.Where(d => d.ForeignKey == ForeignKey);

          List<SupplierDocument> objList = qry.ToList();
          return objList;
      }

      public SupplierDocument GetByTypeAndName(Int64 DocumentTypeID, string OriginalFileName)
      {
          IQueryable<SupplierDocument> qry = GetAllQuery().Where(d => d.DocumentTypeID == DocumentTypeID);
          qry = qry.Where(d => d.OriginalFileName.Equals(OriginalFileName));

          SupplierDocument obj = GetSingle(qry.ToList());
          return obj;
      }

      public void Delete(Int64 DocumentId)
      {
          SupplierDocument obj = GetSingle(GetAllQuery().Where(d => d.DocumentId == DocumentId).ToList());

          if (obj != null)
          {
              base.Delete(obj);
              base.SubmitChanges();
          }
      }

      public void Delete(Int64 DocumentTypeID, Int64 ForeignKey)
      {
          IQueryable<SupplierDocument> qry = GetAllQuery().Where(d => d.DocumentTypeID == DocumentTypeID);
          qry = qry.Where(d => d.ForeignKey == ForeignKey);

          db.SupplierDocuments.DeleteAllOnSubmit(qry.ToList());
          db.SubmitChanges();
      }

      public void DeleteEmployeeDocuments(string DocFor, Int64 ForeignKey)
      {
          IQueryable<SupplierDocument> qry = GetAllQuery().Where(d => d.DocumentFor == DocFor);
          qry = qry.Where(d => d.ForeignKey == ForeignKey);

          db.SupplierDocuments.DeleteAllOnSubmit(qry.ToList());
          db.SubmitChanges();
      }


    }
}

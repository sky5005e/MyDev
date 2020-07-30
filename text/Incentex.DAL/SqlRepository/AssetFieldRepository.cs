using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class AssetFieldRepository : RepositoryBase
    {
        IQueryable<EquipmentFieldMaster> GetAllFieldMaster()
        {
            IQueryable<EquipmentFieldMaster> qry = from c in db.EquipmentFieldMasters
                                                   orderby c.FieldMasterID
                                                   select c;
            return qry;
        }
        IQueryable<EquipmentFieldDetail> GetAllFieldDetail()
        {
            IQueryable<EquipmentFieldDetail> qry = from c in db.EquipmentFieldDetails
                                                   orderby c.FieldDetailID
                                                   select c;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="FieldMasterID"></param>
        /// <returns></returns>
        public EquipmentFieldMaster GetFieldMasterById(Int64 FieldMasterID)
        {
            //IQueryable<EquipmentFieldMaster> qry = GetAllFieldMaster().Where(s => s.FieldMasterID == FieldMasterID);
            //EquipmentFieldMaster obj = GetSingle(qry.ToList());

            EquipmentFieldMaster obj = (from c in db.EquipmentFieldMasters
                                        orderby c.FieldMasterID
                                        where c.FieldMasterID == FieldMasterID
                                        select c).SingleOrDefault();
            return obj;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="EquipmentTypeID"></param>
        /// <returns></returns>
        public List<EquipmentFieldMaster> GetFieldResult(Int64 CompanyID, Int64 EquipmentTypeID)
        {
            //var objlist = GetAllFieldMaster().ToList();
            var objlist = (from c in db.EquipmentFieldMasters
                           orderby c.FieldMasterID
                           where (CompanyID != 0 ? c.CompanyID == CompanyID : true) &&
                                 (EquipmentTypeID != 0 ? c.EquipmentTypeID == EquipmentTypeID : true)
                           select c).ToList();

            //if (CompanyID != 0)
            //{
            //    objlist = objlist.Where(c => c.CompanyID == CompanyID).ToList();
            //}
            //if (EquipmentTypeID != 0)
            //{
            //    objlist = objlist.Where(c => c.EquipmentTypeID == EquipmentTypeID).ToList();
            //}
            return objlist;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="EquipmentTypeID"></param>
        /// <returns></returns>
        public List<EquipmentFieldMaster> GetFieldsForAssetProfile(Int64 CompanyID, Int64 EquipmentTypeID)
        {
            //var objlist = GetAllFieldMaster().ToList();

            //if (CompanyID != 0)
            //{
            //    objlist = objlist.Where(c => c.CompanyID == CompanyID && c.EquipmentTypeID == EquipmentTypeID).ToList();
            //}

            var objlist = (from c in db.EquipmentFieldMasters
                           orderby c.FieldMasterID
                           where (CompanyID != 0 ? (c.CompanyID == CompanyID && c.EquipmentTypeID == EquipmentTypeID) : true)
                           select c).ToList();

            return objlist;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public List<EquipmentFieldDetail> GetFieldDetailDesc(Int64 CompanyID, Int64 EquipmentMasterID)
        {
            //var objlist = GetAllFieldDetail().ToList();
            //if (CompanyID != 0)
            //{
            //    objlist = objlist.Where(c => c.CompanyID == CompanyID && c.EquipmentMasterID == EquipmentMasterID).ToList();
            //}

            var objlist = (from c in db.EquipmentFieldDetails
                           orderby c.FieldDetailID
                           where (CompanyID != 0 ? (c.CompanyID == CompanyID && c.EquipmentMasterID == EquipmentMasterID) : true)
                           select c).ToList();

            return objlist;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="FieldMasterID"></param>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public EquipmentFieldDetail GetFieldDetailById(Int64 FieldMasterID, Int64 EquipmentMasterID)
        {
            //IQueryable<EquipmentFieldDetail> qry = GetAllFieldDetail().Where(s => s.FieldMasterID == FieldMasterID && s.EquipmentMasterID == EquipmentMasterID);
            //EquipmentFieldDetail obj = GetSingle(qry.ToList());

            EquipmentFieldDetail obj = (from c in db.EquipmentFieldDetails
                                        orderby c.FieldDetailID
                                        where c.FieldMasterID == FieldMasterID && c.EquipmentMasterID == EquipmentMasterID
                                        select c).FirstOrDefault();
            return obj;
        }

        public void DeleteField(Int64 FieldMasterID)
        {

            var FieldDetail = (from m in db.GetTable<EquipmentFieldDetail>()//Delete FROM 
                               where m.FieldMasterID == FieldMasterID
                               select m);
            if (FieldDetail != null)
            {
                db.EquipmentFieldDetails.DeleteAllOnSubmit(FieldDetail);
            }
            db.SubmitChanges();

            var FieldMaster = (from m in db.GetTable<EquipmentFieldMaster>()
                               where m.FieldMasterID == FieldMasterID
                               select m);
            if (FieldMaster != null)
            {
                db.EquipmentFieldMasters.DeleteAllOnSubmit(FieldMaster);
            }
            db.SubmitChanges();

        }

    }
}

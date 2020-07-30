using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class PurchaseOrderManagmentRepository : RepositoryBase
    {
        #region Common Functions

        public List<StoreProductCustom> GetAllMasterItemBysupplierID(long supplierID)
        {
            var result = (from sp in db.StoreProducts
                          join pi in db.ProductItems on sp.StoreProductID equals pi.ProductId
                          join il in db.INC_Lookups on pi.MasterStyleID equals il.iLookupID
                          join st in db.CompanyStores on sp.StoreId equals st.StoreID
                          join com in db.Companies on st.CompanyID equals com.CompanyID
                          where sp.SupplierId == supplierID &&
                                sp.IsDeleted == false &&
                                sp.StatusID == Convert.ToInt64(Common.DAEnums.status.Active)    

                          select new StoreProductCustom
                          {
                              storeproductid = sp.StoreProductID,
                              productname = com.CompanyName,
                              summary = il.sLookupName + "(" + com.CompanyName + ")"
                          }).Distinct().ToList();

            return result;
        }

        public List<StoreProductCustom> GetAllMasterItemByCompanyID(Int64 CompanyID)
        {
            var result = (from sp in db.StoreProducts
                          join pi in db.ProductItems on sp.StoreProductID equals pi.ProductId
                          join inclookup in db.INC_Lookups on pi.MasterStyleID equals inclookup.iLookupID
                          join st in db.CompanyStores on sp.StoreId equals st.StoreID
                          join com in db.Companies on st.CompanyID equals com.CompanyID
                          where (CompanyID != 0 ? com.CompanyID == CompanyID : true) && sp.StatusID == 135
                          select new StoreProductCustom
                          {
                              storeproductid = sp.StoreProductID,
                              productname = Convert.ToString(pi.MasterStyleID),
                              summary = inclookup.sLookupName
                          }).ToList();
            return result;
        }

        public string GetProductImages(long storeproductid)
        {
            return (from i in db.StoreProductImages
                    where i.StoreProductID == storeproductid &&
                          i.ProductImageActive == 1 &&
                          i.IsDeleted == false
                    select i.ProductImage).SingleOrDefault();
        }
        public Int64 GetMasterStyleID(Int64 StoreProductId)
        {
            return (from sp in db.StoreProducts
                    join pi in db.ProductItems on sp.StoreProductID equals pi.ProductId
                    where sp.StoreProductID == StoreProductId
                    select pi.MasterStyleID).FirstOrDefault();
        }

        public void InsertPurchaseOrderDetails(List<PurchaseOrderDetail> lstpurchaseorderdetails, long PurchaseOrderID)
        {
            foreach (PurchaseOrderDetail item in lstpurchaseorderdetails)
            {
                item.PurchaseOrderID = PurchaseOrderID;
                base.Insert(item);
                base.SubmitChanges();
            }
        }

        public List<SearchPurchaseOrder> GetPurchaseOrderDetailsBySearch(long VendorID, long MasterItemID, string OrderNumber)
        {
            var result = (from po in db.PurchaseOrderMasters
                          join l1 in db.INC_Lookups on po.OrderSentBy equals l1.iLookupID
                          join l2 in db.INC_Lookups on po.OrderFor equals l2.iLookupID
                          join lS in db.INC_Lookups on po.StatusID equals lS.iLookupID into sr
                          from sl in sr.DefaultIfEmpty()
                          where po.IsDeleted == false &&
                                ((VendorID > 0 ? po.VendorID == VendorID : true) &&
                                 (MasterItemID > 0 ? po.MasterItemID == MasterItemID : true) &&
                                 (!string.IsNullOrEmpty(OrderNumber) ? po.PurchaseOrderNumber.Equals(OrderNumber) : true))
                          select new SearchPurchaseOrder
                          {
                              PurchaseOrderID = po.PurchaseOrderID,
                              PurchaseOrderNumber = po.PurchaseOrderNumber,
                              OrderedBy = l1.sLookupName,
                              OrderedOn = l2.sLookupName,
                              DeliveryDate = (po.DeliveryDate == null ? null : po.DeliveryDate),
                              StartProductionDate = (po.CreatedDate == null ? null : po.CreatedDate),
                              FileName = po.FileName,
                              OriginalFileName = po.OriginalFileName,
                              extension = po.extension,
                              VendorID = po.VendorID,
                              Status = sl.sLookupName,
                              RcvQuantity = GetRcvQuantity(po.PurchaseOrderID)

                          }).ToList();

            return result;
        }
        public Int64 GetRcvQuantity(Int64 PurchaseOrderID)
        {
            return (from p in db.PurchaseOrderDetails
                    where p.PurchaseOrderID == PurchaseOrderID
                    select p.Quantity).Sum(q=>q.Value);
        }
        /// <summary>
        ///  Get TodayFollowUp details
        ///  confirmed delivery date is null, it's also display here 
        ///  ShowonTomorrowsReport is true than Createdate + 1 is today date than display this recoreds also
        /// </summary>
        /// <param name="CreateBy"></param>
        /// <returns></returns>
        public List<SearchPurchaseOrder> GetTodayFollowUpDetails(long CreateBy)
        {
            DateTime currentdate = System.DateTime.Today;

            var result = (from po in db.PurchaseOrderMasters
                          join l1 in db.INC_Lookups on po.OrderSentBy equals l1.iLookupID
                          join l2 in db.INC_Lookups on po.OrderFor equals l2.iLookupID
                          where po.IsDeleted == false
                          select new SearchPurchaseOrder
                          {
                              PurchaseOrderID = po.PurchaseOrderID,
                              PurchaseOrderNumber = po.PurchaseOrderNumber,
                              OrderedBy = l1.sLookupName,
                              OrderedOn = l2.sLookupName,
                              DeliveryDate = po.DeliveryDate.Value,
                              FileName = po.FileName,
                              OriginalFileName = po.OriginalFileName,
                              extension = po.extension,
                              VendorID = po.VendorID,
                              FollowUpdate = (from f in db.PurchaseOrderFollowUps where f.CreatedBy == CreateBy && f.PurchaseOrderID == po.PurchaseOrderID orderby f.PurchaseOrderFollowUpID descending select f.LastFollowUpDate).FirstOrDefault(),
                              FollowUpCreateddate = (from f in db.PurchaseOrderFollowUps where f.CreatedBy == CreateBy && f.PurchaseOrderID == po.PurchaseOrderID orderby f.PurchaseOrderFollowUpID descending select f.CreatedDate).FirstOrDefault(),
                              Showontomorrow = (from f in db.PurchaseOrderFollowUps where f.CreatedBy == CreateBy && f.PurchaseOrderID == po.PurchaseOrderID orderby f.PurchaseOrderFollowUpID descending select f.ShowonTomorrows).FirstOrDefault()
                          }).ToList().Where(t =>
                              (t.FollowUpdate.HasValue ? (t.FollowUpdate.Value.ToString("dd/MM/yyyy") == currentdate.ToString("dd/MM/yyyy")) : false) ||
                              (t.DeliveryDate.HasValue ? t.DeliveryDate.Value.ToString("dd/MM/yyyy") == currentdate.ToString("dd/MM/yyyy") : true) ||
                              ((t.Showontomorrow.HasValue && t.Showontomorrow.Value) ? t.FollowUpCreateddate.Value.AddDays(1).ToString("dd/MM/yyyy") == currentdate.ToString("dd/MM/yyyy") : false)).ToList();


            return result;
        }

        public PurchaseOrderMaster GetPurchaseOrderByID(long PurchaseOrderID)
        {
            return (from po in db.PurchaseOrderMasters where po.PurchaseOrderID == PurchaseOrderID select po).SingleOrDefault();
        }
        public PurchaseOrderDetail GetPurchaseOrderDetailByProductItemID(long ProductItemID)
        {
            return (from po in db.PurchaseOrderDetails where po.ProductItemID == ProductItemID select po).SingleOrDefault();
        }
        public void DeletePurchaseOrder(long PurchaseOrderID, long UserID)
        {
            PurchaseOrderMaster objOrder = GetPurchaseOrderByID(PurchaseOrderID);

            if (objOrder != null)
            {
                objOrder.IsDeleted = true;
                objOrder.DeletedBy = UserID;
                objOrder.DeletedDate = System.DateTime.Now;
                base.SubmitChanges();
            }
        }

        /// <summary>
        /// To Get the Purchase Order Info for Vendor 
        /// </summary>
        /// <param name="POID"></param>
        /// <param name="VendorID"></param>
        /// <returns></returns>
        public List<SearchPurchaseOrderVendor> GetPurchaseOrderDetailsByPOID(Int64 POID, Int64 VendorID)
        {
            var qry = (from pom in db.PurchaseOrderMasters
                       join pod in db.PurchaseOrderDetails on pom.PurchaseOrderID equals pod.PurchaseOrderID
                       join pim in db.ProductItems on pod.ProductItemID equals pim.ProductItemID
                       join lkOrdby in db.INC_Lookups on pom.OrderSentBy equals lkOrdby.iLookupID
                       join lkOrdfor in db.INC_Lookups on pom.OrderFor equals lkOrdfor.iLookupID
                       join lkMstSt in db.INC_Lookups on pim.MasterStyleID equals lkMstSt.iLookupID
                       join lkPrdSt in db.INC_Lookups on pim.ProductStyleID equals lkPrdSt.iLookupID
                       join lkSz in db.INC_Lookups on pim.ItemSizeID equals lkSz.iLookupID
                       join lkClr in db.INC_Lookups on pim.ItemColorID equals lkClr.iLookupID
                       where pom.IsDeleted == false && pom.PurchaseOrderID == POID
                       && pom.VendorID == VendorID
                       select new SearchPurchaseOrderVendor
                       {
                           PurchaseOrderID = pom.PurchaseOrderID,
                           PurchaseOrderNumber = pom.PurchaseOrderNumber,
                           ProductItemID = pim.ProductItemID,
                           ProductStyle = lkPrdSt.sLookupName,
                           ItemColor = lkClr.sLookupName,
                           ItemNumber = pim.ItemNumber,
                           ItemSize = lkSz.sLookupName,
                           Price = Convert.ToDecimal(pod.Price),
                           Quantity = Convert.ToInt32(pod.Quantity),
                           MasterStyle = lkMstSt.sLookupName,
                           OrderedBy = lkOrdby.sLookupName,
                           OrderedOn = lkOrdfor.sLookupName,
                           ExpDeliveryDate = (pom.DeliveryDate == null ? null : pom.DeliveryDate),
                           ConFirmedDeliveryDate = (pom.DeliveryDate == null ? null : pom.DeliveryDate),
                           StartProductionDate = (pom.CreatedDate == null ? null : pom.CreatedDate),
                           StatusID = Convert.ToInt64(pom.StatusID),
                           FileName = pom.FileName,
                           OriginalFileName = pom.OriginalFileName,
                           extension = pom.extension,
                           FinalQty = pod.FinalQty

                       }).ToList();

            return qry;
        }

        public PurchaseOrderDetailCustom GetPurchaseOrderDetails(long PurchaseOrderID)
        {
            var result = (from po in db.PurchaseOrderMasters
                          join s in db.Suppliers on po.VendorID equals s.SupplierID
                          join u in db.UserInformations on s.UserInfoID equals u.UserInfoID
                          where po.IsDeleted == false &&
                                po.PurchaseOrderID == PurchaseOrderID
                          select new PurchaseOrderDetailCustom
                          {
                              PurchaseOrderID = po.PurchaseOrderID,
                              PurchaseOrderNumber = po.PurchaseOrderNumber,
                              Status = po.StatusID,
                              DeliveryDate = po.DeliveryDate,
                              VendorCompany = s.CompanyName,
                              VendorName = u.FirstName + " " + u.LastName,
                              VendorID = s.SupplierID,
                              MasterItemID = po.MasterItemID.Value,
                              FileName = po.FileName,
                              OriginalFileName = po.OriginalFileName
                          }).SingleOrDefault();


            return result;
        }

        public List<ProductItemCustom> ProductItemDetails(Int64 PurchaseOrderID)
        {
            var result = (from stprd in db.GetTable<ProductItem>()
                          join l1 in db.GetTable<INC_Lookup>()
                          on stprd.ItemColorID equals l1.iLookupID
                          join l2 in db.GetTable<INC_Lookup>() on stprd.ItemSizeID equals l2.iLookupID into ank
                          from l2 in ank.DefaultIfEmpty()
                          join l3 in db.GetTable<INC_Lookup>()
                          on stprd.ProductStyleID equals l3.iLookupID
                          join l4 in db.GetTable<INC_Lookup>() on stprd.Soldby equals l4.iLookupID into N
                          from l4 in N.DefaultIfEmpty()
                          join l5 in db.GetTable<INC_Lookup>()
                          on stprd.ProductStyleID equals l5.iLookupID
                          join l6 in db.GetTable<INC_Lookup>()
                          on stprd.MasterStyleID equals l6.iLookupID
                          join l7 in db.GetTable<INC_Lookup>()
                          on stprd.ItemNumberStatusID equals l7.iLookupID
                          join po in db.PurchaseOrderMasters on stprd.ProductId equals po.MasterItemID
                          join pd in db.PurchaseOrderDetails on stprd.ProductItemID equals pd.ProductItemID
                          where stprd.IsDeleted == false &&
                                pd.PurchaseOrderID == PurchaseOrderID
                          orderby stprd.ProductItemID ascending
                          select new ProductItemCustom
                          {
                              ProductItemId = Convert.ToInt32(stprd.ProductItemID),
                              ProductId = Convert.ToInt32(stprd.ProductId),
                              ItemImage = stprd.ItemImage,
                              ProductStyle = l3.sLookupName,
                              ItemNumber = stprd.ItemNumber,
                              ItemColor = l1.sLookupName,
                              ItemSize = l2 == null ? "(No Size)" : l2.sLookupName,
                              MasterStyleID = Convert.ToInt32(l6.iLookupID),
                              MasterStyleName = l6.sLookupName,
                              ItemNumberStatusID = Convert.ToInt32(stprd.ItemNumberStatusID),
                              IconPath = l7.sLookupIcon,
                              SizePriority = Convert.ToInt32(stprd.SizePriority),
                              ItemQuantity = pd.Quantity.Value,
                              ItemPrice = pd.Price.Value,
                              PurchaseOrderDetailsID = pd.PurchaseOrderDetailsID
                          }).Distinct().ToList();

            return result;
        }

        public void UpdateOrderStatus(long PurchaseOrderID, long StatusID)
        {
            PurchaseOrderMaster objOrder = GetPurchaseOrderByID(PurchaseOrderID);
            if (StatusID > 0)
                objOrder.StatusID = StatusID;
            else
                objOrder.StatusID = null;
            base.SubmitChanges();
        }

        public List<PurchaseOrderFollowUpCustom> GetFollowUpTimeline(long PurchaseOrderID)
        {
            var result = (from f in db.PurchaseOrderFollowUps
                          join u in db.UserInformations on f.CreatedBy equals u.UserInfoID
                          from ps in db.INC_Lookups.Where(t => t.iLookupID == f.PriorStatus).DefaultIfEmpty()
                          from cs in db.INC_Lookups.Where(t => t.iLookupID == f.CurrentStatus).DefaultIfEmpty()
                          where f.PurchaseOrderID == PurchaseOrderID
                          select new PurchaseOrderFollowUpCustom
                          {
                              LastFollowUpDate = f.CreatedDate.Value,
                              FollowUpBy = u.FirstName + " " + u.LastName,
                              PriorStatus = ps != null ? ps.sLookupName : string.Empty,
                              CurrentStatus = cs != null ? cs.sLookupName : string.Empty,
                              TodayFollowUpDate = f.LastFollowUpDate.Value
                          }).ToList();

            return result;
        }

        public long? GetFollowupPriorStatusBy(long PurchaseOrderID)
        {
            return (from f in db.PurchaseOrderFollowUps
                    where f.PurchaseOrderID == PurchaseOrderID
                    orderby f.PurchaseOrderFollowUpID descending
                    select f.CurrentStatus).FirstOrDefault();
        }

        public String TrailingNotes(Int64 ForeinKey, string NoteFor, string SpecificNoteFor, String NewLineChar)
        {
            String TrailingNotes = String.Empty;

            List<NoteDetail> objList = new List<NoteDetail>();
            objList = db.NoteDetails.Where(n => n.NoteFor == NoteFor && n.SpecificNoteFor == SpecificNoteFor).OrderByDescending(n => n.NoteID).ToList();

            if (ForeinKey != 0)
            {
                objList = objList.Where(n => n.ForeignKey == ForeinKey).ToList();
            }

            if (objList.Count > 0)
            {
                StringBuilder sbTrailingNotes = new StringBuilder();
                foreach (NoteDetail obj in objList)
                {
                    sbTrailingNotes.Append(obj.Notecontents);
                    sbTrailingNotes.Append(NewLineChar + NewLineChar);
                    UserInformationRepository objUserRepo = new UserInformationRepository();
                    UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);
                    if (objUser != null)
                    {
                        sbTrailingNotes.Append(objUser.FirstName + " " + objUser.LastName + "   ");
                    }
                    sbTrailingNotes.Append(Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy"));
                    sbTrailingNotes.Append(" @ " + Convert.ToDateTime(obj.CreateDate).ToString("HH:mm") + NewLineChar);
                    sbTrailingNotes.Append("______________________________________________________________________________");
                    sbTrailingNotes.Append(NewLineChar + NewLineChar);
                }

                TrailingNotes = sbTrailingNotes.ToString();
            }

            return TrailingNotes;
        }
        #endregion
    }

    #region Extension Class
    public class StoreProductCustom
    {
        public long storeproductid { get; set; }
        public string productname { get; set; }
        public string summary { get; set; }
    }

    public class SearchPurchaseOrder
    {
        public long PurchaseOrderID { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string OrderedBy { get; set; }
        public string OrderedOn { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? StartProductionDate { get; set; }       
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string extension { get; set; }
        public Int64 VendorID { get; set; }
        public String Status { get; set; }
        public Int64 RcvQuantity { get; set; }
        public Nullable<DateTime> FollowUpdate { get; set; }
        public Nullable<DateTime> FollowUpCreateddate { get; set; }
        public Nullable<Boolean> Showontomorrow { get; set; }

    }

    public class PurchaseOrderDetailCustom
    {
        public long PurchaseOrderID { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public Nullable<DateTime> DeliveryDate { get; set; }
        public Nullable<long> Status { get; set; }
        public string VendorName { get; set; }
        public string VendorCompany { get; set; }
        public long VendorID { get; set; }
        public long MasterItemID { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
    }

    public class ProductItemCustom
    {
        public Int32 ProductItemId { get; set; }
        public Int32 ProductId { get; set; }
        public String ProductStyle { get; set; }
        public String ItemNumber { get; set; }
        public String ItemColor { get; set; }
        public String ItemSize { get; set; }
        public Int32 MasterStyleID { get; set; }
        public String MasterStyleName { get; set; }
        public Int32 ItemNumberStatusID { get; set; }
        public String IconPath { get; set; }
        public String ItemImage { get; set; }
        public Int32 SizePriority { get; set; }
        public Int32 ItemQuantity { get; set; }
        public Int64 ItemPrice { get; set; }
        public Int64 PurchaseOrderDetailsID { get; set; }
    }

    public class PurchaseOrderFollowUpCustom
    {
        public DateTime LastFollowUpDate { get; set; }
        public string PriorStatus { get; set; }
        public string CurrentStatus { get; set; }
        public string FollowUpBy { get; set; }
        public DateTime TodayFollowUpDate { get; set; }
    }

    public class SearchPurchaseOrderVendor
    {
        public Int64 PurchaseOrderID { get; set; }
        public Int64 ProductItemID { get; set; }
        public String PurchaseOrderNumber { get; set; }
        public String OrderedBy { get; set; }
        public String OrderedOn { get; set; }
        public DateTime? ExpDeliveryDate { get; set; }
        public DateTime? ConFirmedDeliveryDate { get; set; }
        public DateTime? StartProductionDate { get; set; }
        public Int64? StatusID { get; set; }
        public String FileName { get; set; }
        public String OriginalFileName { get; set; }
        public String extension { get; set; }
        public Decimal Price { get; set; }
        public Int32 Quantity { get; set; }
        public String MasterStyle { get; set; }
        public String ProductStyle { get; set; }
        public String ItemNumber { get; set; }
        public String ItemColor { get; set; }
        public String ItemSize { get; set; }
        public Int32? FinalQty { get; set; } 

    }
    #endregion
}

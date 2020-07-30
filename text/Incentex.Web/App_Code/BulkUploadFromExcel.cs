using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using LinqToExcel;
using System.Globalization;
using LinqToExcel.Domain;

/// <summary>
/// This Class for Bulk Product Upload
/// </summary>
public class BulkProductUpload
{

    #region Object of Table and Repository
    LookupRepository objLookup = new LookupRepository();
    SubCatogeryRepository objsubCate = new SubCatogeryRepository();
    SupplierRepository objSupplier = new SupplierRepository();
    StoreProductRepository objStoreProdRepository = new StoreProductRepository();
    #endregion

    #region Proerties StoreProduct
    // For Store product 
    public String ProductCategory { get; set; }
    public String SubCategory { get; set; }
    public String WorkGroup { get; set; }
    public String Department { get; set; }
    public String GarmentType { get; set; }
    public String SupplierName { get; set; }
    public String InventoryStatus { get; set; }
    public String CreditEligible { get; set; }
    public String ShowInventoryLevelsinStore { get; set; }
    public String InventoryNotificationSystem { get; set; }
    public String AllowforBackOrders { get; set; }
    public String TailoringGuidelineStatus { get; set; }
    public String TailoringMeasurementChart { get; set; }
    public String Creditmessage { get; set; }
    public String Color { get; set; }
    public String Size { get; set; }

    public String ProductDescription { get; set; }
    public String Summary { get; set; }

    #endregion
    /// <summary>
    /// This method is used For Bulk Product Upload
    /// </summary>
    /// <param name="filename">Excel file</param>
    /// <param name="StoreID"></param>
    public void SetStoreProduct(String filename, Int64 StoreID)
    {
        var excel = new LinqToExcel.ExcelQueryFactory(filename);
        object objList = (from list in excel.Worksheet<BulkProductUpload>()
                          select list).ToList();
        List<BulkProductUpload> listData = objList as List<BulkProductUpload>;
        try
        {
            foreach (BulkProductUpload value in listData)
            {
                SaveStoreproductRecord(value, StoreID);
            }
            objStoreProdRepository.SubmitChanges();
        }
        catch (Exception ex)
        {

        }
    }
    /// <summary>
    /// Fetch Records in StoreProduct Table
    /// </summary>
    /// <param name="listData"></param>
    /// <param name="StoreID"></param>
    private void SaveStoreproductRecord(BulkProductUpload listData, Int64 StoreID)
    {
        StoreProduct objStoreProd = new StoreProduct();

        Int64 StatusID = 135;// Active because uploaded item will be always active
        // set all id as per their names from excel sheet field.
        Int64 ProductCategoryID = objsubCate.GetByName(listData.SubCategory).CategoryID;
        Int64 SubCategoryID = objsubCate.GetByName(listData.SubCategory).SubCategoryID;
        Int64 WorkGroupID = objLookup.GetIdByLookupNameforBulk(listData.WorkGroup);
        Int64 DepartmentID = objLookup.GetIdByLookupNameforBulk(listData.Department);

        Int64 GarmentTypeID = objLookup.GetIdByLookupNameforBulk(listData.GarmentType);
        Int64? InventoryStatusID = objLookup.GetIdByLookupNameforBulk(listData.InventoryStatus);
        Int64 CreditEligibleID = objLookup.GetIdByLookupNameforBulk(listData.CreditEligible);
        Int64? ShowInventoryLevelsinStoreID = objLookup.GetIdByLookupNameforBulk(listData.ShowInventoryLevelsinStore);
        Int64? InventoryNotificationSystemID = objLookup.GetIdByLookupNameforBulk(listData.InventoryNotificationSystem);
        Int64? AllowforBackOrdersID = objLookup.GetIdByLookupNameforBulk(listData.AllowforBackOrders);
        Int64? TailoringGuidelineStatusID = objLookup.GetIdByLookupNameforBulk(listData.TailoringGuidelineStatus);
        Int64 TailoringMeasurementChartID = objLookup.GetIdByLookupNameforBulk(listData.TailoringMeasurementChart);
        Int64? CreditmessageID = objLookup.GetIdByLookupNameforBulk(listData.Creditmessage);
        Boolean ColorID = true;// here set true because we are passing opposite from general page.
        Boolean SizeID = true;// here set true because we are passing opposite from general page.
        Int64 SupplierNameID = 0;
        var items = (from id in objSupplier.GetSupplierId()
                     where id.FirstName == listData.SupplierName
                     select id).ToList();
        foreach (var s in items)
        {
            SupplierNameID = Convert.ToInt64(s.SupplierID);
        }

        if (listData.Color == "Active")
            ColorID = false;// Don't confuse check general page, here false means it shows in the front section
        if (listData.Size == "Active")
            SizeID = false;// Don't confuse check general page, here false means it shows in the front section

        #region
        // here Enter Data to the table storeProduct table
        DateTime strDAteNewProductUntil = DateTime.Now;
        objStoreProd.StatusID = StatusID;
        objStoreProd.CategoryID = ProductCategoryID;
        objStoreProd.SubCategoryID = SubCategoryID;
        objStoreProd.AnneversaryCreditEligibleID = CreditEligibleID;
        objStoreProd.WorkgroupID = WorkGroupID;
        objStoreProd.DepartmentID = DepartmentID;
        objStoreProd.GarmentTypeID = GarmentTypeID;
        objStoreProd.NewProductUntil = strDAteNewProductUntil;

        objStoreProd.ProductDescrption = listData.ProductDescription;
        objStoreProd.Summary = listData.Summary;

        objStoreProd.InventoryStatus = InventoryStatusID;
        objStoreProd.SupplierId = SupplierNameID;
        objStoreProd.ShowInventoryLevelInStoreID = InventoryNotificationSystemID;
        objStoreProd.AllowBackOrderID = AllowforBackOrdersID;
        objStoreProd.ToArriveOn = DateTime.Now;
        objStoreProd.TailoringOption = TailoringGuidelineStatusID;
        objStoreProd.TailoringMeasurementStatus = TailoringMeasurementChartID;
        objStoreProd.NotificationSystemID = 136;// 
        //objStoreProd.TailoringRunCharge = 
        //objStoreProd.TailoringServicesLeadTime = 
        objStoreProd.CreditMessage = Convert.ToInt32(CreditmessageID);
        objStoreProd.EmployeeTypeid = 0;// for unknown type

        objStoreProd.ColorOff = ColorID;
        objStoreProd.SizeOff = SizeID;
        objStoreProd.MaterialStatus = false;
        objStoreProd.StoreId = StoreID;

        objStoreProdRepository.Insert(objStoreProd);

        #endregion
    }

}
/// <summary>
/// This class for Bulk Product SubItem Upload
/// </summary>
public class BulkProductSubItemUpload
{
    #region Object of Table and Repository
    LookupRepository objLookup = new LookupRepository();
    ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
    ProductItemPriceRepository objPricingRepo = new ProductItemPriceRepository();
    InventoryReorderRepository objInventoryRepos = new InventoryReorderRepository();
    #endregion

    #region Properties
    public Int64 StoreProductID { get; set; }
    public String StyleNo { get; set; }
    public String MasterNumber { get; set; }
    public String ItemNumber { get; set; }
    public String Color { get; set; }
    public String SoldBy { get; set; }
    public String ItemSize { get; set; }
    public Int64 SizePriority { get; set; }
    public String MaterialStyle { get; set; }
    public String ItemDescription { get; set; }
    public Int64? Inventory { get; set; }
    public Int64? ReOrder { get; set; }
    public Int64? OnOrder { get; set; }
    public Int64 ProductCost { get; set; }
    public Int64 CostL1 { get; set; }
    public Int64 CostL2 { get; set; }
    public Int64 CostL3 { get; set; }
    public Int64 CostL4 { get; set; }


    #endregion

    public void SetProductSubitem(String filename)
    {
        var excel = new LinqToExcel.ExcelQueryFactory(filename);
        object objList = (from list in excel.Worksheet<BulkProductSubItemUpload>()
                          select list).ToList();
        List<BulkProductSubItemUpload> listData = objList as List<BulkProductSubItemUpload>;
        try
        {
            foreach (BulkProductSubItemUpload value in listData)
            {
                SaveSubItemProductRecord(value);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SaveSubItemProductRecord(BulkProductSubItemUpload listData)
    {

        Int64 ItemNumberStatusID = 135;// Active because uploaded item will be always active

        Int64 StyleNoID = objLookup.GetIdByLookupNameforBulk(listData.StyleNo);
        Int64 MasterNumberID = objLookup.GetIdByLookupNameforBulk(listData.MasterNumber);

        Int64 ColorID = objLookup.GetIdByLookupNameforBulk(listData.Color);
        Int64 SoldByID = objLookup.GetIdByLookupNameforBulk(listData.SoldBy);
        Int64 ItemSizeID = objLookup.GetIdByLookupNameforBulk(listData.ItemSize);
        Int64? MaterialStyleID = null;
        if (listData.MaterialStyle != null && listData.MaterialStyle.Length > 0)
            MaterialStyleID = objLookup.GetIdByLookupNameforBulk(listData.MaterialStyle);
        else
            MaterialStyleID = null;

        #region Insert Records in ProductItem table

        ProductItem objprodItem = new ProductItem();
        objprodItem.ProductStyleID = StyleNoID;
        objprodItem.MasterStyleID = MasterNumberID;
        objprodItem.ItemColorID = ColorID;
        objprodItem.ItemSizeID = ItemSizeID;
        objprodItem.ItemNumber = listData.ItemNumber;
        objprodItem.SizePriority = listData.SizePriority;
        objprodItem.ItemNumberStatusID = ItemNumberStatusID;
        objprodItem.Soldby = SoldByID;
        objprodItem.MaterialStyleID = MaterialStyleID;
        objprodItem.ProductId = StoreProductID;
        objprodItem.ItemDescription = listData.ItemDescription;
        objRepository.Insert(objprodItem);
        // get ProductItemID
        Int64 maxProductItemID = objprodItem.ProductItemID;


        #endregion
        #region
        ProductItemInventory objInventory = new ProductItemInventory();
        objInventory.ProductItemID = maxProductItemID;
        if (listData.ReOrder != null)
            objInventory.ReOrderPoint = Convert.ToInt32(listData.ReOrder);
        else
            objInventory.ReOrderPoint = 0;
        if (listData.Inventory != null)
            objInventory.Inventory = Convert.ToInt32(listData.Inventory);
        else
            objInventory.Inventory = 0;
        if (listData.OnOrder != null)
            objInventory.OnOrder = Convert.ToInt32(listData.OnOrder);
        else
            objInventory.OnOrder = 0;

        objInventoryRepos.SubmitChanges();
        #endregion
        #region Insert Records ProductItemPricing

        ProductItemPricing objprice = new ProductItemPricing();
        objprice.ProductItemID = maxProductItemID;
        objprice.Level1 = listData.CostL1;
        objprice.Level2 = listData.CostL2;
        objprice.Level3 = listData.CostL3;
        objprice.Level4 = listData.CostL4;
        objprice.ProductCost = listData.ProductCost;
        objPricingRepo.Insert(objprice);
        objPricingRepo.SubmitChanges();

        #endregion
    }

    /// <summary>
    /// To Return LINQ data to DataTable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="varlist"></param>
    /// <returns></returns>
    public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
    {
        DataTable dtReturn = new DataTable();

        // column names 
        PropertyInfo[] propertyInfo = null;

        if (varlist == null) return dtReturn;

        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others will follow 
            if (propertyInfo == null)
            {
                propertyInfo = ((Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in propertyInfo)
                {
                    Type colType = pi.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }

            DataRow dr = dtReturn.NewRow();

            foreach (PropertyInfo pi in propertyInfo)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                (rec, null);
            }

            dtReturn.Rows.Add(dr);
        }
        return dtReturn;
    }
}


/// <summary>
/// This Class for Bulk Product Upload
/// </summary>
public class AssetBulkUpload
{

    #region Object of Table and Repository
    AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
    #endregion

    #region Asset Basic Properties
    // For Store product 
    public String Asset_Type { get; set; }
    public String Plate { get; set; }
    public String Manufacturer { get; set; }
    public String Model { get; set; }
    public String Serial { get; set; }
    public String Location { get; set; }
    public String Asset_ID { get; set; }
    public String Manufacturing_Date { get; set; }
    public String Flag_this_Asset { get; set; }
    #endregion

    /// <summary>
    /// This method is used For Bulk Product Upload
    /// </summary>
    /// <param name="filename">Excel file</param>
    /// <param name="StoreID"></param>
    public string SetAsset(String filename, Int64 CompanyID)
    {
        try
        {
            String message = String.Empty;
            var excel = new LinqToExcel.ExcelQueryFactory(filename);
            object objList = (from list in excel.Worksheet<AssetBulkUpload>()
                              select list).ToList();
            List<AssetBulkUpload> lstAssets = objList as List<AssetBulkUpload>;

            if (lstAssets.Count == 0)
            {
                message = "No records found to upload.";
                return message;
            }

            LookupRepository objMainLookupRepository = new LookupRepository();
            EquipmentMaster objEquipmentMaster = new EquipmentMaster();
            BaseStationRepository objBaseStationRepo = new BaseStationRepository();

            List<INC_BasedStation> lstBaseStations = objBaseStationRepo.GetAllBaseStation().OrderBy(le => le.sBaseStation).ToList();
            List<INC_Lookup> lstEquipmentType = objMainLookupRepository.GetByLookup("EquipmentType");
            List<INC_Lookup> lstBrand = objMainLookupRepository.GetByLookup("Brand");
            List<INC_Lookup> lstEquipmentModel = objMainLookupRepository.GetByLookup("EquipmentModel");


            foreach (AssetBulkUpload objAssetInfo in lstAssets)
            {
                #region


                if (!String.IsNullOrEmpty(objAssetInfo.Asset_Type))
                    objEquipmentMaster.EquipmentTypeID = Convert.ToInt64(lstEquipmentType.FirstOrDefault(le => le.sLookupName == objAssetInfo.Asset_Type).iLookupID);

                if (!String.IsNullOrEmpty(objAssetInfo.Manufacturer))
                    objEquipmentMaster.BrandID = Convert.ToInt64(lstBrand.FirstOrDefault(le => le.sLookupName == objAssetInfo.Manufacturer).iLookupID);

                if (!String.IsNullOrEmpty(objAssetInfo.Serial))
                    objEquipmentMaster.SerialNumber = objAssetInfo.Serial;
                if (!String.IsNullOrEmpty(objAssetInfo.Asset_ID))
                    objEquipmentMaster.EquipmentID = objAssetInfo.Asset_ID;
                if (!String.IsNullOrEmpty(objAssetInfo.Plate))
                    objEquipmentMaster.PlateNumber = objAssetInfo.Plate;
                if (!String.IsNullOrEmpty(objAssetInfo.Model))
                    objEquipmentMaster.EquipmentModel = Convert.ToInt64(lstEquipmentModel.FirstOrDefault(le => le.sLookupName == objAssetInfo.Model).iLookupID);
                if (!String.IsNullOrEmpty(objAssetInfo.Location))
                    objEquipmentMaster.BaseStationID = Convert.ToInt64(lstBaseStations.FirstOrDefault(le => le.sBaseStation == objAssetInfo.Location).iBaseStationId);
                if (!String.IsNullOrEmpty(objAssetInfo.Manufacturing_Date))
                    objEquipmentMaster.ManufacturingDate = Convert.ToDateTime(objAssetInfo.Manufacturing_Date);

                // Flag Asset
                if (!String.IsNullOrEmpty(objAssetInfo.Flag_this_Asset) && objAssetInfo.Flag_this_Asset.ToLower() == "true")
                    objEquipmentMaster.Flagged = true;
                else
                    objEquipmentMaster.Flagged = false;

                objEquipmentMaster.Status = 135; // For Active 
                objEquipmentMaster.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objEquipmentMaster.CreatedDate = DateTime.Now;

                objAssetMgtRepository.Insert(objEquipmentMaster);

                #endregion
            }

            objAssetMgtRepository.SubmitChanges();
            return message;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            return ex.Message.Replace("'", "&quot;").Replace("\r\n", "");
        }
    }
}

public class UpdateShortDesc
{

    #region Object of Table and Repository
    LookupRepository objLookup = new LookupRepository();
    SubCatogeryRepository objsubCate = new SubCatogeryRepository();
    SupplierRepository objSupplier = new SupplierRepository();

    #endregion

    #region Proerties StoreProduct
    // For Store product 
    public String StoreProductID { get; set; }
    public String ShortSummary { get; set; }
    #endregion

    /// <summary>
    /// This method is used For Bulk Product Upload
    /// </summary>
    /// <param name="filename">Excel file</param>
    /// <param name="StoreID"></param>
    public void UpdateStoreProduct(String filename, Int64 StoreID)
    {
        var excel = new LinqToExcel.ExcelQueryFactory(filename);
        object objList = (from list in excel.Worksheet<UpdateShortDesc>()
                          select list).ToList();
        List<UpdateShortDesc> listData = objList as List<UpdateShortDesc>;
        try
        {
            StoreProductRepository objStoreProdRepository = new StoreProductRepository();
            foreach (UpdateShortDesc value in listData)
            {
                StoreProduct objStoreProd = objStoreProdRepository.GetById(Convert.ToInt64(value.StoreProductID));
                if (objStoreProd != null)
                {
                    objStoreProd.Summary = value.ShortSummary;
                }
            }
            objStoreProdRepository.SubmitChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


}

public class BulkUploadUser
{
    #region Fields

    private UserInformationRepository objUserRepo = new UserInformationRepository();
    private CompanyEmployeeRepository objCompEmpRepo = new CompanyEmployeeRepository();

    #endregion

    #region Properties

    public String First_Name { get; set; }
    public String Last_Name { get; set; }
    public String Email { get; set; }
    public String Password { get; set; }
    public String Gender { get; set; }
    public String Employee_ID { get; set; }
    public String Workgroup { get; set; }
    public String Position { get; set; }
    public String Date_of_Hire { get; set; }
    public String Station_Code { get; set; }
    public String Issuance_Policy { get; set; }
    public String System_Access { get; set; }
    public String System_Status { get; set; }

    #endregion

    public String SaveUsers(String filename, Int64 companyID)
    {
        String message = String.Empty;

        try
        {
            ExcelQueryFactory excelFile = new ExcelQueryFactory(filename);

            List<BulkUploadUser> lstExcelUser = excelFile.Worksheet<BulkUploadUser>().ToList();

            if (lstExcelUser.Count == 0)
            {
                message = "No records found to upload.";
                return message;
            }

            if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.First_Name)) != null)
            {
                message = "First_Name is a mandatory field.";
                return message;
            }

            if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.Last_Name)) != null)
            {
                message = "Last_Name is a mandatory field.";
                return message;
            }

            if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.Email)) != null)
            {
                message = "Email is a mandatory field.";
                return message;
            }

            //if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.Password)) != null)
            //{
            //    message = "Password is a mandatory field.";
            //    return message;
            //}

            //if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.Gender)) != null)
            //{
            //    message = "Gender is a mandatory field.";
            //    return message;
            //}

            //if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.Employee_ID)) != null)
            //{
            //    message = "Employee_ID is a mandatory field.";
            //    return message;
            //}

            //if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.Workgroup)) != null)
            //{
            //    message = "Workgroup is a mandatory field.";
            //    return message;
            //}

            //if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.Position)) != null)
            //{
            //    message = "Position is a mandatory field.";
            //    return message;
            //}

            //if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.Date_of_Hire)) != null)
            //{
            //    message = "Date_of_Hire is a mandatory field.";
            //    return message;
            //}

            //if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.Station_Code)) != null)
            //{
            //    message = "Station_Code is a mandatory field.";
            //    return message;
            //}

            //if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.Issuance_Policy)) != null)
            //{
            //    message = "Issuance_Policy is a mandatory field.";
            //    return message;
            //}

            //if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.System_Access)) != null)
            //{
            //    message = "System_Access is a mandatory field.";
            //    return message;
            //}

            //if (lstExcelUser.FirstOrDefault(le => String.IsNullOrEmpty(le.System_Status)) != null)
            //{
            //    message = "System_Status is a mandatory field.";
            //    return message;
            //}

            LookupRepository objLookupRepo = new LookupRepository();
            List<INC_Lookup> lstGender = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Gender).OrderBy(le => le.sLookupName).ToList();
            List<INC_Lookup> lstWorkGroups = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Workgroup).OrderBy(le => le.sLookupName).ToList();
            List<INC_Lookup> lstEmployeeTitles = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.EmployeeTitle).OrderBy(le => le.sLookupName).ToList();
            List<INC_Lookup> lstStatuses = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Status).OrderBy(le => le.sLookupName).ToList();

            Dictionary<Int32, String> dctEmployeeTypes = Common.GetEnumForBind(typeof(Incentex.DA.DAEnums.CompanyEmployeeTypes));

            BaseStationRepository objBaseStationRepo = new BaseStationRepository();
            List<INC_BasedStation> lstBaseStations = objBaseStationRepo.GetAllBaseStation().OrderBy(le => le.sBaseStation).ToList();

            List<UserInformation> lstUsers = new List<UserInformation>();
            List<CompanyEmployee> lstCompEmployees = new List<CompanyEmployee>();

            foreach (BulkUploadUser objExcelUser in lstExcelUser)
            {
                UserInformation objUserInfo = new UserInformation();
                CompanyEmployee objCompanyEmployee = new CompanyEmployee();

                if (!objUserRepo.CheckEmailExistence(objExcelUser.Email.Trim(), 0))
                {
                    message = "Email " + objExcelUser.Email + " alerady exists in the system.";
                    return message;
                }

                if (!String.IsNullOrEmpty(objExcelUser.Employee_ID))
                {
                    if (!objCompEmpRepo.CheckEmployeeIDExistence(objExcelUser.Employee_ID.Trim(), companyID, 0))
                    {
                        message = "Employee id " + objExcelUser.Employee_ID + " already exists in the system.";
                        return message;
                    }
                    else if (!new RegistrationRepository().CheckEmployeeIDExistence(objExcelUser.Employee_ID.Trim(), companyID, 0))
                    {
                        message = "Employee id " + objExcelUser.Employee_ID + " already exists in registration requests";
                        return message;
                    }
                }

                objUserInfo.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objExcelUser.First_Name.Trim().ToLower());
                objUserInfo.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objExcelUser.Last_Name.Trim().ToLower());

                if (!String.IsNullOrEmpty(objExcelUser.System_Status))
                    objUserInfo.WLSStatusId = Convert.ToInt32(lstStatuses.FirstOrDefault(le => le.sLookupName.Trim() == objExcelUser.System_Status.Trim()).iLookupID);
                else
                    objUserInfo.WLSStatusId = Convert.ToInt32(lstStatuses.FirstOrDefault(le => le.sLookupName == "InActive").iLookupID);

                objUserInfo.Email = objExcelUser.Email.Trim();
                objUserInfo.LoginEmail = objExcelUser.Email.Trim();
                objUserInfo.Password = !String.IsNullOrEmpty(objExcelUser.Password) ? objExcelUser.Password.Trim() : "abc123";
                objUserInfo.CreatedDate = DateTime.Now;
                objUserInfo.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objUserInfo.CompanyId = companyID;

                if (!String.IsNullOrEmpty(objExcelUser.System_Access))
                    objUserInfo.Usertype = Convert.ToInt64(dctEmployeeTypes.FirstOrDefault(le => le.Value.Trim() == objExcelUser.System_Access.Trim()).Key);
                else
                    objUserInfo.Usertype = Convert.ToInt64(dctEmployeeTypes.FirstOrDefault(le => le.Value.Trim() == Convert.ToString(Incentex.DA.DAEnums.CompanyEmployeeTypes.Employee)).Key);

                objUserInfo.ClimateSettingId = null;

                objUserRepo.Insert(objUserInfo);
                lstUsers.Add(objUserInfo);

                objCompanyEmployee.UserInfoID = objUserInfo.UserInfoID;
                objCompanyEmployee.CompanyEmail = objExcelUser.Email.Trim();

                if (!String.IsNullOrEmpty(objExcelUser.System_Access) && objExcelUser.System_Access.Trim() == Incentex.DA.DAEnums.CompanyEmployeeTypes.Admin.ToString())
                {
                    objCompanyEmployee.isCompanyAdmin = true;
                    objCompanyEmployee.ManagementControlForWorkgroup = Convert.ToInt64(lstWorkGroups.FirstOrDefault(le => le.sLookupName.Trim() == objExcelUser.Workgroup.Trim()).iLookupID);
                    objCompanyEmployee.ManagementControlForRegion = null;
                    objCompanyEmployee.ManagementControlForStaionlocation = Convert.ToInt64(lstBaseStations.FirstOrDefault(le => le.sBaseStation.Trim() == objExcelUser.Station_Code.Trim()).iBaseStationId);
                }
                else
                {
                    objCompanyEmployee.IsMOASApprover = false;
                    objCompanyEmployee.ManagementControlForWorkgroup = null;
                    objCompanyEmployee.ManagementControlForDepartment = null;
                    objCompanyEmployee.ManagementControlForRegion = null;
                    objCompanyEmployee.ManagementControlForStaionlocation = null;
                }

                objCompanyEmployee.IsMOASApprover = false;

                if (!String.IsNullOrEmpty(objExcelUser.Date_of_Hire))
                    objCompanyEmployee.HirerdDate = Convert.ToDateTime(objExcelUser.Date_of_Hire.Trim());

                if (!String.IsNullOrEmpty(objExcelUser.Employee_ID))
                    objCompanyEmployee.EmployeeID = objExcelUser.Employee_ID.Trim();

                if (!String.IsNullOrEmpty(objExcelUser.Position))
                    objCompanyEmployee.EmployeeTitleId = Convert.ToInt64(lstEmployeeTitles.FirstOrDefault(le => le.sLookupName.Trim() == objExcelUser.Position.Trim()).iLookupID);

                if (!String.IsNullOrEmpty(objExcelUser.Issuance_Policy))
                    objCompanyEmployee.EmpIssuancePolicyStatus = Convert.ToInt32(lstStatuses.FirstOrDefault(le => le.sLookupName.Trim() == objExcelUser.Issuance_Policy.Trim()).iLookupID);

                if (!String.IsNullOrEmpty(objExcelUser.Station_Code))
                    objCompanyEmployee.BaseStation = Convert.ToInt64(lstBaseStations.FirstOrDefault(le => le.sBaseStation.Trim() == objExcelUser.Station_Code.Trim()).iBaseStationId);

                if (!String.IsNullOrEmpty(objExcelUser.Gender))
                    objCompanyEmployee.GenderID = Convert.ToInt64(lstGender.FirstOrDefault(le => le.sLookupName.Trim() == objExcelUser.Gender.Trim()).iLookupID);

                if (!String.IsNullOrEmpty(objExcelUser.Workgroup))
                    objCompanyEmployee.WorkgroupID = Convert.ToInt64(lstWorkGroups.FirstOrDefault(le => le.sLookupName.Trim() == objExcelUser.Workgroup.Trim()).iLookupID);

                objCompanyEmployee.StoreActivatedBy = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;
                objCompanyEmployee.StoreActivatedDate = DateTime.Now;
                objCompanyEmployee.DateRequestSubmitted = DateTime.Now.ToString("MM/dd/yyyy");

                if (!String.IsNullOrEmpty(objExcelUser.System_Access) && objExcelUser.System_Access.Trim() == "Active")
                    objCompanyEmployee.LastActiveDate = DateTime.Now;
                else
                    objCompanyEmployee.LastInActiveDate = DateTime.Now;

                objCompanyEmployee.RegionID = null;
                objCompanyEmployee.DepartmentID = null;
                objCompanyEmployee.UploadImage = null;
                objCompanyEmployee.IsMOASStationLevelApprover = null;

                objCompEmpRepo.Insert(objCompanyEmployee);
                lstCompEmployees.Add(objCompanyEmployee);
            }

            objUserRepo.SubmitChanges();

            for (Int32 i = 0; i < lstUsers.Count; i++)
            {
                lstCompEmployees[i].UserInfoID = lstUsers[i].UserInfoID;
            }

            objCompEmpRepo.SubmitChanges();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            message = "Error in uploading employees : " + ex.Message;
        }

        return message;
    }
}
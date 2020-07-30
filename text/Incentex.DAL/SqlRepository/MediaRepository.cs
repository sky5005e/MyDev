using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class MediaRepository : RepositoryBase
    {
        public List<MediaFolder> GetAllFolder()
        {
            return db.MediaFolders.Where(x => x.FolderType != "System Folder").OrderBy(x => x.FolderName).ToList();
        }

        public List<MediaFolder> GetSystemFolder()
        {
            return db.MediaFolders.Where(x => x.FolderType != "Folder").OrderBy(x => x.FolderName).ToList();
        }

        public List<MediaDocLinkParent> GetAllDocMainSection()
        {
            return db.MediaDocLinkParents.OrderBy(x=>x.ModuleName).ToList();
        }

        public List<MediadocLinkSub> GetAllDocMainSubSection(Int64 mainid)
        {
            return db.MediadocLinkSubs.Where(x => x.MediaDocumentLinkId == mainid).OrderBy(x => x.ModuleName).ToList();
        }


        public List<MediaHelpVidPrnt> GetAllHelpMainSection()
        {
            return db.MediaHelpVidPrnts.OrderBy(x => x.ModuleName).ToList();
            //test
        }


        public List<MediaLoginpopup> GetLoginpopuplist()
        {
            return db.MediaLoginpopups.OrderBy(x => x.ModuleName).ToList();
        }

        public List< MediaHelpVidSub> GetAllHelpMainSubSection(Int64 mainid)
        {
            return db.MediaHelpVidSubs.Where(x => x.MediaHelpVidPrntID == mainid).OrderBy(x => x.ModuleName).ToList();
        }

        public MediaHelpVidPrnt GetModuleNameForVideoById(Int64 id)
        {
            var result = db.MediaHelpVidPrnts.Where(x => x.MediaHelpVidPrntId == id).SingleOrDefault();
            return result;
        }
        public MediaDocLinkParent GetModuleNameForDocById(Int64 id)
        {
            var result = db.MediaDocLinkParents.Where(x => x.MediaDocLinkParentId == id).SingleOrDefault();
            return result;
        }




        public List<InsertAndGetAllMediaDropDownResult> InsertAndGetAllMediaDropDown(string LookUpCode, string LookUpName, string Flag, long userid, long PARENTID)
        {
            //InsertAndGetAllMediaDropDownResult lstresult = db.InsertAndGetAllMediaDropDown(LookUpCode, LookUpName, Flag, userid, PARENTID).SingleOrDefault();
            //if (lstresult.InsertTransactionStatus == "InsertTransactionStatus")
            //    return null;

            return db.InsertAndGetAllMediaDropDown(LookUpCode, LookUpName, Flag, userid,PARENTID).ToList();
        }

        public bool IsMediaFileExist(string Name)
        {
            var result = db.MediaFiles.Where(x => x.Name.ToLower() == Name.ToLower()).SingleOrDefault();
            if (result == null)
                return false;
            else
                return true;
        }

        public MediaFolder GetFolderNameByName(string MediaFolderName)
        {
            var result = db.MediaFolders.Where(x=>x.FolderName==MediaFolderName).SingleOrDefault();
            return result;
        }
        public List<GetAllFileListResult> GetAllFileList(long folderid,int pagesize,int pageindex)
        {
            
            if (folderid == 0)
            {
                var result = db.GetAllFileList(null,pagesize,pageindex).OrderBy(x => x.name).ToList();
                return result;
            }
            else
            {
                var result = db.GetAllFileList(folderid,pagesize,pageindex).OrderBy(x => x.name).ToList();
                return result;
            }
        }

        public List<MediaFile> GetFilesByfolderid(Int64 folderid)
        {
            var item = db.MediaFiles.Where(x => x.MediaFolderId == folderid).ToList();
            return item;
        }

        public MediaFolder GetFolderByID(Int64 id)
        {
            var result = db.MediaFolders.Where(x => x.MediaFolderId == id).SingleOrDefault();
            return result;
        }

        public MediaFile GetFileById(Int64 id)
        {
            var result = db.MediaFiles.Where(x => x.MediaFileId== id).SingleOrDefault();
            return result;
        }
        public List<MediaPlaceMent> GetPlacement()
        {
            var result = db.MediaPlaceMents.ToList();
            return result;
        }
        public string GetPageUrl(Int64 id,string type)
        {
            string result = "";
            if (type == "mediadocsection")
            {
                var res = db.MediadocLinkSubs.Where(x => x.MediadocLinkSubId == id).SingleOrDefault();
                result = res == null ? "" : res.ThumbnailURL==null?"":res.ThumbnailURL;
            }
            else if (type == "mediahelpvidsection")
            {
                var res = db.MediadocLinkSubs.Where(x => x.MediadocLinkSubId == id).SingleOrDefault();
                result = res == null ? "" : res.ThumbnailURL == null ? "" : res.ThumbnailURL;
            }
            return result;
        }
        public MediadocLinkSub GetLinkSubById(Int64 id)
        {
            var result = db.MediadocLinkSubs.Where(x => x.MediadocLinkSubId == id).SingleOrDefault();
            return  result;
        }

        public MediaHelpVidSub GetHelpSubById(Int64 id)
        {
            var result = db.MediaHelpVidSubs.Where(x => x.MediaHelpVidSubId== id).SingleOrDefault();
            return result;
        }

        public MediaLoginpopup GetLoginPopupbyid(Int64 id)
        {
            var result = db.MediaLoginpopups.Where(x => x.MediaLoginpopupid == id).SingleOrDefault();
            return result;
        }

        public MediaAssigned Getmediaassignedbyid(Int64 id) // get by mediafileid
        {
            var result = db.MediaAssigneds.Where(x => x.MediaFileId== id).SingleOrDefault();
            return result;
        }

        public MediaAssigned GetAssigedMediaById(Int64 id)
        {
            var result = db.MediaAssigneds.Where(x => x.MediaAssignedId== id).SingleOrDefault();
            return result;
        }

        public  string GetMediaCompWorkBymediaid(Int64 mediaid,string type)
        {
            var result = db.MediaCompWorks.Where(x => x.MediaId == mediaid && x.Type == type).ToList();
            string strreult="";
            foreach (var item in result)
            {
                strreult = strreult + item.CompWorkId.ToString() + ";";
            }
            return strreult;
        }


        public string GetMediaModules(Int64 mediaid, string type)
        {
            var result = db.MediaModuleAllocations.Where(x => x.MediaFileId== mediaid && x.ModuleType== type).ToList();
            string strreult = "";
            foreach (var item in result)
            {
                strreult = strreult + item.Moduleid.ToString() + ";";
            }
            return strreult;
        }
        public string GetMediaPlaces(Int64 mediaid, string type)
        {
            var result = db.MediaPlaceAllocations.Where(x => x.MediaFileId == mediaid && x.PlaceType == type).ToList();
            string strreult = "";
            foreach (var item in result)
            {
                strreult = strreult + item.PlaceId.ToString() + ";";
            }
            return strreult;
        }



        public void DeleteCompWork(Int64 mediaid)
        {
           var result=db.MediaCompWorks.Where(x => x.MediaId == mediaid ).ToList();
           if (result != null)
           {
               db.MediaCompWorks.DeleteAllOnSubmit(result);
               db.SubmitChanges();
           }
        }
        public void DeleteModuleAndPlaces(Int64 mediaid)
        {
            var resultformodule = db.MediaModuleAllocations.Where(x => x.MediaFileId == mediaid).ToList();
            if (resultformodule != null)
            {
                db.MediaModuleAllocations.DeleteAllOnSubmit(resultformodule);
                db.SubmitChanges();
            }
            
            var resultforPlace = db.MediaPlaceAllocations.Where(x => x.MediaFileId == mediaid).ToList();
            if (resultforPlace != null)
            {
                db.MediaPlaceAllocations.DeleteAllOnSubmit(resultforPlace);
                db.SubmitChanges();
            }
        }


        public string GetFileName(string filename)
        {
            MediaFile objmediafile = db.MediaFiles.OrderByDescending(x=>x.MediaFileId).FirstOrDefault();
            if (objmediafile == null)
                return "1_" + filename;
            else
                return (objmediafile.MediaFileId+1).ToString() +"_" + filename;
        }

        public List<GetMediaAssignedDetailResult> GetMediaAssignedDetail(Int64 id,string filetype)
        {
            var result = db.GetMediaAssignedDetail(id, filetype).ToList();
            return result;
        }

        public List<GetMediaAssignedSearchResult> GetMediaAssignedSearch(Int64? CompanyId, Int64? Workgroupid, String Keywords, Int64? Status, Int32? Pagesize, Int32? Pagezeindex, String Sortcol, String Sortdirctn)
        {
            var result = db.GetMediaAssignedSearch(CompanyId, Workgroupid, Keywords, Status, Pagesize, Pagezeindex, Sortcol, Sortdirctn).ToList();
            return result;
        }   


        public List<GetStoreProductDetailByMasterItemResult> GetStoreProductDetailByMasterItem(string MasterItem,Int64 ? CategoryId)
        {
            var result = db.GetStoreProductDetailByMasterItem(MasterItem, CategoryId).ToList();
            return result;

        }

        public GetMediaHelpVideoOrDocResult GetMediaHelpVideoOrDoc(string Placement,string Modulename,string PageTitle,long? Userid,long? Companyid)
        {
            Int64 Activeid;
            LookupRepository objLookupRepo = new LookupRepository();
            List<INC_Lookup> lstStatuses = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Status).OrderBy(le => le.sLookupName).ToList();
            Activeid = lstStatuses.FirstOrDefault(le => le.sLookupName == "Active").iLookupID;
            var result = db.GetMediaHelpVideoOrDoc(Placement, Modulename, PageTitle, Userid, Companyid, Activeid).SingleOrDefault();
            //if (result != null)
            //{
            //    MediaFile objfile = GetFileById(result.mediafileid);
            //    if (objfile != null)
            //    {
            //        objfile.View=objfile.View == null ? 1 : objfile.View + 1;
            //        SubmitChanges();
            //    }
            //}
            return result;
        }



        public GetMediaHelpVideoOrDocResult GetMediaHelpVideoOrDocResultWhenLoginFirst(string Placement, string Modulename, string PageTitle, long? Userid, long? Companyid)
        {
            Int64 Activeid;
            LookupRepository objLookupRepo = new LookupRepository();
            List<INC_Lookup> lstStatuses = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Status).OrderBy(le => le.sLookupName).ToList();
            Activeid = lstStatuses.FirstOrDefault(le => le.sLookupName == "Active").iLookupID;
            var result = db.GetMediaHelpVideoOrDoc(Placement, Modulename, PageTitle, Userid, Companyid, Activeid).SingleOrDefault();
            return result;
        }


        public List<StoreProductImage> GetStoreProductImageByStoreProductId(Int32 StoreProductId)
        {
            return db.StoreProductImages.Where(s => s.StoreProductID == StoreProductId && s.IsDeleted == false).ToList();
        }

        public StoreProductImage GetStoreProductImageById(Int64 Id)
        {
            return db.StoreProductImages.Where(s => s.StoreProductImageId == Id).SingleOrDefault();
        }


        public string  getproductdetailbystoreproductid(Int64 id)
        {
            string res="";
            var result = from sp in db.StoreProducts
                         join st in db.CompanyStores on sp.StoreId equals st.StoreID
                         join c in db.Companies on st.CompanyID equals c.CompanyID
                         where sp.StoreProductID == id
                         select new { sp.AdditionalWorkgroupId, sp.WorkgroupID, c.CompanyID };
                         
            foreach (var item in result)
            {
                string Additionalworkgroup = "";

                if (!string.IsNullOrEmpty(item.AdditionalWorkgroupId))
                {
                    Additionalworkgroup = item.WorkgroupID + "," + item.AdditionalWorkgroupId;
                }
                else
                {
                    Additionalworkgroup = item.WorkgroupID.ToString();
                }
             
                res = item.CompanyID + "_" + Additionalworkgroup;
            }
            
            return res;
        }



        

        public List<GetMediaLocationModulesResult> GetMediaLocationModules(string Placement, string Modulename, Int32? PageSize, Int32? PageIndex, string SortColumn, string SortDirection)
        {
            var result = db.GetMediaLocationModules(Placement, Modulename, PageSize, PageIndex, SortColumn, SortDirection).ToList();
            return result;
        }

        public List<GetMediaLocationPagesResult> GetMediaLocationPages(Int64 ModuleId, String Modulename, String Placement, Int32? PageSize, Int32? PageIndex, string SortColumn, string SortDirection)
        {
            var result = db.GetMediaLocationPages(ModuleId, Modulename, Placement, PageSize, PageIndex, SortColumn, SortDirection).ToList();
            return result;
        }

        public bool IsModuleAlreadyExist(Int64 id,string Modulename, string type)
        {
            bool res = false;
            if (type == "Help Video")
            {
                var result = db.MediaHelpVidPrnts.Where(x => x.ModuleName.ToLower() == Modulename.ToLower() & x.MediaHelpVidPrntId != id).FirstOrDefault();
                if (result != null)
                    return true;
                else
                    return false;
            }

            if (type == "Document Link")
            {
                var result = db.MediaDocLinkParents.Where(x => x.ModuleName.ToLower() == Modulename.ToLower() & x.MediaDocLinkParentId != id).FirstOrDefault(); 
                if (result != null)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }



        public bool IsPlaceAlredyEdisted(Int64 id,Int64 moduleid, string Modulename, string type)
        {
            bool res = false;
            if (type == "Help Video")
            {
                var result = db.MediaHelpVidSubs.Where(x => x.ModuleName.ToLower() == Modulename.ToLower() & x.MediaHelpVidSubId != id & x.MediaHelpVidPrntID==moduleid).FirstOrDefault();
                if (result != null)
                    return true;
                else
                    return false;
            }

            if (type == "Document Link")
            {
                var result = db.MediadocLinkSubs.Where(x => x.ModuleName.ToLower() == Modulename.ToLower() & x.MediadocLinkSubId != id & x.MediaDocumentLinkId==moduleid).FirstOrDefault(); 
                if (result != null)
                    return true;
                else
                    return false;
            }

            if (type == "Login Pop-up")
            {
                var result = db.MediaLoginpopups.Where(x => x.ModuleName.ToLower() == Modulename.ToLower() & x.MediaLoginpopupid != id ).FirstOrDefault();
                if (result != null)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }


        public bool IsVideoViewedWhenLoginFirst(Int64 userid,Int64 mediaid)
        {
            var result = db.MediaVieweds.Where(x => x.UserId == userid && x.MediaId==mediaid).ToList();
            if (result.Count > 0)
                return true;
            else
                return false;
        }


        public List<GetMediaSearchKeywordsResult> MediaSearch()
        {
            var result = db.GetMediaSearchKeywords().ToList();
            return result;

        }

        public List<GetMediaPlacesResult> GetMediaPlaces(string type,string moduleids)
        {
            var result = db.GetMediaPlaces(type, moduleids).ToList();
            return result;
        }


        public bool IsFileOrFolderDuplicate(string Name,string type,Int64? id)
        {
            if (id == null)
                id = 0;

            bool res=false;

            if (type == "folder" || type == "system folder")
            {
                var reslut = db.MediaFolders.Where(x => x.FolderName.ToLower() == Name.ToLower() && x.MediaFolderId != id).ToList();
                if (reslut.Count >0)
                    res = true;
            }
            if (type == "file" )
            {
                var reslut = db.MediaFiles.Where(x => x.Name.ToLower() == Name.ToLower() && x.MediaFileId!= id).ToList();
                if (reslut.Count > 0)
                    res = true;
            }

            return res;

            
        }

        
        //public int AssignImageToStore()
        //{
        //}
    }
            

}

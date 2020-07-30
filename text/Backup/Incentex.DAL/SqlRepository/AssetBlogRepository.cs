using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{


    public class AssetBlogRepository : RepositoryBase
    {

        IQueryable<EquipmentBlogTitle> GetAllBlogTitle()
        {
            IQueryable<EquipmentBlogTitle> qry = from c in db.EquipmentBlogTitles
                                                 orderby c.BlogTitleID
                                                 select c;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public List<EquipmentBlogTitle> GetTitle(Int64 companyID)
        {
            List<EquipmentBlogTitle> objResult = new List<EquipmentBlogTitle>();

            //var objlist = GetAllBlogTitle().Where(x=>x.IsDeleted==false).OrderByDescending(y=>y.CreatedDate).ToList();
            var objlist = (from c in db.EquipmentBlogTitles
                           where c.IsDeleted == false
                           orderby c.BlogTitleID
                           select c).OrderByDescending(y => y.CreatedDate).ToList();


            foreach (var item in objlist)
            {
                if (item.IsInternal)
                {
                    if (item.CompanyID == companyID)
                    {
                        objResult.AddRange(objlist.Where(x => x.BlogTitleID == item.BlogTitleID).ToList());
                    }
                }
                else
                {
                    objResult.AddRange(objlist.Where(x => x.BlogTitleID == item.BlogTitleID).ToList());
                }

            }

            return objResult;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public List<EquipmentBlogTitle> GetTitleByString(Int64 companyID, String searchString)
        {
            List<EquipmentBlogTitle> objResult = new List<EquipmentBlogTitle>();
            //var objlist = GetAllBlogTitle().Where(x => x.IsDeleted == false).OrderByDescending(y => y.CreatedDate).ToList();

            var objlist = (from c in db.EquipmentBlogTitles
                           where c.IsDeleted == false
                           orderby c.BlogTitleID
                           select c).OrderByDescending(y => y.CreatedDate).ToList();

            foreach (var item in objlist)
            {
                if (item.IsInternal)
                {
                    if (item.CompanyID == companyID)
                    {
                        objResult.AddRange(objlist.Where(x => x.BlogTitleID == item.BlogTitleID && x.BlogTitleName.Contains(searchString)).ToList());
                    }
                }
                else
                {
                    objResult.AddRange(objlist.Where(x => x.BlogTitleID == item.BlogTitleID && x.BlogTitleName.Contains(searchString)).ToList());
                }

            }

            return objResult;
        }
        public EquipmentBlogTitle GetByTitleID(Int64 blogTitleID)
        {
            EquipmentBlogTitle objEquipmentBlog = db.EquipmentBlogTitles.Where(x => x.IsDeleted == false && x.BlogTitleID == blogTitleID).FirstOrDefault();
            return objEquipmentBlog;
        }
        public bool DeleteBlog(Int64 blogTitleID, Int64 deletedBy)
        {

            var objTitle = GetByTitleID(blogTitleID);
            bool Deleted = false;
            List<EquipmentBlogComment> lstComment = db.EquipmentBlogComments.Where(x => x.IsDeleted == false && x.BlogTitleID == blogTitleID).ToList();
            if (objTitle != null)
            {
                if (objTitle.CreatedBy == deletedBy)
                {
                    lstComment.ToList().ForEach(c => c.IsDeleted = true);
                    objTitle.IsDeleted = true;
                    objTitle.DeletedBy = deletedBy;
                    db.SubmitChanges();
                    Deleted = true;
                }
                else
                {
                    Deleted = false;
                }

            }
            return Deleted;

        }

        public List<EquipmentBlogComment> GetCommentsByTitleID(Int64 blogTitleID)
        {
            var objlist = from c in db.EquipmentBlogComments.Where(x => x.BlogTitleID == blogTitleID && x.IsDeleted == false).ToList()
                          select c;
            return objlist.ToList();
        }

        public bool DeleteComment(Int64 blogCommentID, Int64 deletedBy)
        {

            EquipmentBlogComment objComment = db.EquipmentBlogComments.Where(x => x.BlogCommentID == blogCommentID).FirstOrDefault();
            bool Deleted = false;

            if (objComment != null)
            {
                if (objComment.CreatedBy == deletedBy)
                {
                    objComment.IsDeleted = true;
                    objComment.DeletedBy = deletedBy;
                    db.SubmitChanges();
                    Deleted = true;
                }
                else
                {
                    Deleted = false;
                }
            }
            return Deleted;
        }
    }

}

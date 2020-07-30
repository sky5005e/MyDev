using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace Incentex.DAL.SqlRepository
{
    public class VideoTrainingRepository : RepositoryBase
    {
        public List<VideoTraining> GetAll()
        {
            return (db.VideoTrainings.Select(v=>v).ToList<VideoTraining>());
        }
        
        public VideoTraining GetById(Int64 VideoTrainingID)
        {
            return db.VideoTrainings.FirstOrDefault(v => v.VideoTrainingID == VideoTrainingID);
        }

        //Delete By Id
        public void DeleteById(Int64 VideoTrainingID)
        {
            var matchedVideo = db.VideoTrainings.SingleOrDefault(v => v.VideoTrainingID == VideoTrainingID);
            try
            {
                if (matchedVideo != null)
                {
                    db.VideoTrainings.DeleteOnSubmit(matchedVideo);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<SelectAllVideoTrainingResult> GetAllWithDetail()
        {
            List<SelectAllVideoTrainingResult> qry = db.SelectAllVideoTraining().ToList();
            return qry;
        }

        public VideoTraining GetVideoByStoreIDWorkgroupIDUserTypeAndUrl(Int64 CompanyID,Int64 UserInfoID,Int64 UserType,string Url)
        {
            IEnumerable<VideoTraining> qry = (from v in db.VideoTrainings
                                 join placement in db.INC_Lookups on v.PlacementID equals placement.iLookupID
                                 where SqlMethods.Like(Url.ToLower(),"%" + placement.sLookupIcon.Replace("~/", "").ToLower() + "%")
                                 && v.StartDate.Date<=DateTime.Now.Date && v.ExpiredDate.Date>=DateTime.Now.Date
                                 select v);
            //check StoreID
            qry = qry.Where(v=>v.StoreID==null || v.StoreID==db.CompanyStores.FirstOrDefault(c=>c.CompanyID==CompanyID).StoreID);

            //check WorkgroupID
            qry = qry.Where(v => v.WorkgroupID == null || v.WorkgroupID == db.CompanyEmployees.FirstOrDefault(c => c.UserInfoID == UserInfoID).WorkgroupID);

            //check UserType
            qry = qry.Where(v => v.UserTypeID == null || v.UserTypeID == UserType);

            //return latest uploaded video
            return qry.OrderByDescending(v=>v.CreatedDate).FirstOrDefault();
        }

        public List<VideoTraining> GetAllVideoByStoreIDWorkgroupIDUserTypeAndUrl(Int64 CompanyID, Int64 UserInfoID, Int64 UserType, string Url)
        {
            IEnumerable<VideoTraining> qry = (from v in db.VideoTrainings
                                              join placement in db.INC_Lookups on v.PlacementID equals placement.iLookupID
                                              where SqlMethods.Like(Url.ToLower(), "%" + placement.sLookupIcon.Replace("~/", "").ToLower() + "%")
                                              && v.StartDate.Date <= DateTime.Now.Date && v.ExpiredDate.Date >= DateTime.Now.Date
                                              select v);
            //check StoreID
            qry = qry.Where(v => v.StoreID == null || v.StoreID == db.CompanyStores.FirstOrDefault(c => c.CompanyID == CompanyID).StoreID);

            //check WorkgroupID
            qry = qry.Where(v => v.WorkgroupID == null || v.WorkgroupID == db.CompanyEmployees.FirstOrDefault(c => c.UserInfoID == UserInfoID).WorkgroupID);

            //check UserType
            qry = qry.Where(v => v.UserTypeID == null || v.UserTypeID == UserType);

            return qry.OrderByDescending(v => v.CreatedDate).ToList<VideoTraining>();
        }

        public List<HelpVideoDetails> GetAllHelpVideo(Int64 StoreID)
        {
            var qry = (from hv in db.HelpVideos
                       join lk in db.INC_Lookups on hv.VideoForID equals lk.iLookupID
                       join cs in db.CompanyStores on hv.CompanyID equals cs.CompanyID
                       where cs.StoreID == StoreID
                       select new HelpVideoDetails
                       {
                           CompanyID = cs.CompanyID,
                           CreatedDate = hv.CreateDate,
                           HelpVideoID = hv.HelpVideoID,
                           VideoForID = hv.VideoForID,
                           VideoFor = lk.sLookupName,
                           VideoName = hv.VideoName,
                           VideoPath = hv.VideoPath
                       }).ToList();

            return qry.ToList<HelpVideoDetails>();
        }

        public HelpVideo GetHelpVideoByName(Int64 videoFor)
        {
            return db.HelpVideos.Where(q => q.VideoForID == videoFor).FirstOrDefault();

        }
        public HelpVideo GetHelpVideoByID(Int64 videoID)
        {
            return db.HelpVideos.Where(q => q.HelpVideoID == videoID).FirstOrDefault();
        }

        public void DeleteHelpVideo(Int64 videoID)
        {
            var list = db.HelpVideos.Where(q => q.HelpVideoID == videoID).FirstOrDefault();

            try
            {
                if (list != null)
                {
                    db.HelpVideos.DeleteOnSubmit(list);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class HelpVideoDetails
        {
            public Int64 HelpVideoID { get; set; }
            public Int64 CompanyID { get; set; }
            public Int64? VideoForID { get; set; }
            public String VideoFor { get; set; }
            public String VideoName { get; set; }
            public String VideoPath { get; set; }
            public DateTime? CreatedDate { get; set; }
        }
    }
}

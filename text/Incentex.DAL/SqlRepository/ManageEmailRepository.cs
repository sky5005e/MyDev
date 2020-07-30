using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class ManageEmailRepository : RepositoryBase
    {

        //IQueryable<INC_ManageEmail> GetAllQuery()
        //{
        //    IQueryable<INC_ManageEmail> qry = from e in db.INC_ManageEmails
        //                                      select e;
        //    return qry;
        //}

        //IQueryable<CompanyEmpManageEmail> GetAllCompanyEmpQuery()
        //{
        //    IQueryable<CompanyEmpManageEmail> qry = from e in db.CompanyEmpManageEmails
        //                                            select e;
        //    return qry;
        //}

        //IQueryable<IncEmpManageEmail> GetAllIncentexEmpQuery()
        //{
        //    IQueryable<IncEmpManageEmail> qry = from e in db.IncEmpManageEmails
        //                                        select e;
        //    return qry;
        //}

        //IQueryable<SupplierEmpManageEmail> GetAllSupplierEmpQuery()
        //{
        //    IQueryable<SupplierEmpManageEmail> qry = from e in db.SupplierEmpManageEmails
        //                                             select e;
        //    return qry;
        //}
        public List<INC_ManageEmail> GetEmailControl()
        {
            IQueryable<INC_ManageEmail> qry = db.INC_ManageEmails;
            return qry.ToList<INC_ManageEmail>();
        }
        public bool CheckEmailAuthentication(Int64 UserInfoID, Int64 ManageEmailID)
        {
            bool Result = true;
            if (UserInfoID > 0)
            {
                Int64 Usertype = 0;
                UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
                UserInformation objUsrInfo = new UserInformation();
                objUsrInfo = objUsrInfoRepo.GetById(UserInfoID);
                Usertype = objUsrInfo.Usertype;
                switch (Usertype)
                {
                    case 1:
                    case 2:
                        //IQueryable<IncEmpManageEmail> qryI = GetAllIncentexEmpQuery();
                        //qryI = qryI.Where(s => s.UserInfoID == UserInfoID && s.ManageEmailID == ManageEmailID);
                        //IncEmpManageEmail objI = GetSingle(qryI.ToList());


                        IncEmpManageEmail objI = (from a in db.IncEmpManageEmails
                                                  where a.UserInfoID == UserInfoID && a.ManageEmailID == ManageEmailID
                                                  select a).FirstOrDefault(); 

                        Result = objI != null ? true : false;
                        break;
                    case 3:
                    case 4:
                        //IQueryable<CompanyEmpManageEmail> qryC = GetAllCompanyEmpQuery();
                        //qryC = qryC.Where(s => s.UserInfoID == UserInfoID && s.ManageEmailID == ManageEmailID);
                        //CompanyEmpManageEmail objC = GetSingle(qryC.ToList());
                        CompanyEmpManageEmail objC = (from a in db.CompanyEmpManageEmails
                                                      where a.UserInfoID == UserInfoID && a.ManageEmailID == ManageEmailID
                                                      select a).FirstOrDefault();

                        Result = objC != null ? true : false;
                        break;
                    case 5:
                    case 6:
                        //IQueryable<SupplierEmpManageEmail> qryS = GetAllSupplierEmpQuery();
                        //qryS = qryS.Where(s => s.UserInfoID == UserInfoID && s.ManageEmailID == ManageEmailID);
                        //SupplierEmpManageEmail objS = GetSingle(qryS.ToList());
                        SupplierEmpManageEmail objS = (from a in db.SupplierEmpManageEmails
                                                      where a.UserInfoID == UserInfoID && a.ManageEmailID == ManageEmailID
                                                      select a).FirstOrDefault();
                        Result = objS != null ? true : false;
                        break;

                }
            }

            return Result;
        }
    }
}

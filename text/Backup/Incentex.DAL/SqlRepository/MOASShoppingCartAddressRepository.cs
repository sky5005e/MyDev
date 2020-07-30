
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class MOASShoppingCartAddressRepository:RepositoryBase
    {
        public IQueryable<MOASShoppingCartAddress> GetAllQuery()
        {
            IQueryable<MOASShoppingCartAddress> qry = from q in db.MOASShoppingCartAddresses
                                         select q;

            return qry;
        }
        public MOASShoppingCartAddress GetById(Int64 MOASShoppingCartAddressID)
        {
            MOASShoppingCartAddress obj = GetSingle(db.MOASShoppingCartAddresses.Where(u => u.MOASShoppingCartAddID == MOASShoppingCartAddressID).ToList());
            return obj;
        }
        public List<MOASShoppingCartAddress> GetAllAddress(int storeid)
        {
            IQueryable<MOASShoppingCartAddress> qry = (db.MOASShoppingCartAddresses.Where(u => u.StoreID == storeid));
            List<MOASShoppingCartAddress> objList = qry.ToList();
            return objList;
        }
        public int CheckDuplicate(Int64 storeid, Int64 Workgroupid)
        {
            return (int)db.SELECT_MOASDUPLICTEADDRESS(storeid, Workgroupid).SingleOrDefault().IsDuplicate;
        }
        public MOASShoppingCartAddress GetByStoreIDWorkgroupIDAndDepartmentID(Int64 StoreID, Int64 WorkgroupID)
        {
            try
            {
                return db.MOASShoppingCartAddresses.Where(u => u.StoreID == StoreID && u.WorkgroupId == WorkgroupID).FirstOrDefault();
            }
            catch { return null; }
        }

    }
}


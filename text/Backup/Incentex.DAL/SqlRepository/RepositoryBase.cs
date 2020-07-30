using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Configuration;


namespace Incentex.DAL.SqlRepository
{
    public class RepositoryBase
    {

        protected IncentexBEDataContext db;

        String ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            }
        }

        public RepositoryBase()
        {
            //CheckConn();
            //String conn = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

            db = new IncentexBEDataContext(ConnectionString);
            db.CommandTimeout = 300;
        }

        #region Methods

        /// <summary>
        /// Get table method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Table<T> GetTable<T>() where T:class
        {
            return db.GetTable<T>();
        }


        /// <summary>
        /// Insert mathod to insert in table list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        public void Insert<T>(T Entity) where T : class
        {
            GetTable<T>().InsertOnSubmit(Entity);
        }

        /// <summary>
        /// Delete method to delete from table list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        public void Delete<T>(T Entity) where T : class
        {
            GetTable<T>().DeleteOnSubmit(Entity);
        }

        /// <summary>
        /// Delete method to delete from table list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        public void DeleteAll<T>(IEnumerable<T> entities) where T : class
        {
            GetTable<T>().DeleteAllOnSubmit<T>(entities);
        }

        /// <summary>
        /// Submit all changes like insert\update\delete to database 
        /// </summary>
        public void SubmitChanges()
        {
            db.SubmitChanges();
        }

        protected T GetSingle<T>(List<T> List) where T : class
        {
            if (List.Count > 0)
            {
                return List[0];
            }

            return null;

        }

        #endregion
    }
}

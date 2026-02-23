using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DevExpress.Xpo;
using Emecca.Framework.Common;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;

namespace Emecca.DataAccess.Common
{
    /// <summary>
    /// see XPO Best Practices 
    /// http://www.devexpress.com/Support/Center/KB/p/A2944.aspx
    /// </summary>
    public class EmeccaDataAccess
    {
        #region Added By Alan 20120528 依照XPO Best Practices修改的新Access方法
        /// <summary>
        /// 回傳新的XpoSession
        /// Added By Alan 20120528 
        /// </summary>
        /// <returns></returns>
        public static Session getNewXpoSession()
        {
            return new Session(XpoDefault.DataLayer);
        }

        /// <summary>
        /// 依照查詢條件回傳查詢結果:List
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public ArrayList findListByCriteria(Type clazz, string sql)
        {
            ArrayList ret = new ArrayList();
            XPCollection collection = null;
            try
            {
                EmeccaLogger.getEmeccaLogger().Debug(string.Format("{0}:findByCriteria[{1}]", clazz.Name, sql));
                collection = findXPCollectionByCriteria(clazz, sql);
                ret = new ArrayList(collection);
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Debug(string.Format("Error on [{0}] findByCriteria [{1}]", clazz.Name, sql), ee);
                throw ee;
            }
            finally
            {
                collection.Dispose();
            }
            return ret;
        }

        /// <summary>
        /// 依照查詢條件回傳查詢結果:XPCollection
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public XPCollection findXPCollectionByCriteria(Type clazz, string sql)
        {
            XPCollection collection = null;
            try
            {
                collection = new XPCollection(getNewXpoSession(), clazz);
                collection.SelectDeleted = false;
                collection.DeleteObjectOnRemove = false;
                collection.Criteria = CriteriaOperator.Parse(sql);
                EmeccaLogger.getEmeccaLogger().Debug(string.Format("{0}:getXPCollectionByCriteria[{1}]", clazz.Name, sql));
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error(string.Format("Error on [{0}]:getXPCollectionByCriteria[{1}], Error = {2}", clazz.Name, sql, ee.Message), ee);
                throw ee;
            }
            return collection;
        }

        /// <summary>
        /// 依照查詢條件回傳查詢結果:XPLiteObject
        /// 如果有多筆，則丟出Exception
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object findObjectByCriteria(Type clazz, string sql)
        {
            XPCollection collection = null;
            try
            {
                collection = getXPCollectionByCriteria(clazz, sql);
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error(string.Format("Error on [{0}]:findObject[{1}], Error = {2}", clazz.Name, sql, ee.Message), ee);
                throw ee;
            }
            return checkOnlyOneObject(collection);
        }
        /// <summary>
        /// 檢查是否唯一
        /// Added By Alan 20120528
        /// </summary>
        /// <param name="xpObjects"></param>
        /// <returns></returns>
        private static object checkOnlyOneObject(XPCollection xpObjects)
        {
            object xpObject = null;
            if (xpObjects != null)
            {
                if (xpObjects.Count == 0)
                {
                    return xpObject;
                }
                else if (xpObjects.Count > 1)
                {
                    throw new Exception("checkOnlyOneObject存在重複的記錄!");
                }
                xpObject = xpObjects[0];
            }
            return xpObject;
        }
        #endregion
        private static Hashtable sessionMap = new Hashtable();
        public static Session getXpoSession()
        {
            return getXpoSession("DEFAULT");
        }
        public static Session getXpoSession(string name)
        {
            Session xpoSession = sessionMap[name] as Session;
            if (xpoSession == null)
            {
                try
                {
                    xpoSession = new Session();
                   
                    xpoSession.ConnectionString = EmeccaRuntime.getInstance().getDataSourceString(name);
                    xpoSession.AutoCreateOption = AutoCreateOption.SchemaAlreadyExists;
                    xpoSession.Connect();
                    sessionMap.Add(name, xpoSession);
                    //EmeccaLogger.getEmeccaLogger().Error("Connect to db use getXpoSession " + name);
                }
                catch (Exception ee)
                {
                    EmeccaLogger.getEmeccaLogger().Error("Connect to db use getXpoSession " + name+"|"+ee);
                }
            }

            return xpoSession;
        }

        //public void Save(XPLiteObject obj)
        //{
        //    Save("DEFAILT", obj);
        //}
        //public void Save(string name,XPLiteObject obj)
        //{
        //    Session xpoSession = getXpoSession(name);
        //    xpoSession.Save(obj);
        //}

        //public void Load(XPLiteObject obj)
        //{
        //    Load("DEFAULT", obj);
        //}
        //public void Load(string name, XPLiteObject obj)
        //{
        //    Session xpoSession = getXpoSession(name);
        //    xpoSession.Reload(obj);
        //}

        //public void Delete(XPLiteObject obj)
        //{
        //    Delete("DEFAULT", obj);
        //}
        //public void Delete(string name, XPLiteObject obj)
        //{
        //    Session xpoSession = getXpoSession(name);
        //    xpoSession.Delete(obj);
        //}

        /**
         * findByCriteria 依據SQL 條件式查詢資料庫
         * 回傳值確定為非 NULL ArrayList 物件
         * */
        public ArrayList findByCriteria(Type clazz, string sql)
        {
            return findByCriteria("DEFAULT", clazz, sql);
        }
        public ArrayList findByCriteria(string sessionName, Type clazz, string sql)
        {
            ArrayList ret = new ArrayList();
            XPCollection collection = new XPCollection(clazz);
            collection.SelectDeleted = false;
            collection.DeleteObjectOnRemove = false;
            //collection.Sorting.AddRange(new DevExpress.Xpo.SortProperty[] {
            //new DevExpress.Xpo.SortProperty("[OFFSET]", DevExpress.Xpo.DB.SortingDirection.Ascending)});
            try
            {
                collection.Criteria = CriteriaOperator.Parse(sql);
                EmeccaLogger.getEmeccaLogger().Debug("執行 findByCriteria [" + clazz.Name + "]" + sql);
                collection.Session = getXpoSession(sessionName);
                
                /*
                Note By Alan 20120513 
                The XPCollection.Reload method doesn't actually reload any objects. 
                It only marks them as not-loaded. When these objects are accessed next time, 
                the normal object loading process is initiated.
                */
                collection.Reload(); 
                ret = new ArrayList(collection);
                
                /*
                Note By Alan 20120513 http://www.devexpress.com/Support/Center/p/Q218297.aspx
                The XPCollection.Reload method doesn't reload objects that are already loaded into memory, 
                unless this object was previously modified in another application. 
                However, when Optimistic Locking is disabled, 
                it's impossible to determine whether an object was modified. 
                That is why you have to call the XPBaseObject.Reload or the Session.Reload method 
                for each object, or dispose the Session and all loaded objects and collections, 
                and recreate them. The second method is better to keep data consistency. 
                The first one can be used if you need to reload a certain object or collection only, 
                but don't want to reload other objects.
                */

                /* Marked By Alan 20120513 Optimistic Locking Enabled*/

                //解mark  by chris，2012.05.21
                /*事件說明：RIS中，批價查詢畫面 用病歷號碼：70059140 查詢出資料，批量簽收，
                 *此時，立即退簽剛剛簽收的資料，再在批價查詢畫面按查詢，資料還是標記為簽收的，沒有更新（下面的這段mark了）
                 *只有關閉系統，再重新打開 資料才顯示正確。
                 *將下面foreach開放，查詢資料顯示 就是對的。obj.Reload()應該就是去DB查，不reload只是從Dev 的RAM中查。
                 *
                 * 
                 * 注：會不會與Trigger有關？雖然使用view，但是trigger更新的內容還是不能被dev記住？
                 */
                foreach (XPLiteObject obj in ret)
                {
                    obj.Reload();
                }
                 
            }
            catch (Exception ee)
            {
                ret.Clear();
                EmeccaLogger.getEmeccaLogger().Debug(string.Format("Error findByCriteria [{0}], SQL={1}", clazz.Name, sql));
            }
            finally
            {
                collection.Dispose();
            }


            return ret;
        }

        public XPCollection getXPCollectionByCriteria(Type clazz, string sql)
        {
            XPCollection collection = new XPCollection(clazz);
            collection.SelectDeleted = false;
            collection.DeleteObjectOnRemove = false;
            try
            {
                collection.Criteria = CriteriaOperator.Parse(sql);
                EmeccaLogger.getEmeccaLogger().Debug("執行 findByCriteria [" + clazz.Name + "]" + sql);
                collection.Reload();
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error(string.Format("Error findByCriteria [{0}], SQL={1}", clazz.Name, sql) + " " + ee);
                throw ee;
            }

            return collection;
        }

        //public ArrayList findAll(Type clazz)
        //{
        //    return findByCriteria(clazz,"");
        //}

        //public object findByObjId(Type clazz, string objId)
        //{
        //    if (objId == null || "".Equals(objId))
        //        return null;
        //    return findObject(clazz,"ObjId = '" + objId + "'" );
        //}

        /**
         * findObject 依據SQL 條件式查詢資料庫
         * 回傳值
         * 1.當有查到一筆且唯一一筆時 則傳回
         * 2.當有查到多筆 則傳回第一筆
         * 3.查無目標資料時 傳回值為NULL
         * */
        public object findObject(Type clazz, string sql)
        {
            return findObject("DEFAULT", clazz, sql);
        }
        public object findObject(string sessionName, Type clazz, string sql)
        {
            object ret = null;
            ArrayList tmp = new ArrayList();

            XPCollection collection = new XPCollection(clazz);
            collection.SelectDeleted = false;
            collection.DeleteObjectOnRemove = false;

            try
            {
                collection.Criteria = CriteriaOperator.Parse(sql);
                EmeccaLogger.getEmeccaLogger().Debug("執行 findObject [" + clazz.Name + "]" + sql);
                collection.Session = getXpoSession(sessionName);
                collection.Reload();
                ret = checkOnlyOne(collection);
                if (ret != null)
                {
                    ((XPLiteObject)ret).Reload();
                }
            }
            catch (Exception ee)
            {
                ret = null;
                EmeccaLogger.getEmeccaLogger().Debug("Error findObject [" + clazz.Name + "]" + sql);
            }
            finally
            {
                collection.Dispose();
            }

            return ret;
        }

        private object checkOnlyOne(XPCollection xpObjects)
        {
            if (xpObjects == null || xpObjects.Count == 0)
                return null;

            if (xpObjects.Count > 1)
            {
                EmeccaLogger.getEmeccaLogger().Debug("重複的記錄!!");
            }
            return xpObjects[0];
        }

        public static string And(string sql, string criterion)
        {
            string ret = "";

            if (!string.IsNullOrEmpty(criterion))
            {
                if (string.IsNullOrEmpty(sql))
                {
                    ret = criterion;
                }
                else
                {
                    ret = sql.Trim() + " AND " + criterion;
                }
            }
            else
            {
                ret = sql.Trim();
            }

            return ret;
        }

        public static string Or(string sql, string criterion)
        {
            string ret = "";
            if (!string.IsNullOrEmpty(criterion))
            {
                if (string.IsNullOrEmpty(sql))
                {
                    ret = criterion;
                }
                else
                {
                    ret = sql.Trim() + " OR " + criterion;
                }
            }
            else
            {
                ret = sql.Trim();
            }
            return ret;
        }
        public static string StringEqualOrLike(string field_name, string value)
        {
            string ret = "";

            if (!string.IsNullOrEmpty(value))
            {
                if (value.Contains('*') || value.Contains('%'))
                {
                    ret = " (" + field_name + " LIKE '" + value.Trim().Replace('*', '%') + "')";
                }
                else
                {
                    ret = " (" + field_name + " = '" + value.Trim() + "')";
                }
            }
            return ret;
        }

        public static string Between(string field_name, object first, object second)
        {
            string ret = "";

            if (first == null && second == null)
                return "";

            if (first == null || "".Equals(first))
            {
                return LessEqualThan(field_name, second);
            }

            if (second == null || "".Equals(second))
            {
                return GreatEqualThan(field_name, first);
            }

            if (first != null && second != null)
            {
                ret = " (" + field_name + " >= '" + first.ToString().Trim() + "' AND " + field_name + " <= '" + second.ToString().Trim() + "')";
            }
            return ret;
        }
        
        /// <summary>
        /// Modified By Alan 20120513
        /// 當ArrayList Size 只有一個時，自動轉換為Equal
        /// </summary>
        /// <param name="field_name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string In(string field_name, ArrayList values)
        {
            string ret = "";

            if (values == null || values.Count == 0)
                return ret;

            //Added By Alan 20120513 只有一個條件時改用Equal
            if (values != null && values.Count == 1) 
            {
                return Equal( field_name, values[0].ToString());
            }
            
            string delim = "";
            foreach (string param in values)
            {
                ret = ret + delim + " '" + param + "'";
                delim = ",";
            }

            if (!string.IsNullOrEmpty(ret))
            {
                ret = " (" + field_name + " IN (" + ret.ToString().Trim() + ") )";
            }

            return ret;
        }

        /// <summary>
        /// 判斷當前端勾選所有條件時，則等同全部不選，直接Return
        /// </summary>
        /// <param name="field_name">欄位名稱</param>
        /// <param name="values">符合的所有條件</param>
        /// <param name="ignoreQty">前端全部勾選，視同不作為條件</param>
        /// <returns></returns>
        public static string In(string field_name, ArrayList values, int ignoreQty)
        {
            string ret = "";

            if (values == null || values.Count == 0 || values.Count == ignoreQty) 
            {
                return ret;
            }
            ret = In(field_name, values);
            return ret;
        }


        public static string GreatThan(string field_name, object value)
        {
            string ret = "";
            if (value != null && !"".Equals(value))
            {
                ret = " (" + field_name + " > '" + value.ToString().Trim() + "')";
            }
            return ret;
        }
        public static string GreatEqualThan(string field_name, object value)
        {
            string ret = "";
            if (value != null && !"".Equals(value))
            {
                ret = " (" + field_name + " >= '" + value.ToString().Trim() + "')";
            }
            return ret;
        }

        public static string LessThan(string field_name, object value)
        {
            string ret = "";
            if (value != null && !"".Equals(value))
            {
                ret = " (" + field_name + " < '" + value.ToString().Trim() + "')";
            }
            return ret;
        }
        public static string LessEqualThan(string field_name, object value)
        {
            string ret = "";
            if (value != null && !"".Equals(value))
            {
                ret = " (" + field_name + " <= '" + value.ToString().Trim() + "')";
            }
            return ret;
        }
        public static string Equal(string field_name, string value)
        {
            string ret = "";

            if (!string.IsNullOrEmpty(value))
            {
                ret = " (" + field_name + " = '" + value.Trim() + "')";
            }
            return ret;
        }

        public static string IsNull(string field_name)
        {
            string ret = "";

            ret = " (" + field_name + " IS NULL )";

            return ret;
        }

        public static string Not(string sql)
        {
            string ret = "";

            ret = " NOT (" + sql + " )";

            return ret;
        }
    }
}

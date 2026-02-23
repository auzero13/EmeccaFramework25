using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emecca.Framework.Controller;
using System.Collections;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using Emecca.Framework.Common;
using DevExpress.XtraEditors;
using Emecca.Framework.UI;
using Emecca.DataAccess.Common;
using Emecca.Express.Common;

namespace Emecca.Framework.DataAccess.Dao
{
    public class ResourcesBasDao : EmeccaDataAccess
    {
        private static ResourcesBasDao instance = new ResourcesBasDao();
        public static ResourcesBasDao Instance
        {
            get { return instance; }
        }
        private ResourcesBasDao() { }

        /// <summary>
        /// 根據菜單的ID判斷用戶是否有使用權限
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool isUserAuthorized(object value)
        {
            bool flag = false;
            string resourcesCode = value as string;
            if (resourcesCode != "")
            {
                Hashtable userRoleAcls = EmeccaRuntime.getInstance().getVariable("USER_Resource") as Hashtable;
                if (userRoleAcls.Contains(resourcesCode))
                {
                    flag = true;
                }
                else if (userRoleAcls.Contains("ADMIN-ADMIN"))
                {
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// 返回一個bool 判斷是否有權限  如果有權限則為true否則為false
        /// </summary>
        /// <param name="sender">一個simpleButton或者是一個string類型的值</param>
        /// <returns></returns>
        public bool getUserPermissions(object sender)
        {
            string resourceCode = string.Empty;
            if (sender is string)
            {
                resourceCode = sender.ToString();
            }
            else if (sender is SimpleButton)
            {
                SimpleButton button = sender as SimpleButton;
                resourceCode = button.Tag.ToString();
            }
            bool flag = ResourcesBasDao.Instance.isUserAuthorized(resourceCode);
            if (!flag)
            {
                EmeccaMessageBox.Show("您沒有執行權限，請聯絡管理員!", "承啓醫系科室管理系統");
            }
            return flag;
        }
    }

}

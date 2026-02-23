using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Emecca.Framework.Utility
{
    public class EmeccaCommonUtility
    {
        /// <summary>
        /// 依照傳入的生日取得實際的年齡
        /// 大於2歲回傳 N歲
        /// 大於1歲小於2回傳 Y歲M月
        /// 小於1歲小於2回傳 M月D天
        /// </summary>
        /// <param name="birthday">yyyyMMdd</param>
        /// <returns></returns>
        public static string getPatientRealAge(string birthday,string strAcceptDate)
        {
            string birthInfo = "";
            DateTime birthDay = DateTime.Today; //預設系統日期
            try
            {
                try
                {
                    //驗證是否是DateTime類型
                    birthDay = DateTime.ParseExact(birthday, "yyyyMMdd", new CultureInfo("zh-TW"));
                }
                catch (Exception)
                {
                    birthInfo = string.Format("生日{0}無效", birthday);
                    return birthInfo;
                }
                //DateTime today = DateTime.Today;
                DateTime acceptDate = DateTime.ParseExact(strAcceptDate, "yyyyMMdd", new CultureInfo("zh-TW"));
                TimeSpan timeSpan = acceptDate - birthDay;
                if (timeSpan.Days < 0)
                {
                    birthInfo = string.Format("生日{0}無效", birthday);
                    return birthInfo;
                }

                //取得幾歲
                int ageYear = acceptDate.Year - birthDay.Year;
                if (birthDay.AddYears(ageYear) > acceptDate)
                {
                    ageYear = ageYear - 1;
                }

                if (ageYear >= 2)
                {
                    birthInfo = string.Format("{0}歲", ageYear);
                }
                else if (ageYear < 2)
                {
                    //取得幾月
                    int ageMonth = getMonthCount(birthDay, acceptDate, ageYear);
                    if (ageYear == 1)
                    {
                        birthInfo = string.Format("{0}歲{1}月", ageYear, ageMonth);
                    }
                    else if (ageYear == 0)
                    {
                        int ageDay = getDayCount(birthDay, acceptDate, ageYear, ageMonth);
                        birthInfo = string.Format("{0}月{1}天", ageMonth, ageDay);
                    }
                }
            }
            catch (Exception ee)
            {
                throw new Exception(string.Format("計算生日{0}實際年齡錯誤:{1}", birthday, ee.Message));
            }
            return birthInfo;
        }

        private static int getMonthCount(DateTime birthday, DateTime today, int age)
        {
            int month = 0;
            DateTime bDay = birthday.AddYears(age);
            int bMonth = bDay.Month;
            int tMonth = today.Month;
            if (bDay.Year == today.Year)
            {
                month = tMonth - bMonth;
            }
            else
            {
                month = 12 - bMonth + tMonth;
            }
            if (bDay.Day > today.Day)
            {
                month = month - 1;
            }

            return month;
        }

        private static int getDayCount(DateTime birthday, DateTime today, int age, int month)
        {
            DateTime currentYearBirth = birthday.AddYears(age).AddMonths(month);
            TimeSpan dayCountSpan = today - currentYearBirth;
            int dayCount = dayCountSpan.Days;
            return dayCount;
        }

        public static string maskString(string maskString, int intfrom, int intlength, string mask)
        {
            string strMaskString = maskString;
            string strMask = "";
            for (int i = 0; i < intlength - intfrom; i++)
            {
                strMask += mask;
            }
            strMaskString = maskString.Replace(maskString.Substring(intfrom, intlength - 1), strMask);
            return strMaskString;
        }

        
    }
}

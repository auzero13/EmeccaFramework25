using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Emecca.Framework.Utility
{
    /// <summary>
    /// 驗證輸入的字串是否符合驗證規則，常用的驗證規則可直接呼叫，舉例IsDate
    /// 特別的驗證規則仍可呼叫IsMatch，傳入自己定義的RegularExpressions
    /// </summary>
    public class EmeccaValidatorUtility
    {
        
        /// <summary>
        /// 判斷字串是否與指定正則運算式匹配
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <param name="regularExp">正則運算式</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsMatch(string input, string regularExp)
        {
            return Regex.IsMatch(input, regularExp);
        }

        /// <summary>
        /// 驗證非負整數（正整數 + 0）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUnMinusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnMinusInteger);
        }

        /// <summary>
        /// 驗證正整數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsPlusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.PlusInteger);
        }

        /// <summary>
        /// 驗證非正整數（負整數 + 0）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUnPlusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnPlusInteger);
        }

        /// <summary>
        /// 驗證負整數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsMinusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.MinusInteger);
        }

        /// <summary>
        /// 驗證整數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.Integer);
        }

        /// <summary>
        /// 驗證非負浮點數（正浮點數 + 0）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUnMinusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnMinusFloat);
        }

        /// <summary>
        /// 驗證正浮點數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsPlusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.PlusFloat);
        }

        /// <summary>
        /// 驗證非正浮點數（負浮點數 + 0）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUnPlusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnPlusFloat);
        }

        /// <summary>
        /// 驗證負浮點數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsMinusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.MinusFloat);
        }

        /// <summary>
        /// 驗證浮點數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.Float);
        }

        /// <summary>
        /// 驗證由26個英文字母組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.Letter);
        }

        /// <summary>
        /// 驗證由中文組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsChinese(string input)
        {
            return Regex.IsMatch(input, RegularExp.Chinese);
        }

        /// <summary>
        /// 驗證由26個英文字母的大寫組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUpperLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.UpperLetter);
        }

        /// <summary>
        /// 驗證由26個英文字母的小寫組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsLowerLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.LowerLetter);
        }

        /// <summary>
        /// 驗證由數位和26個英文字母組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsNumericOrLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.NumericOrLetter);
        }

        /// <summary>
        /// 驗證由數位組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsNumeric(string input)
        {
            return Regex.IsMatch(input, RegularExp.Numeric);
        }
        /// <summary>
        /// 驗證由數位和26個英文字母或中文組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsNumericOrLetterOrChinese(string input)
        {
            return Regex.IsMatch(input, RegularExp.NumbericOrLetterOrChinese);
        }

        /// <summary>
        /// 驗證由數位、26個英文字母或者下劃線組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsNumericOrLetterOrUnderline(string input)
        {
            return Regex.IsMatch(input, RegularExp.NumericOrLetterOrUnderline);
        }

        /// <summary>
        /// 驗證email地址
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsEmail(string input)
        {
            return Regex.IsMatch(input, RegularExp.Email);
        }

        /// <summary>
        /// 驗證URL
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUrl(string input)
        {
            return Regex.IsMatch(input, RegularExp.Url);
        }

        /// <summary>
        /// 驗證IP
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsIP(string input)
        {
            return Regex.IsMatch(input, RegularExp.IP);
        }

        /// <summary>
        /// 驗證日期（YYYY-MM-DD）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsDate(string input)
        {
            return Regex.IsMatch(input, RegularExp.Date);
        }

        /// <summary>
        /// 驗證日期和時間（YYYY-MM-DD HH:MM:SS）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsDateTime(string input)
        {
            return Regex.IsMatch(input, RegularExp.DateTime);
        }

        /// <summary>
        /// 驗證顏色（#ff0000）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsColor(string input)
        {
            return Regex.IsMatch(input, RegularExp.Color);
        }
    }

    public struct RegularExp
    {
        public const string Chinese = @"^[\u4E00-\u9FA5\uF900-\uFA2D]+$";
        public const string Color = "^#[a-fA-F0-9]{6}";
        public const string Date = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
        public const string DateTime = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
        public const string Email = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
        public const string Float = @"^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$";//pass
        public const string Integer = @"^-?\d+$";//pass
        public const string IP = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";//pass
        public const string Letter = "^[A-Za-z]+$";//pass
        public const string LowerLetter = "^[a-z]+$";//pass
        public const string MinusFloat = @"^-([1-9]\d*\.\d*|0\.\d*[1-9]\d*)$";//pass
        public const string MinusInteger = @"^-[1-9]\d*$";//pass
        /// <summary>
        /// 简繁体都可以通过
        /// </summary>
        public const string NumbericOrLetterOrChinese = @"^[A-Za-z0-9\u4E00-\u9FA5\uF900-\uFA2D]+$";
        public const string Numeric = "^[0-9]+$";//pass
        public const string NumericOrLetter = "^[A-Za-z0-9]+$";//pass
        public const string NumericOrLetterOrUnderline = @"^\w+$";//pass
        public const string PlusFloat = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$";//pass
        public const string PlusInteger = @"^[1-9]\d*$";//pass
        public const string UnMinusFloat = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0$";//pass
        public const string UnMinusInteger = @"^-[1-9]\d*|0$";//pass
        public const string UnPlusFloat = @"^(-([1-9]\d*\.\d*|0\.\d*[1-9]\d*))|0?\.0+|0$";//pass
        public const string UnPlusInteger = @"^-[1-9]\d*|0$";//pass
        public const string UpperLetter = "^[A-Z]+$";//pass
        /// <summary>
        /// 不一定满足要求
        /// </summary>
        public const string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";//pass 
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace Emecca.Framework.Utility
{
   public class EmeccaEncryptUtility
    {
       /// <summary>
       /// DES 解密
       ///字串非Byte 類型，返回原字串，發生其他錯誤，throw
       /// </summary>
       /// <param name="oldData"></param>
       /// <returns></returns>
       public static string DecodeDES(string oldData)
       {
           try
           {
               string key = @"??^&$%^&";
               string[] inputData = oldData.Split("-".ToCharArray());
               byte[] data = new byte[inputData.Length];

               bool convertSuccess;
               for (int i = 0; i < inputData.Length; i++)
               {

                   convertSuccess = byte.TryParse(inputData[i], NumberStyles.HexNumber,null, out data[i]);
                   if (!convertSuccess)
                   {
                       return oldData;
                   }
                   //data[i] = byte.Parse(inputData[i], NumberStyles.HexNumber);
               }

               DESCryptoServiceProvider des = new DESCryptoServiceProvider();

               des.Key = ASCIIEncoding.ASCII.GetBytes(key);

               des.IV = ASCIIEncoding.ASCII.GetBytes(key);

               ICryptoTransform desencrypt = des.CreateDecryptor();

               byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
               return Encoding.UTF8.GetString(result);
           }
           catch
           {
               throw;
           }
       }

       public static string EncryptDES(string oldData)
       {

           string key = @"??^&$%^&";
           byte[] data = Encoding.UTF8.GetBytes(oldData);

           DESCryptoServiceProvider des = new DESCryptoServiceProvider();

           des.Key = ASCIIEncoding.ASCII.GetBytes(key);
           des.IV = ASCIIEncoding.ASCII.GetBytes(key);

           ICryptoTransform desencrypt = des.CreateEncryptor();
           try
           {
               byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
               return BitConverter.ToString(result);
           }
           catch
           {
               throw;
           }
       }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.DES
{
    public class DESCode
    {
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="p_data"></param>
        /// <param name="p_ErrMsg">異常訊息</param>
        /// <param name="p_key">預設值=公司統一編號(12358067)</param>
        /// <param name="p_iv">預設值=公司統一編號(76085321)</param>
        /// <returns></returns>
        public static string desEncryptBase64(string p_data, ref string p_ErrMsg, string p_key = "12358067", string p_iv = "76085321")
        {
            string m_result = "";

            try
            {
                using (var des = new TripleDESCryptoServiceProvider())
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    byte[] key = Encoding.ASCII.GetBytes(p_key);
                    byte[] iv = Encoding.ASCII.GetBytes(p_iv);
                    byte[] dataByteArray = Encoding.UTF8.GetBytes(p_data);

                    des.Key = md5.ComputeHash(key);
                    des.IV = iv;

                    using (MemoryStream ms = new MemoryStream())
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        m_result = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex) {
                p_ErrMsg = ex.Message;
            }
            
            return m_result;
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="p_EncryptData"></param>
        /// <param name="p_ErrMsg">異常訊息</param>
        /// <param name="p_key">預設值=公司統一編號(12358067)</param>
        /// <param name="p_iv">預設值=公司統一編號(76085321)</param>
        /// <returns></returns>
        public static string desDecryptBase64(string p_EncryptData, ref string p_ErrMsg, string p_key = "12358067", string p_iv = "76085321")
        {
            string m_result = "";

            try
            {
                using (var des = new TripleDESCryptoServiceProvider())
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    byte[] key = Encoding.ASCII.GetBytes(p_key);
                    byte[] iv = Encoding.ASCII.GetBytes(p_iv);
                    des.Key = md5.ComputeHash(key);
                    des.IV = iv;

                    byte[] dataByteArray = Convert.FromBase64String(p_EncryptData);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(dataByteArray, 0, dataByteArray.Length);
                            cs.FlushFinalBlock();
                            m_result = Encoding.UTF8.GetString(ms.ToArray());
                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                p_ErrMsg = ex.Message;
            }

            return m_result;
        }
    }
}

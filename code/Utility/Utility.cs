﻿//get md5 from file

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QFlashKit.code.Utility
{
    public class Utility
    {
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                var fileStream = new FileStream(fileName, FileMode.Open);
                var hash = new MD5CryptoServiceProvider().ComputeHash(fileStream);
                fileStream.Close();
                var stringBuilder = new StringBuilder();
                for (var index = 0; index < hash.Length; ++index)
                    stringBuilder.Append(hash[index].ToString("x2"));
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
    }
}
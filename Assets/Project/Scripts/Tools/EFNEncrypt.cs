using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace EFN {
    public class EFNEncrypt {
        private const string key = "20484096";

        private DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        public EFNEncrypt() {
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
        }

        public string Encrypt(string password) {
            byte[] inBlock = UnicodeEncoding.Unicode.GetBytes(password);
            ICryptoTransform Encrypt = des.CreateEncryptor();
            byte[] outBlock = Encrypt.TransformFinalBlock(inBlock, 0, inBlock.Length);

            return System.Convert.ToBase64String(outBlock);
        }

        public string Decrypt(string encodedPassword) {
            byte[] inBlock = System.Convert.FromBase64String(encodedPassword);
            ICryptoTransform Decrypt = des.CreateDecryptor();
            byte[] outBlock = Decrypt.TransformFinalBlock(inBlock, 0, inBlock.Length);

            return UnicodeEncoding.Unicode.GetString(outBlock);
        }

    }
}
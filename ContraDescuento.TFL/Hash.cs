using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.TFL
{

    public class Hash
    {
        #region Propiedades privadas
        private string mSalt;
        private HashAlgorithm mCryptoService;
        #endregion

        #region Propiedades públicas
        public enum ServiceProviderEnum : int
        {
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            MD5
        }

        public Hash()
        {
            mCryptoService = new SHA1Managed();
        }

        public Hash(ServiceProviderEnum serviceProvider)
        {
            switch (serviceProvider)
            {
                case ServiceProviderEnum.MD5:
                    mCryptoService = new MD5CryptoServiceProvider();
                    break;
                case ServiceProviderEnum.SHA1:
                    mCryptoService = new SHA1Managed();
                    break;
                case ServiceProviderEnum.SHA256:
                    mCryptoService = new SHA256Managed();
                    break;
                case ServiceProviderEnum.SHA384:
                    mCryptoService = new SHA384Managed();
                    break;
                case ServiceProviderEnum.SHA512:
                    mCryptoService = new SHA512Managed();
                    break;
            }
        }

        public Hash(string serviceProviderName)
        {
            try
            {
                mCryptoService = (HashAlgorithm)CryptoConfig.CreateFromName(
                  serviceProviderName.ToUpper());
            }
            catch
            {
                throw;
            }
        }

        public virtual string Encrypt(string plainText)
        {
            byte[] cryptoByte = mCryptoService.ComputeHash(
                ASCIIEncoding.ASCII.GetBytes(plainText + mSalt));

            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.Length);
        }

        public string Salt
        {
            get
            {
                return mSalt;
            }
            set
            {
                mSalt = value;
            }
        }
        #endregion
    }
}


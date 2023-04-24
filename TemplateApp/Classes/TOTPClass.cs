using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protecc.Classes
{
    public class TOTPClass
    {
        private readonly string _Secret;
        private readonly string _Account;
        private readonly string _Issuer;
        private readonly AlgorithmOption _Algorithm = AlgorithmOption.SHA1;
        private readonly int _Digits = 6;
        private readonly int _Period = 30;
        public TOTPClass(string uriString)
        {
            try
            {
                Uri uri = new(uriString);
                if (uri.Host != "totp") throw new Exception("This URI is not totp format");
                string[] label = uri.Segments[1].Split(':');
                if (label.Length == 2)
                {
                    _Account = Uri.UnescapeDataString(label[0]);
                    _Issuer = Uri.UnescapeDataString(label[1]);
                }
                else
                {
                    _Account = uri.Segments[1];
                }
                NameValueCollection queryDictionary = System.Web.HttpUtility.ParseQueryString(uri.Query);
                _Secret = queryDictionary["secret"] ?? _Secret;
                _Algorithm = (AlgorithmOption)Enum.Parse(typeof(AlgorithmOption), queryDictionary["algorithm"] ?? "SHA1");
                _Digits = queryDictionary["digits"] == "8" ? 8 : 6;
                _Period = queryDictionary["period"] == "60" ? 60 : 30;
                _Issuer = queryDictionary["issuer"];
            }
            catch { }
        }

        public TOTPClass(string secret, string account, string issuer, string algorithm, int digits, int period)
        {
            _Secret = secret;
            _Account = account;
            _Issuer = issuer;
            _Algorithm = (AlgorithmOption)Enum.Parse(typeof(AlgorithmOption), algorithm ?? "SHA1");
            _Digits = digits == 8 ? 8 : 6;
            _Period = period == 60 ? 60 : 30;
        }

        public string Secret { get { return _Secret; } }
        public string Account { get { return _Account; } }
        public string Issuer { get { return _Issuer; } }
        public AlgorithmOption Algorithm { get { return _Algorithm; } }
        public int Digits { get { return _Digits; } }
        public int Period { get { return _Period; } }
    }
    public enum AlgorithmOption
    {
        SHA1,
        SHA256,
        SHA512
    }
}

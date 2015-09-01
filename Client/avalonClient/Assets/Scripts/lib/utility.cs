using System.Security.Cryptography;
using System.Text;

namespace utility {

    class crypt {

        public static string sha512(string text)
        {

            SHA512 shaM = new SHA512Managed();
            byte[] hash = shaM.ComputeHash(Encoding.ASCII.GetBytes(text));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();

        }
    }  
}

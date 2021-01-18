using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bint = System.Numerics.BigInteger;
using static System.Console;

namespace ISM.RSA
{
    public class RSACrypto
    {
        #region Private Field

        private static readonly char[] _alphabet = GenerateAllSymbolsToAlphabet();

        #endregion

        #region Public Methods

        public string Encode(string value, long p, long q, out long n, out long d)
        {
            if (!IsSimpleNumber(p) || !IsSimpleNumber(q))
                throw new ArgumentException($"{nameof(p)} or {nameof(q)} are not a simple numbers");

            return EncodeAlgorithm(value, p, q, out n, out d);
        }

        public string Decode(string value, long d, long n)
        {
            return DecodeAlgorithm(value, d, n);
        }

        #endregion

        #region Private Methods

        private static char[] GenerateAllSymbolsToAlphabet()
        {
            var alphabet = new List<char>();

            for (int item = char.MinValue; item <= char.MaxValue; item++)
                alphabet.Add(Convert.ToChar(item));

            return alphabet.ToArray();
        }

        private bool IsSimpleNumber(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }

        private string EncodeAlgorithm(string value, long p, long q, out long n, out long d)
        {
            n = p * q;
            var m = (p - 1) * (q - 1);
            d = CalculateD(m);
            var e = CalculateE(d, m);

            var result = new List<string>();
            for (int i = 0; i < value.Length; i++)
            {
                int index = Array.IndexOf(_alphabet, value[i]);
                var encodedSymbol = (bint.Pow(new bint(index), (int)e)) % new bint((int)n);
                result.Add(encodedSymbol.ToString());
            }

            return string.Join("|", result.ToArray());
        }

        private string DecodeAlgorithm(string value, long d, long n)
        {
            var splitedValue = value.Split('|').ToList();
            var result = new StringBuilder();

            foreach (string item in splitedValue)
            {
                var bi = bint.Pow(new bint(Convert.ToDouble(item)), (int)d) % new bint((int)n);
                var index = Convert.ToInt32(bi.ToString());
                result.Append(_alphabet[index]);
            }

            return result.ToString();
        }

        private long CalculateD(long m)
        {
            var d = m - 1;

            for (long i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0))                     
                {
                    d--;
                    i = 1;
                }

            return d;
        }

        private long CalculateE(long d, long m)
        {
            long e = 10;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }

        #endregion
    }
}

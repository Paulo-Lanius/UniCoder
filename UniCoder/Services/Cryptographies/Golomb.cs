using System.Text;

namespace UniCoder.Services.Cryptographies
{
    public class Golomb : ICryptography
    {
        public int m = 120;

        public string Encrypt(string input)
        {
            Console.WriteLine($"Criptografia Golomb");

            static string Golomb(int number, int k)
            {
                StringBuilder EncryptdText = new();
                int prefix = number / k;
                int sufix = number % k;

                // Prefixo 
                for (int i = 0; i < prefix; i++)
                {
                    EncryptdText.Append('0');
                }

                // StopBit
                EncryptdText.Append('1');

                // Sufix                
                string sufixBinary = Convert.ToString(sufix, 2);
                int bitSize = (int)Math.Ceiling(Math.Log2(k));
                sufixBinary = sufixBinary.PadLeft(bitSize, '0'); // Ajusta o sufixo para o tamanho correto de bits
                EncryptdText.Append(sufixBinary);

                return EncryptdText.ToString();
            }

            StringBuilder result = new();

            foreach (char c in input)
            {
                int asciiValue = (int)c;
                result.Append(Golomb(asciiValue, m));
            }

            return result.ToString();
        }

        public string Decrypt(string input)
        {
            Console.WriteLine($"Descriptografia Golomb");

            static string Golomb(string EncryptdText, int k, StringBuilder DecryptdText, int index = 0)
            {
                if (index >= EncryptdText.Length)
                {
                    return DecryptdText.ToString();
                }

                // Prefixo
                int nPrefix = 0;
                while (EncryptdText[index] == '0')
                {
                    nPrefix++;
                    index++;
                }
                int prefix = nPrefix * k;

                // Pular StopBit
                index++;

                // Sufixo
                int bitSize = (int)Math.Ceiling(Math.Log2(k));
                string sufixBits = EncryptdText.Substring(index, bitSize);
                int sufix = Convert.ToInt32(sufixBits, 2);
                index += bitSize;

                DecryptdText.Append((char)(prefix + sufix));

                return Golomb(EncryptdText, k, DecryptdText, index);
            }

            return Golomb(input, m, new StringBuilder());
        }
    }
}

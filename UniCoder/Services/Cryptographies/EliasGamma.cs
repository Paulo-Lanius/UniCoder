using System.Text;

namespace UniCoder.Services.Cryptographies
{
    public class EliasGamma : ICryptography
    {
        public string Encrypt(string input)
        {
            Console.WriteLine($"Criptografia EliasGamma");

            static string EliasGamma(int number)
            {
                StringBuilder EncryptdText = new();
                int bitSize = (int)Math.Floor(Math.Log(number, 2));

                // Prefixo
                for (int i = 0; i < bitSize; i++)
                {
                    EncryptdText.Append('0');
                }

                // StopBit
                EncryptdText.Append('1');

                // Sufix
                int sufixInt = number - (int)Math.Pow(2, bitSize);
                string sufixBinary = Convert.ToString(sufixInt, 2);
                sufixBinary = sufixBinary.PadLeft(bitSize, '0'); // Ajusta o sufixo para o tamanho correto de bits
                EncryptdText.Append(sufixBinary);

                return EncryptdText.ToString();
            }

            StringBuilder EncryptdString = new();

            foreach (char c in input)
            {
                int asciiValue = (int)c;
                EncryptdString.Append(EliasGamma(asciiValue));
            }

            return EncryptdString.ToString();
        }

        public string Decrypt(string input)
        {
            Console.WriteLine($"Descriptografia EliasGamma");

            static string EliasGamma(string EncryptdText, StringBuilder DecryptdText, int index = 0)
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
                int prefix = (int)Math.Pow(2, nPrefix);

                // Pular StopBit
                index++;

                // Sufixo
                string sufixBits = EncryptdText.Substring(index, nPrefix);
                int sufix = Convert.ToInt32(sufixBits, 2);
                index += nPrefix;

                DecryptdText.Append((char)(prefix + sufix));

                return EliasGamma(EncryptdText, DecryptdText, index);
            }

            return EliasGamma(input, new StringBuilder());
        }
    }
}

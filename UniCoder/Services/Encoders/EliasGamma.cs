using System.Text;

namespace UniCoder.Services.Encoders
{
    public class EliasGamma : IEncoder
    {
        public string Encode(string input)
        {
            Console.WriteLine($"Codificar EliasGamma");

            static string EliasGamma(int number)
            {
                StringBuilder EncodedText = new();
                int bitSize = (int)Math.Floor(Math.Log(number, 2));

                // Prefixo
                for (int i = 0; i < bitSize; i++)
                {
                    EncodedText.Append('0');
                }

                // StopBit
                EncodedText.Append('1');

                // Sufix
                int sufixInt = number - (int)Math.Pow(2, bitSize);
                string sufixBinary = Convert.ToString(sufixInt, 2);
                sufixBinary = sufixBinary.PadLeft(bitSize, '0'); // Ajusta o sufixo para o tamanho correto de bits
                EncodedText.Append(sufixBinary);

                return EncodedText.ToString();
            }

            StringBuilder EncodedString = new();

            foreach (char c in input)
            {
                int asciiValue = (int)c;
                EncodedString.Append(EliasGamma(asciiValue));
            }

            return EncodedString.ToString();
        }

        public string Decode(string input)
        {
            Console.WriteLine($"Decodificar EliasGamma");

            static string EliasGamma(string EncodedText, StringBuilder DecodedText, int index = 0)
            {
                if (index >= EncodedText.Length)
                {
                    return DecodedText.ToString();
                }

                // Prefixo
                int nPrefix = 0;
                while (EncodedText[index] == '0')
                {
                    nPrefix++;
                    index++;
                }
                int prefix = (int)Math.Pow(2, nPrefix);

                // Pular StopBit
                index++;

                // Sufixo
                string sufixBits = EncodedText.Substring(index, nPrefix);
                int sufix = Convert.ToInt32(sufixBits, 2);
                index += nPrefix;

                DecodedText.Append((char)(prefix + sufix));

                return EliasGamma(EncodedText, DecodedText, index);
            }

            return EliasGamma(input, new StringBuilder());
        }
    }
}

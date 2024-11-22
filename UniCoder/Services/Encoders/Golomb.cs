using System.Text;

namespace UniCoder.Services.Encoders
{
    public class Golomb : IEncoder
    {
        public int m = 120;

        public string Encode(string input)
        {
            Console.WriteLine($"Codificar Golomb");

            static string Golomb(int number, int k)
            {
                StringBuilder EncodedText = new();
                int prefix = number / k;
                int sufix = number % k;

                // Prefixo 
                for (int i = 0; i < prefix; i++)
                {
                    EncodedText.Append('0');
                }

                // StopBit
                EncodedText.Append('1');

                // Sufix                
                string sufixBinary = Convert.ToString(sufix, 2);
                int bitSize = (int)Math.Ceiling(Math.Log2(k));
                sufixBinary = sufixBinary.PadLeft(bitSize, '0'); // Ajusta o sufixo para o tamanho correto de bits
                EncodedText.Append(sufixBinary);

                return EncodedText.ToString();
            }

            StringBuilder result = new();

            foreach (char c in input)
            {
                int asciiValue = (int)c;
                result.Append(Golomb(asciiValue, m));
            }

            return result.ToString();
        }

        public string Decode(string input)
        {
            Console.WriteLine($"Decodificar Golomb");

            static string Golomb(string EncodedText, int k, StringBuilder DecodedText, int index = 0)
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
                int prefix = nPrefix * k;

                // Pular StopBit
                index++;

                // Sufixo
                int bitSize = (int)Math.Ceiling(Math.Log2(k));
                string sufixBits = EncodedText.Substring(index, bitSize);
                int sufix = Convert.ToInt32(sufixBits, 2);
                index += bitSize;

                DecodedText.Append((char)(prefix + sufix));

                return Golomb(EncodedText, k, DecodedText, index);
            }

            return Golomb(input, m, new StringBuilder());
        }
    }
}

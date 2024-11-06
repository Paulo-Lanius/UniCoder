using System.Text;

namespace UniCoder.Services
{
    public class DecrypterService
    {
        public DecrypterService() { }

        #region GA

        public static string DecodeEliasGamma(string encodedText)
        {
            static string EliasGamma(string encodedText, StringBuilder decodedText, int index = 0)
            {
                if (index >= encodedText.Length)
                {
                    return decodedText.ToString();
                }

                // Prefixo
                int nPrefix = 0;
                while (encodedText[index] == '0')
                {
                    nPrefix++;
                    index++;
                }
                int prefix = (int)Math.Pow(2, nPrefix);

                // Pular StopBit
                index++;

                // Sufixo
                string sufixBits = encodedText.Substring(index, nPrefix);
                int sufix = Convert.ToInt32(sufixBits, 2);
                index += nPrefix;

                decodedText.Append((char)(prefix + sufix));

                return EliasGamma(encodedText, decodedText, index);
            }

            return EliasGamma(encodedText, new StringBuilder());
        }

        public static string DecodeFibonacciZeckendorf(string encodedText)
        {
            static List<int> GenerateFibonacci(int max)
            {
                List<int> fibonacci = [1, 2];
                while (fibonacci[^1] <= max)
                {
                    int nextFib = fibonacci[^1] + fibonacci[^2];
                    fibonacci.Add(nextFib);
                }
                return fibonacci;
            }

            static int Zeckendorf(string codeword)
            {
                List<int> fibonacci = GenerateFibonacci(255); // Até 255, que é o valor máximo do ASCII
                int ascciValue = 0;

                for (int i = 0; i < codeword.Length - 1; i++) // Length-1 para remover o stop bit
                {
                    if (codeword[i] == '1')
                    {
                        ascciValue += fibonacci[i];
                    }
                }

                return ascciValue;
            }

            StringBuilder decodedString = new();
            string currentCodeword = "";

            foreach (char bit in encodedText)
            {
                currentCodeword += bit;
                // Busca o stop bit
                if (currentCodeword.EndsWith("11"))
                {
                    int asciiValue = Zeckendorf(currentCodeword);
                    decodedString.Append((char)asciiValue);
                    currentCodeword = "";
                }
            }

            return decodedString.ToString();
        }

        public static string DecodeGolomb(string encodedText, int k = 120)
        {
            static string Golomb(string encodedText, int k, StringBuilder decodedText, int index = 0)
            {
                if (index >= encodedText.Length)
                {
                    return decodedText.ToString();
                }

                // Prefixo
                int nPrefix = 0;
                while (encodedText[index] == '0')
                {
                    nPrefix++;
                    index++;
                }
                int prefix = nPrefix * k;

                // Pular StopBit
                index++;

                // Sufixo
                int bitSize = (int)Math.Ceiling(Math.Log2(k));
                string sufixBits = encodedText.Substring(index, bitSize);
                int sufix = Convert.ToInt32(sufixBits, 2);
                index += bitSize;

                decodedText.Append((char)(prefix + sufix));

                return Golomb(encodedText, k, decodedText, index);
            }

            return Golomb(encodedText, k, new StringBuilder());
        }

        public static string DecodeHuffman(string encodedText)
        {
            var huffmanDictionary = HuffmanTreeService.huffmanDictionary;
            var huffmanTable = huffmanDictionary.ToDictionary(pair => pair.Value, pair => pair.Key);

            StringBuilder decodedString = new();

            string currentCode = "";
            foreach (char bit in encodedText)
            {
                currentCode += bit;
                if (huffmanTable.TryGetValue(currentCode, out char value))
                {
                    decodedString.Append(value);
                    currentCode = "";
                }
            }

            if (currentCode != "")
                throw new ArgumentException("Input codificado inválido.");

            return decodedString.ToString();
        }

        #endregion

        public static string DecodeRRepeat(string encodedText, int i = 3)
        {
            var decodedString = new StringBuilder();
            var binaryResult = new StringBuilder();

            while (encodedText.Length > 0)
            {
                // Caso o texto codificado fuja dos padrões
                if (encodedText.Length < i)
                {
                    return string.Empty;
                }

                var stringBit = encodedText[..i];

                var mostRepeated = stringBit
                        .GroupBy(c => c)
                        .OrderByDescending(g => g.Count())
                        .First().Key;

                binaryResult.Append(mostRepeated);

                if (binaryResult.Length == 8)
                {
                    var asciiValue = Convert.ToInt32(binaryResult.ToString(), 2);
                    decodedString.Append((char)asciiValue);
                    binaryResult = new StringBuilder();
                }

                encodedText = encodedText[3..];
            }

            return decodedString.ToString();
        }
    }
}

using System.Text;

namespace UniCoder.Services
{
    public class DecrypterService
    {
        public DecrypterService() { }

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

            StringBuilder decodedString = new();
            int index = 0;

            while (index < encodedText.Length)
            {
                // Contar o número de zeros antes do primeiro '1'
                int zeroCount = 0;
                while (encodedText[index] == '0')
                {
                    zeroCount++;
                    index++;
                }

                // Incluir o '1' após os zeros
                index++;

                // Ler o próximo 'zeroCount + 1' bits para obter a parte binária
                string binaryPart = encodedText.Substring(index, zeroCount + 1);
                int decodedValue = Convert.ToInt32("1" + binaryPart, 2); // Adicionar '1' porque Elias-Gamma omite o primeiro '1'

                decodedString.Append((char)decodedValue);

                // Avançar no índice
                index += zeroCount + 1;
            }

            return decodedString.ToString();
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

                for (int i = 0; i < codeword.Length-1; i++) // Length-1 para remover o stop bit
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
    }
}

using System.Text;
using System.Text.Json;

namespace UniCoder.Services
{
    public class EncrypterService
    {
        public EncrypterService() { }

        public static string EncodeEliasGamma(string text)
        {
            static string EliasGamma2(int number)
            {
                StringBuilder encodedText = new();
                int bitSize = (int)Math.Floor(Math.Log(number, 2));

                // Prefixo
                for (int i = 0; i < bitSize; i++)
                {
                    encodedText.Append('0');
                }

                // StopBit
                encodedText.Append('1');

                // Sufix
                int sufixInt = number - (int)Math.Pow(2, bitSize);
                string sufixBinary = Convert.ToString(sufixInt, 2);
                sufixBinary = sufixBinary.PadLeft(bitSize, '0'); // Ajusta o sufixo para o tamanho correto de bits
                encodedText.Append(sufixBinary);

                return encodedText.ToString();
            }

            static string EliasGamma(int number)
            {
                if (number <= 0)
                    throw new ArgumentException("A número inteiro precisa ser positivo");

                string binary = Convert.ToString(number, 2);
                string prefix = new('0', binary.Length - 1);

                return prefix + binary;
            }

            StringBuilder encodedString = new();

            foreach (char c in text)
            {
                int asciiValue = (int)c;
                encodedString.Append(EliasGamma2(asciiValue));
            }

            return encodedString.ToString();
        }

        public static string EncodeFibonacciZeckendorf(string text)
        {
            List<int> GenerateFibonacci(int max)
            {
                List<int> fibonacci = [1, 2];
                while (fibonacci[^1] <= max)
                {
                    int nextFib = fibonacci[^1] + fibonacci[^2];
                    fibonacci.Add(nextFib);
                }
                fibonacci.Reverse(); // Ordem decrescente para facilitar Zeckendorf
                return fibonacci;
            }

            string Zeckendorf(int number)
            {
                List<int> fibonacci = GenerateFibonacci(number);
                StringBuilder codeword = new();

                foreach (int fib in fibonacci)
                {
                    if (fib <= number)
                    {
                        codeword.Insert(0, '1');
                        number -= fib;
                    }
                    else
                    {
                        if (codeword.Length > 0)
                        {
                            codeword.Insert(0, '0');
                        }
                    }
                }

                codeword.Append('1'); // StopBit
                return codeword.ToString();
            }

            StringBuilder encodedString = new();
            foreach (char c in text)
            {
                int asciiValue = (int)c;
                encodedString.Append(Zeckendorf(asciiValue));
            }

            return encodedString.ToString();
        }

        public static string EncodeGolomb(string text, int m = 120)
        {
            static string Golomb(int number, int k)
            {
                StringBuilder encodedText = new();
                int prefix = number / k;
                int sufix = number % k;

                // Prefixo 
                for (int i = 0; i < prefix; i++)
                {
                    encodedText.Append('0');
                }

                // StopBit
                encodedText.Append('1');

                // Sufix                
                string sufixBinary = Convert.ToString(sufix, 2);
                int bitSize = (int)Math.Ceiling(Math.Log2(k));
                sufixBinary = sufixBinary.PadLeft(bitSize, '0'); // Ajusta o sufixo para o tamanho correto de bits
                encodedText.Append(sufixBinary);

                return encodedText.ToString();
            }

            StringBuilder encodedString = new();

            foreach (char c in text)
            {
                int asciiValue = (int)c;
                encodedString.Append(Golomb(asciiValue, m));
            }

            return encodedString.ToString();
        }        

        public static string EncodeHuffman(string text)
        {
            var huffmanTable = HuffmanTreeService.CreateHuffmanTree(text);
            StringBuilder encodedString = new();

            var jsonString = JsonSerializer.Serialize(huffmanTable);
            Console.WriteLine(jsonString);

            foreach (char c in text)
            {
                if (huffmanTable.TryGetValue(c, out string? value))
                    encodedString.Append(value);
                else
                    throw new ArgumentException($"Character '{c}' não tem codificação Huffman definida.");
            }

            return encodedString.ToString();
        }
    }
}

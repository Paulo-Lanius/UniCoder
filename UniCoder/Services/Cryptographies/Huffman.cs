using System.Text.Json;
using System.Text;

namespace UniCoder.Services.Cryptographies
{
    public class Huffman : ICryptography
    {
        public string Encrypt(string input)
        {
            Console.WriteLine($"Criptografia Huffman");

            var huffmanTable = HuffmanTreeService.CreateHuffmanTree(input);
            StringBuilder EncryptdString = new();

            var jsonString = JsonSerializer.Serialize(huffmanTable);
            Console.WriteLine(jsonString);

            foreach (char c in input)
            {
                if (huffmanTable.TryGetValue(c, out string? value))
                    EncryptdString.Append(value);
                else
                    throw new ArgumentException($"Character '{c}' não tem codificação Huffman definida.");
            }

            return EncryptdString.ToString();
        }

        public string Decrypt(string input)
        {
            Console.WriteLine($"Descriptografia Huffman");

            var huffmanDictionary = HuffmanTreeService.huffmanDictionary;
            var huffmanTable = huffmanDictionary.ToDictionary(pair => pair.Value, pair => pair.Key);

            StringBuilder DecryptdString = new();

            string currentCode = "";
            foreach (char bit in input)
            {
                currentCode += bit;
                if (huffmanTable.TryGetValue(currentCode, out char value))
                {
                    DecryptdString.Append(value);
                    currentCode = "";
                }
            }

            if (currentCode != "")
                throw new ArgumentException("Input codificado inválido.");

            return DecryptdString.ToString();
        }
    }
}

using System.Text.Json;
using System.Text;

namespace UniCoder.Services.Encoders
{
    public class Huffman : IEncoder
    {
        public string Encode(string input)
        {
            Console.WriteLine($"Codificar Huffman");

            var huffmanTable = HuffmanTreeService.CreateHuffmanTree(input);
            StringBuilder EncodedString = new();

            var jsonString = JsonSerializer.Serialize(huffmanTable);
            Console.WriteLine(jsonString);

            foreach (char c in input)
            {
                if (huffmanTable.TryGetValue(c, out string? value))
                    EncodedString.Append(value);
                else
                    throw new ArgumentException($"Character '{c}' não tem codificação Huffman definida.");
            }

            return EncodedString.ToString();
        }

        public string Decode(string input)
        {
            Console.WriteLine($"Decodificar Huffman");

            var huffmanDictionary = HuffmanTreeService.huffmanDictionary;
            var huffmanTable = huffmanDictionary.ToDictionary(pair => pair.Value, pair => pair.Key);

            StringBuilder DecodedString = new();

            string currentCode = "";
            foreach (char bit in input)
            {
                currentCode += bit;
                if (huffmanTable.TryGetValue(currentCode, out char value))
                {
                    DecodedString.Append(value);
                    currentCode = "";
                }
            }

            if (currentCode != "")
                throw new ArgumentException("Input codificado inválido.");

            return DecodedString.ToString();
        }
    }
}

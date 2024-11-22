using System.Text;

namespace UniCoder.Services.Encoders
{
    public class RRepeat : IEncoder
    {
        public int i = 3;

        public string Encode(string input)
        {
            Console.WriteLine($"Codificar RRepeat");

            var bitsAdjust = 8 * i; // Para manter os valores na casa dos 8 bits sempre
            var EncodedString = new StringBuilder();
            var one = string.Empty.PadRight(i, '1');
            var zero = string.Empty.PadRight(i, '0');

            foreach (var c in input)
            {
                var asciiValue = (int)c;
                var binaryAscii = Convert.ToString(asciiValue, 2).PadLeft(bitsAdjust, '0');

                var result = binaryAscii.Replace("1", one);
                result = result.Replace("0", zero);

                EncodedString.Append(result);
            }

            return EncodedString.ToString();
        }

        public string Decode(string input)
        {
            Console.WriteLine($"Decodificar RRepeat");

            var DecodedString = new StringBuilder();
            var binaryResult = new StringBuilder();

            while (input.Length > 0)
            {
                // Caso o texto codificado fuja dos padrões
                if (input.Length < i)
                {
                    return string.Empty;
                }

                var stringBit = input[..i];

                var mostRepeated = stringBit
                        .GroupBy(c => c)
                        .OrderByDescending(g => g.Count())
                        .First().Key;

                binaryResult.Append(mostRepeated);

                if (binaryResult.Length == 8)
                {
                    var asciiValue = Convert.ToInt32(binaryResult.ToString(), 2);
                    DecodedString.Append((char)asciiValue);
                    binaryResult = new StringBuilder();
                }

                input = input[3..];
            }

            return DecodedString.ToString();
        }
    }
}

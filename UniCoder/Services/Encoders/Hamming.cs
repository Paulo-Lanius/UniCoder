using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace UniCoder.Services.Encoders
{
    public class Hamming : IEncoder
    {
        public string Encode(string input)
        {
            Console.WriteLine($"Codificar Hamming");

            var bitsAdjust = 8; // Para manter os valores na casa dos 8 bits sempre
            var EncodedString = new StringBuilder();

            foreach (var c in input)
            {
                var asciiValue = (int)c;
                var binaryAscii = Convert.ToString(asciiValue, 2).PadLeft(bitsAdjust, '0');

                var firstFour = binaryAscii[..4];
                var result = firstFour;
                result += CalculateT(firstFour, 0, 1, 2).ToString(); // t5
                result += CalculateT(firstFour, 1, 2, 3).ToString(); // t6
                result += CalculateT(firstFour, 0, 2, 3).ToString(); // t7

                var lastFour = binaryAscii.Substring(4, 4);
                result += lastFour;
                result += CalculateT(lastFour, 0, 1, 2).ToString(); // t5
                result += CalculateT(lastFour, 1, 2, 3).ToString(); // t6
                result += CalculateT(lastFour, 0, 2, 3).ToString(); // t7

                EncodedString.Append(result);
            }

            return EncodedString.ToString();
        }

        private int CalculateT(string bits, int s1, int s2, int s3)
        {
            return int.Parse(bits[s1].ToString())
                ^ int.Parse(bits[s2].ToString())
                ^ int.Parse(bits[s3].ToString());
        }

        public string Decode(string input)
        {
            Console.WriteLine($"Decodificar Hamming");

            var DecodedString = new StringBuilder();
            var binaryResult = new StringBuilder();

            while (input.Length > 0)
            {
                // Caso o texto codificado fuja dos padrões
                if (input.Length <= 0)
                {
                    return string.Empty;
                }

                var firstFour = input[..7];
                var ff_t5 = CalculateT(firstFour, 0, 1, 2).ToString() == firstFour[4].ToString();
                var ff_t6 = CalculateT(firstFour, 1, 2, 3).ToString() == firstFour[5].ToString();
                var ff_t7 = CalculateT(firstFour, 0, 2, 3).ToString() == firstFour[6].ToString();
                binaryResult.Append(TratamentoDeErro(firstFour, !ff_t5, !ff_t6, !ff_t7));

                var lastFour = input.Substring(7, 7);

                var lf_t5 = CalculateT(lastFour, 0, 1, 2).ToString() == lastFour[4].ToString();
                var lf_t6 = CalculateT(lastFour, 1, 2, 3).ToString() == lastFour[5].ToString();
                var lf_t7 = CalculateT(lastFour, 0, 2, 3).ToString() == lastFour[6].ToString();
                binaryResult.Append(TratamentoDeErro(lastFour, !lf_t5, !lf_t6, !lf_t7));

                var asciiValue = Convert.ToInt32(binaryResult.ToString(), 2);
                DecodedString.Append((char)asciiValue);
                binaryResult = new StringBuilder();

                input = input[14..];
            }

            return DecodedString.ToString();
        }

        private static string TratamentoDeErro(string bits, bool erroT5, bool erroT6, bool erroT7)
        {
            if (erroT5 && erroT6 && erroT7)
            {
                return "ERRO DUPLO";
            }

            if (erroT5 && erroT6) // Erro simples S2
            {
                var s2 = bits[1] == '1' ? '0' : '1';
                return $"{bits[0]}{s2}{bits[2]}{bits[3]}";
            }

            if (erroT5 && erroT7) // Erro simples S1
            {
                var s1 = bits[0] == '1' ? '0' : '1';
                return $"{s1}{bits[1]}{bits[2]}{bits[3]}";
            }

            if (erroT6 && erroT7) // Erro simples S4
            {
                var s4 = bits[3] == '1' ? '0' : '1';
                return $"{bits[0]}{bits[1]}{bits[2]}{s4}";
            }

            return bits[..4];
        }
    }
}

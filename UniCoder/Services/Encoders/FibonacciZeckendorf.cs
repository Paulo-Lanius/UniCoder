using System.Text;

namespace UniCoder.Services.Encoders
{
    public class FibonacciZeckendorf : IEncoder
    {
        public string Encode(string input)
        {
            Console.WriteLine($"Codificar FibonacciZeckendorf");

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

            StringBuilder EncodedString = new();
            foreach (char c in input)
            {
                int asciiValue = (int)c;
                EncodedString.Append(Zeckendorf(asciiValue));
            }

            return EncodedString.ToString();
        }

        public string Decode(string input)
        {
            Console.WriteLine($"Decodificar FibonacciZeckendorf");

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

            StringBuilder DecodedString = new();
            string currentCodeword = "";

            foreach (char bit in input)
            {
                currentCodeword += bit;
                // Busca o stop bit
                if (currentCodeword.EndsWith("11"))
                {
                    int asciiValue = Zeckendorf(currentCodeword);
                    DecodedString.Append((char)asciiValue);
                    currentCodeword = "";
                }
            }

            return DecodedString.ToString();
        }
    }
}

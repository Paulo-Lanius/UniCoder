using UniCoder.Enums;
using UniCoder.Services.Cryptographies;

namespace UniCoder.Services
{
    public class CryptographyFactory
    {
        private static readonly Dictionary<TypeAlgorithm, Type> _cryptographyMap = new()
        {
            { TypeAlgorithm.EliasGamma, typeof(EliasGamma) },
            { TypeAlgorithm.FibonacciZeckendorf, typeof(FibonacciZeckendorf) },
            { TypeAlgorithm.Golomb, typeof(Golomb) },
            { TypeAlgorithm.Huffman, typeof(Huffman) },
            { TypeAlgorithm.RRepeat, typeof(RRepeat) },
            { TypeAlgorithm.Hamming, typeof(Hamming) }
        };

        public static ICryptography GetCryptography(TypeAlgorithm type)
        {
            if (_cryptographyMap.TryGetValue(type, out var cryptoClassType))
            {
                return (ICryptography)Activator.CreateInstance(cryptoClassType);
            }
            throw new ArgumentException("Unsupported cryptography type.");
        }
    }
}
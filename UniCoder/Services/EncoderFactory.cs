using UniCoder.Enums;
using UniCoder.Services.Encoders;

namespace UniCoder.Services
{
    public class EncoderFactory
    {
        private static readonly Dictionary<TypeAlgorithm, Type> _encoderMap = new()
        {
            { TypeAlgorithm.EliasGamma, typeof(EliasGamma) },
            { TypeAlgorithm.FibonacciZeckendorf, typeof(FibonacciZeckendorf) },
            { TypeAlgorithm.Golomb, typeof(Golomb) },
            { TypeAlgorithm.Huffman, typeof(Huffman) },
            { TypeAlgorithm.RRepeat, typeof(RRepeat) },
            { TypeAlgorithm.Hamming, typeof(Hamming) }
        };

        public static IEncoder GetEncode(TypeAlgorithm type)
        {
            if (_encoderMap.TryGetValue(type, out var encodeClassType))
            {
                return (IEncoder)Activator.CreateInstance(encodeClassType);
            }
            throw new ArgumentException("Unsupported Encode type.");
        }
    }
}
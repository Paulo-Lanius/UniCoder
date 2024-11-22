using UniCoder.Enums;

namespace UniCoder.Services
{
    public class Executor
    {
        public static string Execute(TypeAlgorithm algorithmType, TypeAction actionType, string input)
        {
            var encoder = EncoderFactory.GetEncode(algorithmType);

            return actionType switch
            {
                TypeAction.Encode => encoder.Encode(input),
                TypeAction.Decode => encoder.Decode(input),
                _ => throw new ArgumentException("Unsupported operation type.")
            };
        }
    }
}

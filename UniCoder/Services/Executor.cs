using UniCoder.Enums;

namespace UniCoder.Services
{
    public class Executor
    {
        public static string Execute(TypeAlgorithm algorithmType, TypeAction actionType, string input)
        {
            var cryptography = CryptographyFactory.GetCryptography(algorithmType);

            return actionType switch
            {
                TypeAction.Encrypt => cryptography.Encrypt(input),
                TypeAction.Decrypt => cryptography.Decrypt(input),
                _ => throw new ArgumentException("Unsupported operation type.")
            };
        }
    }
}

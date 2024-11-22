using System.ComponentModel;

namespace UniCoder.Enums
{
    public enum TypeAlgorithm
    {
        [Description("Elias-Gamma")]
        EliasGamma = 1,

        [Description("Fibonacci-Zeckendorf")]
        FibonacciZeckendorf = 2,

        [Description("Golomb")]
        Golomb = 3,

        [Description("Huffman")]
        Huffman = 4,

        [Description("R-Repeat")]
        RRepeat = 5,

        [Description("Hamming")]
        Hamming = 6
    }

    public enum TypeAction
    {
        [Description("Codificar")]
        Encode = 1,

        [Description("Decodificar")]
        Decode = 2
    }


    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}

namespace UniCoder.Services
{
    public interface IEncoder
    {
        string Encode(string input);
        string Decode(string input);
    }
}

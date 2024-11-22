namespace UniCoder.Services
{
    public interface ICryptography
    {
        string Encrypt(string input);
        string Decrypt(string input);
    }
}

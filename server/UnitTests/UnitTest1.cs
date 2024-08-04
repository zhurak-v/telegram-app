using Shared;

namespace UnitTests;

public class UnitTest1
{
    [Fact]
    public void EncryptorTest()
    {
        var data = "123456789";
        var key = "qwerty";

        var encrypted = Encryptor.Encrypt(data, key);
        var decrypted = Encryptor.Decrypt(encrypted, key);

        Assert.Equal(data, decrypted);
    }
}
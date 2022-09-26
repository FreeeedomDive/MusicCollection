using System.Security.Cryptography;
using System.Text;

namespace MusicCollection.BusinessLogic.Services.UsersService;

public class CryptoService
{
    public static string Encrypt(string data)
    {
        var md5 = new MD5CryptoServiceProvider();
        var hashSum = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
        var hash = BitConverter.ToString(hashSum).Replace("-", string.Empty);
        return hash;
    }
}
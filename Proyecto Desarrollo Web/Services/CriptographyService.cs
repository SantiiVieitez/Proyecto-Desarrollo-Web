using System.Security.Cryptography;
using System.Text;

namespace Proyecto_Desarrollo_Web.Services
{
    public class CriptographyService
    {
        public static string GetSHA256(string value)
        {
            using var sha256 = SHA256.Create(); // ✅ uso recomendado
            var encoding = Encoding.ASCII;
            var stream = sha256.ComputeHash(encoding.GetBytes(value));

            var sb = new StringBuilder();
            foreach (var b in stream)
                sb.AppendFormat("{0:x2}", b);

            return sb.ToString();
        }

        public static string GenerarSalt(int tamaño = 16)
        {
            byte[] saltBytes = new byte[tamaño];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
    }
}

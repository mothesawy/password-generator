using System.Globalization;
using System.Numerics;
using DaanV2.UUID;

namespace PWGEN;

class Generator
{
    public static string GeneratePassword(InputData inputData)
    {
        inputData.length = Math.Min(32, Math.Max(8, inputData.length));
        var hash = GenerateMD5Hash($"{inputData.secretKey}", $"{inputData.userName}@{inputData.appName}");
        var uuid = GenerateUUID(hash);
        var UUIDToInt = TextHexToInt(uuid, inputData.length);

        var rand = new Random(UUIDToInt);

        var LC_ALPHA = Utilities.TextToShuffledArray("abcdefghijklmnopqrstuvwxyz", rand);
        var UC_ALPHA = Utilities.TextToShuffledArray("ABCDEFGHIJKLMNOPQRSTUVWXYZ", rand);
        var SYMPOLS = Utilities.TextToShuffledArray("@$!%*#?&><)(^-", rand);
        var DIGITS = Utilities.TextToShuffledArray("0123456789", rand);
        var ALL_ALPHA = Utilities.TextToShuffledArray("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@$!%*#?&><)(^-0123456789", rand);

        var password = string.Empty;

        for (var i = 0; i < inputData.length; i++)
        {
            switch (i % 5)
            {
                case 0:
                    password += LC_ALPHA[i % LC_ALPHA.Length];
                    break;
                case 1:
                    password += UC_ALPHA[i % UC_ALPHA.Length];
                    break;
                case 2:
                    password += SYMPOLS[i % SYMPOLS.Length];
                    break;
                case 3:
                    password += DIGITS[i % DIGITS.Length];
                    break;
                case 4:
                    password += ALL_ALPHA[i % ALL_ALPHA.Length];
                    break;
            }
        }

        return password;
    }
    static string GenerateMD5Hash(string secretKey, string name)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            var secretKeyBytes = System.Text.Encoding.ASCII.GetBytes(secretKey);
            var nameBytes = System.Text.Encoding.ASCII.GetBytes(name);

            var secretKeyHash = Convert.ToHexString(md5.ComputeHash(secretKeyBytes));
            var nameHash = Convert.ToHexString(md5.ComputeHash(nameBytes));

            var masterBytes = System.Text.Encoding.ASCII.GetBytes($"{secretKeyHash}{nameHash}");
            var masterHash = md5.ComputeHash(masterBytes);

            return Convert.ToHexString(masterHash);
        }
    }

    static string GenerateUUID(string hash)
    {
        var generatedUUID = UUIDFactory.CreateUUID(5, 1, hash).ToString().Replace("-", "");
        return generatedUUID;
    }

    static int TextHexToInt(string hexText, int length)
    {
        var bigNum = BigInteger.Parse($"0{hexText}", NumberStyles.AllowHexSpecifier);
        var rand = new Random(length);
        var numToStringArray = bigNum.ToString().ToCharArray().Select(c => c.ToString());
        var numToString = string.Join("", numToStringArray.OrderBy(_ => rand.Next()).ToArray()).Substring(0, 9);
        return int.Parse(numToString);
    }
}
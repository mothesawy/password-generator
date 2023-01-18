using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace PWGEN;

class Generator
{
    public static string GeneratePassword(InputData inputData)
    {
        inputData.length = Math.Min(32, Math.Max(8, inputData.length));

        var nameHash = GenerateMD5Hash($"{inputData.userName}@{inputData.appName}");
        var secretKeyHash = GenerateMD5Hash($"{inputData.secretKey}*{inputData.length}");
        var shaHash = GenerateSHA256Hash($"{nameHash}+{secretKeyHash}");

        var seed = HexTextToInt(shaHash, inputData.length);

        var rand = new Random(seed);

        var LC_ALPHA = Utilities.TextToShuffledArray("abcdefghijklmnopqrstuvwxyz", rand);
        var UC_ALPHA = Utilities.TextToShuffledArray("ABCDEFGHIJKLMNOPQRSTUVWXYZ", rand);
        var SYMBOLS = Utilities.TextToShuffledArray("@$!%*#?&><)(^-", rand);
        var DIGITS = Utilities.TextToShuffledArray("0123456789", rand);
        var ALL_ALPHA = Utilities.TextToShuffledArray("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@$!%*#?&><)(^-0123456789", rand);
        var choices = new int[] { 0, 1, 2, 3, 4 }.OrderBy(_ => rand.Next()).ToArray();

        var password = string.Empty;

        for (var i = 0; i < inputData.length; i++)
        {
            if (i % 5 == choices[0]) password += LC_ALPHA[i % LC_ALPHA.Length];
            else if (i % 5 == choices[1]) password += UC_ALPHA[i % UC_ALPHA.Length];
            else if (i % 5 == choices[2]) password += SYMBOLS[i % SYMBOLS.Length];
            else if (i % 5 == choices[3]) password += DIGITS[i % DIGITS.Length];
            else if (i % 5 == choices[4]) password += ALL_ALPHA[i % ALL_ALPHA.Length];
        }

        return password;
    }

    static string GenerateMD5Hash(string text)
    {
        var textBytes = Encoding.ASCII.GetBytes(text);
        var hash = Convert.ToHexString(MD5.Create().ComputeHash(textBytes));
        return hash;
    }

    static string GenerateSHA256Hash(string text)
    {
        var textBytes = Encoding.ASCII.GetBytes(text);
        var hash = Convert.ToHexString(SHA256.Create().ComputeHash(textBytes));
        return hash;
    }

    static int HexTextToInt(string hexText, int length)
    {
        var rand = new Random(length);
        var shuffledTextArray = new string[8];
        Array.Copy(Utilities.TextToShuffledArray(hexText, rand), shuffledTextArray, 8);
        var shuffledText = string.Join("", shuffledTextArray);
        return int.Parse(shuffledText.Substring(0, 8), NumberStyles.AllowHexSpecifier);
    }
}
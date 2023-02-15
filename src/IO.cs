
using TextCopy;

namespace PwGen;

class IO
{
  public static InputData HandleInput()
  {
    var ioData = new InputData();
    Utilities.ColorCli("Enter your username: ", ConsoleColor.White);
    ioData.userName = Console.ReadLine()?.Trim();

    Utilities.ColorCli("Enter the app that you want the password for: ", ConsoleColor.White);
    ioData.appName = Console.ReadLine()?.Trim();

    Utilities.ColorCli("Enter your secret key: ", ConsoleColor.White);
    ioData.secretKey = HandleSecretKeyInput().Trim();
    Console.WriteLine();

    while (true)
    {
      Utilities.ColorCli("Enter the preferred length for password (min=8, max=32): ", ConsoleColor.White);
      try
      {
        if (!int.TryParse(Console.ReadLine(), out ioData.length)) throw new FormatException();
      }
      catch (FormatException)
      {
        Utilities.ColorCli("ERROR: Invalid positive integer.\n", ConsoleColor.Red);
        continue;
      }
      break;
    }

    while (true)
    {
      Utilities.ColorCli("Password will be copied to the clipboard.\n", ConsoleColor.White);
      Utilities.ColorCli("Do you want to also print it? (y: yes, n: no): ", ConsoleColor.White);
      var answer = Console.ReadLine()?.ToLower().Trim();
      if (answer == "y")
      {
        ioData.willPrint = true;
        break;
      }
      else if (answer == "n")
      {
        ioData.willPrint = false;
        break;
      }
      else
      {
        Utilities.ColorCli("ERROR: Invalid answer.\n", ConsoleColor.Red);
      }
    }

    return ioData;
  }

  public static void HandleOutput(InputData inputData, string password)
  {
    if (inputData.willPrint)
    {
      Console.WriteLine($"Your {inputData.appName} password is:");
      Utilities.ColorCli($"{password}\n", ConsoleColor.Green);
    }
    else
    {
      Utilities.ColorCli($"Your {inputData.appName} password has been generated.\n", ConsoleColor.Green);
    }

    ClipboardService.SetText(password);
    Console.WriteLine();
    Utilities.ColorCli("Password copied to the clipboard!\n", ConsoleColor.Cyan);

    Console.WriteLine("Press any key to exit..");
    Console.ReadKey(intercept: true);
    Environment.Exit(0);
  }

  public static string HandleArgsInput()
  {
    Utilities.ColorCli("Enter your secret key: ", ConsoleColor.White);
    return HandleSecretKeyInput();
  }

  public static void HandleArgsOutput()
  {
    Console.WriteLine("");
    Utilities.ColorCli("Passwords successfully generated to pwgen_passwords.txt\n", ConsoleColor.Cyan);
  }

  public static string HandleSecretKeyInput()
  {
    var secretKey = string.Empty;
    ConsoleKey key;
    do
    {
      var keyInfo = Console.ReadKey(intercept: true);
      key = keyInfo.Key;

      if (key == ConsoleKey.Backspace && secretKey.Length > 0)
      {
        Console.Write("\b \b");
        secretKey = secretKey[0..^1];
      }
      else if (!char.IsControl(keyInfo.KeyChar))
      {
        Console.Write("*");
        secretKey += keyInfo.KeyChar;
      }
    } while (key != ConsoleKey.Enter);

    return secretKey;
  }
}

class InputData
{
  public string? userName = string.Empty;
  public string? appName = string.Empty;
  public string? secretKey = string.Empty;
  public int length;
  public bool willPrint;
  public string? password = string.Empty;
}

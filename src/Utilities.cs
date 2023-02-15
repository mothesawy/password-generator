namespace PwGen;

static class Utilities
{
  public static string[] TextToShuffledArray(string text, Random rand)
  {
    var textArray = text.ToCharArray().Select(c => c.ToString()).ToArray();
    return textArray.OrderBy(_ => rand.Next()).ToArray();
  }

  public static InputData[] DumpFileToArray(string filePath, string secretKey, int length)
  {
    var contents = File.ReadAllLines(filePath);
    var inputData = new List<InputData>();
    foreach (var item in contents)
    {
      var data = item.Split("@");
      if (IsDataValid(data))
      {
        var dataItem = new InputData();
        dataItem.userName = data[0].Trim();
        dataItem.appName = data[1].Trim();
        dataItem.secretKey = secretKey;
        dataItem.length = length;
        dataItem.willPrint = false;
        inputData.Add(dataItem);
      }
    }
    return inputData.ToArray();
  }

  public static async Task DumpDataToFile(InputData[] inputDataArr, string parentDir)
  {
    var dataToList = new List<string>();
    foreach (var item in inputDataArr)
    {
      dataToList.Add($"{item.userName}@{item.appName}");
      dataToList.Add($"{item.password}");
      dataToList.Add("");
    }

    await File.WriteAllLinesAsync(Path.Join(parentDir, "pwgen_passwords.txt"), dataToList.ToArray());
  }

  static bool IsDataValid(string[] data)
  {
    if (data.Length != 2) return false;
    if (data[0] == "" || data[1] == "") return false;
    if (data[0].Replace(" ", "").Length == 0 || data[1].Replace(" ", "").Length == 0) return false;
    return true;
  }

  public static void ColorCli(string message, ConsoleColor color)
  {
    Console.ForegroundColor = color;
    Console.Write(message);
    Console.ForegroundColor = ConsoleColor.Gray;
  }
}

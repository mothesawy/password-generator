using CommandLine;
using CommandLine.Text;

namespace PwGen;

class Program
{
  static void Main(string[] args)
  {
    var parser = new CommandLine.Parser(with => with.HelpWriter = null);
    var parserResult = parser.ParseArguments<PasswordOptions>(args);
    parserResult
      .WithParsed<PasswordOptions>(opts =>
      {
        if (opts.filePath != "")
        {
          if (!File.Exists(opts.filePath))
          {
            Console.WriteLine("ERROR: File does not exist.");
            Environment.Exit(1);
          }

          var secretKey = IO.HandleArgsInput();
          var inputDataArr = Utilities.DumpFileToArray(opts.filePath, secretKey, opts.length);

          for (var i = 0; i < inputDataArr.Length; i++)
          {
            inputDataArr[i].password = Generator.GeneratePassword(inputDataArr[i]);
          }

          var parentDir = Path.GetDirectoryName(opts.filePath);
          if (parentDir is not null)
          {
            Utilities.DumpDataToFile(inputDataArr, parentDir).Wait();
            IO.HandleArgsOutput();
          }
        }
        else
        {
          var inputData = IO.HandleInput();
          var password = Generator.GeneratePassword(inputData);
          IO.HandleOutput(inputData, password);
        }
      })
      .WithNotParsed(errs => DisplayHelp(parserResult));
  }

  static void DisplayHelp<T>(ParserResult<T> result)
  {
    var helpText = HelpText.AutoBuild(result, h =>
    {
      h.AdditionalNewLineAfterOption = false;
      h.Heading = "pw-gen -- A utility to generate secure, deterministic, random passwords";
      h.Copyright = "";
      return HelpText.DefaultParsingErrorsHandler(result, h);
    }, e => e);
    Console.WriteLine(helpText);
  }
}

class PasswordOptions
{
  [Option('f', "file-path", Required = false, HelpText = "Path of the file containing your accounts.")]
  public string filePath { get; set; } = "";

  [Option('l', "length", Required = false, Default = 16, HelpText = "The length of the password.")]
  public int length { get; set; }
}
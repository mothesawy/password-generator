using CommandLine;

namespace PWGEN;

class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<FilePathOption>(args).WithParsed<FilePathOption>(opts =>
            {
                if (opts.filePath != "")
                {
                    if (!File.Exists(opts.filePath))
                    {
                        Console.WriteLine("ERROR: File does not exist.");
                        Environment.Exit(1);
                    }

                    var secretKey = IO.HandleArgsInput();
                    var inputDataArr = Utilities.DumpFileToData(opts.filePath, secretKey, opts.length);

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
            });
    }

}

class FilePathOption
{
    [Option('f', "file-path", Required = false, HelpText = "Path of the file containing your accounts.")]
    public string filePath { get; set; } = "";

    [Option('l', "length", Required = false, Default = 16, HelpText = "The length of the password.")]
    public int length { get; set; }
}
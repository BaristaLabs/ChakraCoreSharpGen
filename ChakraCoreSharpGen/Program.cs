namespace ChakraCoreSharpGen
{
    using SharpGen;
    using SharpGen.Config;
    using SharpGen.Logging;
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            var consoleLogger = new ConsoleLogger();
            var logger = new Logger(consoleLogger, null);

            ConfigFile config = new ConfigFile()
            {
                Id = "ChakraCore",
                Assembly = "Test123",
                Namespace = "Test123",
                //IncludeDirs =
                //{
                //    new IncludeDirRule
                //    {
                //        Path = @"C:\Projects\chakracore\lib\Jsrt\"
                //    }
                //},
                Includes =
                {
                    new IncludeRule
                    {
                        Attach = true,
                        File = @"C:\Projects\chakracore\lib\Jsrt\ChakraCore.h",
                        Namespace = "BaristaLabs.ChakraSharp",
                    }
                }
            };

            var outputDi = Directory.CreateDirectory("../output");
            var chakraSharpDir = Directory.CreateDirectory("../output/ChakraSharp");
            var intermediateOutputDir = Directory.CreateDirectory("../output/temp");

            var castXmlPath = Path.GetFullPath("../lib/CastXML/bin/castxml.exe");
            if (!File.Exists(castXmlPath))
            {
                throw new InvalidOperationException("Unable to locate CastXml at " + castXmlPath);
            }

            var codeGenApp = new CodeGenApp(logger)
            {
                GlobalNamespace = new GlobalNamespaceProvider("SharpGen.Runtime"),
                CastXmlExecutablePath = castXmlPath,
                Config = config,
                OutputDirectory = chakraSharpDir.FullName,
                IntermediateOutputPath = intermediateOutputDir.FullName,
            };

            codeGenApp.Init();
            codeGenApp.Run();
        }
    }
}

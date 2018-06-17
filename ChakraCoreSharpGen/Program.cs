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
                Assembly = "BaristaLabs.ChakraCore",
                Namespace = "BaristaLabs.ChakraCore",
                IncludeDirs =
                {
                    new IncludeDirRule
                    {
                        Path = @"C:\Projects\chakracore\lib\Jsrt\"
                    }
                },
                Includes =
                {
                    new IncludeRule
                    {
                        Attach = true,
                        File = @"C:\Projects\chakracore\lib\Jsrt\ChakraCommon.h",
                        Namespace = "ChakraCommon",
                    },
                    new IncludeRule
                    {
                        Attach = true,
                        File = @"C:\Projects\chakracore\lib\Jsrt\ChakraCore.h",
                        Namespace = "ChakraCore",
                    },
                },
                Extension =
                {
                    new CreateExtensionRule
                    {
                        NewClass = $"BaristaLabs.ChakraCore.Functions",
                    }
                },
                Bindings =
                {
                    //Primitive bindings
                    new BindRule("void", "System.Void"),
                    new BindRule("int", "System.Int32"),
                    new BindRule("unsigned int", "System.Int32"),
                    new BindRule("short", "System.Int16"),
                    new BindRule("unsigned short", "System.Int16"),
                    new BindRule("unsigned char", "System.Byte"),
                    new BindRule("longlong", "System.Int64"),
                    new BindRule("unsigned longlong", "System.Int64"),
                    new BindRule("float", "System.Single"),
                    new BindRule("double", "System.Double"),
                    new BindRule("bool", "System.Boolean"),

                    new BindRule("__function__stdcall", "SharpDX.FunctionCallback"),
                },
                Mappings =
                {
                    new MappingRule
                    {
                        Function = "Js.*",
                        FunctionDllName = "\"libChakraCore\"",
                    }
                }
            };

            var outputDi = Directory.CreateDirectory("../output");
            var chakraSharpDir = Directory.CreateDirectory("../output/ChakraSharp");
            var intermediateOutputDir = Directory.CreateDirectory("../output/temp");

            var castXmlPath = Path.GetFullPath("../../../../lib/CastXML/bin/castxml.exe");
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

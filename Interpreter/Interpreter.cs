using Interpreter.Expressions;
using Interpreter.Lexer;
using Interpreter.Optimizer;
using Interpreter.Parser;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Interpreter
{
    class Interpreter : IDisposable
    {
        private readonly string _fileExtension = ".bmac";
        private readonly ILexer _lexer;
        private readonly IParser _parser;
        private readonly IOptimizer _optimizer;
        private Context _context = new Context("Interpreter");

        public Interpreter(ILexer lexer, IParser parser, IOptimizer optimizer = null)
        {
            _lexer = lexer;
            _parser = parser;
            _optimizer = optimizer;
        }

        private IExpression GetProgram(string code)
        {
            var tokens = _lexer.Lex(code);

            var programTree = _parser.Parse(tokens);

            var oldForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("A S T:\n" + programTree.ToConsoleTree());

            if (_optimizer != null)
            {
                programTree.AcceptOptimizer(new Optimizer.Optimizer());

                Console.WriteLine("Optimized AST:\n" + programTree.ToConsoleTree());
            }

            Console.ForegroundColor = oldForegroundColor;

            return programTree;
        }

        public void Run()
        {
            OutputInterpreterInfo();
            while (true)
            {
                Console.Write(">>>");
                var command = Console.ReadLine();
                if (command == null)
                {
                    continue;
                }
                else if (command == "help")
                {
                    OutputBasicCommands();
                    continue;
                }
                else if (command == "q")
                {
                    break;
                }

                try
                {                 
                    if (command.Contains(_fileExtension) && File.Exists(command))
                    {
                        var code = File.ReadAllText(command);
                        var program = GetProgram(code);
                        var indexOfFileName = command.LastIndexOf("/", StringComparison.Ordinal);
                        using (var context =
                            new Context(command.Substring(indexOfFileName == -1 ? 0 : indexOfFileName)))
                        {
                            program.Run(context);
                        }
                    }
                    else
                    {
                        if (!command.EndsWith(";")) command += ";";
                        var program = GetProgram(command);
                        program.Run(_context);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void OutputInterpreterInfo()
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var nameOfAssembly = currentAssembly?.GetName();
            var companyAttribute = currentAssembly?.GetCustomAttributes(typeof(AssemblyCompanyAttribute)).FirstOrDefault() as AssemblyCompanyAttribute;
            var copyrightAttribute =
                currentAssembly?.GetCustomAttributes(typeof(AssemblyCopyrightAttribute)).FirstOrDefault() as
                    AssemblyCopyrightAttribute;
            var descriptionAttribute =
                currentAssembly?.GetCustomAttributes(typeof(AssemblyDescriptionAttribute)).FirstOrDefault() as
                    AssemblyDescriptionAttribute;

            var nameOfProgram = nameOfAssembly?.Name ?? "Interpreter";
            var versionOfProgram = nameOfAssembly?.Version.ToString() ?? "0.0.0.0";
            var companyName = companyAttribute?.Company ?? "Student Of NULP";
            var copyright = copyrightAttribute?.Copyright ?? "No copyright";
            var description = descriptionAttribute?.Description ?? "";

            Console.WriteLine(nameOfProgram + " v" + versionOfProgram
                              + "\t" + companyName
                              + "\t" + copyright);
            Console.WriteLine();
            Console.WriteLine(description);
            Console.WriteLine();
        }

        private void OutputBasicCommands()
        {
            Console.WriteLine("open <link>\tOpens site with link given as a parameter. Absolute link must starts with protocol name(like 'https://')");
            Console.WriteLine();
            Console.WriteLine("click <target>\tClick on target given as a parameter. Target's pattern 'attribute=value'(like 'name=q')");
            Console.WriteLine();
            Console.WriteLine("type <target> <value>\tType value in the target. Target's pattern is similar to patter of click target, value is a string");
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

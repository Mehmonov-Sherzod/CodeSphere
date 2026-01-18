using CodeSphere.Application.Models;
using CodeSphere.Application.Models.JangohModels;
using CodeSphere.DataAccess.Persistence;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace CodeSphere.Application.Service.Impl
{
    public class JangohService : IJangohService
    {
        private readonly AppDbContext _context;
        public JangohService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<Verdrict>> SubmitCode(SubmitCodes codes)
        {
            var result = await _context.DsaQuestions.Where(x => x.Id == codes.Id).Include(x => x.TestCases).FirstOrDefaultAsync();
            if (result == null) 
            {
                return ApiResult<Verdrict>.Failure(new List<string> { "Failed" });
            }
            foreach(var test in result.TestCases)
            {

                var output = Run(Compile(codes.Code), test.Input);
                if (output != test.ExpectedOutput.Trim())
                {
                    return ApiResult<Verdrict>.Success(Verdrict.WrongAnswer);
                }
            }
            return ApiResult<Verdrict>.Success(Verdrict.Accepted);
        }

        private static Assembly Compile (string code)
        {
            var sytaxTree = CSharpSyntaxTree.ParseText (code);
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
            };
            var compilation = CSharpCompilation.Create(
                assemblyName: "UserSubmission",
                syntaxTrees: new[] { sytaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication)
                );
            using var ms = new MemoryStream ();
            var result = compilation.Emit (ms);
            if (!result.Success)
            {
                var errors = result.Diagnostics
               .Where(d => d.Severity == DiagnosticSeverity.Error)
               .Select(d => d.ToString());

                throw new Exception(string.Join("\n", errors));
            }
            ms.Seek(0, SeekOrigin.Begin);
            return Assembly.Load(ms.ToArray());
        }
        private static string Run(Assembly assembly , string input)
        {
            var originalOut = Console.Out;
            var originalIn = Console.In;
            try
            {
                using var sw = new StringWriter();
                using var sr = new StringReader(input);
                Console.SetOut(sw);
                Console.SetIn(sr);
                var entry = assembly.EntryPoint;
                entry.Invoke(null, entry.GetParameters().Length == 0
                    ? null
                    : new object[] { Array.Empty<string>() });

                return sw.ToString().Trim();
            }
            finally
            {
                Console.SetOut(originalOut);
                Console.SetIn(originalIn);
            }
        }
        
    }
}

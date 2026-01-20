using CodeSphere.Application.Models;
using CodeSphere.Application.Models.JangohModels;
using CodeSphere.DataAccess.Persistence;
using CodeSphere.Domain.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

            var result = await _context.DsaQuestions.Where(x => x.Id == codes.Id)
                                                    .Include(x => x.TestCases)    
                                                    .Include(x=>x.Definition)
                                                            .ThenInclude(d=>d.Parameters)
                                                    .FirstOrDefaultAsync();

            if (result == null) 
            {
                return ApiResult<Verdrict>.Failure(new List<string> { "Failed" });
            }
          //  var fullCode = BuildWrapper(result.Definition.ClassName, MethodSignature(result.Definition), codes.Code);
            var compileResult = Compile(codes.Code);
            var method = compileResult.GetTypes().First().GetMethod(result.Definition.MethodName);
            var parameters = method.GetParameters();
            object[] args = new object[parameters.Length];

            foreach (var testCase in result.TestCases)
            {
                var inputStrings = JsonConvert.DeserializeObject<List<string>>(testCase.Input);
                for (int i = 0; i < parameters.Length; i++)
                {
                    var paramType = parameters[i].ParameterType;
                    args[i] = Convert.ChangeType(inputStrings[i], paramType);
                }
                var output = Run(compileResult, result.Definition.MethodName, args);
                if (output == null)
                {
                    return ApiResult<Verdrict>.Success(Verdrict.WrongAnswer);
                }
                if (output.ToString() != testCase.ExpectedOutput)
                {
                    return ApiResult<Verdrict>.Success(Verdrict.WrongAnswer);
                }
            

            }
            return ApiResult<Verdrict>.Success(Verdrict.Accepted);
        }

        private static Assembly Compile (string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var references = new[]
            {
        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
    };

            var compilation = CSharpCompilation.Create(
                assemblyName: "UserSubmission",
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary) // ✅ library
            );

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

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
        private static object Run(Assembly assembly , string methodName , object[] inputs)
        {
            var type = assembly.GetTypes().First();
            var method = type.GetMethod(methodName);
            if(method==null)
            {
                throw new Exception("Method not found");
            }
            var instance  = Activator.CreateInstance(type);
            return method.Invoke(instance, inputs);
        }
        //private static string BuildWrapper(string className, string methodWithSignature, string userCode)
        //{
        //                    return $@"
        //                    using System;
        //                    using System.Collections.Generic;

        //                    public class {className}
        //                    {{
        //                        {methodWithSignature}
        //                        {{
        //                            {userCode}
        //                        }}
        //                    }}
        //                    ";
        //}
        //private static string MethodSignature(DsaQuestionDefinition definition)
        //{
        //    var parameters = string.Join(",", definition.Parameters.Select(p => $"{p.Type} {p.Name}"));
        //    return $"public {definition.ReturnType} {definition.MethodName}({parameters})";
        //}
    }
}

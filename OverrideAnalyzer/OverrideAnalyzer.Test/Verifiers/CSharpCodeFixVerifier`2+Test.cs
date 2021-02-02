using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace OverrideAnalyzer.Test
{
    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        public class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, MSTestVerifier>
        {
            public Test()
            {
                SolutionTransforms.Add(SolutionTransform.Transform);
                // SolutionTransforms.Add((solution, projectId) =>
                // {
                //     var compilationOptions = solution.GetProject(projectId).CompilationOptions;
                //     compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
                //         compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));
                //     solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);
                //     var project = solution.AddProject("OverrideCore", "OverrideCore", LanguageNames.CSharp);
                //     project = project.AddDocument("OverrideAttribute", File.ReadAllText(@"C:\projects\OverrideAnalyzer\OverrideCore\OverrideAttribute.cs")).Project;
                //     solution = project.Solution;
                //     solution = solution.AddProjectReference(projectId, new ProjectReference(project.Id));
                //     return solution;
                // });
            }
        }
    }
}

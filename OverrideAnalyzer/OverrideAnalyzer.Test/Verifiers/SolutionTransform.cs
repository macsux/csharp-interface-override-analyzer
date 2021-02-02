using System.IO;
using Analyzers;
using Microsoft.CodeAnalysis;

namespace OverrideAnalyzer.Test
{
    public class SolutionTransform
    {
        public static Solution Transform(Solution solution, ProjectId projectId)
        {
            var stdLib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var compilationOptions = solution.GetProject(projectId).CompilationOptions;
            compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
                compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));
            solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);
            solution = solution.AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(OverrideAttribute).Assembly.Location));
                //.AddMetadataReference(projectId, stdLib);
            // var libOptions = compilationOptions.WithSpecificDiagnosticOptions(compilationOptions.SpecificDiagnosticOptions.SetItem(OverrideAnalyzerAnalyzer.DiagnosticId, ReportDiagnostic.Suppress));
            // var project = solution.AddProject("OverrideCore", "OverrideCore", LanguageNames.CSharp)
            //     .WithMetadataReferences(solution.GetProject(projectId).MetadataReferences)
            //     .WithCompilationOptions(libOptions);
            //project = project.AddDocument("OverrideAttribute", File.ReadAllText(@"C:\projects\OverrideAnalyzer\OverrideCore\OverrideAttribute.cs")).Project;
            // solution = project.Solution;
            // solution = solution.AddProjectReference(projectId, new ProjectReference(project.Id));
            return solution;
            
        }

        public string FindSolutionDir()
        {
            while (true)
            {
                
            }
        }
        
    }
}
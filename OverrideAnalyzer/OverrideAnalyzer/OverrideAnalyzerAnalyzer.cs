using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace OverrideAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class OverrideAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        
        public const string DiagnosticId = "OverrideAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeClass, SymbolKind.NamedType);
            
        }


        private const string OverrideAttributeQualifiedName = "Analyzers.OverrideAttribute, OverrideCore";
        private static void AnalyzeClass(SymbolAnalysisContext context)
        {
            var classSymbol = (INamedTypeSymbol) context.Symbol;
            if (classSymbol.TypeKind != TypeKind.Class && classSymbol.TypeKind != TypeKind.Struct)
                return;
            var methodsWithOverride = classSymbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Except(classSymbol.Constructors)
                .Where(method => !method.IsOverride && method
                    .GetAttributes()
                    .Select(x => x.AttributeClass)
                    .Any(attr => attr.TypeKind == TypeKind.Class && attr.GetUnversionedAssemblyQualifiedName() == OverrideAttributeQualifiedName))
                .ToList();

            if (!methodsWithOverride.Any()) 
                return;
            var interfaceMethods = classSymbol.AllInterfaces
                .SelectMany(x => x.GetMembers()
                .OfType<IMethodSymbol>())
                .ToList();

            var nonImplementingMethods = methodsWithOverride
                .Where(methodSymbol => !interfaceMethods
                    .Select(interfaceMethod => classSymbol.FindImplementationForInterfaceMember(interfaceMethod))
                    .Any(implementation => SymbolEqualityComparer.Default.Equals(methodSymbol, implementation)))
                .ToList();
            var diagnostics = nonImplementingMethods
                .Select(x => Diagnostic.Create(Rule, x.Locations[0], x.Name))
                .ToList();
            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }
        }

    }

}

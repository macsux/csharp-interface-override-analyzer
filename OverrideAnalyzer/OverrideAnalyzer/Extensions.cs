using System;
using System.Text;
using Microsoft.CodeAnalysis;

namespace OverrideAnalyzer
{
    public static class Extensions
    {
        public static string GetUnversionedAssemblyQualifiedName(this Type type) => $"{type.FullName}, {type.Assembly.GetName().Name}";
        public static string GetUnversionedAssemblyQualifiedName(this ISymbol s)
        {
            return $"{s.ContainingNamespace.ToDisplayString()}.{s.MetadataName}, {s.ContainingAssembly.Name}";
        }
        
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = OverrideAnalyzer.Test.CSharpCodeFixVerifier<
    OverrideAnalyzer.OverrideAnalyzerAnalyzer,
    OverrideAnalyzer.OverrideAnalyzerCodeFixProvider>;

namespace OverrideAnalyzer.Test
{
    [TestClass]
    public class OverrideAnalyzerUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task ImplementingMethodsNoWarning()
        {
            var test = @"
    using System;
    using Analyzers;

    namespace ConsoleApplication1
    {
        class A : IA
        {   
            [Override]
            public void MethodName(int a) {}
        }
        interface IA 
        {
            public void MethodName(int a) {}
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
            // await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task OverridenWithNoMatchingInterfaceMethod()
        {
            var test = @"
    using System;
    using Analyzers;

    namespace ConsoleApplication1
    {
        class A : IA
        {   
            [Override]
            public void {|#0:MethodName|}(long a) {}
        }
        interface IA 
        {
            public void MethodName(int a) {}
        }
    }";


            var expected = VerifyCS.Diagnostic("OverrideAnalyzer").WithLocation(0).WithArguments("MethodName");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
        
        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task OverridenWithGenericMethod_MismatchedGenericArgs()
        {
            var test = @"
    using System;
    using Analyzers;

    namespace ConsoleApplication1
    {
        class A : IA
        {   
            [Override]
            public void {|#0:MethodName|}<T,F>(int a) {}
        }
        interface IA 
        {
            public void MethodName<T>(int a) {}
        }
    }";


            var expected = VerifyCS.Diagnostic("OverrideAnalyzer").WithLocation(0).WithArguments("MethodName");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);

        }
        [TestMethod]
        public async Task OverridenWithGenericMethod_MatchedGenericArgs()
        {
            var test = @"
    using System;
using System.Collections.Generic;
    using Analyzers;

    namespace ConsoleApplication1
    {
        class A : IA
        {   
            [Override]
            public void {|#0:MethodName|}<T>(List<T> a) {}
        }
        interface IA 
        {
            public void MethodName<T>(List<T> a) {}
        }
    }";


            await VerifyCS.VerifyAnalyzerAsync(test);

        }
    }
}

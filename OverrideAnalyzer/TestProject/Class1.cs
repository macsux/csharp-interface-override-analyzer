using System;
using Analyzers;

namespace TestProject
{
    public class A : IA
    {
        [Override]
        public void MethodName(long a) {}
    }
    
    
    
    public interface IA 
    {
        public void MethodName(int a) { /* default implementation */ }
    }
}
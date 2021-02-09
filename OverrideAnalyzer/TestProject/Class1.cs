using System;
using Analyzers;

namespace TestProject
{
    public class A : IA
    {
        [Override]
        public void MethodName<Q>(int a) {}
    }
    
    
    
    public interface IA 
    {
        public void MethodName<Q,R>(int a) { /* default implementation */ }
    }
}
using CodeProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleProject
{
    class Program
    {
        public static void Main(string[] args)
        {
            var a = TestClass1.GetFlags1().ToArray();
            var b = TestClass1.GetFlags2().ToArray();
        }
    }
}

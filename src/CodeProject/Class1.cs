using System;
using SourceGeneratorProject;
using TestNamespace;
namespace CodeProject
{
    [SourceGeneratorProject]

    [Test<long>]
    [Test2<long>(11,22)]
    [SourceGeneratorTest]
    [SourceGeneratorTest1(19)]
    [SourceGeneratorTest2<int>(19)]
    [SourceGeneratorTest3<long>(11)]
    [SourceGeneratorTest4<char>(11)]
    public class TestClass1
    {
        public int intField = -100;
        public string stringField = "stringField";
        private long longField = 100;

        public int intProperty { get; set; } = int.MaxValue;
        public string stringProperty { get; set; } = "stringProperty";
        private long longProperty { get; set; } = long.MinValue;


        public void TestVoidFunc(string stringValue = "AA",int intValue = 100)
        {
        }

        public int TestIntFunc()
        {
            return 0;
        }

        public T TestGenericFunc<T>()
        {
            return default(T);
        }

        public T TestGeneric2Func<T,T2,T3>(T2 t2Value,T3 t3Value)
        {
            return default(T);
        }


        public class TestClass2
        {
            public int intProperty { get; set; } = int.MaxValue;
            public string stringProperty { get; set; } = "stringProperty";
            private long longProperty { get; set; } = long.MinValue;
        }

    }
}

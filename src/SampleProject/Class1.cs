using System;
using System.ComponentModel;
using SourceGeneratorProject;
namespace CodeProject
{
    [HookAttribute]
    public partial class TestClass1
    {
        [EnumFlag]
        public int intProperty { get; private set; } = int.MaxValue;
        [EnumFlag]
        public string stringProperty { get; private set; } = "stringProperty";
        [EnumFlag]
        private long longProperty { get; set; } = long.MinValue;

        public TestClass1()
        {

        }
    }
}

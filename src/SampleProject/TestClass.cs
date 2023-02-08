using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using SourceGeneratorProject;
namespace CodeProject
{
    [HookAttribute]
    public partial class TestClass1
    {
        public interface ITestInterface { }

        [TestGeneric<int>(nameof(longProperty))]
        public int intProperty { get; private set; } = int.MaxValue;

        [TestGeneric2<int>(new[] { "a","b","c"})]
        public int intProperty2 { get; private set; } = int.MaxValue;

        [Test(nameof(longProperty))]
        public float floatProperty { get; private set; } = float.MaxValue;

        [Test2(new[] { "a", "b", "c" })]
        public float floatProperty2 { get; private set; } = float.MaxValue;

        [Test3(null)]
        public float floatProperty3 { get; private set; } = float.MaxValue;

        [EnumFlag]
        private long longProperty { get; set; } = long.MinValue;
    }
}

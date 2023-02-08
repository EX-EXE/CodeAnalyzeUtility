using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using SourceGeneratorProject;
namespace CodeProject
{
    [HookAttribute]
    public partial class SampleClass
    {
        [EnumFlag]
        public int intProperty { get; private set; } = int.MaxValue;
        [EnumFlag]
        public string stringProperty { get; private set; } = "stringProperty";
        [EnumFlag]
        private long longProperty { get; set; } = long.MinValue;
    }
}

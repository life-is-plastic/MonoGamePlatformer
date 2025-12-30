using System.Runtime.CompilerServices;

namespace Engine.Util.Extensions;

public static class InlineArrayExtensions
{
    extension<T>(InlineArray4<T> array)
    {
        public int Length => 4;
    }

    extension<T>(InlineArray8<T> array)
    {
        public int Length => 8;
    }
}

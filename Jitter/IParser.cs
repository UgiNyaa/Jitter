using System;

namespace Jitter
{
    public interface IParser<T>
    {
        int Check(byte[] objdata);
        T Parse(byte[] objdata, out int index);
    }
}
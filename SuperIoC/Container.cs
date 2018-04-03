using System;

namespace SuperIoC
{
    public class Container
    {
        public object GetInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
using Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class ConstantValueProvider : IValueProvider
    {
        public object GetValue()
        {
            return this.GetType().Assembly.GetName().Version;
        }
    }
}

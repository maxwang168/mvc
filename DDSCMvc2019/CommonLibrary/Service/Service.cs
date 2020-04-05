using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Service
{
    public abstract class Service<T>
    {
        public abstract R Use<R>(Func<T, R> codeBlock);

        public abstract void Use(Action<T> codeBlock);

    }
}

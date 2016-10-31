using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandymanMvvm
{
    public abstract class RefreshableModel<T>
    {
        public abstract void Refresh(T model);
    }
}

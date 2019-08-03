using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBookDataLib.Interface
{
    public interface IDBContext<ContextType>
    {
        ContextType Context { get; }
    }
}

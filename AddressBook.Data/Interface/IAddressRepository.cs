using AddressBookDataLib.Model;
using System.Collections.Generic;

namespace AddressBookDataLib.Interface
{
    public interface IAddressRepository : 
        IDataSetRepository<Address, int>,
        IRepository<IDatabaseSetting> {

        IEnumerable<Address> ReadByContactId(int contactId);

    }
}

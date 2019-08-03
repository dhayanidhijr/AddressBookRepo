
using System.Collections.Generic;

namespace AddressBookBusinessLib.Interface
{
    public interface IAddressRepository : IBusinessRepository<Model.Address, int> {
        IEnumerable<Model.Address> ReadByContactId(int contactId);
    }
}

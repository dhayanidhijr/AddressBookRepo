using System.Collections.Generic;

namespace AddressBookBusinessLib.Interface
{
    public interface IContactRepository : IBusinessRepository<Model.Contact, int> {

        bool AddAddress(int ContactId, Model.Address Address);
        IEnumerable<Model.Address> ListAddress(int ContactId);
        bool UpdateAddress(int ContactId, int AddressId, Model.Address Address);
        bool DeleteAddress(int ContactId, int AddressId);
    }
}

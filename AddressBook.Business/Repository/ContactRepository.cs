
using System.Collections.Generic;
using AddressBookBusinessLib.Model;
using Microsoft.Extensions.Logging;
using System.Linq;
using DataModel = AddressBookDataLib.Model;
using DataInterface = AddressBookDataLib.Interface;
using BusinessModel = AddressBookBusinessLib.Model;
using BusinessInterface = AddressBookBusinessLib.Interface;

namespace AddressBookBusinessLib.Repository
{
    public class ContactRepository : BusinessInterface.IContactRepository
    {

        DataInterface.IContactRepository contactRepository;
        ILogger<ContactRepository> logger;

        public ContactRepository(ILogger<ContactRepository> logger, DataInterface.IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
            this.logger = logger;
        }

        public bool AddAddress(int ContactId, Address Address)
        {
            throw new System.NotImplementedException();
        }

        public bool Create(Contact model)
        {
            return contactRepository.Create(model.DataObject);
        }

        public bool Delete(int key)
        {
            return contactRepository.Delete(key);
        }

        public bool DeleteAddress(int ContactId, int AddressId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Address> ListAddress(int ContactId)
        {
            return Read(ContactId).DataObject.AddressList.Select((Item) =>
            {
                return (BusinessModel.Address)Item;
            });
        }

        public Contact Read(int key)
        {
            return contactRepository.Read(key);
        }

        public IEnumerable<Contact> ReadAll()
        {
            return contactRepository.ReadAll().Select((item) =>
            {
                return (BusinessModel.Contact)item;
            });
        }

        public bool Update(Contact model)
        {
            return contactRepository.Update(model.DataObject);
        }

        public bool UpdateAddress(int ContactId, int AddressId, Address Address)
        {
            throw new System.NotImplementedException();
        }
    }
}

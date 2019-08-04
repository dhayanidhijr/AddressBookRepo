
using System.Collections.Generic;
using AddressBookBusinessLib.Model;
using Microsoft.Extensions.Logging;
using System.Linq;
using DataContext = AddressBookDataLib.Context;
using DataInterface = AddressBookDataLib.Interface;
using BusinessModel = AddressBookBusinessLib.Model;
using BusinessInterface = AddressBookBusinessLib.Interface;

namespace AddressBookBusinessLib.Repository
{
    public class AddressRepository : BusinessInterface.IAddressRepository
    {
        DataInterface.IAddressRepository addressRepository;
        ILogger<AddressRepository> logger;
        DataInterface.IDBContext<DataContext.AddressBook> dbContext;

        public AddressRepository(ILogger<AddressRepository> logger,
                DataInterface.IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
            this.logger = logger;
        }
        public bool Create(Address model)
        {
            return addressRepository.Create(model.DataObject);
        }

        public bool Delete(int key)
        {
            return addressRepository.Delete(key);
        }

        public Address Read(int key) {
            logger.LogInformation("Address Business Lib Read {0}", key);
            return addressRepository.Read(key);
        }

        public IEnumerable<Address> ReadByContactId(int contactId)
        {
            return addressRepository.ReadByContactId(contactId).Select((item) =>
            {
                return (BusinessModel.Address)item;
            });
        }

        public IEnumerable<Address> ReadAll()
        {
            return addressRepository.ReadAll().Select((item) =>
            {
                return (BusinessModel.Address)item;
            });
        }

        public bool Update(Address model)
        {
            return addressRepository.Update(model.DataObject);
        }
    }
}

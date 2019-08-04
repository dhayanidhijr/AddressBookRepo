using System.Collections.Generic;
using AddressBookDataLib.Interface;
using AddressBookDataLib.Model;
using AddressBookDataLib.Context;
using AddressBookDataLib.Common;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AddressBookDataLib.Repository
{
    public class AddressRepository : IAddressRepository
    {
        AddressBook addressBook;
        public ILogger Logger { get; set; }
        public IDatabaseSetting Settings { get; set; }

        public AddressRepository(ILogger<AddressRepository> logger, IDatabaseSetting settings, IDBContext<AddressBook> dbContext)
        {
            this.Logger = logger;
            this.Settings = settings;
            addressBook = dbContext.Context;
            addressBook = dbContext.Context;
        }

        public bool Create(Address model)
        {
            Contact contact = addressBook.Contacts.SingleOrDefault((item) => item.Id == model.Contact.Id);
            if (contact == null) return false;
            model.Contact = contact;
            addressBook.AddressList.Add(model);
            return addressBook.SaveChanges().Equals(1);
        }

        public bool Delete(int key)
        {
            addressBook.AddressList.Remove(
                addressBook.AddressList.SingleOrDefault((item) => item.Id == key)
            );
            return addressBook.SaveChanges().Equals(1);
        }

        public Address Read(int key)
        {
            this.Logger.LogInformation("Address Data Lib Read {0}", key);
            return addressBook.AddressList.Include((item) => item.Contact).SingleOrDefault((item) => item.Id == key);
        }

        public IEnumerable<Address> ReadAll()
        {
            return addressBook.AddressList.Include((item) => item.Contact).Where((item) => item.Active == true);
        }

        public IEnumerable<Address> ReadByContactId(int contactId)
        {
            return addressBook.AddressList
                .Include((item) => item.Contact).Where((item) => (item.Contact.Id == contactId && item.Active == true));
        }

        public bool Update(Address model)
        {
            Address updateModel = addressBook.AddressList
                .Include((item) => item.Contact)
                .SingleOrDefault((item) => item.Id == model.Id);

            Contact updateContactModel = addressBook.Contacts
                .SingleOrDefault((item) => item.Id == model.Contact.Id);

            if ((updateContactModel == null) && (model.Contact.Id != 0)) return false;

            if (updateModel == null) return false;

            updateModel.State = model.State.GetOrDefault(updateModel.State);
            updateModel.City = model.City.GetOrDefault(updateModel.City);
            updateModel.Street = model.Street.GetOrDefault(updateModel.Street);
            updateModel.ZipCode = model.ZipCode.GetOrDefault(updateModel.ZipCode);
            updateModel.Contact = updateContactModel.GetOrDefault(updateModel.Contact);
            updateModel.UpdateTime();

            addressBook.AddressList.Update(updateModel);
            return addressBook.SaveChanges().Equals(1);
        }
    }
}

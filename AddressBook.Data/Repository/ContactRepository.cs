using System.Collections.Generic;
using AddressBookDataLib.Interface;
using AddressBookDataLib.Model;
using AddressBookDataLib.Context;
using Microsoft.EntityFrameworkCore;
using AddressBookDataLib.Common;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace AddressBookDataLib.Repository
{
    public class ContactRepository : IContactRepository
    {

        AddressBook addressBook;
        public ILogger Logger { get; set; }
        public IDatabaseSetting Settings { get; set; }

        public ContactRepository(ILogger<AddressRepository> logger, IDatabaseSetting settings, IDBContext<AddressBook> dbContext)
        {
            this.Logger = logger;
            this.Settings = settings;
            addressBook = dbContext.Context;
        }

        public bool Create(Contact model)
        {
            model.Type = addressBook.ContactTypes.SingleOrDefault((item) => item.Id == model.Type.Id);
            addressBook.Contacts.Add(model);
            return addressBook.SaveChanges().Equals(1);
        }

        public bool Delete(int key)
        {
            addressBook.Contacts.Remove(
                addressBook.Contacts.SingleOrDefault((item) => item.Id == key)
            );
            return addressBook.SaveChanges().Equals(1);
        }

        public Contact Read(int key)
        {
            Contact contact = addressBook.Contacts.Include((item) => item.AddressList).SingleOrDefault((item) => item.Id == key);
            return contact;
        }

        public IEnumerable<Contact> ReadAll()
        {
            return addressBook.Contacts
                .Include((item) => item.Type)
                .Where((item) => item.Active == true);
        }

        public bool Update(Contact model)
        {
            Contact contact = addressBook.Contacts.SingleOrDefault((item) => item.Id == model.Id);

            model.Type = addressBook.ContactTypes.SingleOrDefault((item) => item.Id == model.Type.Id);

            contact.FirstName = model.FirstName.GetOrDefault(contact.FirstName);
            contact.LastName = model.LastName.GetOrDefault(contact.LastName);
            contact.BusinessName = model.BusinessName.GetOrDefault(contact.BusinessName);
            contact.Type = model.Type.GetOrDefault(contact.Type);
            contact.UpdateTime();
            addressBook.Contacts.Update(contact);
            return addressBook.SaveChanges().Equals(1);
        }
    }
}

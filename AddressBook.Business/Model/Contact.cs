using System;
using System.Collections.Generic;
using System.Text;
using DataModel = AddressBookDataLib.Model;
using AddressBookDataLib.Common;
namespace AddressBookBusinessLib.Model
{
    public class Contact : DataMap.Contact<DataModel.Contact>
    {

        public Contact()
        {
            InitializeData(new DataModel.Contact());
        }

        private Contact(DataModel.Contact contact)
        {
            InitializeData(contact);
        }

        private void InitializeData(DataModel.Contact contact)
        {
            base.DataObject = contact;
            DataObject.Type = DataObject.Type.GetOrDefault(new DataModel.ContactType() { Id = 0 });
        }

        public string FullName
        {
            get
            {
                return DataObject.FirstName + " " + DataObject.LastName;
            }
        }

        public int ContactType
        {
            get
            {
                return DataObject.Type.Id;
            }
            set
            {
                DataObject.Type.Id = value;
            }
        }

        public string ContactTypeName
        {
            get
            {
                DataModel.ContactType contactType = DataObject.Type;
                return (contactType != null) ? contactType.Type : string.Empty;
            }
        }

        public static implicit operator Contact(DataModel.Contact contact)
        {
            if (contact == null) return null;
            return new Contact(contact);
        }
    }
}

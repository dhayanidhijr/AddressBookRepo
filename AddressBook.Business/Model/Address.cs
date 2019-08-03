using DataModel = AddressBookDataLib.Model;
using AddressBookDataLib.Common;
namespace AddressBookBusinessLib.Model
{
    public class Address : DataMap.Address<DataModel.Address>
    {
        public Address()
        {
            InitializeData(new DataModel.Address());
        }

        private Address(DataModel.Address address)
        {
            InitializeData(address);
        }

        private void InitializeData(DataModel.Address address)
        {
            base.DataObject = address;
            DataObject.Contact = DataObject.Contact.GetOrDefault(new DataModel.Contact() { Id = 0 });
        }

        public string FullAddress
        {
            get => DataObject.Street + "," + DataObject.City + "," + DataObject.State;
        }

        public int ContactId
        {
            get
            {
                return DataObject.Contact.Id;
            }
            set
            {
                DataObject.Contact.Id = value;
            }
        }

        public static implicit operator Address(DataModel.Address address)
        {
            if (address == null) return null;
            return new Address(address);
        }
    }
}

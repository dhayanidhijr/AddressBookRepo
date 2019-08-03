using DataModel = AddressBookDataLib.Model;
using DataInterface = AddressBookDataLib.Interface;
namespace AddressBookBusinessLib.DataMap
{
    public abstract class Address<FromDataObject> : Common.DataMapper<FromDataObject> 
        where FromDataObject : DataInterface.IDataObject
    {
        public int Id
        {
            get
            {
                return (int)this.GetProperty("Id");
            }
            set
            {
                this.SetProperty("Id", value);
            }
        }

        public string Street
        {
            get
            {
                return (string)this.GetProperty("Street");
            }
            set
            {
                this.SetProperty("Street", value);
            }
        }

        public string City
        {
            get
            {
                return (string)this.GetProperty("City");
            }
            set
            {
                this.SetProperty("City", value);
            }
        }

        public string State
        {
            get
            {
                return (string)this.GetProperty("State");
            }
            set
            {
                this.SetProperty("State", value);
            }
        }

        public string ZipCode
        {
            get
            {
                return (string)this.GetProperty("ZipCode");
            }
            set
            {
                this.SetProperty("ZipCode", value);
            }
        }

    }
}

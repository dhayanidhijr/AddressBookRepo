using System.Collections.Generic;
using AddressBookDataLib.Model;
using DataInterface = AddressBookDataLib.Interface;

namespace AddressBookBusinessLib.DataMap
{
    public abstract class Contact<FromDataObject> : Common.DataMapper<FromDataObject> 
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

        public string FirstName
        {
            get
            {
                return (string)this.GetProperty("FirstName");
            }
            set
            {
                this.SetProperty("FirstName", value);
            }
        }

        public string LastName
        {
            get
            {
                return (string)this.GetProperty("LastName");
            }
            set
            {
                this.SetProperty("LastName", value);
            }
        }

        public string BusinessName
        {
            get
            {
                return (string)this.GetProperty("BusinessName");
            }
            set
            {
                this.SetProperty("BusinessName", value);
            }
        }

    }
}

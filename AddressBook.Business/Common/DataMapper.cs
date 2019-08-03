using System;
using DataInterface = AddressBookDataLib.Interface;
using BusinessInterface = AddressBookBusinessLib.Interface;
namespace AddressBookBusinessLib.Common
{
    public abstract class DataMapper<FromDataObject> : BusinessInterface.IBusinessObject where FromDataObject : DataInterface.IDataObject
    {
        internal FromDataObject DataObject { get; set; }

        protected object GetProperty(string propertyName)
        {
            return GetProperty(DataObject, propertyName);
        }

        protected object GetProperty(object dataObject, string propertyName)
        {
            return dataObject.GetType().GetProperty(propertyName).GetValue(dataObject);
        }

        protected void SetProperty(string propertyName, object value)
        {
            SetProperty(DataObject, propertyName, value);
        }

        protected void SetProperty(object dataObject, string propertyName, object value)
        {
            dataObject.GetType().GetProperty(propertyName).SetValue(dataObject, value);
        }

    }
}

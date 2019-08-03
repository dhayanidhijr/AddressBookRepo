using AddressBookDataLib.Model;
using AddressBookDataLib.Settings;

namespace AddressBookDataLib.Interface
{
    public interface IContactRepository :
        IDataSetRepository<Contact, int>,
        IRepository<IDatabaseSetting> { }
}

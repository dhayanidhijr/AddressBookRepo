using AddressBookDataLib.Interface;

namespace AddressBookDataLib.Settings
{
    public class Database : IDatabaseSetting
    {
        public string ConnectionString { get; set; }
    }
}

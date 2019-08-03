
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AddressBookDataLib.Settings;

namespace AddressBookDataLib.Interface
{
    public interface IRepository<SettingType>
    {
        ILogger Logger { get; set; }
        SettingType Settings { get; set; }
    }
}

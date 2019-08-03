using AddressBookDataLib.Common;
using System.ComponentModel.DataAnnotations;

namespace AddressBookDataLib.Model
{
    public class Address : DataObject
    {
        [Key]
        public int Id { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public Contact Contact { get; set; }
    }
}

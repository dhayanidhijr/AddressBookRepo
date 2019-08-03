using System.Collections.Generic;
using AddressBookDataLib.Common;
using System.ComponentModel.DataAnnotations;

namespace AddressBookDataLib.Model
{

    public class Contact : DataObject
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BusinessName { get; set; }

        public ContactType Type { get; set; }

        public ICollection<Address> AddressList { get; set; }
    }
}

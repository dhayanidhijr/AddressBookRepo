using System;
using System.Collections.Generic;
using AddressBookDataLib.Context;
using AddressBookDataLib.Model;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore;

namespace AddressBookDataLib.Settings
{
    public class DataInitializer
    {
        public static IEnumerable<ContactType> GetContactTypeSeedData()
        {
            IList<ContactType> contactTypeSeedData = new List<ContactType>();

            contactTypeSeedData.Add(new ContactType() { Id = 1, Type = "Person" });
            contactTypeSeedData.Add(new ContactType() { Id = 2, Type = "Business" });
            contactTypeSeedData.Add(new ContactType() { Id = 3, Type = "Other" });

            return contactTypeSeedData;

        }
    }
}

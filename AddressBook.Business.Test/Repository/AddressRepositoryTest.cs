using NUnit.Framework;
using BusinessRepository = AddressBookBusinessLib.Repository;
using DataInterface = AddressBookDataLib.Interface;
using DataContext = AddressBookDataLib.Context;
using DataModel = AddressBookDataLib.Model;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;

namespace AddressBookBusinessLibTest.Repository
{
    public class AddressRepositoryTest
    {

        Mock<DataInterface.IAddressRepository> mockAddressRepository;
        Mock<ILogger<BusinessRepository.AddressRepository>> mockLogger;
        Mock<DataInterface.IDBContext<DataContext.AddressBook>> mockDataContext;
        Mock<DataInterface.IDatabaseSetting> mockDatabaseSettings;
        DataModel.Address addressModel;

        [SetUp]
        public void Setup()
        {
            mockAddressRepository = new Mock<DataInterface.IAddressRepository>();
            mockDatabaseSettings = new Mock<DataInterface.IDatabaseSetting>();
            mockLogger = new Mock<ILogger<BusinessRepository.AddressRepository>>();
            mockDataContext = new Mock<DataInterface.IDBContext<DataContext.AddressBook>>();
            addressModel = new DataModel.Address();
        }

        [Test]
        public void Create_AddressRecord_ExpectedSuccessfulCreationFlag()
        {

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object,
                mockAddressRepository.Object
            );

            mockAddressRepository.Setup((item) => item.Create(addressModel)).Returns(true);

            Assert.IsTrue(addressRepository.Create(addressModel));
        }


        [Test]
        public void Create_AddressRecord_ExpectedFailedCreationFlag()
        {

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object,
                mockAddressRepository.Object
            );

            mockAddressRepository.Setup((item) => item.Create(addressModel)).Returns(false);

            Assert.IsFalse(addressRepository.Create(addressModel));
        }

        [Test]
        public void ReadAll_AddressRecord_ExpectedAllAdddressList()
        {

            mockAddressRepository.Setup((item) => item.ReadAll()).Returns(new DataModel.Address[] {
                new DataModel.Address() {Id = 1, Street = "Street"}, 
                new DataModel.Address() {Id = 2, Street = "Street1"}
            });

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object, 
                mockAddressRepository.Object
            );

            var result = addressRepository.ReadAll();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.SingleOrDefault((item) => item.Id == 1).Id == 1);
            Assert.IsTrue(result.SingleOrDefault((item) => item.Street == "Street1").Id == 2);
        }


        [Test]
        public void Get_ReadAll_ExpectedEmptyResult()
        {
            mockAddressRepository.Setup((item) => item.ReadAll()).Returns(new DataModel.Address[] { });

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object,
                mockAddressRepository.Object
            );

            var result = addressRepository.ReadAll();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            Assert.IsNull(result.SingleOrDefault((item) => item.Id == 1));
            Assert.IsNull(result.SingleOrDefault((item) => item.Street == "Street1"));
        }

        [Test]
        public void ReadByContactId_AddressRecord_ExpectedAddressRecordForProvidedContactId()
        {

            DataModel.Address address1, address2;
            DataModel.Contact contact = new DataModel.Contact()
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Type = new DataModel.ContactType()
                {
                    Id = 1,
                    Type = "PERSON"
                }
            };

            mockAddressRepository.Setup((item) => item.ReadByContactId(contact.Id)).Returns(new DataModel.Address[] {
                address1 = new DataModel.Address() {Id = 1, Street = "Street", Contact = contact},
                address2 = new DataModel.Address() {Id = 2, Street = "Street1", Contact = contact}
            });

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object,
                mockAddressRepository.Object
            );

            var result = addressRepository.ReadByContactId(contact.Id);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.SingleOrDefault((item) => item.Id == 1).FullAddress == GetFullAddress(address1));
            Assert.IsTrue(result.SingleOrDefault((item) => item.Street == "Street1").ContactId == contact.Id);
        }

        [Test]
        public void ReadByContactId_AddressRecord_ExpectedNoRecordsProvidedContactId()
        {
            DataModel.Contact contact = new DataModel.Contact()
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Type = new DataModel.ContactType()
                {
                    Id = 1,
                    Type = "PERSON"
                }
            };

            mockAddressRepository.Setup((item) => item.ReadByContactId(contact.Id)).Returns(new DataModel.Address[] { });

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object,
                mockAddressRepository.Object
            );

            var result = addressRepository.ReadByContactId(contact.Id);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            Assert.IsNull(result.SingleOrDefault((item) => item.Id == 1));
            Assert.IsNull(result.SingleOrDefault((item) => item.Street == "Street1"));
        }

        [Test]
        public void ReadById_AddressRecord_ExpectedAddressRecordForProvidedAddressId()
        {

            int addressId = 1;
            DataModel.Address address;
            DataModel.Contact contact = new DataModel.Contact()
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Type = new DataModel.ContactType()
                {
                    Id = 1,
                    Type = "PERSON"
                }
            };

            mockAddressRepository.Setup((item) => item.Read(addressId)).Returns(
                address = new DataModel.Address() { Id = 1, Street = "Street", Contact = contact }
            );

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object,
                mockAddressRepository.Object
            );

            var result = addressRepository.Read(addressId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.FullAddress == GetFullAddress(address));
            Assert.IsTrue(result.ContactId == contact.Id);
        }

        [Test]
        public void ReadById_AddressRecord_ExpectedNoRecordsProvidedAddressId()
        {
            int addressId = 1;

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object,
                mockAddressRepository.Object
            );

            var result = addressRepository.Read(addressId);

            Assert.IsNull(result);
        }

        [Test]
        public void Update_AddressRecord_ExpectedSuccessfulUpdateFlag()
        {

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object,
                mockAddressRepository.Object
            );


            mockAddressRepository.Setup((item) => item.Update(addressModel)).Returns(true);

            Assert.IsTrue(addressRepository.Update(addressModel));
        }


        [Test]
        public void Update_AddressRecord_ExpectedFailedUpdateFlag()
        {

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object,
                mockAddressRepository.Object
            );

            mockAddressRepository.Setup((item) => item.Update(addressModel)).Returns(false);

            Assert.IsFalse(addressRepository.Update(addressModel));
        }

        [Test]
        public void Delete_AddressRecord_ExpectedSuccessfulDeleteFlag()
        {

            int addressId = 1;

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object,
                mockAddressRepository.Object
            );


            mockAddressRepository.Setup((item) => item.Delete(addressId)).Returns(true);

            Assert.IsTrue(addressRepository.Delete(addressId));
        }


        [Test]
        public void Delete_AddressRecord_ExpectedFailedDeleteFlag()
        {
            int addressId = 1;

            var addressRepository = new BusinessRepository.AddressRepository(mockLogger.Object,
                mockAddressRepository.Object
            );

            mockAddressRepository.Setup((item) => item.Delete(addressId)).Returns(false);

            Assert.IsFalse(addressRepository.Delete(addressId));
        }


        private string GetFullAddress(DataModel.Address address)
        {
            return address.Street + "," + address.City + "," + address.State;
        }
    }
}

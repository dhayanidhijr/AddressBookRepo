using NUnit.Framework;
using System.Collections.Generic;
using EntityFramework = Microsoft.EntityFrameworkCore;
using DataInterface = AddressBookDataLib.Interface;
using DataContext = AddressBookDataLib.Context;
using DataRepository = AddressBookDataLib.Repository;
using DataModel = AddressBookDataLib.Model;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;

namespace AddressBookBusinessLibTest.Repository
{
    public class AddressRepositoryTest
    {

        Mock<ILogger<DataRepository.AddressRepository>> mockLogger;
        Mock<DataInterface.IDBContext<DataContext.AddressBook>> mockDataContext;
        Mock<DataInterface.IDatabaseSetting> mockDatabaseSettings;
        Mock<DataContext.AddressBook> mockAddressBook;
        Mock<EntityFramework.DbSet<DataModel.Contact>> mockedContactDBSet;
        Mock<EntityFramework.DbSet<DataModel.Address>> mockedAddressDBSet;
        DataModel.Address addressModel;
        IEnumerable<DataModel.Contact> contactModelList;
        IEnumerable<DataModel.Address> addressModelList;

        [SetUp]
        public void Setup()
        {
            mockDatabaseSettings = new Mock<DataInterface.IDatabaseSetting>();
            mockLogger = new Mock<ILogger<DataRepository.AddressRepository>>();
            mockDataContext = new Mock<DataInterface.IDBContext<DataContext.AddressBook>>();
            mockAddressBook = new Mock<DataContext.AddressBook>();

            contactModelList = new DataModel.Contact[] {
                new DataModel.Contact() {Id = 1, FirstName = "FirstName", LastName = "LastName"},
                new DataModel.Contact() {Id = 2, FirstName = "FirstName1", LastName = "LastName1"}
            };

            addressModelList = new DataModel.Address[] {
                new DataModel.Address() {Id = 1, Street = "Street", City = "City", State = "State", Contact = contactModelList.First()},
                new DataModel.Address() {Id = 2, Street = "Street1", City = "City1", State = "State1", Contact = contactModelList.First()},
                new DataModel.Address() {Id = 3, Street = "Street", City = "City", State = "State", Contact = contactModelList.Last()},
                new DataModel.Address() {Id = 4, Street = "Street1", City = "City1", State = "State1", Contact = contactModelList.Last()}
            };

            mockedContactDBSet = GetQueryableMockDbSet<DataModel.Contact>(contactModelList.ToList());
            mockedAddressDBSet = GetQueryableMockDbSet<DataModel.Address>(addressModelList.ToList());

            addressModel = addressModelList.First();
        }


        private static Mock<EntityFramework.DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var mockedDBDataSet = new Mock<EntityFramework.DbSet<T>>();
            mockedDBDataSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockedDBDataSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockedDBDataSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockedDBDataSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockedDBDataSet;
        }

        [Test]
        public void Create_AddressRecord_ExpectedSuccessfulCreationFlag()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var addressRepository = new DataRepository.AddressRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            List<DataModel.Address> addressList = addressModelList.ToList();

            DataModel.Address newAddressModel = new DataModel.Address() { Id = 10, Street = "Street", City = "City", State = "State", Contact = contactModelList.First() };

            addressList.Add(newAddressModel);

            mockedAddressDBSet = GetQueryableMockDbSet<DataModel.Address>(addressList.ToList());

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(1);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsTrue(addressRepository.Create(newAddressModel));
            Assert.IsTrue(mockAddressBook.Object.AddressList.SingleOrDefault((item) => item.Id == newAddressModel.Id).Id == newAddressModel.Id);
        }

        [Test]
        public void Create_AddressRecord_ExpectedFailedCreationFlag()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var addressRepository = new DataRepository.AddressRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(0);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsFalse(addressRepository.Create(addressModel));
        }

        [Test]
        public void Read_AddressRecordById_ExpectedAddressRecordForProvidedId()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var addressRepository = new DataRepository.AddressRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(1);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsTrue(addressRepository.Read(1).Id == 1);
            Assert.IsTrue(addressRepository.Read(2).Id == 2);
        }

        [Test]
        public void Read_AddressRecordById_ExpectedNullForProvidedId()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var addressRepository = new DataRepository.AddressRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(1);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsNull(addressRepository.Read(11));
        }

        [Test]
        public void ReadAll_AddressRecord_ExpectedAllAddressRecords()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var addressRepository = new DataRepository.AddressRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(1);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.True(addressRepository.ReadAll().FirstOrDefault().Id == 1);
        }


        [Test]
        public void ReadAll_AddressRecord_ExpectedEmptyResult()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var addressRepository = new DataRepository.AddressRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            var mockedAddressNoResultDBSet = GetQueryableMockDbSet<DataModel.Address>((new DataModel.Address[] { }).ToList());

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(1);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressNoResultDBSet.Object);

            Assert.IsEmpty(addressRepository.ReadAll());
        }
    }
}

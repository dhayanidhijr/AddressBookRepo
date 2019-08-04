using NUnit.Framework;
using System.Collections.Generic;
using EntityFramework = Microsoft.EntityFrameworkCore;
using DataInterface = AddressBookDataLib.Interface;
using DataContext = AddressBookDataLib.Context;
using DataRepository = AddressBookDataLib.Repository;
using DataModel = AddressBookDataLib.Model;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace AddressBookDataLibTest.Repository
{
    public class ContactRepositoryTest
    {
        Mock<ILogger<DataRepository.AddressRepository>> mockLogger;
        Mock<DataInterface.IDBContext<DataContext.AddressBook>> mockDataContext;
        Mock<DataInterface.IDatabaseSetting> mockDatabaseSettings;
        Mock<DataContext.AddressBook> mockAddressBook;
        Mock<EntityFramework.DbSet<DataModel.Contact>> mockedContactDBSet;
        Mock<EntityFramework.DbSet<DataModel.ContactType>> mockedContactTypeDBSet;
        Mock<EntityFramework.DbSet<DataModel.Address>> mockedAddressDBSet;
        DataModel.Contact contactModel;
        DataModel.Address addressModel;
        IEnumerable<DataModel.Contact> contactModelList;
        IEnumerable<DataModel.ContactType> contactTypeModelList;
        IEnumerable<DataModel.Address> addressModelList;

        [SetUp]
        public void Setup()
        {
            mockDatabaseSettings = new Mock<DataInterface.IDatabaseSetting>();
            mockLogger = new Mock<ILogger<DataRepository.AddressRepository>>();
            mockDataContext = new Mock<DataInterface.IDBContext<DataContext.AddressBook>>();
            mockAddressBook = new Mock<DataContext.AddressBook>();

            contactTypeModelList = new DataModel.ContactType[] {
                new DataModel.ContactType() {Id = 1, Type = "PERSON"},
                new DataModel.ContactType() {Id = 2, Type = "BUSINESS"},
                new DataModel.ContactType() {Id = 3, Type = "OTHER"},

            };

            contactModelList = new DataModel.Contact[] {
                new DataModel.Contact() {Id = 1, FirstName = "FirstName", LastName = "LastName", Type = contactTypeModelList.Single((item) => item.Id == 1)},
                new DataModel.Contact() {Id = 2, FirstName = "FirstName1", LastName = "LastName1", Type = contactTypeModelList.Single((item) => item.Id == 2)},
                new DataModel.Contact() {Id = 3, FirstName = "FirstName1", LastName = "LastName1", Type = contactTypeModelList.Single((item) => item.Id == 3)}
            };

            addressModelList = new DataModel.Address[] {
                new DataModel.Address() {Id = 1, Street = "Street", City = "City", State = "State", Contact = contactModelList.First()},
                new DataModel.Address() {Id = 2, Street = "Street1", City = "City1", State = "State1", Contact = contactModelList.First()},
                new DataModel.Address() {Id = 3, Street = "Street", City = "City", State = "State", Contact = contactModelList.Last()},
                new DataModel.Address() {Id = 4, Street = "Street1", City = "City1", State = "State1", Contact = contactModelList.Last()}
            };

            mockedContactDBSet = GetQueryableMockDbSet<DataModel.Contact>(contactModelList.ToList());
            mockedAddressDBSet = GetQueryableMockDbSet<DataModel.Address>(addressModelList.ToList());
            mockedContactTypeDBSet = GetQueryableMockDbSet<DataModel.ContactType>(contactTypeModelList.ToList());

            contactModel = contactModelList.First();
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
        public void Create_ContactRecord_ExpectedSuccessfulCreationFlag()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var contactRepository = new DataRepository.ContactRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            List<DataModel.Contact> contactList = contactModelList.ToList();

            DataModel.Contact newContactModel = new DataModel.Contact() { Id = 10, FirstName = "FirstName", LastName = "LastName", Type = contactTypeModelList.First() };

            contactList.Add(newContactModel);

            mockedContactDBSet = GetQueryableMockDbSet<DataModel.Contact>(contactList.ToList());

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(1);
            mockAddressBook.Setup((item) => item.ContactTypes).Returns(mockedContactTypeDBSet.Object);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsTrue(contactRepository.Create(newContactModel));
            Assert.IsTrue(mockAddressBook.Object.Contacts.SingleOrDefault((item) => item.Id == newContactModel.Id).Id == newContactModel.Id);
        }

        [Test]
        public void Create_ContactRecord_ExpectedFailedCreationFlag()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var contactRepository = new DataRepository.ContactRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            List<DataModel.Contact> contactList = contactModelList.ToList();

            DataModel.Contact newContactModel = new DataModel.Contact() { Id = 10, FirstName = "FirstName", LastName = "LastName", Type = contactTypeModelList.First() };

            contactList.Add(newContactModel);

            mockAddressBook.Setup((item) => item.Add(newContactModel)).Throws(new Exception("Unexpected"));
            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(0);
            mockAddressBook.Setup((item) => item.ContactTypes).Returns(mockedContactTypeDBSet.Object);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsFalse(contactRepository.Create(newContactModel));
            Assert.IsNull(mockAddressBook.Object.Contacts.SingleOrDefault((item) => item.Id == newContactModel.Id));
        }

        [Test]
        public void Read_ContactRecordById_ExpectedContactRecordForProvidedId()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var contactRepository = new DataRepository.ContactRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(1);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsTrue(contactRepository.Read(1).Id == 1);
            Assert.IsTrue(contactRepository.Read(2).Id == 2);
            Assert.IsTrue(contactRepository.Read(3).Id == 3);
        }

        [Test]
        public void Read_ContactRecordById_ExpectedNullForProvidedId()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var contactRepository = new DataRepository.ContactRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(1);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsNull(contactRepository.Read(11));
            Assert.IsNull(contactRepository.Read(12));
            Assert.IsNull(contactRepository.Read(13));
        }

        [Test]
        public void ReadAll_ContactRecord_ExpectedAllContactRecords()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var contactRepository = new DataRepository.ContactRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(1);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsTrue(contactRepository.ReadAll().First().Id == 1);
            Assert.IsTrue(contactRepository.ReadAll().Last().Id == 3);
        }


        [Test]
        public void ReadAll_ContactRecord_ExpectedEmptyResult()
        {

            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var contactRepository = new DataRepository.ContactRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            var mockedContactNoResultDBSet = GetQueryableMockDbSet<DataModel.Contact>((new DataModel.Contact[] { }).ToList());

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(1);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactNoResultDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsEmpty(contactRepository.ReadAll());
        }

        [Test]
        public void Update_ContactRecord_ExpectedSuccessfulUpdateFlag()
        {

            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var contactRepository = new DataRepository.ContactRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            List<DataModel.Contact> contactList = contactModelList.ToList();

            DataModel.Contact updateContactModel = contactModelList.First();

            DateTime oldUpdatedTime = updateContactModel.Updated;

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(1);
            mockAddressBook.Setup((item) => item.ContactTypes).Returns(mockedContactTypeDBSet.Object);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsTrue(contactRepository.Update(updateContactModel));
            Assert.AreNotEqual(updateContactModel.Updated, oldUpdatedTime);
            Assert.IsTrue(mockAddressBook.Object.Contacts.SingleOrDefault((item) => item.Id == updateContactModel.Id).Id == updateContactModel.Id);
        }

        [Test]
        public void Update_Contact_ExpectedFailedUpdateFlag()
        {
            mockDataContext.Setup((item) => item.Context).Returns(mockAddressBook.Object);

            var contactRepository = new DataRepository.ContactRepository(mockLogger.Object, mockDatabaseSettings.Object, mockDataContext.Object);

            List<DataModel.Contact> contactList = contactModelList.ToList();

            DataModel.Contact updateContactModel = new DataModel.Contact() { Id = 10, FirstName = "Street", LastName = "City", Type = contactTypeModelList.First() };

            DateTime oldUpdatedTime = updateContactModel.Updated;

            mockAddressBook.Setup((item) => item.SaveChanges()).Returns(0);
            mockAddressBook.Setup((item) => item.ContactTypes).Returns(mockedContactTypeDBSet.Object);
            mockAddressBook.Setup((item) => item.Contacts).Returns(mockedContactDBSet.Object);
            mockAddressBook.Setup((item) => item.AddressList).Returns(mockedAddressDBSet.Object);

            Assert.IsFalse(contactRepository.Update(updateContactModel));
            Assert.AreEqual(updateContactModel.Updated, oldUpdatedTime);
            Assert.IsNull(mockAddressBook.Object.Contacts.SingleOrDefault((item) => item.Id == updateContactModel.Id));
        }
    }
}

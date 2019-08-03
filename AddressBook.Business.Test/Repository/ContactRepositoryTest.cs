using NUnit.Framework;
using BusinessRepository = AddressBookBusinessLib.Repository;
using DataInterface = AddressBookDataLib.Interface;
using DataModel = AddressBookDataLib.Model;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;

namespace AddressBookBusinessLibTest.Repository
{
    public class ContactRepositoryTest
    {
        Mock<DataInterface.IContactRepository> mockContactRepository;
        Mock<ILogger<BusinessRepository.ContactRepository>> mockLogger;
        DataModel.Contact contactModel;

        [SetUp]
        public void Setup()
        {
            mockContactRepository = new Mock<DataInterface.IContactRepository>();
            mockLogger = new Mock<ILogger<BusinessRepository.ContactRepository>>();
            contactModel = new DataModel.Contact();
        }

        [Test]
        public void Create_ContactRecord_ExpectedSuccessfulCreationFlag()
        {

            var contactRepository = new BusinessRepository.ContactRepository(mockLogger.Object,
                mockContactRepository.Object
            );

            mockContactRepository.Setup((item) => item.Create(contactModel)).Returns(true);

            Assert.IsTrue(contactRepository.Create(contactModel));
        }


        [Test]
        public void Create_ContactRecord_ExpectedFailedCreationFlag()
        {

            var contactRepository = new BusinessRepository.ContactRepository(mockLogger.Object,
                mockContactRepository.Object
            );

            mockContactRepository.Setup((item) => item.Create(contactModel)).Returns(false);

            Assert.IsFalse(contactRepository.Create(contactModel));
        }

        [Test]
        public void ReadAll_ContactRecord_ExpectedAllAdddressList()
        {

            DataModel.Contact contact1, contact2;
            mockContactRepository.Setup((item) => item.ReadAll()).Returns(new DataModel.Contact[] {
                contact1 = new DataModel.Contact() {Id = 1, FirstName = "FirstName", LastName = "LastName"},
                contact2 = new DataModel.Contact() {Id = 2, FirstName = "FirstName1", LastName = "LastName"}
            });

            var contactRepository = new BusinessRepository.ContactRepository(mockLogger.Object,
                mockContactRepository.Object
            );

            var result = contactRepository.ReadAll();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.SingleOrDefault((item) => item.Id == 1).Id == 1);
            Assert.IsTrue(result.SingleOrDefault((item) => item.FirstName == "FirstName1").Id == 2);
            Assert.IsTrue(result.SingleOrDefault((item) => item.FirstName == "FirstName1").FullName == GetFullName(contact2));
        }


        [Test]
        public void Get_ReadAll_ExpectedEmptyResult()
        {
            mockContactRepository.Setup((item) => item.ReadAll()).Returns(new DataModel.Contact[] { });

            var contactRepository = new BusinessRepository.ContactRepository(mockLogger.Object,
                mockContactRepository.Object
            );

            var result = contactRepository.ReadAll();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            Assert.IsNull(result.SingleOrDefault((item) => item.Id == 1));
            Assert.IsNull(result.SingleOrDefault((item) => item.FirstName == "FirstName"));

        }


        [Test]
        public void ReadById_ContactRecord_ExpectedContactRecordForProvidedContactId()
        {

            int contactId = 1;
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

            mockContactRepository.Setup((item) => item.Read(contactId)).Returns(
                contact
            );

            var contactRepository = new BusinessRepository.ContactRepository(mockLogger.Object,
                mockContactRepository.Object
            );

            var result = contactRepository.Read(contactId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.FullName == GetFullName(contact));
            Assert.IsTrue(result.ContactType == contact.Type.Id);
        }

        [Test]
        public void ReadById_AddressRecord_ExpectedNoRecordsProvidedAddressId()
        {
            int contactId = 1;

            var contactRepository = new BusinessRepository.ContactRepository(mockLogger.Object,
                mockContactRepository.Object
            );

            var result = contactRepository.Read(contactId);

            Assert.IsNull(result);
        }


        [Test]
        public void Update_ContactRecord_ExpectedSuccessfulUpdateFlag()
        {

            var contactRepository = new BusinessRepository.ContactRepository(mockLogger.Object,
                mockContactRepository.Object
            );


            mockContactRepository.Setup((item) => item.Update(contactModel)).Returns(true);

            Assert.IsTrue(contactRepository.Update(contactModel));
        }


        [Test]
        public void Update_ContactRecord_ExpectedFailedUpdateFlag()
        {

            var contactRepository = new BusinessRepository.ContactRepository(mockLogger.Object,
                mockContactRepository.Object
            );

            mockContactRepository.Setup((item) => item.Update(contactModel)).Returns(false);

            Assert.IsFalse(contactRepository.Update(contactModel));
        }

        [Test]
        public void Delete_ConactRecord_ExpectedSuccessfulDeleteFlag()
        {

            int contactId = 1;

            var contactRepository = new BusinessRepository.ContactRepository(mockLogger.Object,
                mockContactRepository.Object
            );


            mockContactRepository.Setup((item) => item.Delete(contactId)).Returns(true);

            Assert.IsTrue(contactRepository.Delete(contactId));
        }


        [Test]
        public void Delete_ContactRecord_ExpectedFailedDeleteFlag()
        {
            int contactId = 1;

            var contactRepository = new BusinessRepository.ContactRepository(mockLogger.Object,
                mockContactRepository.Object
            );

            mockContactRepository.Setup((item) => item.Delete(contactId)).Returns(false);

            Assert.IsFalse(contactRepository.Delete(contactId));
        }


        private string GetFullName(DataModel.Contact contact)
        {
            return contact.FirstName + " " + contact.LastName;
        }
    }
}

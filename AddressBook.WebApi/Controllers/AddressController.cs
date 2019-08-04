using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessInterface = AddressBookBusinessLib.Interface;
using BusinessModel = AddressBookBusinessLib.Model;

namespace AddressBook.WebApi.Controllers
{
    /// <summary>
    /// Address Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        BusinessInterface.IAddressRepository addressRepository;
        
        /// <summary>
        /// Address Controller
        /// </summary>
        /// <param name="addressRepository">Address Repository Interface</param>
        public AddressController(BusinessInterface.IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
        }
        
        
        // GET api/Address
        /// <summary>
        /// Use to retrive all active address of all contacts 
        /// </summary>
        /// <returns>Returns array of address object</returns>
        [HttpGet]
        public ActionResult<IEnumerable<BusinessModel.Address>> Get()
        {
            return addressRepository.ReadAll().ToList();
        }

        // GET api/Address/{Id}
        /// <summary>
        /// Use to retrive specific address object for the provided Id
        /// </summary>
        /// <param name="id">Record of the specific record which has to be retrived</param>
        /// <returns>Address object for the provided valid record id</returns>
        [HttpGet("{id}")]
        public ActionResult<BusinessModel.Address> Get(int id)
        {
            return addressRepository.Read(id);
        }

        // GET api/Address/ByContact/{contactId}
        /// <summary>
        /// Use to retrive all address associated with the provided contact id, where contact id refers to the record of Contact repository
        /// </summary>
        /// <param name="contactId">Contact record id, which is specified to retrive address associated</param>
        /// <returns>Array of address object associated to the Contact record</returns>
        [HttpGet, Route("ByContact/{contactId}")]
        public ActionResult<IEnumerable<BusinessModel.Address>> GetByContactId(int contactId)
        {
            return addressRepository.ReadByContactId(contactId).ToList();
        }

        // POST api/Address
        /// <summary>
        /// Use to create new Address entry with the provided address model
        /// </summary>
        /// <param name="address">Object encapsulates the address information</param>
        /// <returns>Status OK if success and NOT FOUND when creation failed</returns>
        [HttpPost]
        public ActionResult Post([FromBody] BusinessModel.Address address)
        {
            if(addressRepository.Create(address))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Use to update any existing address record
        /// </summary>
        /// <param name="id">Address record id, in which the changes has to be updated</param>
        /// <param name="address">Changes encapsulated as an address object</param>
        /// <returns>Status OK if success and NOT FOUND when update failed</returns>
        // PUT api/Address/{Id}
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] BusinessModel.Address address)
        {

            address.Id = id;
            if (addressRepository.Update(address))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete any existing address record
        /// </summary>
        /// <param name="id">Address record id, which is required to be deleted</param>
        // DELETE api/Address/{Id}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            addressRepository.Delete(id);
        }
    }
}

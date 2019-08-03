using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessInterface = AddressBookBusinessLib.Interface;
using BusinessModel = AddressBookBusinessLib.Model;

namespace AddressBook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        BusinessInterface.IAddressRepository addressRepository;

        public AddressController(BusinessInterface.IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
        }
        
        
        // GET api/Address
        [HttpGet]
        public ActionResult<IEnumerable<BusinessModel.Address>> Get()
        {
            return addressRepository.ReadAll().ToList();
        }

        // GET api/Address/{Id}
        [HttpGet("{id}")]
        public ActionResult<BusinessModel.Address> Get(int id)
        {
            return addressRepository.Read(id);
        }

        // GET api/Address/ByContact/{contactId}
        [HttpGet, Route("ByContact/{contactId}")]
        public ActionResult<IEnumerable<BusinessModel.Address>> GetByContactId(int contactId)
        {
            return addressRepository.ReadByContactId(contactId).ToList();
        }

        // POST api/Address
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

        // DELETE api/Address/{Id}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            addressRepository.Delete(id);
        }
    }
}

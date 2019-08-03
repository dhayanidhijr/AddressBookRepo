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
    public class ContactController : ControllerBase
    {
        BusinessInterface.IContactRepository contactRepository;

        public ContactController(BusinessInterface.IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }
        
        
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<BusinessModel.Contact>> Get()
        {
            return contactRepository.ReadAll().ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<BusinessModel.Contact> Get(int id)
        {
            return contactRepository.Read(id);
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] BusinessModel.Contact contact)
        {
            if (contactRepository.Create(contact))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] BusinessModel.Contact contact)
        {
            contact.Id = id;
            if (contactRepository.Update(contact))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            contactRepository.Delete(id);
        }
    }
}

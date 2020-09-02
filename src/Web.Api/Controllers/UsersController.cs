using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Models.Request;

using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: api/<UsersController>
        [Authorize]
        [HttpGet]
        public async Task<object> Get()
        {
            IPagedCollection<User> users = _userRepository.FindAll();
            Console.Out.WriteLine(User.Identity.Name);
            return new
            {
                Page = 1,
                TotalPages = users.TotalPages(),
                TotalItems = users.TotalItems(),
                Data = await users.GetItemsForPage(1),
            };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(string id)
        {
            return await _userRepository.FindById(id);
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] RegisterUserRequest request)
        {
            
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

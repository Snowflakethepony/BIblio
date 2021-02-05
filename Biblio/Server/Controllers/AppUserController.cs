using AutoMapper;
using Biblio.Server.Interfaces;
using Biblio.Shared.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IMapper _mapper;

        public AppUserController(IRepositoryWrapper wrapper, IMapper mapper)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
        }

        /// <summary>
        /// All get user currently assosiated with a specific library by it's ID.
        /// </summary>
        /// <param name="libraryId">Id of the library.</param>
        /// <returns>List of app users found.</returns>
        [HttpGet]
        public async Task<ActionResult<List<ApplicationUserDTO>>> GetAllApplicationUsersByLibrary(int libraryId)
        {
            return Ok(_mapper.Map<List<ApplicationUserDTO>>(await _wrapper.AppUserRepository.GetAllApplicationUsersByLibrary(libraryId)));
        }

        /// <summary>
        /// Get user by ID.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>App user found</returns>
        [HttpGet]
        public async Task<ActionResult<ApplicationUserDTO>> GetApplicationUserById(string id)
        {
            return Ok(_mapper.Map<ApplicationUserDTO>(await _wrapper.AppUserRepository.GetApplicationUserById(id)));
        }

        /// <summary>
        /// Get user from a rented books ID.
        /// </summary>
        /// <param name="bookCopyId">Id of the book rented by the user.</param>
        /// <returns>User who has the book.</returns>
        [HttpGet]
        public async Task<ActionResult<ApplicationUserDTO>> GetApplicationUserByBookId(int bookCopyId)
        {
            return Ok(_mapper.Map<ApplicationUserDTO>(await _wrapper.AppUserRepository.GetApplicationUserByBookId(bookCopyId)));
        }
    }
}

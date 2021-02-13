using AutoMapper;
using Biblio.Server.Interfaces;
using Biblio.Shared.Models;
using Biblio.Shared.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<AppUser> _usermanager;

        public AppUserController(IRepositoryWrapper wrapper, IMapper mapper, UserManager<AppUser> usermanager)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
            this._usermanager = usermanager;
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

        [HttpGet]
        public async Task<ActionResult<ApplicationUserDTO>> GetApplicationUserByUsername(string username)
        {
            return Ok(_mapper.Map<ApplicationUserDTO>(await _wrapper.AppUserRepository.GetApplicationUserByUsername(username)));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUserLibrary(string username, int libraryId)
        {
            var user = await _usermanager.FindByNameAsync(username);

            user.HomeLibraryId = libraryId;

            _wrapper.AppUserRepository.UpdateAppUser(user);

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> GiveUserLibraryAdminRole(string userId, string libraryName)
        {
            var user = await _usermanager.FindByIdAsync(userId);

            string[] roles =
            {
                "LibraryAdmin",
                libraryName
            };

            try
            {
                await _usermanager.AddToRolesAsync(user, roles);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppUser(string appUserId, [FromBody] ApplicationUserDTO appUser)
        {
            if (appUserId != appUser.Id)
            {
                return BadRequest();
            }

            _wrapper.AppUserRepository.UpdateAppUser(_mapper.Map<AppUser>(appUser));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!await AppUserExists(appUserId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }

            return Ok();
        }

        private async Task<bool> AppUserExists(string id)
        {
            return await _wrapper.AppUserRepository.GetApplicationUserById(id) != null;
        }
    }
}

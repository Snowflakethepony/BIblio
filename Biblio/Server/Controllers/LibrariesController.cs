using AutoMapper;
using Biblio.Server.Interfaces;
using Biblio.Shared.Models;
using Biblio.Shared.Models.DTOs;
using Microsoft.AspNetCore.Http;
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
    public class LibrariesController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IMapper _mapper;

        public LibrariesController(IRepositoryWrapper wrapper, IMapper mapper)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<LibraryDTOMinusRelations>>> GetAllLibraries()
        {
            return Ok(_mapper.Map<List<LibraryDTOMinusRelations>>(await _wrapper.LibraryRepository.GetAllLibraries()));
        }

        [HttpGet]
        public async Task<ActionResult<LibraryDTOMinusRelations>> GetLibrayByName(string name)
        {
            return Ok(_mapper.Map<LibraryDTOMinusRelations>(await _wrapper.LibraryRepository.GetLibraryByName(name)));
        }

        [HttpPost]
        public async Task<ActionResult> CreateLibraryFromApplication([FromBody] ApplicationDTO applicationDto)
        {
            // Set application to handled
            applicationDto.IsHandled = true;
            applicationDto.IsVerified = true;

            // Created new library from the application
            var newLibray = new Library()
            {
                AddressLine = applicationDto.AddressLine,
                City = applicationDto.City,
                EmailAddress = applicationDto.EmailAddress,
                Name = applicationDto.Name,
                PostalCode = applicationDto.PostalCode,
                PhoneNumber = applicationDto.PhoneNumber

            };

            // Add to context
            _wrapper.LibraryRepository.CreateLibrary(newLibray);
            _wrapper.ApplicationRepository.UpdateApplication(_mapper.Map<Application>(applicationDto));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}

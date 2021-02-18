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
    public class ApplicationsController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IMapper _mapper;

        public ApplicationsController(IRepositoryWrapper wrapper, IMapper mapper)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationDTO>>> GetAllOpenApplications()
        {
            return Ok(_mapper.Map<List<ApplicationDTO>>(await _wrapper.ApplicationRepository.GetAllOpenApplications()));
        }

        [HttpGet]
        public async Task<ActionResult<ApplicationDTO>> GetApplicationById(int applicationId)
        {
            return Ok(_mapper.Map<ApplicationDTO>(await _wrapper.ApplicationRepository.GetApplicationById(applicationId)));
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationDTO>> CreateApplication([FromBody] ApplicationDTO application)
        {
            _wrapper.ApplicationRepository.CreateApplication(_mapper.Map<Application>(application));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(CreateApplication), new { id = application.ApplicationId }, application);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateApplication([FromBody] ApplicationDTO application)
        {

            _wrapper.ApplicationRepository.UpdateApplication(_mapper.Map<Application>(application));

            try
            {
                // Save changes
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteApplication(int applicationId)
        {
            var application = await _wrapper.ApplicationRepository.GetApplicationById(applicationId);

            if (application == null)
            {
                return NotFound();
            }

            _wrapper.ApplicationRepository.DeleteApplication(application);

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

        private async Task<bool> BookCopyExists(int id)
        {
            return await _wrapper.BookCopyRepository.GetBookCopyById(id) != null;
        }
    }
}

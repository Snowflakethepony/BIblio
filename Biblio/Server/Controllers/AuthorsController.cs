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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IMapper _mapper;

        public AuthorsController(IRepositoryWrapper wrapper, IMapper mapper)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorDTO>>> GetAuthors()
        {
            return Ok(_mapper.Map<List<AuthorDTO>>(await _wrapper.AuthorRepository.GetAllAuthors()));
        }

        [HttpGet]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int authorId)
        {
            return Ok(_mapper.Map<AuthorDTO>(await _wrapper.AuthorRepository.GetAuthorById(authorId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorDTO>>> GetAuthorsLikePseudonym(string pseudonym)
        {
            return Ok(_mapper.Map<List<AuthorDTO>>(await _wrapper.AuthorRepository.GetAuthorsByPseudonym(pseudonym)));
        }

        [HttpGet]
        public async Task<ActionResult<AuthorDTO>> GetAuthorByPseudonym(string pseudonym)
        {
            return Ok(_mapper.Map<AuthorDTO>(await _wrapper.AuthorRepository.GetAuthorByPseudonym(pseudonym)));
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> CreateAuthor([FromBody] AuthorDTO author)
        {
            _wrapper.AuthorRepository.CreateAuthor(_mapper.Map<Author>(author));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(CreateAuthor), new { id = author.AuthorId }, author);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAuthor(int authorId, [FromBody] AuthorDTO author)
        {
            if (authorId != author.AuthorId)
            {
                return BadRequest();
            }

            _wrapper.AuthorRepository.UpdateAuthor(_mapper.Map<Author>(author));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!await AuthorExists(authorId))
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

        [HttpDelete]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            var author = await _wrapper.AuthorRepository.GetAuthorById(authorId);

            if (author == null)
            {
                return NotFound();
            }

            _wrapper.AuthorRepository.DeleteAuthor(author);

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

        private async Task<bool> AuthorExists(int id)
        {
            return await _wrapper.AuthorRepository.GetAuthorById(id) != null;
        }
    }
}

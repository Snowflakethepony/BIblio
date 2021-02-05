using AutoMapper;
using Biblio.Server.Interfaces;
using Biblio.Shared.Models;
using Biblio.Shared.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
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
    public class GenresController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IMapper _mapper;

        public GenresController(IRepositoryWrapper wrapper, IMapper mapper)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> GetGenres()
        {
            return Ok(_mapper.Map<List<GenreDTO>>(await _wrapper.GenreRepository.GetAllGenres()));
        }

        [HttpGet]
        public async Task<ActionResult<GenreDTO>> GetGenre(int genreId)
        {
            var genre = await _wrapper.GenreRepository.GetGenreById(genreId);

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GenreDTO>(genre));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateGenre(int genreId, [FromBody] GenreDTO genre)
        {
            if (genreId != genre.GenreId)
            {
                return BadRequest();
            }

            _wrapper.GenreRepository.UpdateGenre(_mapper.Map<Genre>(genre));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!await GenreExists(genreId))
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GenreDTO>> CreateGenre([FromBody] GenreDTO genre)
        {
            _wrapper.GenreRepository.CreateGenre(_mapper.Map<Genre>(genre));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(CreateGenre), new { id = genre.GenreId }, genre);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteGenre(int genreId)
        {
            var genre = await _wrapper.GenreRepository.GetGenreById(genreId);

            if (genre == null)
            {
                return NotFound();
            }

            _wrapper.GenreRepository.DeleteGenre(genre);

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

        private async Task<bool> GenreExists(int id)
        {
            return await _wrapper.GenreRepository.GetGenreById(id) != null;
        }
    }
}

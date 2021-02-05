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
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IMapper _mapper;

        public BooksController(IRepositoryWrapper wrapper, IMapper mapper)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDTO>>> GetBooks()
        {
            return Ok(_mapper.Map<List<BookDTO>>(await _wrapper.BookRepository.GetAllBooks()));
        }

        [HttpGet]
        public async Task<ActionResult<BookDTO>> GetBook(int bookId)
        {
            return Ok(_mapper.Map<BookDTO>(await _wrapper.BookRepository.GetBookById(bookId)));
        }

        [HttpGet]
        public async Task<ActionResult<BookDTO>> GetBookByIdIncludingRelations(int bookId)
        {
            return Ok(_mapper.Map<BookDTO>(await _wrapper.BookRepository.GetBookByIdIncludingRelations(bookId)));
        }

        [HttpGet]
        public async Task<ActionResult<BookDTO>> GetBookByTitle(string title)
        {
            return Ok(_mapper.Map<BookDTO>(await _wrapper.BookRepository.GetBookByTitle(title)));
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDTO>>> GetBooksByAuthor(int authorId)
        {
            return Ok(_mapper.Map<List<BookDTO>>(await _wrapper.BookRepository.GetBooksByAuthor(authorId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDTO>>> GetBooksByGenre(int genreId)
        {
            return Ok(_mapper.Map<List<BookDTO>>(await _wrapper.BookRepository.GetBooksByGenre(genreId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDTO>>> SearchBooksByTitle(string title)
        {
            return Ok(_mapper.Map<List<BookDTO>>(await _wrapper.BookRepository.SearchBooksByTitle(title)));
        }

        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook([FromBody] BookDTO book)
        {
            _wrapper.BookRepository.CreateBook(_mapper.Map<Book>(book));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(CreateBook), new { id = book.BookId }, book);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook(int bookId, [FromBody] BookDTO book)
        {
            if (bookId != book.BookId)
            {
                return BadRequest();
            }

            _wrapper.BookRepository.UpdateBook(_mapper.Map<Book>(book));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!await BookExists(bookId))
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
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var book = await _wrapper.BookRepository.GetBookById(bookId);

            if (book == null)
            {
                return NotFound();
            }

            _wrapper.BookRepository.DeleteBook(book);

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

        private async Task<bool> BookExists(int id)
        {
            return await _wrapper.BookRepository.GetBookById(id) != null;
        }
    }
}

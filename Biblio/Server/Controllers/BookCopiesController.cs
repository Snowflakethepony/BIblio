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
    public class BookCopiesController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _usermanager;

        public BookCopiesController(IRepositoryWrapper wrapper, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
            this._usermanager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookCopyDTO>>> GetAllBookCopiesForUser(string username)
        {
            var user = await _usermanager.FindByNameAsync(username);

            return Ok(_mapper.Map<List<BookCopyDTO>>(await _wrapper.BookCopyRepository.GetAllBookCopiesForUser(user.Id)));
        }

        [HttpGet]
        public async Task<ActionResult<List<BookCopyDTO>>> GetAllBookCopiesByBookId(int bookId)
        {
            return Ok(_mapper.Map<List<BookCopyDTO>>(await _wrapper.BookCopyRepository.GetAllBookCopiesByBookId(bookId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<BookCopyDTO>>> GetAllBookCopiesByBookIdForLibrary(int bookId, int libraryId)
        {
            return Ok(_mapper.Map<List<BookCopyDTO>>(await _wrapper.BookCopyRepository.GetAllBookCopiesByBookIdForLibrary(bookId, libraryId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<BookCopyDTO>>> GetAllBookCopiesForLibrary(int libraryId)
        {
            return Ok(_mapper.Map<List<BookCopyDTO>>(await _wrapper.BookCopyRepository.GetAllBookCopiesForLibrary(libraryId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<BookCopyDTO>>> FindBookCopiesByQuery(string type, string queryText)
        {
            IEnumerable<BookCopy> bookCopies = new List<BookCopy>();

            if (type == "Author")
            {
                bookCopies = await _wrapper.BookCopyRepository.GetAllBookCopiesByAuthor(queryText);
            }
            else if (type == "Title")
            {
                bookCopies = await _wrapper.BookCopyRepository.GetAllBookCopiesByBookTitle(queryText);
            }
            else if (type == "Genre")
            {
                bookCopies = await _wrapper.BookCopyRepository.GetAllBookCopiesByGenre(queryText);
            }
            else if (type == "RFID")
            {
                bookCopies.Append(await _wrapper.BookCopyRepository.GetBookCopyByRFID(queryText));
            }

            return Ok(_mapper.Map<List<BookCopyDTO>>(bookCopies));
        }


        [HttpGet]
        public async Task<ActionResult<List<BookCopyDTO>>> GetAllBookCopiesReturnOverdueForLibrary(int libraryId)
        {
            return Ok(_mapper.Map<List<BookCopyDTO>>(await _wrapper.BookCopyRepository.GetAllBookCopiesReturnOverdueForLibrary(libraryId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<BookCopyDTO>>> GetBooksAvailableByRFID(string RFIDValues)
        {
            string[] RFIDs = RFIDValues.Split(' ');
            List<BookCopyDTO> books = new List<BookCopyDTO>();

            foreach (var item in RFIDs)
            {
                books.Add(_mapper.Map<BookCopyDTO>(await _wrapper.BookCopyRepository.GetBookCopyByRFID(item)));
            }

            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult<BookCopyDTO>> CreateBookCopy([FromBody] BookCopyDTO bookCopy)
        {
            _wrapper.BookCopyRepository.CreateBookCopy(_mapper.Map<BookCopy>(bookCopy));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(CreateBookCopy), new { id = bookCopy.BookCopyId }, bookCopy);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBookCopyFromDTO(int bookCopyId, [FromBody] BookCopyDTO bookCopy)
        {
            if (bookCopyId != bookCopy.BookCopyId)
            {
                return BadRequest();
            }

            var mappedBook = _mapper.Map<BookCopy>(_mapper.Map<BookCopyDTOMinusRelations>(bookCopy));
            _wrapper.BookCopyRepository.UpdateBookCopy(mappedBook);

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!await BookCopyExists(bookCopyId))
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

        [HttpPut]
        public async Task<ActionResult> UpdateBooksAvailableByRFID(string RFIDValues)
        {
            // Split RFIDs
            string[] RFIDs = RFIDValues.Split(' ');

            // List for all found bookcopies
            List<BookCopy> books = new List<BookCopy>();

            // Forech ID find the assosiated bookcopy
            foreach (var item in RFIDs)
            {
                try
                {
                    books.Add(await _wrapper.BookCopyRepository.GetBookCopyByRFID(item));
                }
                catch
                {

                }
            }

            // Foreach found book set not available
            foreach (var book in books)
            {
                book.IsAvailable = false;
                _wrapper.BookCopyRepository.UpdateBookCopy(book);
            }

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

        [HttpPut]
        public async Task<ActionResult> RentBooks([FromBody] List<BookCopyDTO> bookCopiesDto)
        {
            var books = _mapper.Map<List<BookCopy>>(bookCopiesDto);

            // Foreach found book set properties for renting purposes
            foreach (var book in books)
            {
                book.IsAvailable = false;
                book.BorrowedAt = DateTime.Now;
                book.ReturnBy = DateTime.Now.AddDays(14);

                _wrapper.BookCopyRepository.UpdateBookCopy(book);
            }

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
        public async Task<IActionResult> DeleteBookCopy(int bookCopyId)
        {
            var bookCopy = await _wrapper.BookCopyRepository.GetBookCopyById(bookCopyId);

            if (bookCopy == null)
            {
                return NotFound();
            }

            _wrapper.BookCopyRepository.DeleteBookCopy(bookCopy);

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

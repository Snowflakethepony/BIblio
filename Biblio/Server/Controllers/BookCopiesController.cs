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
        private readonly UserManager<AppUser> _usermanager;

        public BookCopiesController(IRepositoryWrapper wrapper, IMapper mapper, UserManager<AppUser> userManager)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
            this._usermanager = userManager;
        }

        /// <summary>
        /// Gets all bookcopies borrowed by a user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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
        public async Task<ActionResult<List<BookCopyDTO>>> GetBooksAvailableByRFID(string RFIDValues, string userId)
        {
            var reservations = await _wrapper.ReservationRepository.GetAllReservationsByUser(userId);
            List<string> RFIDs = new List<string>();
            RFIDs.AddRange(RFIDValues.Split(' '));
            List<BookCopyDTO> books = new List<BookCopyDTO>();

            foreach (var item in RFIDs)
            {
                // If reservation on it get the book
                if (reservations.Any(r => r.ReservedCopy.RFID == item))
                {
                    books.Add(_mapper.Map<BookCopyDTO>(await _wrapper.BookCopyRepository.GetBookCopyByRFID(item)));
                }
                else
                {
                    var book = _mapper.Map<BookCopyDTO>(await _wrapper.BookCopyRepository.GetAvailableBookCopyByRFID(item));

                    if (book != null)
                    {
                        books.Add(book);
                    }
                }
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
                    books.Add(await _wrapper.BookCopyRepository.GetBookCopyByRFIDNoRelations(item));
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
        public async Task<ActionResult> CheckinBookstoLibrayByRFID(string libraryName, string RFIDValues)
        {
            libraryName = "Horsens Bibliotek";
            // Get library
            var library = await _wrapper.LibraryRepository.GetLibraryByName(libraryName);

            // Split RFIDs
            string[] RFIDs = RFIDValues.Split(' ');

            // List for all found bookcopies
            List<BookCopy> books = new List<BookCopy>();

            // Forech ID find the assosiated bookcopy
            foreach (var item in RFIDs)
            {
                try
                {
                    books.Add(await _wrapper.BookCopyRepository.GetBookCopyByRFIDNoRelations(item));
                }
                catch
                {

                }
            }

            // Foreach found book set new library
            foreach (var book in books)
            {
                book.CurrentLibraryId = library.LibraryId;
                _wrapper.BookCopyRepository.UpdateBookCopy(book);

                // Check for active reservation 
                var reservation = await _wrapper.ReservationRepository.GetReservationByBookCopyId(book.BookCopyId);

                // Update it with an expiration date if any
                if (reservation != null)
                {
                    reservation.ExpirationDate = DateTime.Now.AddDays(3);
                    _wrapper.ReservationRepository.UpdateReservation(reservation);
                }
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
        public async Task<ActionResult<DateTime>> ExtentBorrowPeriodById(int bookCopyId)
        {
            // List for all found bookcopies
            var book = await _wrapper.BookCopyRepository.GetBookCopyById(bookCopyId);

            // Serverside check
            if (book.TimesRerented >= BookCopy.MaxRerents)
            {
                return BadRequest("Reached max rerents!");
            }

            book.ReturnBy = book.ReturnBy?.AddDays(14);
            book.TimesRerented += 1;

            try
            {
                // Save changes
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(book.ReturnBy);
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

        [HttpPut]
        public async Task<ActionResult> BorrowBooks(string userId, string RFIDValues)
        {
            var reservations = await _wrapper.ReservationRepository.GetAllReservationsByUser(userId);
            var books = new List<BookCopy>();

            foreach (var id in RFIDValues.Split(' '))
            {
                books.Add(await _wrapper.BookCopyRepository.GetBookCopyByRFIDNoRelations(id));
            }

            // Foreach found book set properties for renting purposes
            foreach (var book in books)
            {
                // Check if there was a reservation for the book. If set it for deletion.
                if (reservations.Any(r => r.ReservedCopyId == book.BookCopyId))
                {
                    _wrapper.ReservationRepository.DeleteReservation(reservations.FirstOrDefault(r => r.ReservedCopyId == book.BookCopyId));
                }

                book.IsAvailable = false;
                book.BorrowedAt = DateTime.Now;
                book.ReturnBy = DateTime.Now.AddDays(14);
                book.BorrowerId = userId;

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
        public async Task<ActionResult> ReturnBooks(string RFIDValues)
        {
            var books = new List<BookCopy>();

            foreach (var id in RFIDValues.Split(' '))
            {
                books.Add(await _wrapper.BookCopyRepository.GetBookCopyByRFIDNoRelations(id));
            }

            // Foreach found book set properties for renting purposes
            foreach (var book in books)
            {
                _wrapper.BorrowedBookHistoryRepository.CreateRentedBookHistory(new BorrowedBookHistory()
                {
                    BookCopyId = book.BookCopyId,
                    BorrowedAt = (DateTime)book.BorrowedAt,
                    ReturnedAt = DateTime.Now,
                    TimesRerented = (int)book.TimesRerented,
                    BorrowerId = book.BorrowerId
                });

                book.IsAvailable = true;
                book.BorrowedAt = null;
                book.ReturnBy = null;
                book.BorrowerId = null;
                book.TimesRerented = 0;

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

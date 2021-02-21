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
    public class ReservationsController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _usermanager;

        public ReservationsController(IRepositoryWrapper wrapper, IMapper mapper, UserManager<AppUser> userManager)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
            this._usermanager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReservationDTO>>> GetAllReservationsByUser(string username)
        {
            var user = await _usermanager.FindByNameAsync(username);

            return Ok(_mapper.Map<List<ReservationDTO>>(await _wrapper.ReservationRepository.GetAllReservationsByUser(user.Id)));
        }

        [HttpGet]
        public async Task<ActionResult<List<ReservationDTO>>> GetAllReservationsByLibray(int librayId)
        {
            return Ok(_mapper.Map<List<ReservationDTO>>(await _wrapper.ReservationRepository.GetAllReservationByLibrary(librayId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<ReservationDTO>>> GetReservationsByBookTitle(string title)
        {
            return Ok(_mapper.Map<List<ReservationDTO>>(await _wrapper.ReservationRepository.GetReservationsByBookTitle(title)));
        }

        [HttpGet]
        public async Task<ActionResult<ReservationDTO>> GetReservationById(int id)
        {
            return Ok(_mapper.Map<ReservationDTO>(await _wrapper.ReservationRepository.GetReservationById(id)));
        }

        [HttpGet]
        public async Task<ActionResult<bool>> IsBookCopyReservedByUser(string username, int bookCopyId)
        {
            var user = await _usermanager.FindByNameAsync(username);

            return Ok(await _wrapper.ReservationRepository.DoesReservationExistForUserByBookCopyId(user.Id, bookCopyId));
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDTO>> CreateReservationByBookCopyId(string username, int bookCopyId)
        {
            var user = await _usermanager.FindByNameAsync(username);
            var bookCopy = await _wrapper.BookCopyRepository.GetBookCopyById(bookCopyId);

            var isForeign = !(user.HomeLibraryId == bookCopy.OriginLibraryId);

            var reservation = new Reservation()
            {
                IsForeignBorrower = isForeign,
                ExpirationDate = isForeign ? null : DateTime.Now.AddDays(3),
                LibraryId = bookCopy.OriginLibraryId,
                ReservedAt = DateTime.Now,
                ReservedById = user.Id,
                ReservedCopyId = bookCopy.BookCopyId
            };

            // Set book to unavailable
            bookCopy.IsAvailable = false;

            _wrapper.BookCopyRepository.UpdateBookCopy(_mapper.Map<BookCopy>(bookCopy));
            _wrapper.ReservationRepository.CreateReservation(reservation);

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(CreateReservationByBookCopyId), new { id = reservation.ReservationId }, _mapper.Map<ReservationDTO>(reservation));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateReservation(int reservationId, [FromBody] ReservationDTO reservation)
        {
            if (reservationId != reservation.ReservationId)
            {
                return BadRequest();
            }

            _wrapper.ReservationRepository.UpdateReservation(_mapper.Map<Reservation>(reservation));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!await ReservationExists(reservationId))
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

        [HttpGet]
        public async Task<IActionResult> DeleteReservationForUserByBookCopyId(string username, int bookCopyId)
        {
            // Get user
            var user = await _usermanager.FindByNameAsync(username);

            // Look for existing reservation.
            var reservation = await _wrapper.ReservationRepository.GetReservationForUserByBookCipyId(user.Id, bookCopyId);

            var bookCopy = await _wrapper.BookCopyRepository.GetBookCopyById(bookCopyId);

            if (reservation == null || bookCopy == null)
            {
                return NotFound();
            }
            else
            {
                bookCopy.IsAvailable = true;

                _wrapper.BookCopyRepository.UpdateBookCopy(bookCopy);
                _wrapper.ReservationRepository.DeleteReservation(reservation);
            }

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

        [HttpDelete]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            var reservation = await _wrapper.ReservationRepository.GetReservationById(reservationId);

            if (reservation == null)
            {
                return NotFound();
            }
            else
            {
                var bookCopy = await _wrapper.BookCopyRepository.GetBookCopyById(reservation.ReservedCopyId);
                bookCopy.IsAvailable = true;

                _wrapper.BookCopyRepository.UpdateBookCopy(bookCopy);
                _wrapper.ReservationRepository.DeleteReservation(reservation);
            }

            

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

        private async Task<bool> ReservationExists(int id)
        {
            return await _wrapper.ReservationRepository.GetReservationById(id) != null;
        }
    }
}

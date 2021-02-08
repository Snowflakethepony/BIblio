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
        private readonly UserManager<ApplicationUser> _usermanager;

        public ReservationsController(IRepositoryWrapper wrapper, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
            this._usermanager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReservationDTO>>> GetAllReservationsByUser(string userId)
        {
            return Ok(_mapper.Map<List<ReservationDTO>>(await _wrapper.ReservationRepository.GetAllReservationsByUser(userId)));
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

            return Ok(_wrapper.ReservationRepository.DoesReservationExistForUserByBookCopyId(user.Id, bookCopyId));
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDTO>> CreateReservation([FromBody] ReservationDTO reservation)
        {
            _wrapper.ReservationRepository.CreateReservation(_mapper.Map<Reservation>(reservation));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(CreateReservation), new { id = reservation.ReservationId }, reservation);
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

        [HttpDelete]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            var reservation = await _wrapper.ReservationRepository.GetReservationById(reservationId);

            if (reservation == null)
            {
                return NotFound();
            }

            _wrapper.ReservationRepository.DeleteReservation(reservation);

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

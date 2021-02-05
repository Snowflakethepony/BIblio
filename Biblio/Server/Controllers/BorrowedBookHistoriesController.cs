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
    public class BorrowedBookHistoriesController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IMapper _mapper;

        public BorrowedBookHistoriesController(IRepositoryWrapper wrapper, IMapper mapper)
        {
            this._wrapper = wrapper;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<BorrowedBookHistoryDTO>>> GetAllRentedBookHistoriesForUser(string userId)
        {
            return Ok(_mapper.Map<List<BorrowedBookHistoryDTO>>(await _wrapper.BorrowedBookHistoryRepository.GetAllRentedBookHistoriesForUser(userId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<BorrowedBookHistoryDTO>>> GetAllRentedBookHistoriesForUserByBookId(string userId, int bookId)
        {
            return Ok(_mapper.Map<List<BorrowedBookHistoryDTO>>(await _wrapper.BorrowedBookHistoryRepository.GetAllRentedBookHistoriesForUserByBookId(userId, bookId)));
        }

        [HttpGet]
        public async Task<ActionResult<BorrowedBookHistoryDTO>> GetRentedBookHistoryById(int rentedBookHistoryId)
        {
            return Ok(_mapper.Map<BorrowedBookHistoryDTO>(await _wrapper.BorrowedBookHistoryRepository.GetRentedBookHistoryById(rentedBookHistoryId)));
        }

        [HttpGet]
        public async Task<ActionResult<BorrowedBookHistoryDTO>> GetRentedBookHistoryByIdIncludingRelations(int rentedBookHistoryId)
        {
            return Ok(_mapper.Map<BorrowedBookHistoryDTO>(await _wrapper.BorrowedBookHistoryRepository.GetRentedBookHistoryByIdIncludingRelations(rentedBookHistoryId)));
        }

        [HttpPost]
        public async Task<ActionResult<BorrowedBookHistoryDTO>> CreateRentedBookHistory([FromBody] BorrowedBookHistoryDTO rentedBookHistory)
        {
            _wrapper.BorrowedBookHistoryRepository.CreateRentedBookHistory(_mapper.Map<BorrowedBookHistory>(rentedBookHistory));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(CreateRentedBookHistory), new { id = rentedBookHistory.BorrowedBookHistoryId }, rentedBookHistory);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRentedBookHistory(int rentedBookHistoryId, [FromBody] BorrowedBookHistoryDTO rentedBookHistory)
        {
            if (rentedBookHistoryId != rentedBookHistory.BorrowedBookHistoryId)
            {
                return BadRequest();
            }

            _wrapper.BorrowedBookHistoryRepository.UpdateRentedBookHistory(_mapper.Map<BorrowedBookHistory>(rentedBookHistory));

            try
            {
                await _wrapper.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!await RentedBookHistoryExists(rentedBookHistoryId))
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
        public async Task<IActionResult> DeleteRentedBookHistory(int rentedBookHistoryId)
        {
            var rentedBookHistory = await _wrapper.BorrowedBookHistoryRepository.GetRentedBookHistoryById(rentedBookHistoryId);

            if (rentedBookHistory == null)
            {
                return NotFound();
            }

            _wrapper.BorrowedBookHistoryRepository.DeleteRentedBookHistory(rentedBookHistory);

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

        private async Task<bool> RentedBookHistoryExists(int id)
        {
            return await _wrapper.BorrowedBookHistoryRepository.GetRentedBookHistoryById(id) != null;
        }
    }
}

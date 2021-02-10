using AutoMapper;
using Biblio.Server.Interfaces;
using Biblio.Shared.Models;
using Biblio.Shared.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var t = typeof(Library);
            var t2 = Type.GetType(t.FullName + ", " + typeof(Library).Assembly.FullName);
            var t3 = t.GetProperty("Name");

            return Ok(_mapper.Map<LibraryDTOMinusRelations>(await _wrapper.LibraryRepository.GetLibrariesByName(name)));
        }
    }
}

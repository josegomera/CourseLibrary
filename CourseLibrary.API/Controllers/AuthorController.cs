using AutoMapper;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibrary;
        private readonly IMapper _mapper;

        public AuthorController(ICourseLibraryRepository courseLibrary, IMapper mapper)
        {
            _courseLibrary = courseLibrary ??
                throw new ArgumentNullException(nameof(courseLibrary));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthor()
        {
            var authors = _courseLibrary.GetAuthors();
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authors));
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthor(Guid id)
        {
            var author = _courseLibrary.GetAuthor(id);

            if (author == null)
            {
                return NotFound("The Author was not Found");
            }

            return Ok(_mapper.Map<AuthorDto>(author));
        }
    }
}
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public ActionResult<IEnumerable<AuthorDto>> GetAuthor([FromQuery] AuthorResourceParameters authorResource)
        {
            var authors = _courseLibrary.GetAuthors(authorResource);
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authors));
        }

        [HttpGet("{id}", Name ="GetAuthor")]
        public ActionResult GetAuthor(Guid id)
        {
            var author = _courseLibrary.GetAuthor(id);

            if (author == null)
            {
                return NotFound("The Author was not Found");
            }

            return Ok(_mapper.Map<AuthorDto>(author));
        }

        [HttpPost]
        public ActionResult<AuthorDto> Create(AuthorForCreationDTO creationDTO)
        {
            var entity = _mapper.Map<Author>(creationDTO);
            _courseLibrary.AddAuthor(entity);
            _courseLibrary.Save();

            var entityToReturn = _mapper.Map<AuthorDto>(entity);

            return CreatedAtRoute("GetAuthor", new { Id = entityToReturn.Id }, entityToReturn);
        }
    }
}
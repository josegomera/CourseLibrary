using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorCollectionController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibrary;
        private readonly IMapper _mapper;

        public AuthorCollectionController(ICourseLibraryRepository courseLibrary, IMapper mapper)
        {
            _courseLibrary = courseLibrary ??
                throw new ArgumentNullException(nameof(courseLibrary));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection([FromRoute] [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var authorEntities = _courseLibrary.GetAuthors(ids);

            if (ids.Count() != authorEntities.Count())
            {
                return NotFound();
            }

            var authorToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);

            return Ok(authorToReturn);
        }

        public ActionResult<IEnumerable<AuthorDto>> CreateCollection(IEnumerable<AuthorForCreationDTO> authorCollection)
        {
            var authorEntities = _mapper.Map<IEnumerable<Author>>(authorCollection);
            foreach (var author in authorEntities)
            {
                _courseLibrary.AddAuthor(author);
            }

            _courseLibrary.Save();

            var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            var idsAsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));
            return CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, authorCollectionToReturn);

        }
    }
}
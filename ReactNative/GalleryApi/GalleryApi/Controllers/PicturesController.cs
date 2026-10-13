using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GalleryApi.Data;
using GalleryApi.Models;

namespace GalleryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly GalleryApiContext _context;

        public PicturesController(GalleryApiContext context)
        {
            _context = context;
        }

        // GET: api/Pictures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Picture>>> GetPicture()
        {
            return await _context.Picture.ToListAsync();
        }

        // GET: api/Pictures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Picture>> GetPicture(int id)
        {
            var picture = await _context.Picture.FindAsync(id);

            if (picture == null)
            {
                return NotFound();
            }

            return picture;
        }

        // POST: api/Pictures
        [HttpPost]
        public async Task<ActionResult<Picture>> PostPicture(Picture picture)
        {
            // розбиваємо рядок тегів
            string[] tags = picture.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries);

            // прибираємо пробіли, пусті, додаємо # та дублікати всередині одного запиту (ігноруємо регістр)
            tags = tags
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t =>
                {
                    // додаємо # якщо його немає на початку
                    if (t.StartsWith("#"))
                    {
                        return t;
                    }
                    return "#" + t;
                })
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            // зберігаємо назад уже "очищений" рядок тегів у картинку
            picture.Tags = string.Join(",", tags);

            // отримуємо всі існуючі теги з бази
            List<Tag> existingTags = await _context.Tag.ToListAsync();

            List<Tag> newTags = new List<Tag>();

            // додаємо тільки ті, яких ще немає в таблиці Tag (ігноруємо регістр)
            foreach (string t in tags)
            {
                if (!existingTags.Any(x => x.TagName.Equals(t, StringComparison.OrdinalIgnoreCase)))
                {
                    newTags.Add(new Tag() { TagName = t });
                }
            }

            if (newTags.Count > 0)
            {
                await _context.Tag.AddRangeAsync(newTags);
            }

            await _context.Picture.AddAsync(picture);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPicture", new { id = picture.Id }, picture);
        }

        [HttpGet("tags")]
        //[Route("tags")]
        public async Task<ActionResult<List<Tag>>> GetTags()
        {
            return await _context.Tag.ToListAsync();
        }

    }
}

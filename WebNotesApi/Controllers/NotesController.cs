using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNotesApi.Data;
using WebNotesApi.DTOs;
using WebNotesApi.Models;

namespace WebNotesApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly WebNotesApiContext _context;

        public NotesController(WebNotesApiContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNote(string searchString, string searchDate)
        {
            List<Note> sortedNotes = new List<Note>();

            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(searchDate)) // if search string was received 
            {
                sortedNotes = _context.Note.Where(x =>
                (x.Title.ToLower().Contains(searchString.ToLower()) && x.Created.ToString().Contains(searchDate.ToLower())) ||
                (x.Text.ToLower().Contains(searchString.ToLower()) && x.Created.ToString().Contains(searchDate.ToLower()))).ToList();
             
            }
            else if (!String.IsNullOrEmpty(searchString))
            {
                sortedNotes = _context.Note.Where(x => x.Text.ToLower().Contains(searchString.ToLower()) ||
                x.Title.ToLower().Contains(searchString.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchDate))
            {
                sortedNotes = _context.Note.Where(x => 
                x.Created.ToString().Contains(searchDate.ToLower())).ToList();
            }
            else
            {
                sortedNotes = _context.Note.ToList();
            }


            return sortedNotes;
        }

        /// /////////////////////////////////////////
        /// 
        //// GET: api/Notes
        //[HttpPost]
        //[Route("sorting/")]
        //public async Task<ActionResult<IEnumerable<Note>>> GetNote(Sorting sorting)
        //{

        //    List<Note> sortedNotes = new List<Note>();

        //    if (!String.IsNullOrEmpty(sorting.searchString)) // if search string was received 
        //    {
        //        sortedNotes = _context.Note.Where(x => x.Text.ToLower().Contains(sorting.searchString.ToLower()) ||
        //        x.Title.ToLower().Contains(sorting.searchString.ToLower()) ||
        //        x.Created.ToString().Contains(sorting.searchString.ToLower()) ||
        //         x.Created.ToString().Contains(sorting.searchString.ToLower())
        //         //|| x.Created.ToString("dd-MM-yyyy").Contains(sorting.searchString.ToLower())
                 
        //         ).ToList();


                
        //    }
        //    return sortedNotes ;
        //}

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var note = await _context.Note.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        // PUT: api/Notes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(int id, Note note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }

            _context.Entry(note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Notes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(NewNote note)
        {
            Note newNote = new Note();
            newNote.Title = note.Title;
            newNote.Text = note.Text;
            newNote.Created = DateTime.Now;



            _context.Note.Add(newNote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = newNote.Id }, newNote);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Note>> DeleteNote(int id)
        {
            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Note.Remove(note);
            await _context.SaveChangesAsync();

            return note;
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}

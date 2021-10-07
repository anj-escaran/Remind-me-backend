using System.Threading.Tasks;
using System.Net;
using System.IO;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using Firebase.Storage;
using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using RemindMe.Repositories;
using RemindMe.Models;
using Microsoft.IdentityModel.Protocols;

namespace RemindMe.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PassportController : ControllerBase
    {
        private const string mon = "Month(s)";

        private readonly IPassportRepository _passportRepo;
        public PassportController(IPassportRepository repository)
        {
            _passportRepo = repository;
        }

        //Get All Passports
        [HttpGet]
        [Route("GetList")]
        public async Task<IEnumerable<Passport>> Get()
        {
            return await _passportRepo.GetPassports();
        }

        //Get One Passport
        [HttpGet("{id}")]
        //[Route("GetPassport")]
        public async Task<ActionResult<Passport>> GetPassport(int id)
        {
            var passport = await _passportRepo.GetPassport(id);
            if (passport == null)
            {
                return NotFound();
            }

            return passport;

        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Add([FromBody] Passport p)
        {
            var cancellation = new CancellationTokenSource();
            p.RemindDate = GetRemindDate(p.ValidUntil, p.SpanPriorExpi, p.SpanCalendarType);
            var newPassport = new Passport
            {
                LastName = p.LastName,
                FirstName = p.FirstName,
                Email = p.Email,
                CellNumber = p.CellNumber,
                PassportNumber = p.PassportNumber,
                ValidUntil = p.ValidUntil,
                SpanPriorExpi = p.SpanPriorExpi,
                SpanCalendarType = p.SpanCalendarType,
                RemindDate = p.RemindDate,
                IsAlarm = p.IsAlarm
            };
            
            var newPass = await _passportRepo.AddPassport(newPassport);
            //return CreatedAtAction(nameof(Get), new { id = newDoc.Id }, newDoc);
            return Ok(new Response { Status = "Success", Message = "Passport successfully added." });

        }

        [HttpPut("{id}")]
        //[Route("Edit")]
        public async Task<ActionResult<Passport>> Edit(int id, [FromBody] Passport p)
        {
            if (id != p.Id)
            {
                return BadRequest();
            }

            p.RemindDate = GetRemindDate(p.ValidUntil, p.SpanPriorExpi, p.SpanCalendarType);

            await _passportRepo.UpdatePassport(p);

            return Ok(new Response { Status = "Success", Message = "Passport successfully updated." });
        }

        [HttpDelete("{id}")]
        //[Route("Delete")]
        public async Task<ActionResult<Passport>> Delete(int id)
        {
            var passportToDelete = await _passportRepo.GetPassport(id);                        
            if (passportToDelete == null)
            {
                return NotFound();
            }
            var result = await _passportRepo.DeletePassport(passportToDelete.Id);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Error in deleting passport!" });
            }
            return Ok(new Response { Status = "Success", Message = "Successfully deleted!" });

        }
        private DateTime GetRemindDate(DateTime validDate, int num, String calType)
        {
            DateTime remDate = DateTime.MinValue;

            if (calType == mon) //if calendar type is Month
            {
                remDate = validDate.AddMonths((-1) * (num));
            }
            else //if calendar type is Day
            {
                remDate = validDate.AddDays((-1) * (num));
            }

            return remDate;
        }
    }
}

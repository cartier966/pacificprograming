using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services;
using Services.Data;
using Services.Entities;
using Services.Interfaces;
using Services.Models;



namespace PacificPrograming.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly ILogger<AvatarController> _logger;
        private readonly IAvatarUrlService _avatarUrlService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AvatarController(ILogger<AvatarController> logger, IAvatarUrlService avatarUrlService, IWebHostEnvironment webHostEnvironment)
        {
            _avatarUrlService = avatarUrlService;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Avatar/5
        [HttpGet("{userIdentifier}")]
        public async Task<IActionResult> GetAvatarUrl(string userIdentifier)
        {
            if (string.IsNullOrEmpty(userIdentifier) || string.IsNullOrWhiteSpace(userIdentifier))
            {
                _logger.LogDebug("attempt at retrieving AvatarUrl with empty userIdentifier parameter");
                return BadRequest("invalid 'userIdentifier' parameter");
            }

            try
            {   
                var identifierLastCharacter = userIdentifier.Last();
                if (Char.IsNumber(identifierLastCharacter) && Convert.ToInt32(Char.GetNumericValue(identifierLastCharacter)) != 0)
                {
                    double temp = Char.GetNumericValue(identifierLastCharacter);
                    int userIdentifierLastNumber = Convert.ToInt32(temp);
                    if (userIdentifierLastNumber > 5 && userIdentifierLastNumber < 10)
                    {

                        //var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "db.json");
                        //var avatarUrlObject = await _avatarUrlService.GetUrlFromJsonFile(filePath,userIdentifierLastNumber);
                        //if (avatarUrlObject != null)
                        //{
                        //    _logger.LogInformation("avatarUrl was retrieved from JSON file for {userIdentifier}: {avatarUrlObject}", userIdentifier, avatarUrlObject);
                        //    return Ok(avatarUrlObject);
                        //}
                        //else
                        //{
                        //    _logger.LogInformation("avatarUrl was NOT found from JSON file for {userIdentifier}", userIdentifier);
                        //   return NotFound();
                        //}

                        var avatarUrlWithIdentifierLastNumber = _avatarUrlService.ConcatenateUrlWithUserIdentifierLastNumber(userIdentifierLastNumber);
                        _logger.LogInformation("avatarUrl was retrieved concatenated string with user identifier last digit being {userIdentifierLastNumber} for {userIdentifier}: {userIdentifierLastNumberUrl}", userIdentifierLastNumber, userIdentifier, avatarUrlWithIdentifierLastNumber);
                        return Ok(new { Url = avatarUrlWithIdentifierLastNumber });

                    }
                    else if(userIdentifierLastNumber > 0 && userIdentifierLastNumber < 6)
                    {

                        var avatarUrlEntity = await _avatarUrlService.GetUrlFromSQLite(userIdentifierLastNumber);
                        if (avatarUrlEntity != null)
                        {
                            _logger.LogInformation("avatarUrl was retrieved from DB for {userIdentifier}: {avatarUrlEntity}", userIdentifier, avatarUrlEntity);
                            return Ok(avatarUrlEntity);
                        }
                        else
                        {
                            _logger.LogInformation("avatarUrl was NOT found from DB for {userIdentifier}", userIdentifier);
                            return NotFound();
                        }

                    }
                }
                
                if (new string[] { "a", "e", "i", "o", "u" }.Any(s=>userIdentifier.Contains(s)))
                {
                    var avatarUrlForVowel = _avatarUrlService.GetStandardUrlForVowel();
                    _logger.LogInformation("avatarUrl was retrieved from standard url because a vowel was detected in the identifier {userIdentifier}: {vowelUrl}", userIdentifier, avatarUrlForVowel);
                    return Ok(new { Url = avatarUrlForVowel });

                }
                else if (!userIdentifier.All(Char.IsLetterOrDigit))
                {
                    var avatarUrlWithRandomNumber = _avatarUrlService.ConcatenateUrlWithRandomNumber();
                    _logger.LogInformation("avatarUrl was concatenated string with random number for {userIdentifier}: {randomNumberUrl}", userIdentifier, avatarUrlWithRandomNumber);
                    return Ok(new { Url = avatarUrlWithRandomNumber });

                }
                else
                {
                    var defaultAvatarUrl = _avatarUrlService.GetDefaultUrl();
                    _logger.LogInformation("default avatar url was returned for identifier {userIdentifier}: {randomNumberUrl}", userIdentifier, defaultAvatarUrl);
                    return Ok(new { Url = defaultAvatarUrl });

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("an error occured while retrieving AvatarUrl with userIdentifier {userIdentifier} with error {ex}", userIdentifier, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
    
}

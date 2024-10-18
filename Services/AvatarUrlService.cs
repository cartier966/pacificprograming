using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Data;
using Services.Entities;
using Services.Interfaces;
using Services.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net.Http.Headers;
using System.Net;

namespace Services
{
    public class AvatarUrlService : IAvatarUrlService
    {
        private readonly ILogger<AvatarUrlService> _logger;
        private readonly AppDbContext _context;
        public AvatarUrlService(ILogger<AvatarUrlService> logger, AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
        }

        public string ConcatenateUrlWithRandomNumber()
        {
            var random = new Random();
            var randomNumberInRange = random.Next(1, 6);
            var randomNumberUrl = $"https://api.dicebear.com/8.x/pixel-art/png?seed={randomNumberInRange}&size=150";
            return  randomNumberUrl;
        }

        public string ConcatenateUrlWithUserIdentifierLastNumber(int userIdentifierLastNumber)
        {
            var userIdentifierLastNumberUrl = $"https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/{userIdentifierLastNumber}";
           return userIdentifierLastNumberUrl;
        }

        public string GetDefaultUrl()
        {
            return "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
        }

        public string GetStandardUrlForVowel()
        {
            var vowelUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150";
            return vowelUrl;
        }

        public async Task<AvatarUrl> GetUrlFromJsonFile(string filePath, int userIdentifierLastNumber)
        {
            using StreamReader reader = new StreamReader(filePath);
            var json = await reader.ReadToEndAsync();
            ImagesModel model = JsonConvert.DeserializeObject<ImagesModel>(json);
            var avatarUrlObject = model.Images.FirstOrDefault(u => u.Id == userIdentifierLastNumber);
            return avatarUrlObject;
        }

        public async Task<AvatarUrl> GetUrlFromService(int userIdentifierLastNumber)
        {
            var serviceUrl = $"https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/{userIdentifierLastNumber}";
            AvatarUrl avatar;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(serviceUrl))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    avatar = JsonConvert.DeserializeObject<AvatarUrl>(apiResponse);
                }
            }
            return avatar;
        }

        public async Task<AvatarUrl> GetUrlFromSQLite(int userIdentifierLastNumber)
        {
            var avatarUrlEntity = await _context.FindAsync<AvatarUrl>(userIdentifierLastNumber);
            return avatarUrlEntity;

        }
    }
}

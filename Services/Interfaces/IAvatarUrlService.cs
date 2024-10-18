using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAvatarUrlService
    {
        Task<AvatarUrl> GetUrlFromService(int userIdentifierLastNumber);
        Task<AvatarUrl> GetUrlFromJsonFile(string filePath, int userIdentifierLastNumber);
        Task<AvatarUrl> GetUrlFromSQLite(int userIdentifierLastNumber);
        string ConcatenateUrlWithUserIdentifierLastNumber(int userIdentifierLastNumber);
        string ConcatenateUrlWithRandomNumber();
        string GetStandardUrlForVowel();
        string GetDefaultUrl();

    }
}

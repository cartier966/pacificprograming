using Services.Data;
using Microsoft.EntityFrameworkCore;
namespace Services.Entities
{
    public class AvatarUrl
    {
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
using Microsoft.AspNetCore.Http;

namespace WebApplication1
{
    public class InputFile
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}

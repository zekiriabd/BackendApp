using Microsoft.AspNetCore.Mvc;

namespace UploadFileBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost("UploadFile")]
        public void UploadFile(IFormFile file)
        {
            using (FileStream fileStream = System.IO.File.Create($"c:\\MyImages\\{file.FileName}"))
            {
                file.CopyTo(fileStream);
            }
        }



    }
}
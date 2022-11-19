using BackendApp.Models.Dto;
using BackendApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointController : ControllerBase
    {
        private readonly IPointService _PointService;
        public PointController(IPointService pointService)
        {
            _PointService = pointService;
        }

        [HttpGet]
        public async Task<IEnumerable<PointModel>> Get() => await _PointService.GetAllPoints();

        [HttpGet("{Id}")]
        public Task<PointModel> GetPointById(int Id) => _PointService.GetPointById(Id);

        [HttpPost]
        public void UpdatePoint([FromBody] PointModel point)
        {
            _PointService.UpdatePoint(point);
        }
        
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeletePoint(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok(await _PointService.DeletePoint(Id));
            }
            
        }
        
        [HttpPut]
        public void InsertPoint([FromBody] PointModel point)
        {
            _PointService.InsertPoint(point);
        }
    }
}
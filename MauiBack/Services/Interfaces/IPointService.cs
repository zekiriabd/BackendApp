using BackendApp.Models.Dto;

namespace BackendApp.Services.Interfaces
{
    public interface IPointService
    {
        Task<List<PointModel>> GetAllPoints();
        Task<PointModel> GetPointById(int Id);
        Task UpdatePoint(PointModel point);
        Task<bool> DeletePoint(int Id);
        Task<bool> InsertPoint(PointModel point);
    }
}

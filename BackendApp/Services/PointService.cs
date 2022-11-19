using AutoMapper;
using BackendApp.Models.Dto;
using BackendApp.Models.Entities;
using BackendApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackendApp.Services
{
    internal class PointService : IPointService
    {
        private readonly DataContext _Db;
        private readonly IMapper _Mapper;

        public PointService(DataContext db, IMapper mapper)
        {
            _Db = db;
            _Mapper = mapper;

        }
        public async Task<List<PointModel>> GetAllPoints()
        {
            return _Mapper.Map<List<PointModel>>(_Db.PointTbs.AsNoTracking());
        }

        public async Task<PointModel> GetPointById(int Id)
        {
            var pointTb = _Db.PointTbs.FirstOrDefault(x => x.Id == Id);
            return (pointTb==null)? new PointModel() : _Mapper.Map<PointModel>(pointTb);
        }
        public async Task UpdatePoint(PointModel point)
        {
            try
            {
                //PointTb pointTb = _Mapper.Map<PointTb>(point);
                //_Db.Attach(pointTb);
                //_Db.Entry(pointTb).State= EntityState.Modified;
                
                PointTb pointTb = _Db.PointTbs.First(x => x.Id == point.Id);
                pointTb.Title = point.Title;
                pointTb.Point = point.Point;
                pointTb.Date = point.Date;
                _Db.PointTbs.Update(pointTb);
                await _Db.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                var x = ex;
            }
        }
        public async Task<bool> DeletePoint(int Id)
        {
            PointTb? pointTb = _Db.PointTbs.FirstOrDefault(x => x.Id == Id);
            if (pointTb != null)
            {
                try
                {
                    _Db.PointTbs.Remove(pointTb);
                    await _Db.SaveChangesAsync();
                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task InsertPoint(PointModel point)
        {
            PointTb pointTb = _Mapper.Map<PointTb>(point);
            await _Db.PointTbs.AddAsync(pointTb);
            await _Db.SaveChangesAsync();
        }
    }
}

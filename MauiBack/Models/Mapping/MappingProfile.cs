using BackendApp.Models.Entities;
using BackendApp.Models.Dto;
using AutoMapper;

namespace BackendApp.Models.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<PointModel, PointTb>();
            CreateMap<PointTb, PointModel>();
        }
    }
}

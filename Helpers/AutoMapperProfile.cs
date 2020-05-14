using AutoMapper;
using RecordLabelApi.Data;
using RecordLabelApi.Models.Users;

namespace RecordLabelApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}

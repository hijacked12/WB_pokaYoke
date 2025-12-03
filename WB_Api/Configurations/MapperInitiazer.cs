using System;
using AutoMapper;
using WB_Api.Data;
using WB_Api.Models;
using System.Collections.Generic;
using System.Linq;
using WB_Api.IRepository;
using System.Threading.Tasks;

namespace WB_Api.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer() { 
            CreateMap<scanner, ScannerDTO>().ReverseMap();
            CreateMap<scanner, CreateScannerDTO>().ReverseMap();
            CreateMap<ApiUser, UserDTO>().ReverseMap();
        }
    }
}

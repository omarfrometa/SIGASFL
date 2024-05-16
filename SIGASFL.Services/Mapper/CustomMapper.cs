using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Services.Mapper
{
    public class CustomMapper : ICustomMapper
    {
        private readonly IMapper _mapper;
        public CustomMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map<TSource, TDestination>(source, destination);
        }
    }
}

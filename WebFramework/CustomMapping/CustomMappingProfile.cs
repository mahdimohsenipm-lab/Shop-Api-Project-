using AutoMapper;

namespace WebFramework.CustomMapping
{
    internal class CustomMappingProfile : Profile
    {

        public CustomMappingProfile(IEnumerable<IHaveCustomMapping?> list)
        {
            foreach (var item in list) 
            {
                item.CratedMapping(this);


            }
        }
    }
}
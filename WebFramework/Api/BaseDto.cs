using AutoMapper;
using Entites.Common;
using WebFramework.CustomMapping;
namespace WebFramework.Api
{
    public abstract class BaseDto<TDto,TEntity,TKey> : IHaveCustomMapping
        where TDto : class, new()
        where TEntity : BaseEntity<TKey>, new()
    {
        public TKey Id {get; set;}

        public TEntity ToEntity(IMapper mapper)
        {
            return mapper.Map<TEntity>(CastToDerivedClass(mapper, this));
        }
        public TEntity ToEntity(IMapper mapper, TEntity entity)
        {
            return mapper.Map(CastToDerivedClass(mapper, this), entity);
        }
        public TDto FromEntity(IMapper mapper, TEntity Model)
        {
            return mapper.Map<TDto>(Model);
        }
        public List<TDto> FromEntityList(IMapper mapper, List<TEntity> models)
        {
            return mapper.Map<List<TDto>>(models);
        }
        protected TDto CastToDerivedClass(IMapper mapper, BaseDto<TDto, TEntity, TKey> baseInstance)
        {
            return mapper.Map<TDto>(baseInstance);
        }

        public void CratedMapping(Profile profile)
        {
            var mappingExpression = profile.CreateMap<TDto, TEntity>();

            var entity = typeof(TEntity);
            var dto = typeof(TDto);

            foreach (var property in entity.GetProperties())
            {
                if (dto.GetProperty(property.Name) == null)
                    mappingExpression.ForMember(property.Name, x => x.Ignore());

            }
            CustomMapping(mappingExpression.ReverseMap());

        }
        public virtual void CustomMapping(IMappingExpression<TEntity, TDto> mapping)
        {
        }

       
    }
    public abstract class BaseDto<TDto, TEntity> : BaseDto<TDto, TEntity, int>
    where TDto : class, new()
    where TEntity : BaseEntity<int>, new()
    {

    }
}

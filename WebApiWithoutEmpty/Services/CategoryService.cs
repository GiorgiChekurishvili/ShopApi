using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WebApiWithoutEmpty.Dtos;
using WebApiWithoutEmpty.Entities;
using WebApiWithoutEmpty.Repositories;

namespace WebApiWithoutEmpty.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        public CategoryService(ICategoryRepository repository, IMapper mapper, IDistributedCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<CategoryReturnDto>> GetCategories()
        {
            var cachekey = "GetCategories";
            var cachedata = await _cache.GetStringAsync(cachekey);
            if (!string.IsNullOrEmpty(cachedata))
            {
                return JsonConvert.DeserializeObject<IEnumerable<CategoryReturnDto>>(cachedata);
            }


            var categories = _repository.Getall();
            var data = _mapper.Map<IEnumerable<CategoryReturnDto>>(categories);
            var cacheoptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
            await _cache.SetStringAsync(cachekey, JsonConvert.SerializeObject(data), cacheoptions);

            return _mapper.Map<IEnumerable<CategoryReturnDto>>(categories);

        }

        public async Task<CategoryDto> GetCategoryById(int id)
        {
            var cachekey = $"CategoryById-{id}";
            var cachedata = await _cache.GetStringAsync(cachekey);
            if (!string.IsNullOrEmpty(cachedata))
            {
                return JsonConvert.DeserializeObject<CategoryDto>(cachedata);
            }

            var categories = _repository.Getbyid(id);
            var data = _mapper.Map<CategoryDto>(categories);

            var cacheoptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(3))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            await _cache.SetStringAsync(cachekey, JsonConvert.SerializeObject(data), cacheoptions);

            return _mapper.Map<CategoryDto>(categories);
        }
        public CategoryDto CreateCategory(CategoryDto category, int userid)
        {

            var categories = _mapper.Map<Category>(category);
            categories.UserId = userid;
            categories = _repository.Insert(categories);
            return _mapper.Map<CategoryDto>(categories);
        }

        public void UpdateCategory(CategoryDto category)
        {
            var categories = _mapper.Map<Category>(category);
            if (categories != null)
            {
                _repository.Update(categories);
            }
            
        }
        public void DeleteCategory(int id)
        {
            var categories = _repository.Getbyid(id);

            if (categories != null)
            {
                _repository.Delete(categories);
            }
        }
    }
}

using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using WebApiWithoutEmpty.Dtos;
using WebApiWithoutEmpty.Entities;
using WebApiWithoutEmpty.Repositories;

namespace WebApiWithoutEmpty.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public ProductService(IProductRepository productRepository, IMapper mapper, IDistributedCache cache)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async  Task<IEnumerable<ProductDto>> GetProducts()
        {
            var cachekey = $"GetAllProducts";
            var cachedata = await _cache.GetStringAsync(cachekey);

            if (cachedata != null)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(cachedata);
            }


            var products = _productRepository.Getall();
            var data = _mapper.Map<IEnumerable<ProductDto>>(products);

            var cacheoptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
            await _cache.SetStringAsync(cachekey, JsonConvert.SerializeObject(data), cacheoptions);

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var cachekey = $"ProductById-{id}";
            var cachedata = await _cache.GetStringAsync(cachekey);

            if (!string.IsNullOrEmpty(cachedata))
            {
                return JsonConvert.DeserializeObject<ProductDto>(cachedata);
            }
            

            var products = _productRepository.GetById(id);
            var data = _mapper.Map<ProductDto>(products);
            var catcheoptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(3))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            await _cache.SetStringAsync(cachekey, JsonConvert.SerializeObject(data), catcheoptions);

            return data;
        }

        public ProductDto CreateProduct(ProductReturnDto productdto, int userid)
        {
            var products = _mapper.Map<Product>(productdto);
            products.UserId = userid;
            products = _productRepository.Insert(products);
            return _mapper.Map<ProductDto>(products);
        }
        public void UpdateProduct(ProductDto product)
        {
            var products = _mapper.Map<Product>(product);
            _productRepository.Update(products);
        }
        public void DeleteProduct(int id)
        {
            var products = _productRepository.GetById(id);
            if (products != null)
            {
                _productRepository.Delete(products);
            }
        }
        
    }
}

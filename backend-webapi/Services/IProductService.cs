using System.Collections.Generic;
using webapi.Dtos;

namespace webapi.Services
{
    public interface IProductService
    {
        bool CreateNewProduct(ProductDto productDto);
        bool DeleteProduct(int id);
        ProductDto GetProductById(int id);
        IEnumerable<ProductDto> GetProductsByCatogary(int catogaryId, double latitude, double longitude);
        IEnumerable<ProductDetails> GetProductsByDiscounted(double Latitude, double Longitude);
        IEnumerable<ProductDetails> GetProductsByLocation(double Latitude, double Longitude);
        IEnumerable<ProductDetails> GetProductsByPopular(double Latitude, double Longitude);
        IEnumerable<ProductDetails> GetProductsBySearchResult(string s, double Latitude, double Longitude);
        IEnumerable<ProductDto> GetProductsByShop(int sellerId);
        void ReduceProductQuentity(int id, int Quantity);
        bool UpdateProduct(ProductDto productDto);
    }
}
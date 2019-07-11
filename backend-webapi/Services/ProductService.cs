using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webapi.Dtos;
using webapi.Entities;
using webapi.Repositories;

namespace webapi.Services
{
    public class ProductService : IProductService
    {
        private ICommonRepository<Product> _productRepository;
        private ICommonRepository<Seller> _sellerRepository;
        private ISellerService _sellerService;

        public ProductService(ICommonRepository<Product> productRepository, ICommonRepository<Seller> sellerRepository, ISellerService sellerService)
        {
            _productRepository = productRepository;
            _sellerRepository = sellerRepository;
            _sellerService = sellerService;
        }  
        
        //GetProductById
        public ProductDto GetProductById(int id)
        {
            Product ProductrFromRepo = _productRepository.Get(id);
            return Mapper.Map<ProductDto>(ProductrFromRepo);
        }

        //GetProductsByCatogary
        public IEnumerable<ProductDto> GetProductsByCatogary(int catogaryId,double latitude, double longitude)
        {
            var allProducts = _sellerService.GetShopsNearByLocation(latitude, longitude).ToList();
            var allProductsDto = allProducts.Select(x => Mapper.Map<ProductDto>(catogaryId));
            return allProductsDto;
        }
        

        //GetProductsByShop
        public IEnumerable<ProductDto> GetProductsByShop(int sellerId)
        {
            var allProducts = _productRepository.GetAll().ToList();
            var allProductsDto = allProducts.Select(x => Mapper.Map<ProductDto>(sellerId));
            return allProductsDto;
        }

        //GetProductsBySearchResult
        public IEnumerable<ProductDetails> GetProductsBySearchResult (string searchText, double Latitude, double Longitude)
        {
            var s = "laptop";
            var query = (
                       from product in _productRepository.GetAll()
                       where product.Name.ToLower().Contains(searchText.ToLower())

                       from shop in _sellerService.GetShopsNearByLocation(Latitude, Longitude)
                       where product.SellerId== shop.Id

                       select new ProductDetails
                       {
                           Id = product.Id,
                           Name = product.Name,
                           Description = product.Description,
                           ShortDescription = product.ShortDescription,
                           Image = product.Image,
                           Quantity = product.Quantity,
                           Like = product.Like,
                           Rating = product.Rating,
                           UnitPrice = product.UnitPrice,
                           Discount = product.Discount,
                           shopDetails = shop,
                           SellingPrice = product.UnitPrice * ((1 - product.Discount / 100))
                       }).ToArray();

            return query;
        }

        //GetProductsByPopular
        public IEnumerable<ProductDetails> GetProductsByPopular(double Latitude, double Longitude)
        {
            var query = (
                    from shop in _sellerService.GetShopsNearByLocation(Latitude, Longitude)
                    from product in _productRepository.GetAll()
                    where product.Id == shop.Id

                    orderby product.Rating descending

                    select new ProductDetails
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ShortDescription = product.ShortDescription,
                        Image = product.Image,
                        Quantity = product.Quantity,
                        Like = product.Like,
                        Rating = product.Rating,
                        UnitPrice = product.UnitPrice,
                        Discount = product.Discount,
                        shopDetails = shop,
                        SellingPrice = product.UnitPrice * ((1 - product.Discount / 100))
                    }).ToArray();

            return query;
        }

        //GetProductsByDiscounted
        public IEnumerable<ProductDetails> GetProductsByDiscounted(double Latitude, double Longitude)
        {
            var query = (
                       from product in _productRepository.GetAll()
                       from shop in _sellerService.GetShopsNearByLocation(Latitude, Longitude)
                       where product.SellerId == shop.Id
                       where product.Discount >0
                       orderby product.Discount descending

                       select new ProductDetails
                       {
                           Id=product.Id,
                           Name=product.Name,
                           Description=product.Description,
                           ShortDescription=product.ShortDescription,
                           Image=product.Image,
                           Quantity=product.Quantity,
                           Like=product.Like,
                           Rating=product.Rating,
                           UnitPrice=product.UnitPrice,
                           Discount=product.Discount,
                           shopDetails=shop,
                           SellingPrice = product.UnitPrice * ((1 - product.Discount / 100))
                       }).ToArray();

            return query;
        }

        //GetProductsByLocation
        public IEnumerable<ProductDetails> GetProductsByLocation(double Latitude, double Longitude)
        {
            var query = (
                       from product in _productRepository.GetAll()
                       from shop in _sellerService.GetShopsNearByLocation(Latitude, Longitude)
                       where product.SellerId == shop.Id

                       select new ProductDetails
                       {
                           Id = product.Id,
                           Name = product.Name,
                           Description = product.Description,
                           ShortDescription = product.ShortDescription,
                           Image = product.Image,
                           Quantity = product.Quantity,
                           Like = product.Like,
                           Rating = product.Rating,
                           UnitPrice = product.UnitPrice,
                           Discount = product.Discount,
                           shopDetails = shop,
                           SellingPrice = product.UnitPrice * ((1 - product.Discount/100))
                       }).ToArray();

            return query;
        }

        //ReduceProductQuentity
        public void ReduceProductQuentity(int id, int Quantity)
        {
            Product product = _productRepository.Get(id);
            if ((product.Quantity -= Quantity) >= 0)
            {
                _productRepository.Update(product);
                _productRepository.Save();
            }
        }

        //CreateNewProduct
        public bool CreateNewProduct(ProductDto productDto)
        {
            Product toAdd = Mapper.Map<Product>(productDto);

            _productRepository.Add(toAdd);

            bool result = _productRepository.Save();

            return result;
        }

        //DeleteProduct
        public bool DeleteProduct(int id)
        {
            var product = _productRepository.Get(x => x.Id == id).FirstOrDefault();

            if (product != null)
            {
                _productRepository.Remove(product);
                _productRepository.Save();
                return true;
            }
            return false;
        }

        //UpdateProduct
        public bool UpdateProduct(ProductDto productDto)
        {
            var productinDb = _productRepository.Get(x => x.Id == productDto.Id).FirstOrDefault();

            if (productinDb != null)
            {
                Product product = Mapper.Map<Product>(productDto);
                _productRepository.Update(product);
                return _productRepository.Save();
            }
            return false;
        }
    }
}

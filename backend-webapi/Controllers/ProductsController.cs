using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Dtos;
using webapi.Entities;
using webapi.ViewModels;
using webapi.Repositories;
using webapi.Services;
using System.IO;
using System.Text;
using System.Drawing;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private ICommonRepository<Product> _productRepository;
        private IProductService _productService;

        public ProductsController (ICommonRepository<Product> productRepository , IProductService productService)
        {
            _productRepository = productRepository;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allProducts = _productRepository.GetAll().ToList();
            var allProductsDto = allProducts.Select(x => Mapper.Map<ProductDto>(x));
            return Ok(allProductsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetSingle(int id)
        {
            ProductDto productDto = _productService.GetProductById(id);
            if (productDto == null)
            {
                return NotFound();
            }
            return Ok(productDto);
        }
        
        [HttpPost]
        [Route("create")]
        public IActionResult Add([FromBody] ProductDto product)
        {
            Product toAdd = Mapper.Map<Product>(product);

            //Image img = Image.FromFile(product.Image);
            //byte[] bArr = imgToByteArray(img);
            //toAdd.Image = Convert.ToBase64String(bArr);

            _productRepository.Add(toAdd);

            bool result = _productRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(400);
            }
            
            return Ok(toAdd);
        }

        [HttpGet]
        [Route("GetPopularProducts")]
        public IActionResult GetPopularProducts()
        {
            double lat = 6.795134521923838;
            double lng = 79.9003317207098;
            var x = _productService.GetProductsByPopular(lat, lng);
            return Ok(x);
        }

        [HttpGet]
        [Route("GetDiscountedProducts")]
        public IActionResult GetDiscountedProducts()
        {
            double lat = 6.795134521923838;
            double lng = 79.9003317207098;
            var x = _productService.GetProductsByDiscounted(lat, lng);
            return Ok(x);
        }

        [HttpGet]
        [Route("GetMoreProducts")]
        public IActionResult GetMoreProducts()
        {
            double lat = 6.795134521923838;
            double lng = 79.9003317207098;
            var x = _productService.GetProductsByLocation(lat, lng);
            return Ok(x);
        }

        [HttpGet]
        [Route("GetProductsSearch/{searchText}")]
        public IActionResult GetProductsSearch(string searchText)
        {
            double lat = 6.795134521923838;
            double lng = 79.9003317207098;
            var x = _productService.GetProductsBySearchResult(searchText,lat, lng);
            return Ok(x);
        }
        
        //convert image to bytearray
        public byte[] imgToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }
    }
}

/*
[HttpGet]
[Route("image")]
public string GetImageFromByteArray()
{
    //image to byteArray
    Image img = Image.FromFile("E:\\Huawei.jpg");
    byte[] bArr = imgToByteArray(img);
    Console.WriteLine(bArr);
    string base64String = Convert.ToBase64String(bArr);
    return base64String;
}
*/

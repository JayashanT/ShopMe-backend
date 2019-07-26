using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webapi.Dtos;
using webapi.Entities;
using webapi.ViewModels;
using webapi.Repositories;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : Controller
    {
        private ICommonRepository<Admin> _adminRepository;

        public AdminsController(ICommonRepository<Admin> adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allAdmins = _adminRepository.GetAll().ToList();
            var allAdminsDto = allAdmins.Select(x => Mapper.Map<AdminDto>(x));
            return Ok(allAdminsDto);
        }


        [HttpGet]
        [Route("{id}")]
        public IActionResult GetSingle(int id)
        {
            Admin adminrFromRepo = _adminRepository.Get(id);
            if (adminrFromRepo == null)
            {
                return NotFound();
            }
            return Ok(adminrFromRepo);
        }


        //post  api/admins/addAdmin
        [HttpPost]
        [Route("addAdmin")]
        public IActionResult Add([FromBody] AdminDto admin)
        {
            Admin toAdd = Mapper.Map<Admin>(admin);

            _adminRepository.Add(toAdd);

            bool result = _adminRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(400);
            }
            //return Ok(Mapper.Map<Admin>(toAdd));
            return Ok(toAdd);
        }
    }
}
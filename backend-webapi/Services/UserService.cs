using AutoMapper;
using backend_webapi.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using webapi.Dtos;
using webapi.Entities;
using webapi.Helpers;
using webapi.Repositories;
using webapi.ViewModels;

namespace webapi.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _jwtSettings;
        private readonly ICommonRepository<Customer> _customerRepository;
        private readonly ICommonRepository<Seller> _sellerRepository;
        private readonly ICommonRepository<Deliverer> _delivererRepository;
        private readonly ICommonRepository<Admin> _adminRepository;
        private readonly ICommonRepository<Login> _loginRepository;
        private readonly ICommonRepository<Location> _locationRepository;
        private string key = "1234567890-abcde";

        public UserService(IOptions<AppSettings> jwtSettings, ICommonRepository<Customer> customerRepository,
            ICommonRepository<Seller> sellerRepository,ICommonRepository<Deliverer> delivererRepository
            ,ICommonRepository<Admin> adminRepository, ICommonRepository<Login> loginRepository, ICommonRepository<Location> locationRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _customerRepository = customerRepository;
            _sellerRepository = sellerRepository;
            _delivererRepository = delivererRepository;
            _adminRepository = adminRepository;
            _loginRepository = loginRepository;
            _locationRepository = locationRepository;
        }
        
        public Object SignIn(string email, string password)
        {
            //retrive data from db
            var user = (dynamic) null;
            var login = _loginRepository.Get(x => x.Email == email && Decrypt(x.Password, key) == password).FirstOrDefault();//Decrypt(x.Password, key)
            if (login == null)
                return null;
            else if (login.Role == "Customer")
            {
                var details = _customerRepository.Get(x => x.LoginId == login.Id).FirstOrDefault();
                if(details!=null)
                    user = new
                    {
                        Data=details,
                        Role=login.Role,
                    };
            }
                
            else if (login.Role == "Deliverer")
            {
                var details = _delivererRepository.Get(x => x.LoginId == login.Id).FirstOrDefault();
                if (details != null)
                    user = new
                    {
                        Data = details,
                        Role = login.Role
                    };
            }
                
            else if (login.Role == "Seller")
            {
                var details = _sellerRepository.Get(x => x.LoginId == login.Id).FirstOrDefault();
                if (details != null)
                    user = new
                    {
                        Data = details,
                        Role = login.Role
                    };
            }
                

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            user.Data.Token = Authentication(user.Data.Id, user.Data.Token);

            return user;
        }

        public string Authentication(int id, string token)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, id.ToString())
                }),
                //Expires = DateTime.UtcNow.AddDays(7),
                Expires=DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var Token = tokenHandler.CreateToken(tokenDescriptor);
            token = tokenHandler.WriteToken(Token);
            return token;
        }

        public static string Encrypt(string password, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(password);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string Decrypt(string password, string keyString)
        {
            var fullCipher = Convert.FromBase64String(password);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length]; 

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length); 
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        public CustomerDto SignUp(CustomerVM customerVM)
        {
            try
            {
                var customerDto = new CustomerDto()
                {
                    FirstName = customerVM.FirstName,
                    LastName = customerVM.LastName,
                    MobileNumber = customerVM.MobileNumber,
                    ProfileImage = customerVM.ProfileImage,
                };

                using (TransactionScope scope = new TransactionScope())
                {
                    Login login = Mapper.Map<Login>(customerVM.LoginVM);
                    login.Password = Encrypt(login.Password, key);
                    _loginRepository.Add(login);
                    _loginRepository.Save();

                    Login loginId = _loginRepository.Get(x => x.Email == login.Email).FirstOrDefault();
                    customerDto.LoginId = loginId.Id;

                    Customer toAdd = Mapper.Map<Customer>(customerDto);
                    _customerRepository.Add(toAdd);
                    _customerRepository.Save();

                    customerDto.Token = Authentication(customerDto.Id, customerDto.Token);
                    scope.Complete();
                }
                return customerDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(new Exception(ex.Message));
            }
            return null;
        }

        public SellerDto SignUp(SellerVM sellerVM)
        {
            try
            {
                SellerDto sellerDto = new SellerDto()
                {
                    FirstName = sellerVM.FirstName,
                    LastName = sellerVM.LastName,
                    MobileNumber = sellerVM.MobileNumber,
                    ProfileImage = sellerVM.ProfileImage,
                    AccountNo = sellerVM.AccountNo,
                    ShopAddress = sellerVM.ShopAddress,
                    ShopName = sellerVM.ShopName,
                    ShopLocationLatitude = sellerVM.ShopLocationLatitude,
                    ShopLocationLongitude = sellerVM.ShopLocationLongitude
                };
                using (TransactionScope scope = new TransactionScope())
                {
                    Login login = Mapper.Map<Login>(sellerVM.LoginVM);
                    login.Password = Encrypt(login.Password, key);
                    _loginRepository.Add(login);
                    _loginRepository.Save();

                    Login loginId = _loginRepository.Get(x => x.Email == login.Email).FirstOrDefault();
                    sellerDto.LoginId = loginId.Id;

                    Seller toAdd = Mapper.Map<Seller>(sellerDto);
                    _sellerRepository.Add(toAdd);
                    _sellerRepository.Save();
                    
                    sellerDto.Token = Authentication(sellerDto.Id, sellerDto.Token);
                    scope.Complete();
                }
                return sellerDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(new Exception(ex.Message));
            }
            return null;
        }

        public DelivererDto SignUp(DelivererVM delivererVM)
        {
            try
            {
                var delivererDto = new DelivererDto()
                {
                    FirstName = delivererVM.FirstName,
                    LastName = delivererVM.LastName,
                    MobileNumber = delivererVM.MobileNumber,
                    ProfileImage = delivererVM.ProfileImage,
                    NIC = delivererVM.NIC,
                    VehicleNo=delivererVM.VehicleNo,
                    VehicleType=delivererVM.VehicleType,
                };

                using (TransactionScope scope = new TransactionScope())
                {
                    Login login = Mapper.Map<Login>(delivererVM.LoginVM);
                    login.Password = Encrypt(login.Password, key);
                    _loginRepository.Add(login);
                    _loginRepository.Save();

                    Login loginId = _loginRepository.Get( x=> x.Email==login.Email).FirstOrDefault();
                    delivererDto.LoginId = loginId.Id;

                    Deliverer deliverer = Mapper.Map<Deliverer>(delivererDto);
                    _delivererRepository.Add(deliverer);
                    _delivererRepository.Save();

                    Location location = new Location()
                    {
                        DelivererId = deliverer.Id,
                    };
                    _locationRepository.Add(location);
                    _locationRepository.Save();

                    delivererDto.Token = Authentication(delivererDto.Id, delivererDto.Token);
                    scope.Complete();
                }
                return delivererDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(new Exception(ex.Message));
            }
            return null;
        }
    }
}

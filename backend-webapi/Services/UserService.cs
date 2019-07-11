using AutoMapper;
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
        private string key = "1234567890-abcde";

        public UserService(IOptions<AppSettings> jwtSettings, ICommonRepository<Customer> customerRepository,
            ICommonRepository<Seller> sellerRepository,ICommonRepository<Deliverer> delivererRepository
            ,ICommonRepository<Admin> adminRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _customerRepository = customerRepository;
            _sellerRepository = sellerRepository;
            _delivererRepository = delivererRepository;
            _adminRepository = adminRepository;
        }
        
        public Object SignIn(string email, string password, string role)
        {

            //retrive data from db
            var user = (dynamic) null;
            if (role == "customer")
            {
                user = _customerRepository.Get(x => x.Email == email && Decrypt(x.Password, key) == password).FirstOrDefault();
            }
            else if (role == "seller")
            {
                user = _sellerRepository.Get(x => x.Email == email && Decrypt(x.Password, key) == password).FirstOrDefault();
            }else if (role == "Deliverer")
            {
                user = _delivererRepository.Get(x => x.Email == email && Decrypt(x.Password, key) == password).FirstOrDefault();
            }else
            {
                user = _adminRepository.Get(x => x.Email == email && Decrypt(x.Password, key) == password).FirstOrDefault();
            }

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            user.Token = Authentication(user.Id, user.Token);

            // remove password before returning
            user.Password = null;

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
                Expires = DateTime.UtcNow.AddDays(7),
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
                CustomerDto customerDto = new CustomerDto();
                using (TransactionScope scope = new TransactionScope())
                {
                    Customer toAdd = Mapper.Map<Customer>(customerVM);
                    toAdd.Password = Encrypt(toAdd.Password, key);
                    _customerRepository.Add(toAdd);
                    bool result = _customerRepository.Save();
                    customerDto = Mapper.Map<CustomerDto>(toAdd);
                    customerDto.Token = Authentication(customerDto.Id, customerDto.Token);
                    customerDto.Password = Decrypt(customerDto.Password, key);
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
                SellerDto sellerDto = new SellerDto(); 
                using (TransactionScope scope = new TransactionScope())
                {
                    Seller toAdd = Mapper.Map<Seller>(sellerVM);
                    toAdd.Password = Encrypt(toAdd.Password, key);
                    _sellerRepository.Add(toAdd);
                    bool result = _sellerRepository.Save();
                    sellerDto = Mapper.Map<SellerDto>(toAdd);
                    sellerDto.Token = Authentication(sellerDto.Id, sellerDto.Token);
                    sellerDto.Password = null;
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
                DelivererDto delivererDto = new DelivererDto();
                using (TransactionScope scope = new TransactionScope())
                {
                    Deliverer toAdd = Mapper.Map<Deliverer>(delivererVM);
                    toAdd.Password = Encrypt(toAdd.Password, key);
                    _delivererRepository.Add(toAdd);
                    bool result = _delivererRepository.Save();
                    delivererDto = Mapper.Map<DelivererDto>(toAdd);
                    delivererDto.Token = Authentication(delivererDto.Id, delivererDto.Token);
                    delivererDto.Password = null;
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

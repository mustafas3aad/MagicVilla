﻿using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Reposaitory.IReposaitory;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Reposaitory
{
    public class UserReposaitory :IUserReposaitory
    {
        
        public UserManager<ApplicationUser> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private string secretkey;
        protected readonly APIResponse _response;

        public UserReposaitory(ApplicationDbContext db, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IMapper mapper,RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            secretkey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        

        public bool IsUniqueUser(string username)
        {
            
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task <LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            


            
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            
            bool IsValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
          

        
            if(user == null || IsValid == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null

                };
            }

           
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretkey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim (ClaimTypes.Name,user.Id.ToString()),
                    
                    new Claim (ClaimTypes.Role,roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                
                User = _mapper.Map<UserDTO>(user),
                


            };
            return loginResponseDTO;
          
        }

        public async Task< UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
          

            ApplicationUser user = new()
            {
                UserName = registerationRequestDTO.UserName,
                Email = registerationRequestDTO.UserName,
                NormalizedEmail = registerationRequestDTO.UserName.ToUpper(),
                Name = registerationRequestDTO.Name,
            };

            try
            {
                var result = await _userManager.CreateAsync(user,registerationRequestDTO.Password);
                if (result.Succeeded)
                {
                    if(!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(user,"admin");
                    var userToReturn = _db.ApplicationUsers
                        .FirstOrDefault(u=>u.UserName == registerationRequestDTO.UserName);
                  
                    return _mapper.Map<UserDTO>(userToReturn);
                }
                
            }
            catch (Exception ex)
            {

            }
            return new UserDTO();
            
        }
    }
}

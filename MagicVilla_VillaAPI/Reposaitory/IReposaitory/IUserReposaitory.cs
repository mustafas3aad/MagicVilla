using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Reposaitory.IReposaitory
{
    public interface IUserReposaitory
    {
        bool IsUniqueUser (string username);
       
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task< UserDTO> Register (RegisterationRequestDTO registerationRequestDTO);
    }
}

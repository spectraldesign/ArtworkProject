using Application.DTO.User;
using Domain.Entities;

namespace Application.Extensions
{
    public static class UserExtensions
    {
        public static UserDTO ToUserDTO(this User user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                UserName = user.UserName,
            };
        }

        public static User ToUser(this CreateUserDTO userDTO)
        {
            return new User()
            {
                UserName = userDTO.UserName,
            };
        }
    }
}

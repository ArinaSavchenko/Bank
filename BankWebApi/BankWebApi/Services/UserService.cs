using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Datalayer.Context;
using Bank.Datalayer.Entities;
using BankWebApi.DTOs;
using BankWebApi.Helpers;
using BankWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankWebApi.Services
{
    public class UserService
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public UserService(BankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserViewModel> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            return _mapper.Map<UserViewModel>(user);
        }

        public async Task<Response<string>> UpdateUserAsync(int id, UpdateUserModel model)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null || user.Status == false)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "There is no such user"
                };
            }

            _mapper.Map(model, user);
            await _context.SaveChangesAsync();

            return new Response<string>
            {
                 Success = true,
                 Message = "User was succesfully updated"
            };
        }

        // public async Task<Response<string>> ChangePasswordAsync(int id, ChangePasswordModel model)
        // {
        //
        //     var user = await _context.Users.FindAsync(id);
        //
        //     if (user == null || user.Status == "Deleted")
        //     {
        //         return new Response<string>
        //         {
        //             Success = false,
        //             Message = "There is no such user"
        //         };
        //     }
        //
        //     if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
        //     {
        //         return new Response<string>
        //         {
        //             Success = false,
        //             Message = "Old password is incorrect"
        //         };
        //     }
        //
        //     user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        //
        //     await _context.SaveChangesAsync();
        //
        //     return new Response<string>
        //     {
        //         Success = true,
        //         Message = "Password was succesfully changed"
        //     };
        // }

        public async Task<Response<string>> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "There is no such user"
                };
            }

            _context.Remove(user);
            await _context.SaveChangesAsync();

            return new Response<string>
            {
                Success = true,
                Message = "User was succesfully deleted"
            };
        }

        // public async Task<Response<User>> AuthenticateAsync(AuthenticateModel model)
        // {
        //     var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Status != "Deleted");
        //
        //     if (user == null)
        //     {
        //         return new Response<User>
        //         {
        //             Success = false,
        //             Message = "There is no user with such email"
        //         };
        //     }
        //
        //     if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
        //     {
        //         return new Response<User>
        //         {
        //             Success = false,
        //             Message = "Password is incorrect"
        //         };
        //     }
        //
        //     return new Response<User>
        //     {
        //         Message = "User authorized",
        //         Data = user
        //     };
        // }

        public async Task<Response<string>> RegisterAsync(RegisterModel model)
        {
            var activeUserExists = _context.Users.Any(x => x.Email == model.Email && x.Status != false);

            if (activeUserExists)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "User with such email already exists"
                };
            }
                
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var newUser = _mapper.Map<User>(model);

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();

            string userFullName = $"{newUser.FirstName} {newUser.LastName}";

            return new Response<string>
            {
                Message = "User was successfully added",
                Data = userFullName
            };
        }
    }
}
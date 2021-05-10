using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Datalayer.Context;
using Bank.Datalayer.Entities;
using BankWebApi.Contracts;
using BankWebApi.DTOs;
using BankWebApi.Helpers;
using BankWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankWebApi.Services
{
    public class UserService: IUser
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

        public async Task<ICollection<UserViewModel>> GetUsers(UserForSearchModel model)
        {
            var users = _context.Users.Where(user => user.Status == true).AsQueryable();
            var result = new List<UserViewModel>();

            if (!string.IsNullOrEmpty(model.FirstName))
            {
                users = users.Where(user => user.FirstName.Contains(model.FirstName));
            }

            if (!string.IsNullOrEmpty(model.LastName))
            {
                users = users.Where(user => user.LastName.Contains(model.LastName));
            }

            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                users = users.Where(user => user.PhoneNumber.ToString().Contains(model.PhoneNumber));
            }
            
            if (model.Role != null)
            {
                users = users.Where(user => user.Role == model.Role);
            }

            if (model.Role == "Client")
            {
                result = await users
                    .Include(user => user.Client)
                    .Select(user => new UserViewModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        DateOfBirth = user.DateOfBirth,
                        Role = user.Role,
                        Email = user.Email,
                        Address = user.Client.Address,
                        Position = "",
                        PhoneNumber = user.PhoneNumber,
                        Status = user.Status
                    })
                    .ToListAsync();
            }

            if (model.Role == "Worker")
            {
                result = await users
                    .Include(user => user.Worker)
                    .Select(user => new UserViewModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        DateOfBirth = user.DateOfBirth,
                        Role = user.Role,
                        Email = user.Email,
                        Address = "",
                        Position = user.Worker.Position,
                        PhoneNumber = user.PhoneNumber,
                        Status = user.Status
                    })
                    .ToListAsync();
            }

            return result;
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

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DateOfBirth = model.DateOfBirth;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            
            if (model.OfficeId != 0 && model.Position != null)
            {
                var worker = await _context.Workers.Where(worker => worker.UserId == id).FirstAsync();

                if (worker == null)
                {
                    return new Response<string>
                    {
                        Success = false,
                        Message = "There is no such worker"
                    };
                }

                worker.Position = model.Position;
            }

            await _context.SaveChangesAsync();

            return new Response<string>
            {
                 Success = true,
                 Message = "User was successfully updated"
            };
        }

        public async Task<Response<string>> ChangePasswordAsync(int id, ChangePasswordModel model)
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
        
            if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "Old password is incorrect"
                };
            }
        
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        
            await _context.SaveChangesAsync();
        
            return new Response<string>
            {
                Success = true,
                Message = "Password was successfully changed"
            };
        }

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

            user.Status = false;
            await _context.SaveChangesAsync();

            return new Response<string>
            {
                Success = true,
                Message = "User was successfully deleted"
            };
        }

        public async Task<Response<User>> AuthenticateAsync(AuthenticateModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Status == true);
        
            if (user == null)
            {
                return new Response<User>
                {
                    Success = false,
                    Message = "There is no user with such email"
                };
            }
        
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return new Response<User>
                {
                    Success = false,
                    Message = "Password is incorrect"
                };
            }
        
            return new Response<User>
            {
                Message = "User authorized",
                Data = user
            };
        }

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

            _context.Users.Add(newUser);

            _context.SaveChanges();

            if (!string.IsNullOrEmpty(model.Address) && model.Role == "Client")
            {
                _context.Clients.Add(new Client
                {
                    UserId = newUser.Id,
                    Address = model.Address
                });
            }

            if (!string.IsNullOrEmpty(model.Position) && model.Role == "Worker")
            {   
                _context.Workers.Add(new Worker
                {
                    UserId = newUser.Id,
                    Position = model.Position
                });
            }
            
            await _context.SaveChangesAsync();

            string userFullName = $"{newUser.FirstName} {newUser.LastName}";

            return new Response<string>
            {
                Message = "User was successfully added",
                Data = userFullName
            };
        }

        public async Task<ClientViewModel> GetClientById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var client = _context.Clients.Where(client => client.UserId == id).FirstOrDefault();

            var newClient = new ClientViewModel
            {
                Id = client.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                userId = user.Id,
                Address = client.Address
            };

            return newClient;
        }

        public async Task<Response<string>> UpdateClientAsync(ClientViewModel model)
        {
            var user = await _context.Users.FindAsync(model.userId);

            if (user == null || user.Status == false)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "There is no such user"
                };
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DateOfBirth = model.DateOfBirth;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var client = await _context.Clients.FindAsync(model.Id);

            if (client == null)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "There is no such client"
                };
            }

            client.Address = model.Address;

            await _context.SaveChangesAsync();

            return new Response<string>
            {
                Success = true,
                Message = "User was successfully updated"
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLearning.Dtos;
using MyLearning.Services;

namespace MyLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("AllowOrigin")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Users retrieved successfully</returns>
        /// <returns code="200">Users retrieved successfully</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> Get()
        {
            var users = await _userRepository.GetAllUsers();

            return Ok(new {
                Status = true,
                Message = "Users retrieved successfully",
                Data = users
            });
        }

        /// <summary>
        /// Get specific user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User retrieved successfully</returns>
        /// <returns code="200">User retrieved successfully</returns>
        /// <returns code="422">The user does not exist</returns>
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userRepository.GetUser(id);
            if(user == null)
            {
                return StatusCode(422, new
                {
                    Status = false,
                    Message = "The user does not exist",
                    Data = new { }
                });
            }
            else
            {
                return Ok(new
                {
                    Status = true,
                    Message = "User retrieved successfully",
                    Data = user
                });
            }


        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userCreateDto"></param>
        /// <returns>User successfully created</returns>
        /// <returns code="201">User successfully created</returns>
        /// <returns code="400">Model state error</returns>
        /// <returns code="422">The username already exist</returns>
        /// <returns code="500">Internal Server Error</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> Post([FromBody] UserCreateDto userCreateDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new {
                    Status = false,
                    Message = ModelState,
                    Data = new { }
                });
            }

            //check if user exist
            var userExist = await _userRepository.CheckUserExistByUserName(userCreateDto.UserName);
            if(userExist)
            {
                return StatusCode(422, new
                {
                    Status = false,
                    Message = "The username already exist",
                    Data = new { }
                });
            }

            var user = await _userRepository.AddUser(new Models.User
            {
                UserName = userCreateDto.UserName,
                Password = userCreateDto.UserName,
                Name = userCreateDto.Name,
                Phone = userCreateDto.Phone,
                Address = userCreateDto.Address,
                CreatedAt = DateTime.Now
            });

            if(user.Id > 0)
            {
                return StatusCode(201, new {
                    Status = true,
                    Message = "User successfully created",
                    Data = user
                });
            }
            else
            {
                return StatusCode(500, new
                {
                    Status = false,
                    Message = "Internal server error",
                    Data = new { }
                });
            }
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="userCreateDto"></param>
        /// <param name="id"></param>
        /// <returns>User was successfully updated</returns>
        /// <returns code="201">User was successfully updated</returns>
        /// <returns code="422">User does not exist</returns>
        /// <returns code="500">Internal Server Error</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> Put([FromBody] UserCreateDto userCreateDto, int id)
        {
            var user = await _userRepository.GetUser(id);

            if(user == null)
            {
                return StatusCode(422, new
                {
                    Status = false,
                    Message = "The user does not exist",
                    Data = new { }
                });
            }

            user.UserName = userCreateDto.UserName;
            user.Password = userCreateDto.UserName;
            user.Name = userCreateDto.Name;
            user.Phone = userCreateDto.Phone;
            user.Address = userCreateDto.Address;

            var result = await _userRepository.UpdateUser(user);
            if(result)
            {
                return StatusCode(201, new
                {
                    Status = true,
                    Message = "User successfully updated",
                    Data = user
                });
            }
            else
            {
                return StatusCode(500, new
                {
                    Status = false,
                    Message = "Internal server error",
                    Data = new { }
                });
            }
        }


        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User was successfully deleted</returns>
        /// <returns code="200">User was successfully deleted</returns>
        /// <returns code="422">The user does not exist</returns>
        /// <returns code="500">Internal Server Error</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetUser(id);

            if (user == null)
            {
                return StatusCode(422, new
                {
                    Status = false,
                    Message = "The user does not exist",
                    Data = new { }
                });
            }

            var result = await _userRepository.DeleteUser(user);

            if(result)
            {
                return StatusCode(200, new
                {
                    Status = true,
                    Message = "User successfully deleted",
                    Data = new { }
                });
            }
            else
            {
                return StatusCode(500, new
                {
                    Status = false,
                    Message = "Internal server error",
                    Data = new { }
                });
            }
        }
    }
}

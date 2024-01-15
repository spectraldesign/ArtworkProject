using Application.Commands.UserCommands;
using Application.DTO.User;
using Application.Queries.UserQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// User API endpoint
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseApiController
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="createUserDTO">User info as json object containing {Username: _, Password: _}</param>
        /// <returns>Jwt Token so the user gets logged in to the new user</returns>
        /// <response code="200">Jwt Token</response>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<IdentityResult>> CreateUser([FromBody] CreateUserDTO userDTO)
        {
            var result = await Mediator.Send(new CreateUserCommand(userDTO));
            if (result.Succeeded)
            {
                return await AuthUser(new CreateUserDTO { UserName = userDTO.UserName, Password = userDTO.Password });
            }
            return new BadRequestObjectResult(result);
        }

        /// <summary>
        /// Login to a user.
        /// </summary>
        /// <param name="loginUserDTO">User info as a json object containing {UserName: _, Password: _}</param>
        /// <returns>Jwt Token used to authorize user</returns>
        /// <response code="200">Jwt Token</response>
        /// <response code="401">Unauthorized</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IdentityResult>> AuthUser([FromBody] CreateUserDTO loginUserDTO)
        {
            var validated = await Mediator.Send(new ValidateUserCommand(loginUserDTO));
            if (validated)
            {
                var token = await Mediator.Send(new CreateTokenCommand(loginUserDTO));
                return Ok(token);
            }
            return Unauthorized();
        }
        /// <summary>
        /// Gets the currently logged in user
        /// </summary>
        /// <returns>{Id, UserName, CreatedAt}</returns>
        [HttpPost("currentUser")]
        public async Task<ActionResult<UserDTO>> GetLoggedInUser()
        {
            var result = await Mediator.Send(new GetLoggedInUserQuery());
            return result == null ? new BadRequestObjectResult(result) : Ok(result);
        }

        /// <summary>
        /// If authorized, deletes the user with the provided ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteUser(string id)
        {
            var result = await Mediator.Send(new DeleteUserCommand(id));
            if (result == -1)
            {
                return Problem(
                    title: "Error, no such user",
                    detail: $"User with ID: {id} was not found",
                    statusCode: StatusCodes.Status404NotFound
                    );
            }
            if (result == -2)
            {
                return Problem(
                    title: "Error, permission denied",
                    detail: $"Logged in user did not have permission to delete user with ID: {id}",
                    statusCode: StatusCodes.Status403Forbidden
                    );
            }
            return Ok($"User with ID: {id} deleted");
        }
    }
}

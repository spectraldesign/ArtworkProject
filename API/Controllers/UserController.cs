using Application;
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
        /// <param name="userDTO">User info as json object containing {Username: _, Password: _}</param>
        /// <returns>{data: JWT?, success, message, responseCode}</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponseType<string>>> CreateUser([FromBody] CreateUserDTO userDTO)
        {
            IdentityResult result = await Mediator.Send(new CreateUserCommand(userDTO));
            if (result.Succeeded)
            {
                return await AuthUser(new CreateUserDTO { UserName = userDTO.UserName, Password = userDTO.Password });
            }
            else
            {
                IdentityError err = result.Errors.First();
                return new BadRequestObjectResult(new ApiResponseType<string>("", false, err.Description, 403));
            }
        }

        /// <summary>
        /// Login to a user.
        /// </summary>
        /// <param name="loginUserDTO">User info as a json object containing {UserName: _, Password: _}</param>
        /// <returns>{data: JWT?, success, message, responseCode}</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponseType<string>>> AuthUser([FromBody] CreateUserDTO loginUserDTO)
        {
            var validated = await Mediator.Send(new ValidateUserCommand(loginUserDTO));
            if (validated)
            {
                string token = await Mediator.Send(new CreateTokenCommand(loginUserDTO));
                return new ApiResponseType<string>(token, true, "Login success", 200);
            }
            return new BadRequestObjectResult(new ApiResponseType<string>("", false, "Failed to authorize user.", 401)); ;
        }

        /// <summary>
        /// Verify a user's token.
        /// </summary>
        /// <param name="verifyTokenDTO">The user id and token to validate.</param>
        /// <returns>{data: bool, success, message, responseCode}</returns>
        [AllowAnonymous]
        [HttpPost("verify")]
        public async Task<ActionResult<ApiResponseType<bool>>> VerifyToken([FromBody] VerifyTokenDTO verifyTokenDTO)
        {
            bool isValid = await Mediator.Send(new ValidateTokenCommand(verifyTokenDTO));
            if (isValid)
            {
                return new ApiResponseType<bool>(isValid, true, "Valid token.", 200);
            }
            return new BadRequestObjectResult(new ApiResponseType<bool>(false, false, "Invalid token.", 401));
        }

        /// <summary>
        /// Gets the currently logged in user
        /// </summary>
        /// <returns>{data: {Id, UserName, CreatedAt}?, success, message}</returns>
        [HttpPost("currentUser")]
        public async Task<ActionResult<ApiResponseType<UserDTO>>> GetLoggedInUser()
        {
            UserDTO result = await Mediator.Send(new GetLoggedInUserQuery());
            if (result == null)
            {
                return new BadRequestObjectResult(new ApiResponseType<UserDTO>(new UserDTO(), false, "An error occurred getting current user.", 404));
            }
            else
            {
                return new ApiResponseType<UserDTO>(result, true, "", 404);
            }
        }

        /// <summary>
        /// If authorized, deletes the user with id (and all their content)
        /// </summary>
        /// <param name="userID">userID to delete</param>
        /// <returns>{data: "", success, message, responseCode}</returns>
        [HttpDelete("{userID}")]
        public async Task<ActionResult<ApiResponseType<string>>> DeleteUser(string userID)
        {
            var result = await Mediator.Send(new DeleteUserCommand(userID));
            if (result == -1)
            {
                return new BadRequestObjectResult(new ApiResponseType<string>("", false, $"User with ID: {userID} was not found", 404));
            }
            if (result == -2)
            {
                return new BadRequestObjectResult(new ApiResponseType<string>("", false, $"Logged in user did not have permission to delete user with ID: {userID}", 403));
            }
            return new ApiResponseType<string>("", true, $"User with ID: {userID} deleted", 200);
        }

        /// <summary>
        /// Updates a user with Id based on provided information.
        /// You must have permission to update the user with ID, normally this will only be your own account.
        /// </summary>
        /// <param name="userID">The ID for the user to update</param>
        /// <param name="updateUserDTO">Json object with update info, valid fields are any of {UserName, Password}</param>
        /// <returns>{data: "", success, message, responseCode}</returns>
        [HttpPut("{userID}")]
        public async Task<ActionResult<ApiResponseType<string>>> UpdateUserById(string userID, [FromBody] CreateUserDTO updateUserDTO)
        {
            var result = await Mediator.Send(new UpdateUserCommand(new UpdateUserDTO() { Id = userID, UserName = updateUserDTO.UserName, Password = updateUserDTO.Password }));
            if (!result.Succeeded)
            {
                IdentityError err = result.Errors.First();
                return new BadRequestObjectResult(new ApiResponseType<string>("", false, $"{err.Code} | {err.Description}", Int32.Parse(err.Code)));
            }
            return new ApiResponseType<string>("", true, $"User with ID: {userID} updated successfully", 200);
        }
    }
}

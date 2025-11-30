using Shared.DTOs;

namespace Frontend.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsers();
    Task<UserDto?> GetUser(string id);
    Task<UserResponse> CreateUser(CreateUserRequest request);
    Task<UserResponse> UpdateUserRoles(string id, UpdateUserRolesRequest request);
    Task<bool> DeleteUser(string id);
    Task<IEnumerable<string>> GetRoles();
}

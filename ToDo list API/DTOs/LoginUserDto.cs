using System.ComponentModel.DataAnnotations;

namespace ToDo_list_API.DTOs;

public record class LoginUserDto(
[Required][StringLength(100)] string Email
);
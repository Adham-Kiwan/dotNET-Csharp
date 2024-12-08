using System.ComponentModel.DataAnnotations;

namespace ToDo_list_API.DTOs;

public record class CreateUserDto(
[Required][StringLength(50)] string Name,
[Required][StringLength(200)] string Email
);
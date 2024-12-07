using System.ComponentModel.DataAnnotations;

namespace ToDo_list_API.DTOs;

public record class CreateToDoDto(
[Required][StringLength(50)] string Title,
[Required][StringLength(200)] string Text
);


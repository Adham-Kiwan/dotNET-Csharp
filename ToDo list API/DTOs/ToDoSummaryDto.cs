using System;

namespace ToDo_list_API.DTOs;

public record class ToDoSummaryDto(
    int Id,
    string Title,
    string Text
    );

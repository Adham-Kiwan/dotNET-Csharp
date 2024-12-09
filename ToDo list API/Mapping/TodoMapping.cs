using System;
using ToDo_list_API.DTOs;
using ToDo_list_API.Entities;


namespace ToDo_list_API.Mapping;

public static class TodoMapping
{
    public static Todos ToEntity(this CreateToDoDto todo)
    {
        return new Todos()
        {
            Title = todo.Title,
            Text = todo.Text,
            UserId = 1 // assuming there is a user with id 1
        };
    }

    public static Todos ToEntity(this UpdateToDoDto todo, int id)
    {
        return new Todos()
        {
            Id = id,
            Title = todo.Title,
            Text = todo.Text,
            UserId = 1 // assuming there is a user with id 1
        };
    }
    public static ToDoSummaryDto ToToDoSummaryDto(this Todos todo)
    {
        return new(

                todo.Id,
                todo.Title,
                todo.Text
            );
    }


    public static ToDoDto ToDto(this Todos todo)
    {
        return new(
                 todo.Id,
                 todo.Title,
                 todo.Text
             );
    }
}

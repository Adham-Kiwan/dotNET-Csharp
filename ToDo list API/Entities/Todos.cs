namespace ToDo_list_API.Entities;

public class Todos {
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Text { get; set; }

    public int UserId { get; set; }

    public Users? User { get; set; }
}
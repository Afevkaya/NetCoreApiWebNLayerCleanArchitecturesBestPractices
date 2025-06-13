namespace App.Repositories.BaseEntites;

public class BaseEntity<T>
{
    public T Id { get; set; } = default!;
}
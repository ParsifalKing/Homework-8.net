namespace Domain.Models;

public class ListOfSome<T1, T2>
{
    public T1 Any { get; set; }

    public List<T2> SomeList { get; set; }
}

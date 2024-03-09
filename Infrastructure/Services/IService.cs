using Infrastructure.DataContext;

namespace Infrastructure.Services;

public interface IService<T>
{
    public Task<string> Add(T some);
    public Task<List<T>> Get();
    public Task<string> Update(T some);
    public Task<string> Delete(int id);
}

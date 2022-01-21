namespace Infra.DAL.Contracts;
public interface IGenericDAL<T>
{
    Task<T?> Get(int Id);
    Task<bool> Update(T entity);
    Task<bool> Create(T entity);
    Task<bool> Delete(T entity);
}
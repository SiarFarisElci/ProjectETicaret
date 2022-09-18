using Project.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesingPatterns.GenericRepository.InRep
{
    public interface IRepository<T> where T : BaseEntity
    {
        //List Command

        List<T> GetAll();
        List<T> GetActives();
        List<T> GetDeleted();
        List<T> GetUpdated();

        //Modify Command

        void Add(T item);
        void Updated(T item);
        void Deleted(T item);
        void Destroy(T item);

        void AddRange(List<T> list);
        void UpdatedRange(List<T> list);
        void DeletedRange(List<T> list);
        void DestroyRange(List<T> list);

        //Linq Command

        bool Any(Expression<Func<T , bool>> exp);

        List<T> Where(Expression<Func<T, bool>> exp);

        T FirstOrDefault(Expression<Func<T, bool>> exp);

        object Select(Expression<Func<T, object>> exp);

        IQueryable<X> Select<X>(Expression<Func<T , X>> exp);

        //Find Command

        T Find(int number);

        //Get First Datas

        List<T> GetFirstDatas(int number);

        //Get Last Datas
        List<T> GetLastDatas(int number);

    }
}

using HotelReservation.Data.Common;
using Unity;

namespace HotelReservation.Bo
{
    public class BoBase<TEntity> where TEntity : class
    {
        public UnityContainer UnityContainer;
        public IRepository<TEntity> Repository;
       // public IUnitOfWork UnitOfWork;
        public BoBase()
        {
            UnityContainer = UnityConfiguration.Container;
            //UnitOfWork = UnityContainer.Resolve<IUnitOfWork>();
            Repository = new RepositoryBase<TEntity>();
        }
    }
}

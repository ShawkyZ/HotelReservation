using HotelReservation.Data.Common;
using Unity;

namespace HotelReservation.Bo
{
    public class BoBase<TEntity> where TEntity : class
    {
        public UnityContainer UnityContainer;
        public IRepository<TEntity> Repository;
        private IUnitOfWork uof;
        public BoBase()
        {
            UnityContainer = UnityConfiguration.Container;
            uof = UnityContainer.Resolve<IUnitOfWork>();
            Repository = uof.RepositoryFor<TEntity>();
        }
    }
}

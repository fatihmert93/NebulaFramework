using Nebula.CoreLibrary.Shared;
using Nebula.DataAccessLibrary.Dapper.PostgreSql;

namespace Nebula.Membership.Repositories.Postgresql
{
    public class NpgUserRepository : NpgGenericRepository<User>, IUserRepository
    {
        public NpgUserRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
            
            
        }


        public void CreateDeneme()
        {
            
        }
    }
}

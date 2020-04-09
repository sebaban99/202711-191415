using IMMRequest.Domain;
using System;
using System.Data.Common;
using System.Linq;

namespace IMMRequest.DataAccess.Repositories
{
    public class TopicRepository : BaseRepository<Topic>
    {
        public TopicRepository(IMMRequestContext context)
        {
            Context = context;
        }

        public override Topic Get(Guid id)
        {
            try
            {
                return Context.Topics.First(x => x.Id == id);
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
            catch (DbException)
            {
                throw new DataAccessException("Error: could not get specific Entity");
            }
        }
    }
}

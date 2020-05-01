using IMMRequest.Domain;
using System;
using System.Data.Common;
using System.Linq;

namespace IMMRequest.DataAccess.Repositories
{
    public class AdminRepository : BaseRepository<Admin>
    {
        public AdminRepository(IMMRequestContext context)
        {
            Context = context;
        }

        public override Admin Get(Guid id)
        {
            try
            {
                return Context.Administrators.First(x => x.Id == id);
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

        public bool ModifyToken(Admin admin)
        {
            try
            {
                var saved = GetByCondition(a => a.Email == admin.Email);
                saved.SessionToken = admin.SessionToken;
                Context.Attach(saved);
                Context.Update(saved);
                Context.SaveChanges();
                return true;

            }
            catch (DbException e)
            {
                throw (new DataAccessException("Error al Modificar Token " + e.Message));
            }

        }
    }
}
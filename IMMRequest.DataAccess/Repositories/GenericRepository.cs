using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace IMMRequest.DataAccess
{
    public abstract class GenericRepository<T> : IRepository<T> where T: class  {
        private IMMRequestContext context;
        private DbSet<T> table;
        public GenericRepository()
        {
            context = ContextFactory.GetNewContext(ContextType.SQL);
            table = context.Set<T>();
        }
         public GenericRepository(IMMRequestContext _context)
        {
            this.context = _context;
            table = _context.Set<T>();
        }
        public void Add(T entidad){
            if(entidad!=null) {
                context.Add(entidad);
                Save();
            }
            else throw new ExceptionRepository("Error en create: La entidad no debe ser nula");
        }

        public void Remove(T elem){
            if(elem!=null){
                //T existing = this.Get(id);
                //if(existing!=null){
                table.Remove(elem);
                Save();
                
                
            }
            else throw new ExceptionRepository("Error en delete:  La entidad no existe");
            //}else throw new ExceptionRepository("Error en delete:  El id es vacio o invalido");
        }
        

        public void Update(T entidad){
            if(entidad!=null) {
                try{
                    //context.Update(entidad);
                    //context.SaveChanges();
                    table.Attach(entidad);
                    context.Entry(entidad).State = EntityState.Modified; 
                    
                    context.SaveChanges();
                }catch(DbUpdateException exc){
                    throw new ExceptionRepository("Error en update: no se pudo relizar el cambio en la bd, Excepcion: "+ exc.Message, exc);
                }
            }
            else throw new ExceptionRepository("Error en update: La entidad no debe ser nula");
        }

        public IEnumerable<T> GetAll(){
            return table;
        }

        public T Get(Guid id){
            if(id!=Guid.Empty)
                return table.Find(id);
                
            else return null;    
        }

        public void Save(){
            try{
                this.context.SaveChanges();
            }catch(DbUpdateException exc){
                    throw new ExceptionRepository("Error en save: no se pudo relizar el cambio en la bd, Excepcion: "+ exc.Message, exc);
                }
        }
    }
}
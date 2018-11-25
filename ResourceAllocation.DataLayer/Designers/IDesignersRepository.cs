using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.Designers
{
   public interface IDesignersRepository
    {
        Task Add(DesignerEntity entity);
        Task Delete(Guid id);
        Task<IEnumerable<DesignerEntity>> GetAll();
        Task<DesignerEntity> GetById(Guid id);
        Task Update(DesignerEntity entity);
    }
}

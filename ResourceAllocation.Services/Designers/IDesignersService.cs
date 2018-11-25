using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.Designers
{
    public interface IDesignersService
    {
        Task Add(DesignerEntity entity);
        Task Delete(Guid id);
        Task<IEnumerable<DesignerEntity>> GetAll();
        Task<DesignerEntity> GetById(Guid id);
        Task Update(DesignerEntity entity);
    }
}

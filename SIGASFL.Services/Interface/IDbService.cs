using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models;

namespace SIGASFL.Services.Interface
{
    public interface IDbService<TView, TId>
    {
        Task<ClientResponse<IEnumerable<TView>>> GetAll();
        Task<ClientResponse<TView>> GetById(TId Id);
        Task<ClientResponse<TView>> Create(TView view);
        Task<ClientResponse<TView>> Edit(TView view);
        Task<ClientResponse<bool>> Delete(TId Id);
    }
}

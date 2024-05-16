using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models.Views;
using SIGASFL.Models;

namespace SIGASFL.Services.Interface
{
    public interface IBaseService<T>
    {
        Task<ClientResponse<IEnumerable<T>>> GetAll();
        Task<ClientResponse<T>> GetById(string Id);
        Task<ClientResponse<T>> Create(T _view);
        Task<ClientResponse<T>> Edit(T _view);
        Task<ClientResponse<bool>> Delete(string Id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models;
using SIGASFL.Repositories;
using SIGASFL.Services.Interface;
using SIGASFL.Services.Mapper;

namespace SIGASFL.Services.Implementation
{
    public class StoredProcedureService : IStoredProcedureService
    {
        private readonly ApplicationContext db;
        private readonly ICustomMapper customMapper;
        public StoredProcedureService(ApplicationContext context, ICustomMapper mapper)
        {
            db = context;
            customMapper = mapper;
        }

        /*public async Task<ClientResponse<IEnumerable<RecurringExpensesView>>> GetRecurringExpenses(RecurringExpensesRequest request)
        {
            var response = new ClientResponse<IEnumerable<RecurringExpensesView>>();
            var entities = await db.RecurringExpenses.SelectFromStoreProcedure(request, "GetRecurrentExpenses").ToListAsync();

            response.Data = customMapper.Map<IEnumerable<RecurringExpensesView>>(entities);
            return response;
        }*/
    }
}

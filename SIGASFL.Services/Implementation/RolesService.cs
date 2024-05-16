using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models;
using SIGASFL.Models.Views;
using SIGASFL.Repositories;
using SIGASFL.Services.Interface;
using SIGASFL.Services.Mapper;

namespace SIGASFL.Services.Implementation
{
    public class RolesService : IRolesService
    {
        private readonly ApplicationContext db;
        private readonly ICustomMapper customMapper;
        public RolesService(ApplicationContext context, ICustomMapper mapper)
        {
            db = context;
            customMapper = mapper;
        }

        public async Task<ClientResponse<RolesView>> Create(RolesView _view)
        {
            var response = new ClientResponse<RolesView>();

            try
            {
                var entity = customMapper.Map<RolesView>(_view);

                db.Add(entity);
                var result = await db.SaveChangesAsync();

                if (result <= 0)
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_CREATED, ref response);
                else
                    response.Data = customMapper.Map<RolesView>(entity);

            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, dbEx.Message);
            }
            catch (Exception ex)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, ex.Message);
            }

            return response;
        }

        public async Task<ClientResponse<bool>> Delete(string Id)
        {
            var response = new ClientResponse<bool>();
            try
            {
                var entity = await db.Roles.FindAsync(Id);
                if (entity == null)
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_FOUND, ref response);
                    return response;
                }

                db.Roles.Remove(entity);
                var result = await db.SaveChangesAsync() > 0;
                if (result)
                {
                    response.Data = true;
                }
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, dbEx.Message);
            }
            catch (Exception ex)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, ex.Message);
            }

            return response;
        }

        public async Task<ClientResponse<RolesView>> Edit(RolesView _view)
        {
            var response = new ClientResponse<RolesView>();
            try
            {

                var entity = await db.Roles.FindAsync(_view.Id);
                if (entity == null)
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_FOUND, ref response);
                    return response;
                }
                entity = customMapper.Map<Entities.Roles>(_view);

                db.Update(entity).Property(x => x.Seq).IsModified = false;

                var rows = await db.SaveChangesAsync();
                if (rows <= 0)
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_CREATED, ref response);
                    return response;
                }

                response.Data = customMapper.Map<RolesView>(entity);
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, dbEx.Message);
            }
            catch (Exception ex)
            {
                CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref response, ex.Message);
            }

            return response;
        }

        public async Task<ClientResponse<IEnumerable<RolesView>>> GetAll()
        {
            var response = new ClientResponse<IEnumerable<RolesView>>();
            var entities = await db.Roles
                                .AsNoTracking()
                                .ToListAsync();

            response.Data = customMapper.Map<IEnumerable<RolesView>>(entities);
            return response;
        }

        public async Task<ClientResponse<RolesView>> GetById(string Id)
        {
            var response = new ClientResponse<RolesView>();
            var entity = await db.Roles
                                .AsNoTracking()
                                .FirstOrDefaultAsync(f => f.Id == Id);

            response.Data = customMapper.Map<RolesView>(entity);
            return response;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models;
using SIGASFL.Repositories.Interface;
using SIGASFL.Services.Interface;
using SIGASFL.Services.Mapper;

namespace SIGASFL.Services.Implementation
{
    public class DbService<TEntity, TView, TId> : IDbService<TView, TId>
     where TEntity : class
     where TView : BaseView<TId>
    {

        public readonly IBaseRepository<TEntity> Repository;
        protected readonly ICustomMapper CustomMapper;
        public DbService(IBaseRepository<TEntity> repo, ICustomMapper mapper)
        {
            Repository = repo;
            CustomMapper = mapper;
        }

        public async Task<ClientResponse<TView>> Create(TView view)
        {
            var response = new ClientResponse<TView>();

            try
            {
                SetAuditFieldsCreate(view);
                var entity = CustomMapper.Map<TEntity>(view);
                Repository.Add(entity);
                var result = await Repository.SaveChangesAsync();

                if (result <= 0)
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_CREATED, ref response);
                else
                    response.Data = CustomMapper.Map<TView>(entity);

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

        public async Task<ClientResponse<TView>> Edit(TView view)
        {
            var response = new ClientResponse<TView>();

            try
            {
                SetAuditFieldsUpdate(view);
                var entity = Repository.GetById(view.Id);
                if (entity == null)
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_FOUND, ref response);
                else
                {
                    Repository.Update(CustomMapper.Map<TEntity>(view));
                    var result = await Repository.SaveChangesAsync();

                    if (result <= 0)
                        CommonMessage.SetMessage(CommonMessage.ERROR_PASS_NOT_UPDATED, ref response);
                    else
                        response.Data = CustomMapper.Map<TView>(entity);
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

        public async Task<ClientResponse<bool>> Delete(TId Id)
        {
            var response = new ClientResponse<bool>();

            try
            {
                var entity = Repository.GetById(Id);

                if (entity != null)
                {
                    Repository.Remove(entity);

                    var result = await Repository.SaveChangesAsync();

                    if (result <= 0)
                        CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_REMOVED, ref response);
                    else
                        response.Data = true;
                }
                else
                {
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_FOUND, ref response);
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


        public async Task<ClientResponse<IEnumerable<TView>>> GetAll()
        {
            var response = new ClientResponse<IEnumerable<TView>>();

            try
            {
                var entities = Repository.GetAll();

                if (entities == null || entities.Count() == 0)
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_FOUND, ref response);
                else
                    response.Data = CustomMapper.Map<IEnumerable<TView>>(entities);

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

        public async Task<ClientResponse<TView>> GetById(TId Id)
        {
            var response = new ClientResponse<TView>();

            try
            {
                var entity = Repository.GetById(Id);

                if (entity == null)
                    CommonMessage.SetMessage(CommonMessage.ERROR_RECORD_NOT_FOUND, ref response);
                else
                    response.Data = CustomMapper.Map<TView>(entity);

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

        private void SetAuditFieldsCreate(TView view)
        {
            var viewToEdit = view as BaseAuditFields<TId>;
            //if (viewToEdit != null && AccountService != null && AccountService.Profile != null)
            //{
            //    viewToEdit.CreatedByUserId = AccountService.Profile.UserId;
            //    viewToEdit.CreatedDate = DateTime.Now;
            //}
        }

        private void SetAuditFieldsUpdate(TView view)
        {
            var viewToEdit = view as BaseAuditFields<TId>;
            //if (viewToEdit != null && AccountService != null && AccountService.Profile != null)
            //{
            //    viewToEdit.ModifiedByUserId = AccountService.Profile.UserId;
            //    viewToEdit.ModifiedDate = DateTime.Now;
            //}
        }
    }
}

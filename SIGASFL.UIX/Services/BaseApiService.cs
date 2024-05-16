namespace SIGASFL.UIX.Services
{
    public abstract class BaseApiService
    {
        protected readonly IApiClientService Api;

        public BaseApiService(IApiClientService api)
        {
            Api = api;
        }
    }
}

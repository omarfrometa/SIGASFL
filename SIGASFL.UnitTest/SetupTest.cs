using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.UnitTest
{
    public class SetupTest
    {
        private static readonly string _connStringApi = "Server=sql.fvtech.net;Database=FH;MultipleActiveResultSets=true;Application Name=FH.WEBAPI;User=fh;Password=fh*";
        /*
        static SetupTest()
        {
            DbContextOptionsBuilder<ApplicationContext> builderApplicationContext = new DbContextOptionsBuilder<ApplicationContext>();
            GetApplicationContext = new ApplicationContext(builderApplicationContext.UseSqlServer(_connStringApi).Options);

            var configMap = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            CustomMapper customMapper = new CustomMapper(configMap.CreateMapper());

            GetUserRep = new Repository.Implementation.Repository<FH.Entities.User>(GetApplicationContext);
            GetTransactionCategoryRep = new Repository.Implementation.Repository<FH.Entities.TransactionCategory>(GetApplicationContext);
            GetPorfolioRep = new Repository.Implementation.Repository<FH.Entities.Porfolio>(GetApplicationContext);
            GetBankAccountRep = new Repository.Implementation.Repository<FH.Entities.BankAccount>(GetApplicationContext);
            GetAssetsRep = new Repository.Implementation.Repository<FH.Entities.Assets>(GetApplicationContext);
            GetStoredProcedureService = new StoredProcedureService(GetApplicationContext, customMapper);

            //GetYodleeService = new YodleeService(GetUserRep, GetTransactionCategoryRep, GetPorfolioRep, GetBankAccountRep, GetAssetsRep);
        }

        public static ApplicationContext GetApplicationContext;
        public static IRepository<FH.Entities.User> GetUserRep;
        public static IRepository<FH.Entities.TransactionCategory> GetTransactionCategoryRep;
        public static IRepository<FH.Entities.Porfolio> GetPorfolioRep;
        public static IRepository<FH.Entities.BankAccount> GetBankAccountRep;
        public static IRepository<FH.Entities.Assets> GetAssetsRep;

        public static IYodleeService GetYodleeService;
        public static IStoredProcedureService GetStoredProcedureService;*/
    }
}

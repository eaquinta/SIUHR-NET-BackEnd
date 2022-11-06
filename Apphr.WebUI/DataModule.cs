// ToDelete


//using Apphr.WebUI.Models;
//using Autofac;

//namespace Apphr.WebUI
//{
//    public class DataModule : Module
//    {
//        private string _connstr;
//        public DataModule(string connstr)
//        {
//            _connstr = connstr;
//        }
//        protected override void Load(ContainerBuilder builder)
//        {
//            builder.Register(c => new ApphrDbContext(_connstr)).InstancePerRequest();

//            base.Load(builder);
//        }
//    }       
//}
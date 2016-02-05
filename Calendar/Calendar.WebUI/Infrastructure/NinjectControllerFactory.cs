using System;
using System.Web.Mvc;
using System.Web.Routing;
using Calendar.Domain.Abstract;
using Ninject;
using Calendar.Domain.Concrete;
using Calendar.WebUI.Infrastructure.Abstract;
using Calendar.WebUI.Infrastructure.Concrete;

namespace Calendar.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {

        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext
            requestContext, Type controllerType)
        {
            return controllerType == null
            ? null
            : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IDayRepository>().To<EFDayRepository>();
            ninjectKernel.Bind<ICardRepository>().To<EFCardRepository>();

            ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();

        }
    }
}
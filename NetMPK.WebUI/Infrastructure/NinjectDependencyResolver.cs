using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Moq;
using NetMPK.Domain.Abstract;
using NetMPK.Domain.Entities;
using NetMPK.Domain.Concrete;

namespace NetMPK.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            throw new NotImplementedException();
        }
        private void AddBindings()
        {
            kernel.Bind<IStopRepository>().To<StopRepository>();
        }
    }
}
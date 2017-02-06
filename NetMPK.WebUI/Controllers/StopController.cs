using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetMPK.Domain.Abstract;
using NetMPK.Domain.Entities;

namespace NetMPK.WebUI.Controllers
{
    public class StopController : Controller
    {
        private IStopRepository repository;
        private int pageSize = 25;
        
        public StopController(IStopRepository stopRepository)
        {
            repository = stopRepository;
        }

        public ViewResult List(int page = 1)
        {
            return View(repository.Stops.OrderBy(s => s.name).Skip((page-1) * pageSize).Take(pageSize));
        }
    }
}
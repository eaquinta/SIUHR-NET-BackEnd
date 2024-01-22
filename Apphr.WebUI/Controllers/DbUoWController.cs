using Apphr.Application;
using Apphr.Application.Common.Models;
using Apphr.Infrastructure.Persistence.DBF;
using Apphr.WebUI.Models;
using Apphr.WebUI.Models.Repository;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Apphr.WebUI.Controllers
{
    public class DbUoWController : Controller
    {        
        protected UnitOfWork uow = new UnitOfWork();
        protected DBFContext dbfContext;
        protected string userName;
        //protected string dbfPath = ConfigurationManager.AppSettings["SiahrPath"].ToString();
        protected int pageSize                              = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"].ToString()); 
        protected int autoCompleteSize                      = Convert.ToInt32(ConfigurationManager.AppSettings["AutocompleteSize"].ToString()); 
        protected readonly IMapper mapper                   = AutoMapperConfig._mapper;
        protected Dictionary<string, ToastTemplate> DTT     = new Dictionary<string, ToastTemplate>();
        protected ViewConfig C                              = new ViewConfig();
        protected PagedListRenderOptions PagedListOptions;
        public DbUoWController() //(ApphrDbContext _db)
        {
            //db = new ApphrDbContext(System.Web.HttpContext.Current.User.Identity.Name);
            dbfContext                                      = new DBFContext(); 
            userName                                        = System.Web.HttpContext.Current.User.Identity.GetUserName();
            PagedListOptions                                = new PagedListRenderOptions
            {
                MaximumPageNumbersToDisplay = 5,
                DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                LinkToFirstPageFormat = "<i class=\"fas fa-fast-backward\"></i>",
                LinkToPreviousPageFormat = "<i class=\"fas fa-step-backward\"></i>",
                LinkToNextPageFormat = "<i class=\"fas fa-step-forward\"></i>",
                LinkToLastPageFormat = "<i class=\"fas fa-fast-forward\"></i>"
            };

            DTT.Add("success.delete", new ToastTemplate()
            {
                Title = "Eliminado",
                Message = "Registro eliminado con éxito",
                Type = "success"
            });
            DTT.Add("success.create", new ToastTemplate()
            {
                Title = "Creado",
                Message = "Registro creado con éxito",
                Type = "success"
            });
            DTT.Add("success.edit", new ToastTemplate()
            {
                Title = "Actualizado",
                Message = "Registro actualizado con éxito",
                Type = "success"
            });
        }

        protected List<ToastTemplate> GetToastList(List<string> Toast)
        {
            List<ToastTemplate> ListToastTemplate = new List<ToastTemplate>();
            if (Toast != null)
                foreach (ToastTemplate tt in DTT.Where(x => Toast.Contains(x.Key)).Select(x => x.Value).ToList())
                    ListToastTemplate.Add(tt);
            return ListToastTemplate;
        }

        protected int GetUserId(string UserName)
        {
            return uow.UserRepository.GetAll(x => x.Email == userName).FirstOrDefault().Id;
        }
        protected int GetUserId()
        {
            return Int32.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
        }

        public bool IsActionAvailable(string Action)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string areaName = this.ControllerContext.RouteData.DataTokens["area"].ToString();           
            return false;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                if (this.dbfContext != null)
                {
                    this.dbfContext.Dispose();
                    this.dbfContext = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}

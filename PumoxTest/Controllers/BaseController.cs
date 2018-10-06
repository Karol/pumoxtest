using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PumoxTest.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PumoxTest.Controllers
{
    public class BaseController : Controller
    {
        private UnitOfWork _unitOfWork;
        protected IConfiguration Configuration { get; set; }
        protected IMapper Mapper;

        public BaseController(IConfiguration config, IMapper mapper)
        {
            Configuration = config;
            Mapper = mapper;
        }

        /// <summary>
        /// UnitOfWork
        /// </summary>
        protected UnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                {
                    var conString = Configuration.GetConnectionString("DefaultConnection");
                    _unitOfWork = new UnitOfWork(conString);
                }
                return _unitOfWork;
            }
        }
    }
}

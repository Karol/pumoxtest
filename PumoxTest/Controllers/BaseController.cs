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
        protected IConfiguration _configuration { get; set; }
        protected IMapper _mapper;

        public BaseController(IConfiguration config, IMapper mapper)
        {
            _configuration = config;
            _mapper = mapper;
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
                    var conString = _configuration.GetConnectionString("DefaultConnection");
                    _unitOfWork = new UnitOfWork(conString);
                }
                return _unitOfWork;
            }
        }
    }
}

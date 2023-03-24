using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Irontrax.WebApplication.Controllers
{
    public class ReactController : Controller
    {
        public string Congratulate(string id)
        {
            return $"Congratulations {id}";
        }
    }
}
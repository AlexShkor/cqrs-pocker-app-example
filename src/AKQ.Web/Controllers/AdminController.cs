using System.Web.Mvc;
using AKQ.Domain.Infrastructure;

namespace AKQ.Web.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly IdGenerator _idGenerator;

        public AdminController(IdGenerator idGenerator)
        {
            _idGenerator = idGenerator;
        }
    }
}
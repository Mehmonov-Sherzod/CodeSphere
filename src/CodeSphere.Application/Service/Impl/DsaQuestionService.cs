using CodeSphere.DataAccess.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service.Impl
{
    public class DsaQuestionService
    {
        private readonly AppDbContext _appDbContext;
        public DsaQuestionService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
    }
}

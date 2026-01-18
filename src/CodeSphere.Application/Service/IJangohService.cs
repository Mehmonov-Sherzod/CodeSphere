using CodeSphere.Application.Models;
using CodeSphere.Application.Models.JangohModels;
using CodeSphere.Application.Service.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service
{
    public interface IJangohService
    {
        Task<ApiResult<Verdrict>> SubmitCode(SubmitCodes codes);
    }
}

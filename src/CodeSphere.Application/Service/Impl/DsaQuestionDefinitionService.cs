using CodeSphere.Application.Models;
using CodeSphere.Application.Models.DsaQuestionDefinition;
using CodeSphere.DataAccess.Persistence;
using CodeSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service.Impl
{
    public class DsaQuestionDefinitionService : IDsaQuestionDefinitionService
    {
        private readonly AppDbContext _appDbContext;

        public DsaQuestionDefinitionService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ApiResult<string>> CreateDsaQuestionDefinition(CreateDsaQuestionDefinition createDsaQuestionDefinition)
        {
            var questionExists = await _appDbContext.DsaQuestions.AnyAsync(q => q.Id == createDsaQuestionDefinition.DsaQuestionsId);
            if (!questionExists)
                return ApiResult<string>.Failure(new List<string> { "DSA Question not found" });

            var existingDefinition = await _appDbContext.DsaQuestionDefinitions
                .AnyAsync(d => d.DsaQuestionsId == createDsaQuestionDefinition.DsaQuestionsId);
            if (existingDefinition)
                return ApiResult<string>.Failure(new List<string> { "A definition already exists for this question" });

            var definition = new Domain.Entities.DsaQuestionDefinition
            {
                Id = Guid.NewGuid(),
                ClassName = createDsaQuestionDefinition.ClassName,
                MethodName = createDsaQuestionDefinition.MethodName,
                ReturnType = createDsaQuestionDefinition.ReturnType,
                DsaQuestionsId = createDsaQuestionDefinition.DsaQuestionsId,
                Parameters = new List<DsaQuestionDefinitionParameteres>()
            };

            if (createDsaQuestionDefinition.Parameters != null && createDsaQuestionDefinition.Parameters.Any())
            {
                foreach (var param in createDsaQuestionDefinition.Parameters)
                {
                    definition.Parameters.Add(new DsaQuestionDefinitionParameteres
                    {
                        Id = Guid.NewGuid(),
                        Name = param.Name,
                        Type = param.Type,
                        DsaQuestionDefinitionId = definition.Id
                    });
                }
            }

            await _appDbContext.DsaQuestionDefinitions.AddAsync(definition);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("DSA Question Definition has been created");
        }

        public async Task<ApiResult<GetDsaQuestionDefinition>> GetDsaQuestionDefinitionById(Guid id)
        {
            var definition = await _appDbContext.DsaQuestionDefinitions
                .Include(d => d.Parameters)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (definition == null)
                return ApiResult<GetDsaQuestionDefinition>.Failure(new List<string> { "DSA Question Definition not found" });

            var result = new GetDsaQuestionDefinition
            {
                Id = definition.Id,
                ClassName = definition.ClassName,
                MethodName = definition.MethodName,
                ReturnType = definition.ReturnType,
                DsaQuestionsId = definition.DsaQuestionsId,
                Parameters = definition.Parameters?.Select(p => new GetDsaQuestionDefinitionParameter
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type
                }).ToList() ?? new List<GetDsaQuestionDefinitionParameter>()
            };

            return ApiResult<GetDsaQuestionDefinition>.Success(result);
        }

        public async Task<ApiResult<GetDsaQuestionDefinition>> GetDsaQuestionDefinitionByQuestionId(Guid dsaQuestionId)
        {
            var definition = await _appDbContext.DsaQuestionDefinitions
                .Include(d => d.Parameters)
                .FirstOrDefaultAsync(d => d.DsaQuestionsId == dsaQuestionId);

            if (definition == null)
                return ApiResult<GetDsaQuestionDefinition>.Failure(new List<string> { "DSA Question Definition not found" });

            var result = new GetDsaQuestionDefinition
            {
                Id = definition.Id,
                ClassName = definition.ClassName,
                MethodName = definition.MethodName,
                ReturnType = definition.ReturnType,
                DsaQuestionsId = definition.DsaQuestionsId,
                Parameters = definition.Parameters?.Select(p => new GetDsaQuestionDefinitionParameter
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type
                }).ToList() ?? new List<GetDsaQuestionDefinitionParameter>()
            };

            return ApiResult<GetDsaQuestionDefinition>.Success(result);
        }

        public async Task<ApiResult<string>> UpdateDsaQuestionDefinition(UpdateDsaQuestionDefinition updateDsaQuestionDefinition, Guid id)
        {
            var definition = await _appDbContext.DsaQuestionDefinitions
                .Include(d => d.Parameters)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (definition == null)
                return ApiResult<string>.Failure(new List<string> { "DSA Question Definition not found" });

            definition.ClassName = updateDsaQuestionDefinition.ClassName;
            definition.MethodName = updateDsaQuestionDefinition.MethodName;
            definition.ReturnType = updateDsaQuestionDefinition.ReturnType;

            if (updateDsaQuestionDefinition.Parameters != null)
            {
                _appDbContext.DsaQuestionDefinitionParameteres.RemoveRange(definition.Parameters);

                foreach (var param in updateDsaQuestionDefinition.Parameters)
                {
                    _appDbContext.DsaQuestionDefinitionParameteres.Add(new DsaQuestionDefinitionParameteres
                    {
                        Id = param.Id ?? Guid.NewGuid(),
                        Name = param.Name,
                        Type = param.Type,
                        DsaQuestionDefinitionId = id
                    });
                }
            }

            _appDbContext.DsaQuestionDefinitions.Update(definition);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("DSA Question Definition has been updated");
        }

        public async Task<ApiResult<string>> DeleteDsaQuestionDefinition(Guid id)
        {
            var definition = await _appDbContext.DsaQuestionDefinitions
                .Include(d => d.Parameters)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (definition == null)
                return ApiResult<string>.Failure(new List<string> { "DSA Question Definition not found" });

            if (definition.Parameters != null && definition.Parameters.Any())
            {
                _appDbContext.DsaQuestionDefinitionParameteres.RemoveRange(definition.Parameters);
            }

            _appDbContext.DsaQuestionDefinitions.Remove(definition);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("DSA Question Definition has been deleted");
        }
    }
}

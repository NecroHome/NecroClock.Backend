using NecroClock.Application.Models;
using NecroClock.Application.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace NecroClock.Application.Interfaces.Repositories
{
    public interface IDemandasRepositorie
    {
        Task<bool> AddDemanda(DemandaModel model);
        Task<DemandaModel> GetDemandaByIdAndUserID(long demandaId, long userID);
        Task<bool> UpdateDemanda(DemandaModel model);
        Task<bool> UpdateDemanda(List<DemandaModel> demandas);
        Task<bool> DeleteDemanda(DemandaModel model);
        Task<List<DemandaModel>> GetDemandasByIntervalAndUserId(DateOnly inicio, DateOnly fim, long userID);
        Task<List<DemandaModel>> FilterDemandasBySearchTermAndUserID(string busca, long userID);
        Task<DemandaModel> FindDemandaPorNumeroEUserIDMesmaSemana(DateOnly inicio, DateOnly fim, string numeroDemanda, long userID);
        Task<List<DemandaModel>> GetDemandasByNumeroDemanda(string numeroDemanda, long userID);
    }
}

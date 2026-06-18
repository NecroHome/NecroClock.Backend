using Microsoft.EntityFrameworkCore;
using NecroClock.Application.Interfaces.Repositories;
using NecroClock.Application.Models;
using NecroClock.Application.Models.DTOs;
using NecroClock.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace NecroClock.Infrastructure.Repositories
{
    public class DemandasRepositorie : IDemandasRepositorie
    {
        private readonly AppDbContext _context;

        public DemandasRepositorie(
            AppDbContext context
            )
        {
            _context = context;
        }

        public async Task<bool> AddDemanda(DemandaModel model)
        {
            _context.Demandas.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DemandaModel> GetDemandaByIdAndUserID(long demandaID, long userID)
        {
            return await _context.Demandas.FirstOrDefaultAsync(f => f.Id == demandaID && f.UserId == userID);
        }

        public async Task<bool> UpdateDemanda(DemandaModel model)
        {
            _context.Demandas.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDemanda(List<DemandaModel> demandas)
        {
            _context.Demandas.UpdateRange(demandas);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDemanda(DemandaModel model)
        {
            _context.Demandas.Remove(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DemandaModel>> GetDemandasByIntervalAndUserId(DateOnly inicio, DateOnly fim, long userID)
        {
            return await _context.Demandas
                .Where(w =>
                    w.Data >= inicio &&
                    w.Data <= fim &&
                    w.UserId == userID)
                .ToListAsync();
        }

        public async Task<List<DemandaModel>> FilterDemandasBySearchTermAndUserID(string busca, long userID)
        {
            return await _context.Demandas
                .Where(w =>
                    (w.NumeroDemanda.Contains(busca) || w.Descricao.Contains(busca)) &&
                    w.UserId == userID)
                .ToListAsync();
        }

        public async Task<DemandaModel> FindDemandaPorNumeroEUserIDMesmaSemana(DateOnly inicio, DateOnly fim, string numeroDemanda, long userID)
        {
            return await _context.Demandas
                .Where(w =>
                    w.Data >= inicio &&
                    w.Data <= fim &&
                    w.NumeroDemanda == numeroDemanda &&
                    w.UserId == userID)
                .FirstOrDefaultAsync();
        }

        public async Task<List<DemandaModel>> GetDemandasByNumeroDemanda(string numeroDemanda, long userID)
        {
            return await _context.Demandas
                .Where(w =>
                    w.NumeroDemanda == numeroDemanda &&
                    w.UserId == userID)
                .ToListAsync();
        }
    }
}

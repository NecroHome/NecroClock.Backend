using NecroClock.Application.Interfaces;
using NecroClock.Application.Interfaces.Repositories;
using NecroClock.Application.Models;
using NecroClock.Application.Models.DTOs;
using NecroClock.Application.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace NecroClock.Application.Services
{
    public class DemandasService : IDemandasService
    {
        private readonly IDemandasRepositorie _demandaRepositorie;
        public DemandasService(
            IDemandasRepositorie demandasRepositorie
            )
        {
            _demandaRepositorie = demandasRepositorie;
        }

        public async Task<bool> AddDemanda(DemandaDTO dto, long userID)
        {
            /**
             * Check if theres already a Demanda with that number for that user in the same wekk
             * If it does, them add the hours to the existing entry
             * otherwise, create a new entry
             */
            DateOnly inicio = DataUtil.ObterInicioSemana(dto.Data);
            DateOnly fim = DataUtil.ObterFimSemana(dto.Data);

            DemandaModel model = await _demandaRepositorie.FindDemandaPorNumeroEUserIDMesmaSemana(inicio, fim, dto.NumeroDemanda, userID);
            if (model != null)
            {
                model.Horas = model.Horas + dto.Horas;
                if (dto.SQL != model.SQL && !String.IsNullOrWhiteSpace(dto.SQL))
                {
                    model.SQL = model.SQL + '\n' + dto.SQL;
                }

                if (dto.Anotacoes != model.Anotacoes && !String.IsNullOrWhiteSpace(dto.Anotacoes))
                {
                    model.Anotacoes = model.Anotacoes + '\n' + dto.Anotacoes;
                }

                await _demandaRepositorie.UpdateDemanda(model);
                return true;
            }

            model = new DemandaModel();
            model.NumeroDemanda = dto.NumeroDemanda;
            model.Descricao = dto.Descricao;
            model.Data = dto.Data;
            model.Horas = dto.Horas;
            model.UserId = userID;
            model.SQL = dto.SQL;
            model.Anotacoes = dto.Anotacoes;

            await _demandaRepositorie.AddDemanda(model);

            return true;
        }

        public async Task<bool> UpdateDemanda(DemandaDTO dto, long userID)
        {
            DemandaModel model = await _demandaRepositorie.GetDemandaByIdAndUserID(dto.Id, userID);
            if (model == null)
            {
                throw new Exception("Demanda não encontrada.");
            }

            if (dto.NumeroDemanda != model.NumeroDemanda)
            {
                List<DemandaModel> demandas = await _demandaRepositorie.GetDemandasByNumeroDemanda(model.NumeroDemanda, userID);
                foreach (DemandaModel demanda in demandas)
                {
                    demanda.NumeroDemanda = dto.NumeroDemanda;
                    demanda.Descricao = dto.Descricao;
                    demanda.Data = dto.Data;
                    demanda.Horas = dto.Horas;
                    demanda.SQL = dto.SQL;
                    demanda.Anotacoes = dto.Anotacoes;
                }

                await _demandaRepositorie.UpdateDemanda(demandas);
                return true;
            }

            model.NumeroDemanda = dto.NumeroDemanda;
            model.Descricao = dto.Descricao;
            model.Data = dto.Data;
            model.Horas = dto.Horas;
            model.SQL = dto.SQL;
            model.Anotacoes = dto.Anotacoes;

            await _demandaRepositorie.UpdateDemanda(model);

            return true;
        }

        public async Task<bool> DeleteDemanda(long demandaID, long userID)
        {
            DemandaModel model = await _demandaRepositorie.GetDemandaByIdAndUserID(demandaID, userID);
            if (model == null)
            {
                throw new Exception("Demanda não encontrada.");
            }
            await _demandaRepositorie.DeleteDemanda(model);
            return true;
        }

        public async Task<List<DemandaDTO>> GetDemandas(DateOnly inicio, DateOnly fim, long userID)
        {
            List<DemandaModel> demandas = await _demandaRepositorie.GetDemandasByIntervalAndUserId(inicio, fim, userID);
            return DemandaDTO.GenerateList(demandas);
        }

        public async Task<List<DemandaDTO>> FiltrarDemandas(string busca, long userID)
        {
            List<DemandaModel> demandas = await _demandaRepositorie.FilterDemandasBySearchTermAndUserID(busca, userID);
            return DemandaDTO.GenerateList(demandas);
        }
    }
}

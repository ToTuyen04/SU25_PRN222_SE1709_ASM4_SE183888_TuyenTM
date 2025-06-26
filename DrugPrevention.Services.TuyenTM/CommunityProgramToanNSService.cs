﻿using DrugPrevention.Repositories.TuyenTM;
using DrugPrevention.Repositories.TuyenTM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugPrevention.Services.TuyenTM
{
    public class CommunityProgramToanNSService : ICommunityProgramToanNSService
    {
        //private CommunityProgramToanNSRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CommunityProgramToanNSService() => _unitOfWork = new UnitOfWork();
        public async Task<List<CommunityProgramsToanN>> GetAllAsync()
        {
            return await _unitOfWork.CommunityProgramToanNSRepository.GetAllAsync();
        }

        public async Task<CommunityProgramsToanN> GetByIdAsync(int id)
        {
            return await _unitOfWork.CommunityProgramToanNSRepository.GetByIdAsync(id);
        }
    }
}

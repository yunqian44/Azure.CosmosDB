﻿using AutoMapper;
using Azure.CosmosDB.Models;
using Azure.CosmosDB.Repository.Interface;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Azure.CosmosDB.Service
{
    /// <summary>
    /// UserService 服务接口实现类,继承 服务接口
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        // 用来进行DTO
        private readonly IMapper _mapper;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public UserService(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _UserRepository = userRepository;
            _mapper = mapper;
        }

        public IEnumerable<UserViewModel> GetAll()
        {
            //第一种写法 Map
            return _mapper.Map<IEnumerable<UserViewModel>>(_UserRepository.GetAll());
        }

        public UserViewModel GetById(string partitionKey)
        {
            return _mapper.Map<UserViewModel>(_UserRepository.GetById(partitionKey).Result);
        }

        public async Task<int> Register(UserViewModel userViewModel)
        {
            var partitionKey = _UserRepository.GetAll().Max(x => int.Parse(x.PartitionKey));
            userViewModel.PartitionKey = (++partitionKey).ToString();
            await _UserRepository.Add(_mapper.Map<UserModel>(userViewModel));
            return await _UserRepository.SaveChangesAsync();
        }

        public void Remove(string partitionKey)
        {

            _UserRepository.Remove(_mapper.Map<UserModel>(_UserRepository.GetById(partitionKey).Result));
            _UserRepository.SaveChangesAsync();
        }

        public int Update(UserViewModel userViewModel)
        {
            _UserRepository.Update(_mapper.Map<UserModel>(userViewModel));
            return _UserRepository.SaveChanges();
        }
    }
}

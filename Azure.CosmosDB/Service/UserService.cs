﻿using AutoMapper;
using Azure.CosmosDB.Models;
using Azure.CosmosDB.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.CosmosDB.Service
{
    /// <summary>
    /// UserService 服务接口实现类,继承 服务接口
    /// 通过 DTO 实现视图模型和领域模型的关系处理
    /// 作为调度者，协调领域层和基础层，
    /// 这里只是做一个面向用户用例的服务接口,不包含业务规则或者知识
    /// </summary>
    public class UserService : IUserService
    {
        // 注意这里是要IoC依赖注入的，还没有实现
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

        public UserViewModel GetById(int id)
        {
            return _mapper.Map<UserViewModel>(_UserRepository.GetById(id));
        }

        public void Register(UserViewModel userViewModel)
        {
            _UserRepository.Add(_mapper.Map<UserModel>(userViewModel));
            _UserRepository.SaveChanges();
        }

        public void Remove(int id)
        {

            _UserRepository.Remove(_mapper.Map<UserModel>(_UserRepository.GetById(id)));
            _UserRepository.SaveChanges();
        }

        public void Update(UserViewModel userViewModel)
        {
            _UserRepository.Update(_mapper.Map<UserModel>(userViewModel));
            _UserRepository.SaveChanges();
        }
    }
}
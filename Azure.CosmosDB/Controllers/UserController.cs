using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.CosmosDB.Models;
using Azure.CosmosDB.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Azure.CosmosDB.Controllers
{
    public class UserController : Controller
    {
        //
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: User
        public ActionResult Index()
        {
            return View(_userService.GetAll());
        }

        // GET: User/Details/5
        public ActionResult Details(string partitionKey)
        {
            try
            {
                // TODO: Add insert logic here

                // 执行查询方法
                var userViewModel= _userService.GetById(partitionKey);


                return View(userViewModel);
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel userViewModel)
        {
            try
            {
                // TODO: Add insert logic here

                // 视图模型验证
                if (!ModelState.IsValid)
                    return View(userViewModel);


                // 执行添加方法
                _userService.Register(userViewModel);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(string partitionKey)
        {
            return View(_userService.GetById(partitionKey));
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel userViewModel)
        {
            //try
            //{
                // TODO: Add update logic here
                // 视图模型验证
                if (!ModelState.IsValid)
                    return View(userViewModel);

                // 执行添加方法
                _userService.Update(userViewModel);

                return RedirectToAction(nameof(Index));
            //}
            //catch(Exception ex)
            //{
                
            //    //return View(ex);
            //}
        }

        // POST: User/Delete/5
        [HttpGet]
        public ActionResult Delete(string partitionKey)
        {
            try
            {
                // TODO: Add delete logic here
                // 执行添加方法
                _userService.Remove(partitionKey);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

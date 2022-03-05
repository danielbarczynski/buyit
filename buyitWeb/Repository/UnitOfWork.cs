﻿using buyitWeb.Data;
using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            Category = new CategoryRepository(_applicationDbContext);
        }
        public ICategory Category { get; private set; }

        public void Save()
        {
            _applicationDbContext.SaveChanges();
        }
    }
}

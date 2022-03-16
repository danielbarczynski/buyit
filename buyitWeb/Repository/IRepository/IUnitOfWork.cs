﻿using buyitWeb.Repository.IRepository;

namespace buyitWeb.Repository
{
    public interface IUnitOfWork 
    {
        ICategory Category { get; }
        ICoverType CoverType { get; }
        ICart Cart { get; }
        IBook Book { get; }
        void Save();
    }
}

﻿namespace HotelComercy_WebAPI.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IVillaRepository Vila { get; }
        void Save();
    }
}

using EsplanadeEventBooking.Shared.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsplanadeEventBooking.Server.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        Task Save(HttpContext httpContext);
        IGenericRepository<Bookevent> Bookevents { get; }
        IGenericRepository<Creator> Creators { get; }
        IGenericRepository<Euser> Eusers { get; }
        IGenericRepository<Ticket> Tickets { get; }
        IGenericRepository<Location> Locations { get; }
    }
}

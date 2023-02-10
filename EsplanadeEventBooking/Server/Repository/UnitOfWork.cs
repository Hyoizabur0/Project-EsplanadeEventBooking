using EsplanadeEventBooking.Server.Data;
using EsplanadeEventBooking.Server.IRepository;
using EsplanadeEventBooking.Server.Models;
using EsplanadeEventBooking.Shared.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EsplanadeEventBooking.Server.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<Bookevent> _bookevents;
        private IGenericRepository<Euser> _eusers;
        private IGenericRepository<Creator> _creators;
        private IGenericRepository<Ticket> _tickets;
        private IGenericRepository<Location> _locations;
        private UserManager<ApplicationUser> _userManager;

        public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
            
        public IGenericRepository<Bookevent> Bookevents
            => _bookevents ??= new GenericRepository<Bookevent>(_context);
        public IGenericRepository<Euser> Eusers
            => _eusers ??= new GenericRepository<Euser>(_context);
        public IGenericRepository<Creator> Creators
            => _creators ??= new GenericRepository<Creator>(_context);
        public IGenericRepository<Ticket> Tickets
            => _tickets ??= new GenericRepository<Ticket>(_context);
        public IGenericRepository<Location> Locations
            => _locations ??= new GenericRepository<Location>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save(HttpContext httpContext)
        {
            //To be implemented
            string user = "System";

            var entries = _context.ChangeTracker.Entries()
                .Where(q => q.State == EntityState.Modified ||
                    q.State == EntityState.Added);

            foreach (var entry in entries)
            {
                ((BaseDomainModel)entry.Entity).DateUpdated = DateTime.Now;
                ((BaseDomainModel)entry.Entity).UpdatedBy = user;
                if (entry.State == EntityState.Added)
                {
                    ((BaseDomainModel)entry.Entity).DateCreated = DateTime.Now;
                    ((BaseDomainModel)entry.Entity).CreatedBy = user;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
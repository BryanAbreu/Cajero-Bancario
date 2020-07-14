using Database.Models;
using Microsoft.EntityFrameworkCore;
using RepositoryBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Repository.Repository
{
    public class ProductosRepository : RepositoryBase<Productos, IbankingContext>
    {
        private readonly IbankingContext _context;
        public ProductosRepository(IbankingContext context) : base(context)
        {
            _context = context;
        }

        
        

    }
}

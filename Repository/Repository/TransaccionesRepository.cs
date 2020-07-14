using Database.Models;
using RepositoryBase.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repository
{
    public class TransaccionesRepository : RepositoryBase<Transacciones, IbankingContext>
    {
        public TransaccionesRepository(IbankingContext context) : base(context)
        {

        }
    }
}

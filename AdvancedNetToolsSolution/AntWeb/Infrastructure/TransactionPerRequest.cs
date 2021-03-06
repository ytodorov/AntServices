﻿using System.Data.Entity;
using System.Web;
using FailTracker.Web.Infrastructure.Tasks;
using AntDal;

namespace FailTracker.Web.Infrastructure
{
    public class TransactionPerRequest :
        IRunOnEachRequest, IRunOnError, IRunAfterEachRequest
    {
#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
        private readonly AntDbContext _dbContext;
        private readonly HttpContextBase _httpContext;
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency

        public TransactionPerRequest(AntDbContext dbContext,
            HttpContextBase httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        void IRunOnEachRequest.Execute()
        {

            _httpContext.Items["_Transaction"] =
                _dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

        }

        void IRunOnError.Execute()
        {
            _httpContext.Items["_Error"] = true;
        }

        void IRunAfterEachRequest.Execute()
        {
            var transaction = (DbContextTransaction) _httpContext.Items["_Transaction"];

            if (_httpContext.Items["_Error"] != null)
            {
                transaction.Rollback();
            }
            else
            {
                transaction.Commit();
            }
        }
    }
}
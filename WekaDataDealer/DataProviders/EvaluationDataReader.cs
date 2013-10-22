using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FeatureDealer.Models;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.DataProviders
{
    internal class EvaluationDataReader : IDisposable
    {
        private readonly EvaluationContext context = new EvaluationContext();

        #region IDataReader Members

        public IDataItem ReadRequest(int requestId)
        {
            IQueryable<Request> requests = from r in context.Requests where r.Id.Equals(requestId) select r;
            return requests.FirstOrDefault();
        }

        public IEnumerable<IDataItem> ReadRequests(IEnumerable<int> requestIds)
        {
            return context.Requests.Where(r => requestIds.Contains(r.Id)).AsEnumerable();
        }

        public IEnumerable<IDataItem> Read(IEnumerable<KeyValuePair<int, int>> evals)
        {
            var evaluations = new List<Evaluation>();
            foreach (var eval in evals)
            {
                var qids = context.Evaluations.Where(e => e.RequestId == eval.Key);
                var evaluation = qids.FirstOrDefault(q => q.PostId == eval.Value);
                if(evaluation!=null)
                    evaluations.Add(evaluation);
            }
            return evaluations;
        }

        public IEnumerable<IDataItem> Read()
        {
            DbSet<Evaluation> evaluations = context.Evaluations;
            return evaluations;
        }

        #endregion

        public void Dispose()
        {
            context.Dispose();
        }

        public IQueryable<Request> ReadRequests()
        {
            return context.Requests;
        }

        public IQueryable<Evaluation> ReadEvaluation(int requestId)
        {
            return from e in context.Evaluations where e.RequestId == requestId select e;
        }
    }
}
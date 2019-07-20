using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Enums;
using Common.Models.ServiceReponses;

namespace Services.Abstractions
{
    public interface ISearchable<TDocument> where  TDocument : class
    {
        Task ReIndex(List<TDocument> list);

        Task AddToSearch(TDocument document);

        Task UpdateSearchDoc(TDocument document);

        Task<ServiceBaseResult<SearchOperationStatus, IReadOnlyCollection<TDocument>>> Search(string query, int page,
            int pageSize, params Expression<Func<TDocument, object>>[] fields);
    }
}
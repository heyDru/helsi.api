using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Enums;
using Common.Extensions;
using Common.Models.ServiceReponses;
using Common.Utils;
using Nest;
using Services.Abstractions;

namespace Services
{
    public class Elasticsearch<TDocument> : ISearchable<TDocument> where TDocument : class
    {
        private readonly IElasticClient _elasticClient;

        public Elasticsearch(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task ReIndex(List<TDocument> documents)
        {
            await _elasticClient.DeleteByQueryAsync<TDocument>(q => q.MatchAll());

            foreach (var doc in documents)
            {
                await _elasticClient.IndexDocumentAsync(doc);
            }

        }

        public async Task AddToSearch(TDocument document)
        {
            await _elasticClient.IndexDocumentAsync(document);
        }

        public async Task UpdateSearchDoc(TDocument document)
        {
            await _elasticClient.UpdateAsync<TDocument>(document, u => u.Doc(document));
        }

        public async Task<ServiceBaseResult<SearchOperationStatus, IReadOnlyCollection<TDocument>>> Search(string query, int page, int pageSize,
            params Expression<Func<TDocument, object>>[] fields)
        {
            if (!SearchValidator.ValidateSearchRequest(query))
                return new ServiceBaseResult<SearchOperationStatus, IReadOnlyCollection<TDocument>>(SearchOperationStatus.IncorrectRequest,
                    SearchOperationStatus.IncorrectRequest.GetDescription());

            var searchResult = await _elasticClient.SearchAsync<TDocument>(s => s
                .From((page - 1) * pageSize)
                .Size(pageSize)
                .Query(qry =>
                    qry.QueryString(q =>
                        q.Fields(f =>
                            f.Fields(fields)).Query("*" + query + "*"))
                ));

            if (!searchResult.Documents.Any())
            {
                return new ServiceBaseResult<SearchOperationStatus, IReadOnlyCollection<TDocument>>(SearchOperationStatus.NotFound,
                    SearchOperationStatus.NotFound.GetDescription());
            }

            return new ServiceBaseResult<SearchOperationStatus, IReadOnlyCollection<TDocument>>(SearchOperationStatus.Success,
                SearchOperationStatus.Success.GetDescription(), searchResult.Documents);
        }
    }
}
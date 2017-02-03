using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Polly;

namespace UrlShortnerNetCore.Models
{
    public interface ITableRepository<T> where T : TableEntity, new()
    {
        Task Insert(T entity);
        Task Update(string partitionKey, string rowKey, Action<T> update);
        Task<T> GetByPartitionKeyAndRowKey(string partitionKey, string rowKey);
    }

    public class TableRepository<T> : ITableRepository<T>
        where T: TableEntity, new()
    {
        private readonly CloudTableClient _client;
        private readonly CloudTable _table;

        public TableRepository(CloudStorageAccount storageAccount)
        {
            _client = storageAccount.CreateCloudTableClient();
            _table = _client.GetTableReference(typeof(T).Name);
            _table.CreateIfNotExistsAsync().Wait();
        }

        public async Task Insert(T entity)
        {
            var op = TableOperation.Insert(entity);
            await _table.ExecuteAsync(op);
        }

        public async Task Update(T entity)
        {
            var op = TableOperation.Merge(entity);
            await _table.ExecuteAsync(op);
        }

        public async Task<T> GetByPartitionKeyAndRowKey(string partitionKey, string rowKey)
        {
            var query = new TableQuery<T>();
            query.Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                TableOperators.And, TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey)));

            var contitnuationToken = new TableContinuationToken();
            var result = await _table.ExecuteQuerySegmentedAsync(query, contitnuationToken);

            //Since its a PK & RK query there would be no changes
            return result.FirstOrDefault();
        }

        public async Task Update(string partitionKey, string rowKey, Action<T> updateAction)
        {
            var policy = Policy.Handle<StorageException>(ex => ex.RequestInformation.HttpStatusCode == 412)
                .RetryAsync(3);

            await policy.ExecuteAsync(async () =>
            {
                var entityToBeUpdated = await GetByPartitionKeyAndRowKey(partitionKey, rowKey);
                if (entityToBeUpdated == null)
                    throw new ArgumentOutOfRangeException($"Entity not found with PK:{partitionKey} & RK:{rowKey}", (Exception)null);
                updateAction(entityToBeUpdated);
                var op = TableOperation.Replace(entityToBeUpdated);

                await _table.ExecuteAsync(op);
            });
        }
    }
}

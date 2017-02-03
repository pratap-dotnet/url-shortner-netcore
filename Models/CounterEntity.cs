using Microsoft.WindowsAzure.Storage.Table;

namespace UrlShortnerNetCore.Models
{
    public class CounterEntity : TableEntity
    {
        public long Count { get; set; }

        public CounterEntity()
        {

        }

        public CounterEntity(long count)
        {
            Count = count;
            PartitionKey = Constants.DefaultPartitionKey;
            RowKey = Constants.DefaultRowKey;
        }
    }
}

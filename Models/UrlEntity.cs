using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace UrlShortnerNetCore.Models
{
    public class UrlEntity : TableEntity
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public DateTime CreatedDateTime { get; set;}

        public UrlEntity()
        {
        }

        public UrlEntity(string id, string url)
        {
            CreatedDateTime = DateTime.UtcNow;
            Url = url;
            Id = id;
            RowKey = id;
            PartitionKey = Constants.DefaultPartitionKey;    
        }
    }
}

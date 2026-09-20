using Catalog.Contracts.Events;
using MassTransit;
using OpenSearch.Client;
using Search.Module.Documents;

namespace Search.Module.Consumers;

public sealed class ProductUpdatedConsumer(IOpenSearchClient openSearchClient) : IConsumer<ProductUpdated>
{
    public async Task Consume(ConsumeContext<ProductUpdated> context)
    {
        var message = context.Message;

        var document = new ProductDocument
        {
            Id = message.Id,
            Name = message.Name,
            Description = message.Description,
            Price = message.Price,
            CategoryId = message.CategoryId,
            CategoryName = message.CategoryName,
            Tags = message.Tags,
            Attributes = message.Attributes,
            PrimaryImageUrl = message.PrimaryImageUrl
        };
        
        var response = await openSearchClient.IndexDocumentAsync(document);
        
        if (!response.IsValid)
        {
            throw new InvalidOperationException($"Cannot update product with Id: {message.Id} in OpenSearch.", response.OriginalException);
        }
    }
}
using Catalog.Contracts.Events;
using MassTransit;
using OpenSearch.Client;
using Search.Module.Documents;

namespace Search.Module.Consumers;

public sealed class ProductCreatedConsumer(IOpenSearchClient openSearchClient) : IConsumer<ProductCreated>
{
    public async Task Consume(ConsumeContext<ProductCreated> context)
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
            throw new InvalidOperationException($"Cannot save product with Id: {message.Id} in OpenSearch.", response.OriginalException);
        }
    }
}
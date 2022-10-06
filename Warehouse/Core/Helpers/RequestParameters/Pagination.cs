namespace Warehouse.Core.RequestParameters
{
    public record Pagination
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 5;

    }
}

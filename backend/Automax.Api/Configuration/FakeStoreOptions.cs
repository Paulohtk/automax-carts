namespace Automax.Api.Configuration;

public sealed class FakeStoreOptions
{
    public const string SectionName = "FakeStore";

    public string CartsUrl { get; set; } = "https://fakestoreapi.com/carts";
}

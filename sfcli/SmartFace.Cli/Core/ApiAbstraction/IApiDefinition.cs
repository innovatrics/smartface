namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface IApiDefinition
    {
        int ZeroMqPort { get; }
        string ApiUrl { get; }
        string ODataUrl { get; }
        string OdataBaseUrl { get; }
        string ZeroMqHost { get; }
    }
}
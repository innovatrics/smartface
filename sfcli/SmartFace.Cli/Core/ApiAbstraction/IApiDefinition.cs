namespace SmartFace.Cli.ApiAbstraction
{
    public interface IApiDefinition
    {
        int ZeroMqPort { get; }
        string Host { get; }
        string ApiUrl { get; }
        string ODataUrl { get; }
    }
}
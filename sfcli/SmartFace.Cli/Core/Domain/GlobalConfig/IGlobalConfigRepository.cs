namespace SmartFace.Cli.Core.Domain.GlobalConfig
{
    public interface IGlobalConfigRepository
    {
        GlobalConfig Get();

        void Set(GlobalConfig config);
    }
}
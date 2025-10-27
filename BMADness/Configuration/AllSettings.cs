using BMADness.Providers;

namespace BMADness.Configuration;

public class AllSettings
{
    public PROVIDERS DefaultProvider { get; set; } = PROVIDERS.LMSTUDIO;
    public ProvdersSettings Providers { get; set; } =  new ProvdersSettings();
}
namespace Postwomen.Extensions;

public class Translate : IMarkupExtension
{
    public string Key { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new Binding
        {
            Mode = BindingMode.OneWay,
            Path = $"[{Key}]",
            Source = Translator.Instance,
        };
        return binding;
    }
}

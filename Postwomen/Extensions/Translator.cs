using Postwomen.Resources.Strings;
using System.ComponentModel;
using System.Globalization;

namespace Postwomen.Extensions;

public class Translator : INotifyPropertyChanged , IMarkupExtension
{
    public string this[string key]
    {
        get => AppResources.ResourceManager.GetString(key, CultureInfo);
    }

    public CultureInfo CultureInfo { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }

	public string Key { get; set; }

	public object ProvideValue(IServiceProvider serviceProvider)
	{
        return new Binding
        {
            Mode = BindingMode.OneWay,
            Path = $"[{Key}]",
            Source = this
		};
	}
}

using Postwomen.Models;
using Postwomen.Others;
using Postwomen.Resources.Strings;
using System.Collections.ObjectModel;

namespace Postwomen.Views;

public partial class LogsPage : ContentPage
{
    public string DataCount { get; set; } = "0";

    public ObservableCollection<LogModel> Logs { get; set; } //NULL İKEN DATA ÇAĞIRIR,

    public LogsPage(PostwomenDatabase postwomenDatabase)
    {
        InitializeComponent();
        Logs = new ObservableCollection<LogModel>();
        this.BindingContext = this;
        Load(postwomenDatabase);
    }

    private async void Load(PostwomenDatabase postwomenDatabase)
    {
        var logs = await postwomenDatabase.GetItemsAsync<LogModel>();
        foreach (var item in logs.OrderByDescending(u => u.CreationDate))
            Logs.Add(item);
        DataCount = logs.Count.ToString();
        OnPropertyChanged(nameof(Logs));
        OnPropertyChanged(nameof(DataCount));
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var log = (LogModel)e.CurrentSelection.FirstOrDefault();

        await App.Current.MainPage.DisplayAlert(log.CreationDate.ToString("dd.MM.yyyy - HH:mm:ss"), log.Description, AppResources.ok);
    }
}


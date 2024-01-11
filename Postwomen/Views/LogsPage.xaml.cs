using DesenMobileDatabase.Models;
using Postwomen.Extensions;
using Postwomen.Services;
using System.Collections.ObjectModel;

namespace Postwomen.Views;

public partial class LogsPage : ContentPage
{
	public Translator Translator { get; set; }
	private int LogCount_ { get; set; }
    public int LogCount { get { return LogCount_; } set { LogCount_ = value; OnPropertyChanged(nameof(LogCount)); } }

    public ObservableCollection<LogsModel> Logs { get; set; } //NULL İKEN DATA ÇAĞIRIR,

    private IDbService dbService { get; set; }

    public LogsPage(IDbService dbService, Translator translator)
    {
        InitializeComponent();

		this.Translator = translator;
		this.dbService = dbService;

        Logs = new ObservableCollection<LogsModel>();

        this.BindingContext = this;
    }

    private async void RefreshLogs()
    {
        await Task.Delay(1000);
        await Task.Run(async () =>
        {
            var logs = await dbService.GetLogs();
            LogCount = logs.Count;
            logs = logs.OrderByDescending(x => x.Creation).ToList();
            logs.ForEach(Logs.Add);
        });
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var log = (LogsModel)e.CurrentSelection.FirstOrDefault();
        await App.Current.MainPage.DisplayAlert(log.Creation.ToString("dd.MM.yyyy - HH:mm:ss"), log.Desc, Translator["ok"]);
    }

    private void CollectionView_Loaded(object sender, EventArgs e)
    {
        RefreshLogs();
    }
}


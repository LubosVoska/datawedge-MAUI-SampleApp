using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace datawedge_MAUI_SampleApp;

public partial class Page1 : ContentPage, INotifyPropertyChanged
{

    public Page1()
    {
        InitializeComponent();

        BindingContext = this;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Register<string>(this, (r, m) =>
        {
            if (m.Length > 2)
                BarcodeRecieved(m);
        });
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (sender is VisualElement element)
        {
            // Animate the row when tapped
            await element.ScaleTo(0.95, 100, Easing.CubicInOut); // Shrink slightly
            await element.ScaleTo(1.0, 100, Easing.CubicInOut); // Return to original size
        }

        if (e.CurrentSelection.FirstOrDefault() is DataItem selectedItem)
        {
            // Perform action with the selected item
            await DisplayAlert("Item Selected", $"{selectedItem.Umiesnenie}", "OK");
        }

    // Optionally clear selection to allow re-selection of the same item
    ((CollectionView)sender).SelectedItem = null;
    }

    private void Init()
    {
        if (DataItems != null)
        {
            DataItems.Clear();
            DataItems = null;
        }

        RegCis = null;
        Name = null;
    }

    private async void BarcodeRecieved(string code)
    {
        try
        {
            Init();

            // Show spinner
            Spinner.IsVisible = true;
            Spinner.IsRunning = true;

            await Task.Delay(2000); // Simulates a delay for processing the barcode

            // Sample data
            var rawData = new List<DataItem>
            {
                new DataItem { Umiesnenie = "Umiesnenie 1", UmiesnenieKod = "Kardex", Mnozstvo = 2, Prednastavene = false },
                new DataItem { Umiesnenie = "Umiesnenie 1", UmiesnenieKod = "Kardex", Mnozstvo = 2, Prednastavene = false },
                new DataItem { Umiesnenie = "Umiesnenie 2", UmiesnenieKod = "AA", Mnozstvo = 1, Prednastavene = true },
                new DataItem { Umiesnenie = "Umiesnenie 3", UmiesnenieKod = "AB", Mnozstvo = 3, Prednastavene = false },
                new DataItem { Umiesnenie = "Umiesnenie 3", UmiesnenieKod = "AB", Mnozstvo = 3, Prednastavene = false },
                new DataItem { Umiesnenie = "Umiesnenie 4", UmiesnenieKod = "AC", Mnozstvo = 1, Prednastavene = false },
                new DataItem { Umiesnenie = "Umiesnenie 4", UmiesnenieKod = "AC", Mnozstvo = 1, Prednastavene = false }
            };

            // Sort by Prednastavene (descending) and then by Mnozstvo (ascending)
            var sortedData = rawData
                .OrderByDescending(item => item.Prednastavene)
                .ThenByDescending(item => item.Mnozstvo);

            // Convert sorted data to ObservableCollection
            DataItems = new ObservableCollection<DataItem>(sortedData);

            RegCis = code;
            Name = "Profil 40x40";
            // Simulate data processing (replace with actual logic)

        }
        finally
        {
            // Hide spinner
            Spinner.IsRunning = false;
            Spinner.IsVisible = false;
        }

       
    }

    private void ContentPage_Unloaded(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<string>(this);
        Init();
    }

    private ObservableCollection<DataItem> _dataItems;
    public ObservableCollection<DataItem> DataItems
    {
        get => _dataItems;
        set
        {
            _dataItems = value;
            OnPropertyChanged();
        }
    }

    private string _regCis;
    public string RegCis
    {
        get => _regCis;
        set
        {
            _regCis = value;
            OnPropertyChanged();
        }
    }

    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }
}

// Model for the data
public class DataItem
{
    public string Umiesnenie { get; set; }
    public string UmiesnenieKod { get; set; }
    public decimal Mnozstvo { get; set; }
    public bool Prednastavene { get; set; }
}
using CommunityToolkit.Mvvm.Messaging;

namespace datawedge_MAUI_SampleApp;

public partial class Settings : ContentPage
{
	public Settings()
	{
        InitializeComponent();
    }

    private void OnDWOFFClicked(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Send("11");
        WeakReferenceMessenger.Default.Send("SWITCHING OFF DW");
    }

    private void OnDWONClicked(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Send("22");
        WeakReferenceMessenger.Default.Send("SWITCHING ON DW");
    }

    private void OnDWGetActiveProfile(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Send("33");
        WeakReferenceMessenger.Default.Send("GETTING ACTIVE PROFILE");
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        // Refresh the page to its initial state
        lbDisplayBarcodeData.Text = string.Empty; // Clear the display label

        WeakReferenceMessenger.Default.Register<string>(this, (r, m) =>
        {
            if (m.Length > 2)
                MainThread.BeginInvokeOnMainThread(() => { lbDisplayBarcodeData.Text += "\n" + m; });
        });
    }

    private void ContentPage_Unloaded(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<string>(this);
    }
}
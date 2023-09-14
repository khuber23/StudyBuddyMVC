namespace NtcMaui.Views.SignAndCreate;

public partial class CreateAccount : ContentPage
{
    //note for tomorrow. re-do all the stuff. slowly starting with sign in and working way down after making all the views. THEN
	public CreateAccount()
	{
		InitializeComponent();
	}
    public async void CompleteCreation(object sender, EventArgs e)
    {
        //eventually add some checks/add to database before going back to sign in.
        await Shell.Current.GoToAsync(nameof(SignIn));
    }
}
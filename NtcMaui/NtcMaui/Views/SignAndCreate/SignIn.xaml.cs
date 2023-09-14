namespace NtcMaui.Views.SignAndCreate;

public partial class SignIn : ContentPage
{
	public SignIn()
	{
		InitializeComponent();
	}
    public async void CompleteSignIn(object sender, EventArgs e)
    {
        //add method before this with other tasks that check stuff like if user exists in database and the password they typed is re-typed again correctly.
        await Shell.Current.GoToAsync(nameof(Success));
    }

    public async void GoToAccountCreation(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CreateAccount));
    }
}
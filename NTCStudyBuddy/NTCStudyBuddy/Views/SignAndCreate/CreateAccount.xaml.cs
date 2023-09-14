namespace NTCStudyBuddy.Views.SignAndCreate;

public partial class CreateAccount : ContentPage
{
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
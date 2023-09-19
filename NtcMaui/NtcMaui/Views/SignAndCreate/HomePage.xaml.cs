using System.ComponentModel;
using ApiStudyBuddy.Models;

namespace NtcMaui.Views.SignAndCreate;

public partial class HomePage : ContentPage, IQueryAttributable, INotifyPropertyChanged
{
	public HomePage()
	{
		InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        LoggedInUser = query["Current User"] as User;
        OnPropertyChanged("Current User");
        UserNameText.Text = LoggedInUser.Username;
    }
    public User LoggedInUser { get; set; }
}
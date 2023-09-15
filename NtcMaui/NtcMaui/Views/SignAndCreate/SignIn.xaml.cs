using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using NtcStudyBuddy.DataAccess;
using NtcStudyBuddy.DataAccess.Models;
using NtcStudyBuddy.DataAccess.Services;

namespace NtcMaui.Views.SignAndCreate;

public partial class SignIn : ContentPage
{
    private HttpClient _client = new HttpClient();
    private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
public SignIn()
	{
		InitializeComponent();
	}
    public void CompleteSignIn(object sender, EventArgs e)
    {
        //add method before this with other tasks that check stuff like if user exists in database and the password they typed is re-typed again correctly.
        this.ValidateUser();
    }

    public async void GoToAccountCreation(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CreateAccount));
    }

    //local Url test to retrieve users from localhost and check something.
    public async Task<List<User>> GetAllUsers()
    {
       List<User> users= new List<User>();
        

        Uri uri = new Uri(string.Format($"{Constants.LocalUrl}/api/users", string.Empty));
        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                users = JsonSerializer.Deserialize<List<User>>(content, _serializerOptions);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }

        return users;
    }

    public async void ValidateUser()
    {
        List<User> users = await GetAllUsers();
        foreach (User user in users)
        {
             if (user.Username != UserNameEntry.Text)
            {
                UserNameEntry.Text = string.Empty;
                UserNameEntry.Placeholder = "Invalid Username";
            }
            if (user.Password != PasswordEntry.Text)
            {
                PasswordEntry.Text = string.Empty;
                PasswordEntry.Placeholder = "Invalid Password";
            }
            else if (user.Username == UserNameEntry.Text && user.Password == PasswordEntry.Text)
            {
                await Shell.Current.GoToAsync(nameof(Success));
            }

        }
    }
}
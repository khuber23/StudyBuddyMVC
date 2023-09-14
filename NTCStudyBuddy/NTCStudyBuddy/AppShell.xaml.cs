namespace NTCStudyBuddy
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //use this 1st in routing and register route for the appshell.xaml and in this app.
            Routing.RegisterRoute(nameof(Views.SignAndCreate.SignIn), typeof(Views.SignAndCreate.SignIn));
            Routing.RegisterRoute(nameof(Views.SignAndCreate.CreateAccount), typeof(Views.SignAndCreate.CreateAccount));
            Routing.RegisterRoute(nameof(Views.SignAndCreate.Success), typeof(Views.SignAndCreate.Success));
        }
    }
}
namespace NTCStudyBuddy
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //use this routing and register route for appshell.xaml and in this app.
            Routing.RegisterRoute(nameof(Views.SignIn), typeof(Views.SignIn));
            Routing.RegisterRoute(nameof(Views.CreateAccount), typeof(Views.CreateAccount));
        }
    }
}
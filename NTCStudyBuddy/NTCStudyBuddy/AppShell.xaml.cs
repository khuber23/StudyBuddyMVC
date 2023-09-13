namespace NTCStudyBuddy
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.SignIn), typeof(Views.SignIn));
        }
    }
}
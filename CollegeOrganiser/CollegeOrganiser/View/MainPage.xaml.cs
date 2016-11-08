using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using CollegeOrganiser.View;
using Windows.UI.Xaml;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CollegeOrganiser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        // Closes this MainPage when the below function is called to open the MenuPage
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        // Navigates to the Menu Page when Tapped
        private void Page_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage));
        }

        // Start the animation when the object loads
        // Adapted from[ref] https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.media.animation.doubleanimation.aspx
        private void Start_Animation(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin();
        }
    }
}

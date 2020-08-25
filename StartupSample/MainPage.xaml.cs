using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StartupSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string TaskId = "StartupSampleId";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var startupTask = await StartupTask.GetAsync(TaskId);
            switch (startupTask.State)
            {
                case StartupTaskState.Disabled:
                    _ = await startupTask.RequestEnableAsync();
                    await StartupStateUpdateAsync();
                    break;
                case StartupTaskState.DisabledByUser:
                    MessageDialog dialog = new MessageDialog(
                        "I know you don't want this app to run " +
                        "as soon as you sign in, but if you change your mind, " +
                        "you can enable this in the Startup tab in Task Manager.",
                        "StartupUWP");
                    await dialog.ShowAsync();
                    break;
                case StartupTaskState.Enabled:
                    startupTask.Disable();
                    await StartupStateUpdateAsync();
                    break;
                case StartupTaskState.DisabledByPolicy:
                    Debug.WriteLine("Startup disabled by group policy, or not supported on this device");
                    break;
                case StartupTaskState.EnabledByPolicy:
                    break;
            }
        }

        private async Task StartupStateUpdateAsync()
        {
            var startupTask = await StartupTask.GetAsync(TaskId);
            StartupState.Text = startupTask.State.ToString();
            if (startupTask.State == StartupTaskState.Disabled)
            {
                EnableButton.Visibility = Visibility.Visible;
                DiableButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                EnableButton.Visibility = Visibility.Collapsed;
                DiableButton.Visibility = Visibility.Visible;
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            switch (e.Parameter)
            {
                case "":
                    ActivationKind.Text = "User Launched";
                    break;
                case string startupTaskId:
                    ActivationKind.Text = $"Startup Launched. TaskId is {startupTaskId}";
                    break;
            }
            await StartupStateUpdateAsync();
        }
    }
}

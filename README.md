# Register Desktop Bridge app and UWP app as Startup program
Future Of DotNet
https://youtube.com/FutureOfDotNet

## Reference
- Windows 10 Anniversary Update
- Configure your app to start at log-in
  + https://bit.ly/31qzlTZ
- Integrate your desktop app with Windows 10 and UWP
  + https://bit.ly/3aUakn9

## Desktop Bridge vs UWP
- How
- Namespace
- EntryPoint
- Enabled
- Multiple startupTask
- Diabled/Enabled

## Constrained in UWP apps
- The default is Disable
- Show a user-prompt dialog for UWP apps
- StartupTask includes a Disable method
- If the user disables, then the prompt is not shown again
- If the feature is disabled by local admin or group policy, then the user prompt is not shown, and startup cannot be enabled
- Platforms other than Desktop that don¡¯t support startup tasks also report DisabledByPolicy

## Package.appxmanifest
- xmlns:uap5="http://schemas.microsoft.com/appx/manifest/uap/windows10/5"
- IgnorableNamespaces="uap mp uap5"

```xaml
<Application>
    <Extensions>
        <uap5:Extension Category="windows.startupTask¡° Executable="StartupXXX.exe¡° EntryPoint="StartupXXX.App">
            <uap5:StartupTask TaskId="StartupXXX¡° Enabled="false¡° DisplayName="StartupXXX¡°/>
        </uap5:Extension>
    </Extensions>
</Application>

```

## MainPage.xaml.cs

```c#
var startupTask = await StartupTask.GetAsync("StartupXXX");
StartupTaskState newState = await startupTask.RequestEnableAsync();

```

## App.xaml.cs

```c#
protected override void OnActivated(IActivatedEventArgs args)

var staea = args as StartupTaskActivatedEventArgs;
rootFrame.Navigate(typeof(MainPage), staea.TaskId);
Window.Current.Activate();

```
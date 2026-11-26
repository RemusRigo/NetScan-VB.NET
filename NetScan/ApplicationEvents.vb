Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
   ' The following events are available for MyApplication:
   ' Startup: Raised when the application starts, before the startup form is created.
   ' Shutdown: Raised after all application forms are closed. This event is not raised if the application terminates abnormally.
   ' UnhandledException: Raised if the application encounters an unhandled exception.
   ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
   ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

   ' **NEW** ApplyApplicationDefaults: Raised when the application queries default values to be set for the application.

   ' Example:
   ' Private Sub MyApplication_ApplyApplicationDefaults(sender As Object, e As ApplyApplicationDefaultsEventArgs) Handles Me.ApplyApplicationDefaults
   '
   '   ' Setting the application-wide default Font:
   '   e.Font = New Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular)
   '
   '   ' Setting the HighDpiMode for the Application:
   '   e.HighDpiMode = HighDpiMode.PerMonitorV2
   '
   '   ' If a splash dialog is used, this sets the minimum display time:
   '   e.MinimumSplashScreenDisplayTime = 4000
   ' End Sub

   Partial Friend Class MyApplication

#If Not NETFRAMEWORK Then
      ' HighDpiMode exists only in .NET 5+, so it is set here instead of in Application.Designer.vb
      ' (.NET Framework 4.8 is DPI unaware by default)
      Private Sub MyApplication_ApplyApplicationDefaults(sender As Object, e As ApplyApplicationDefaultsEventArgs) Handles Me.ApplyApplicationDefaults
         e.HighDpiMode = HighDpiMode.DpiUnaware
      End Sub
#End If

   End Class
End Namespace

'--------------------------------------------------------------------------------------------------
' About form
'    © 2026 Remus Rigo
'       v1.1.20261110
'--------------------------------------------------------------------------------------------------
Public Class frmAbout

   '-----------------------------------------------------------------------------------------------
   ' LoadImage: Load an image from the assembly resources. Return Nothing if the resource is missing.
   Friend Shared Function LoadImage(name As String) As Image
      Using res = GetType(frmAbout).Assembly.GetManifestResourceStream(name)
         If res Is Nothing Then Return Nothing
         Using img = Image.FromStream(res)
            Return New Bitmap(img)
         End Using
      End Using
   End Function

   '-----------------------------------------------------------------------------------------------
   ' frmAbout: Load
   Private Sub frmAbout_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      lblTitle.Text = appName
      lblVer.Text = appVersion & "." & appBuild
      lnkLblGitHub.Text = appLink
      lnkLblGitHub.Links.Add(0, lnkLblGitHub.Text.Length, "https://github.com/RemusRigo/")

      imgPayPal.Image = LoadImage("PayPal.png")
      imgRevolut.Image = LoadImage("Revolut.png")
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lnkLblGitHub: Click
   Private Sub lnkLblGitHub_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkLblGitHub.LinkClicked
      Process.Start(New ProcessStartInfo("https://github.com/RemusRigo/") With {.UseShellExecute = True})
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' imgPayPal: Click
   Private Sub imgPayPal_Click(sender As Object, e As EventArgs) Handles imgPayPal.Click
      Process.Start(New ProcessStartInfo("https://paypal.me/remusrigo") With {.UseShellExecute = True})
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' imgRevolut: Click
   Private Sub imgRevolut_Click(sender As Object, e As EventArgs) Handles imgRevolut.Click
      Process.Start(New ProcessStartInfo("https://revolut.me/remusrigo") With {.UseShellExecute = True})
   End Sub

End Class
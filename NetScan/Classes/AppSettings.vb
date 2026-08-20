Imports System.IO
Imports System.Xml
Imports System.Text.Json
Imports System.Text.Json.Nodes

Public Class AppSettings

   Public Property IPRange As String = ""
   Private ReadOnly Property SettingsFile As String
      Get
         Dim exePath = Application.ExecutablePath
         Return Path.Combine(Path.GetDirectoryName(exePath), Path.GetFileNameWithoutExtension(exePath) & ".json")
      End Get
   End Property

   ' Load Settings from JSON file
   Public Sub LoadSettings()
      If Not File.Exists(SettingsFile) Then Return
      Dim root As JsonNode = JsonNode.Parse(File.ReadAllText(SettingsFile))
      IPRange = root?("IP Range")?.AsValue()?.GetValue(Of String)()

      'Dim jsonIPRange As JsonObject = TryCast(root?("IP Range"), JsonObject)
      'If jsonIPRange IsNot Nothing Then
      '   IPRange = If(jsonIPRange("IP Range")?.AsValue().GetValue(Of String)(), 0)
      'End If
   End Sub

   ' Save Settings to JSON file
   Public Sub SaveSettings()
      'Dim jsonIPRange As New JsonObject From {
      '    {"IP Range", IPRange}
      '}

      Dim jsonRoot As New JsonObject From {
          {"IP Range", IPRange}
      }

      Dim options As New JsonSerializerOptions With {.WriteIndented = True}
      File.WriteAllText(SettingsFile, jsonRoot.ToJsonString(options))
   End Sub

End Class
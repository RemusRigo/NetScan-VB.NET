'--------------------------------------------------------------------------------------------------
' AppSetting.vb: Load/Save settings to JSON file
'    © 2026 Remus Rigo
'       v1.2.20261126
'--------------------------------------------------------------------------------------------------

Imports System.IO
#If NETFRAMEWORK Then
Imports System.Web.Script.Serialization   ' .NET Framework has no System.Text.Json
#Else
Imports System.Text.Json
Imports System.Text.Json.Nodes
#End If

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
#If NETFRAMEWORK Then
      Dim root = TryCast(New JavaScriptSerializer().DeserializeObject(File.ReadAllText(SettingsFile)), Dictionary(Of String, Object))
      Dim value As Object = Nothing
      If root IsNot Nothing AndAlso root.TryGetValue("IP Range", value) Then IPRange = TryCast(value, String)
#Else
      Dim root As JsonNode = JsonNode.Parse(File.ReadAllText(SettingsFile))
      IPRange = root?("IP Range")?.AsValue()?.GetValue(Of String)()
#End If

      'Dim jsonIPRange As JsonObject = TryCast(root?("IP Range"), JsonObject)
      'If jsonIPRange IsNot Nothing Then
      '   IPRange = If(jsonIPRange("IP Range")?.AsValue().GetValue(Of String)(), 0)
      'End If
   End Sub

   ' Save Settings to JSON file
   Public Sub SaveSettings()
#If NETFRAMEWORK Then
      ' same indented layout as System.Text.Json writes
      Dim json = "{" & vbLf & "  ""IP Range"": " & New JavaScriptSerializer().Serialize(IPRange) & vbLf & "}"
      File.WriteAllText(SettingsFile, json)
#Else
      Dim jsonRoot As New JsonObject From {
          {"IP Range", IPRange}
      }

      Dim options As New JsonSerializerOptions With {.WriteIndented = True}
      File.WriteAllText(SettingsFile, jsonRoot.ToJsonString(options))
#End If
   End Sub

End Class

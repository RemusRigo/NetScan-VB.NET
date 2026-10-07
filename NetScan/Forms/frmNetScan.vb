'--------------------------------------------------------------------------------------------------
' NetScan: frmNetScan.vb: Main form
'    © 2026 Remus Rigo
'       v1.1.20261007
'--------------------------------------------------------------------------------------------------

Imports System.IO
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Threading

Imports NetScan.API

Public Class frmNetScan

   Private Const SYSMENU_ABOUT_ID As UInteger = 1000

   ' Load/Save settings
   Private cfg As New AppSettings()
   Private presetsDir As String = Path.Combine(Application.StartupPath, "Presets")

   ' Percent ProgressBar
   Private pbLoad As rrProgressBar
   Private itemsMin As Integer
   Private itemsMax As Integer

   ' Hide/Show columns
   Private hideOffline As Boolean = False
   Private hideMAC As Boolean = False
   Private hideHostname As Boolean = False
   Private hideVendor As Boolean = False

   ' Task specific data
   Private bkTask As Task
   Private isScanning As Boolean = False
   Private appExit As Boolean = False

   ' Pause/Stop scan
   Private scanCTS As CancellationTokenSource
   Private pauseGate As New ManualResetEventSlim(True) ' set = running, reset = paused
   Private pbBarColor As Color
   Private ReadOnly pbPausedColor As Color = Color.FromArgb(255, 185, 0)

   Private Structure ParsedRange
      Public BaseIP As String ' 192.168.0.
      Public MinIP As Integer ' from 1
      Public MaxIP As Integer ' to 255
      Public Total As Integer ' total IP's (from min to max)
   End Structure

   '-----------------------------------------------------------------------------------------------
   ' OnHandleCreated: add About menu item
   Protected Overrides Sub OnHandleCreated(e As EventArgs)
      MyBase.OnHandleCreated(e)
      Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
      ' Add a separator and then your custom item
      AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
      AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "About...")
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' WndProc: open About DialogBox
   Protected Overrides Sub WndProc(ByRef m As Message)
      MyBase.WndProc(m)
      If m.Msg = WM_SYSCOMMAND Then
         If CUInt(m.WParam) = SYSMENU_ABOUT_ID Then
            frmAbout.ShowDialog()
         End If
      End If
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' ParseRange
   ''' <summary>Parses a range string in the format "192.168.1.10-20"
   ''' this will return:
   ''' BaseIP = "192.168.1."
   ''' MinIP = 10
   ''' MaxIP = 20
   ''' Total = 11
   ''' </summary>
   Private Function ParseRangeStr(rangeStr As String, ByRef result As ParsedRange) As Boolean
      Dim dashIdx = rangeStr.IndexOf("-"c)
      If dashIdx = -1 Then Return False

      Dim maxStr = rangeStr.Substring(dashIdx + 1)
      Dim leftStr = rangeStr.Substring(0, dashIdx)

      Dim lastDotIdx = leftStr.LastIndexOf("."c)
      If lastDotIdx = -1 Then Return False

      result.BaseIP = leftStr.Substring(0, lastDotIdx)
      Dim minStr = leftStr.Substring(lastDotIdx + 1)

      If Not Integer.TryParse(minStr, result.MinIP) Then Return False
      If Not Integer.TryParse(maxStr, result.MaxIP) Then Return False

      If result.MinIP > result.MaxIP Then Return False
      result.Total = (result.MaxIP - result.MinIP) + 1
      Return True
   End Function

   '-----------------------------------------------------------------------------------------------
   ' ParseMultipleRanges
   Private Function ParseMultipleRanges(input As String, ByRef results As List(Of ParsedRange)) As Boolean
      results = New List(Of ParsedRange)()

      'For Each part In input.Split(","c)
      For Each part In input.Split({vbCrLf, vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries)
         Dim trimmed = part.Trim()
         If trimmed = "" Then Continue For

         Dim parsed As New ParsedRange()
         If Not ParseRangeStr(trimmed, parsed) Then
            Return False ' one bad range fails the whole batch
         End If
         results.Add(parsed)
      Next

      Return results.Count > 0
   End Function

   '-----------------------------------------------------------------------------------------------
   ' ScanLocalIP: scans local network interfaces and populates the IP range textbox
   Private Sub ScanLocalIP()
      txtBoxIPRange.Clear()
      For Each ni In NetworkInterface.GetAllNetworkInterfaces()
         If ni.OperationalStatus <> OperationalStatus.Up Then Continue For

         For Each ip In ni.GetIPProperties().UnicastAddresses
            If ip.Address.AddressFamily = AddressFamily.InterNetwork AndAlso ' IPv4 only
               Not IPAddress.IsLoopback(ip.Address) AndAlso ' skip loopback (127.0.0.1)    
               Not ip.ToString().StartsWith("169.254.") Then ' skip Link-local/APIPA (169.254.x.x)

               txtBoxIPRange.AppendText(IPToRange(ip.Address.ToString) & vbCrLf)
            End If
         Next

      Next
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' ScanRanges
   Private Sub ScanRanges(ranges As List(Of ParsedRange))
      isScanning = True
      appExit = False

      lvDevices.Items.Clear()

      ' Populate ListView with all IPs in the specified ranges
      For Each r In ranges
         For index As Integer = r.MinIP To r.MaxIP
            Dim currentIP = r.BaseIP & "." & index.ToString()
            Dim item = lvDevices.Items.Add("Pending")
            item.SubItems.Add(currentIP)
            item.SubItems.Add("") ' Host Name
            item.SubItems.Add("") ' MAC Address
            item.SubItems.Add("") ' Vendor
            item.SubItems.Add("") ' TTL
            item.SubItems.Add("") ' Type
            item.Checked = True
            item.Tag = IPToDWORD(currentIP)
         Next
      Next

      ' Prepare a list of items to process (checked items)
      Dim itemsToProcess As New List(Of ListViewItem)()
      For Each item As ListViewItem In lvDevices.Items
         If item.Checked Then
            itemsToProcess.Add(item)
         End If
      Next

      pbLoad.Maximum = itemsToProcess.Count
      pbLoad.Value = 0
      Dim completedCount = 0
      Me.Text = $"{appTitle}: Scanning {itemsToProcess.Count} devices..."

      scanCTS?.Dispose()
      scanCTS = New CancellationTokenSource()
      Dim token = scanCTS.Token
      Dim opts As New ParallelOptions With {.CancellationToken = token}
      pauseGate.Set()
      SetScanState(True)

      bkTask = Task.Run(Sub()
                           Try
                              Parallel.For(0, itemsToProcess.Count, opts, Sub(index As Integer)
                                                                             ' block here while paused; Stop/exit releases the wait
                                                                             Try
                                                                                pauseGate.Wait(token)
                                                                             Catch ex As OperationCanceledException
                                                                                Exit Sub
                                                                             End Try
                                                                             If appExit Then Exit Sub

                                                                             Dim processItem = itemsToProcess(index)
                                                                             Dim targetAddr = CUInt(processItem.Tag)
                                                                             If targetAddr = INADDR_NONE Then Exit Sub
                                                                             Dim currentIP = processItem.SubItems(1).Text
                                                                             Dim hostName As String = ""
                                                                             Dim macAddress As String = ""
                                                                             Dim vendor As String = "Unknown"


                                                                             Dim pingResult = PingIPWithTTL(currentIP)
                                                                             Dim isOnline = pingResult.Success
                                                                             If isOnline Then
                                                                                If Not hideHostname Then hostName = GetHostNameFromIP(targetAddr)
                                                                                If Not hideMAC Then macAddress = GetMACFromIP(targetAddr)
                                                                                If Not hideVendor Then
                                                                                   If macAddress <> "Unknown" Then
                                                                                      vendor = GetVendorFromMAC(macAddress)
                                                                                   End If
                                                                                End If
                                                                             End If

                                                                             If Not appExit Then
                                                                                Invoke(Sub()
                                                                                          processItem.Text = If(isOnline, "Online", "Offline")
                                                                                          If isOnline Then
                                                                                             If Not hideHostname Then processItem.SubItems(2).Text = hostName
                                                                                             If Not hideMAC Then processItem.SubItems(3).Text = macAddress
                                                                                             If Not hideVendor Then processItem.SubItems(4).Text = vendor
                                                                                             processItem.SubItems(5).Text = pingResult.Ttl.ToString()
                                                                                             processItem.SubItems(6).Text = DetectOSFromTTL(pingResult.Ttl)
                                                                                          Else
                                                                                             If hideOffline Then lvDevices.Items.Remove(processItem) ' Remove offline devices if needed
                                                                                          End If
                                                                                       End Sub)
                                                                             End If

                                                                             Dim currentProgress = Interlocked.Increment(completedCount)
                                                                             If Not appExit Then
                                                                                Invoke(Sub() pbLoad.Value = Math.Min(currentProgress, pbLoad.Maximum))
                                                                             End If
                                                                          End Sub)
                              ' end of Parallel.For
                           Catch ex As OperationCanceledException
                              ' scan stopped by user or app exit
                           End Try
                           Dim wasStopped = token.IsCancellationRequested
                           Invoke(Sub()
                                     isScanning = False
                                     If appExit Then Return
                                     If wasStopped Then
                                        For Each item As ListViewItem In lvDevices.Items
                                           If item.Text = "Pending" Then item.Text = "Skipped"
                                        Next
                                        Me.Text = $"{appTitle}: Scan stopped ({completedCount}/{itemsToProcess.Count})"
                                     Else
                                        pbLoad.Value = pbLoad.Maximum
                                     End If
                                     pbLoad.BarColor = pbBarColor
                                     pbLoad.Invalidate()
                                     SetScanState(False)
                                  End Sub)
                        End Sub)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' SetScanState: enable/disable controls for scanning / idle
   Private Sub SetScanState(scanning As Boolean)
      tsBtnScan.Enabled = Not scanning
      tsBtnPause.Enabled = scanning
      tsBtnPause.Checked = False
      tsBtnStop.Enabled = scanning
      txtBoxIPRange.ReadOnly = scanning
      contextMnuIP_rescan.Enabled = Not scanning
      contextMnuIP_LoadPreset.Enabled = Not scanning
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmNetScan: OnLoad
   Private Sub frmNetScan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      Me.Text = appTitle

      ' build ListView
      lvDevices.View = View.Details
      lvDevices.CheckBoxes = True
      lvDevices.FullRowSelect = True
      lvDevices.Columns.Add("Status", 80, HorizontalAlignment.Left)
      lvDevices.Columns.Add("IP Address", 100, HorizontalAlignment.Left)
      lvDevices.Columns.Add("Hostname", 120, HorizontalAlignment.Left)
      lvDevices.Columns.Add("MAC Address", 120, HorizontalAlignment.Left)
      lvDevices.Columns.Add("Vendor", 225, HorizontalAlignment.Left)
      lvDevices.Columns.Add("TTL", 35, HorizontalAlignment.Left)
      lvDevices.Columns.Add("Type", 160, HorizontalAlignment.Left)

      ' build ProgressBar
      pbLoad = New rrProgressBar()
      pbLoad.Dock = DockStyle.None
      pbLoad.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      pbLoad.Location = New Point(3, Me.ClientSize.Height - pbLoad.Height - 5)
      pbLoad.Size = New Size(Me.ClientSize.Width - 10, 20)
      Me.Controls.Add(pbLoad)
      pbBarColor = pbLoad.BarColor

      SetScanState(False)

      ' Load settings
      Dim exePath = Application.ExecutablePath
      Dim cfgFile As String = Path.Combine(Path.GetDirectoryName(exePath), Path.GetFileNameWithoutExtension(exePath) & ".cfg")
      If File.Exists(cfgFile) Then
         cfg.LoadSettings()
         txtBoxIPRange.Text = cfg.IPRange
      Else
         'txtBoxIPRange.Text = IPToRange(GetLocalIP())
         ScanLocalIP()
      End If

      ' Load OIU file
      Dim csvFile As String = Path.Combine(Path.GetDirectoryName(exePath), "oui.csv")
      Dim ouiCount As Integer
      If File.Exists(csvFile) Then
         ouiCount = LoadExternalOuiTable()
      Else
         ouiCount = LoadInternalOuiTable()
      End If

      If ouiCount > 0 Then Me.Text = appTitle & " [" & ouiCount & " OUI items found]"

      If Not System.IO.Directory.Exists(presetsDir) Then System.IO.Directory.CreateDirectory(presetsDir)


   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmNetScan: OnFormClosing
   Private Sub frmNetScan_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
      ' save settings
      cfg.IPRange = txtBoxIPRange.Text
      cfg.SaveSettings()

      If Not isScanning Then Return

      e.Cancel = True
      appExit = True ' signal background loop to stop
      scanCTS?.Cancel() ' also releases workers waiting on pause

      If bkTask IsNot Nothing Then
         ' Application.DoEvents keeps the UI responsive during the brief wait,
         ' mirroring the original's Application.ProcessMessages loop.
         While isScanning
            Application.DoEvents()
            Thread.Sleep(10)
         End While
      End If

      Close() ' now safe — isScanning is False
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' tsBtnScan: OnClick
   Private Sub tsBtnScan_Click(sender As Object, e As EventArgs) Handles tsBtnScan.Click
      If isScanning Then Exit Sub
      Dim ranges As New List(Of ParsedRange)()
      If Not ParseMultipleRanges(txtBoxIPRange.Text, ranges) Then
         MessageBox.Show("Invalid range")
         Exit Sub
      End If
      ScanRanges(ranges)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' tsBtnPause: OnClick (toggles Pause / Resume)
   Private Sub tsBtnPause_Click(sender As Object, e As EventArgs) Handles tsBtnPause.Click
      If Not isScanning Then Exit Sub
      If pauseGate.IsSet Then
         pauseGate.Reset()
         tsBtnPause.Checked = True
         pbLoad.BarColor = pbPausedColor
         Me.Text = $"{appTitle}: Paused ({pbLoad.Value}/{pbLoad.Maximum})"
      Else
         pauseGate.Set()
         tsBtnPause.Checked = False
         pbLoad.BarColor = pbBarColor
         Me.Text = $"{appTitle}: Scanning {pbLoad.Maximum} devices..."
      End If
      pbLoad.Invalidate()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' tsBtnStop: OnClick
   Private Sub tsBtnStop_Click(sender As Object, e As EventArgs) Handles tsBtnStop.Click
      If Not isScanning Then Exit Sub
      tsBtnPause.Enabled = False
      tsBtnStop.Enabled = False
      Me.Text = $"{appTitle}: Stopping..."
      scanCTS.Cancel() ' in-flight pings finish, no new ones start
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' tsBtnHideOffline: OnClick
   Private Sub tsBtnHideOffline_Click(sender As Object, e As EventArgs) Handles tsBtnHideOffline.Click
      hideOffline = tsBtnHideOffline.Checked
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' tsBtnHideHostname: OnClick
   Private Sub tsBtnHideHostname_Click(sender As Object, e As EventArgs) Handles tsBtnHideHostname.Click
      hideHostname = tsBtnHideHostname.Checked
      If hideHostname Then
         lvDevices.Columns(2).Width = 0
      Else
         lvDevices.Columns(2).Width = 120
      End If
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' tsBtnHideMAC: OnClick
   Private Sub tsBtnHideMAC_Click(sender As Object, e As EventArgs) Handles tsBtnHideMAC.Click
      hideMAC = tsBtnHideMAC.Checked
      If hideMAC Then
         lvDevices.Columns(3).Width = 0
      Else
         lvDevices.Columns(3).Width = 120
      End If
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' tsBtnHideVendor: OnClick
   Private Sub tsBtnHideVendor_Click(sender As Object, e As EventArgs) Handles tsBtnHideVendor.Click
      hideVendor = tsBtnHideVendor.Checked
      If hideVendor Then
         lvDevices.Columns(4).Width = 0
      Else
         lvDevices.Columns(4).Width = 225
      End If
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' contextMnuIP_rescan: OnClick
   Private Sub contextMnuIP_rescan_Click(sender As Object, e As EventArgs) Handles contextMnuIP_rescan.Click
      ScanLocalIP()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' contextMnuIP_SaveAsPreset: OnClick
   Private Sub contextMnuIP_SaveAsPreset_Click(sender As Object, e As EventArgs) Handles contextMnuIP_SaveAsPreset.Click
      Using dlgSave As New SaveFileDialog()
         dlgSave.Title = "Save Preset"
         dlgSave.Filter = "Preset Files (*.preset)|*.preset"
         dlgSave.DefaultExt = "preset"
         dlgSave.InitialDirectory = presetsDir
         If dlgSave.ShowDialog() = DialogResult.OK Then
            Try
               File.WriteAllText(dlgSave.FileName, txtBoxIPRange.Text)
            Catch ex As Exception
               MessageBox.Show("Error saving preset: " & ex.Message)
            End Try
         End If
      End Using
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' contextMnuIP_LoadPreset: OnClick
   Private Sub contextMnuIP_LoadPreset_Click(sender As Object, e As EventArgs) Handles contextMnuIP_LoadPreset.Click
      Using dlgOpen As New OpenFileDialog()
         dlgOpen.Title = "Load Preset"
         dlgOpen.Filter = "Preset Files (*.preset)|*.preset"
         dlgOpen.DefaultExt = "preset"
         dlgOpen.InitialDirectory = presetsDir
         If dlgOpen.ShowDialog() = DialogResult.OK Then
            txtBoxIPRange.Clear()
            Try
               txtBoxIPRange.Text = File.ReadAllText(dlgOpen.FileName)
            Catch ex As Exception
               MessageBox.Show("Error loading preset: " & ex.Message)
            End Try
         End If
      End Using
   End Sub

   Private Sub tsBtn_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tsBtn.ItemClicked

   End Sub
End Class

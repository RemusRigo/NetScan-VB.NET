'--------------------------------------------------------------------------------------------------
' NetScan: frmNetScan.vb: Main form
'    © 2026 Remus Rigo
'       v1.0.20260804
'--------------------------------------------------------------------------------------------------

Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports System.Threading

Imports NetScan.API

Public Class frmNetScan

   Private Const SYSMENU_ABOUT_ID As UInteger = 1000

   Private pbLoad As rrProgressBar

   Private itemsMin As Integer
   Private itemsMax As Integer

   Private hideOffline As Boolean = False
   Private hideMAC As Boolean = False
   Private hideHostname As Boolean = False
   Private hideVendor As Boolean = False

   Private bkTask As Task
   Private isScanning As Boolean = False
   Private appExit As Boolean = False

   Private ReadOnly vendorThrottle As New SemaphoreSlim(2) ' max 2 concurrent vendor lookups

   Private Structure ParsedRange
      Public BaseIP As String
      Public MinIP As Integer
      Public MaxIP As Integer
      Public Total As Integer
   End Structure

   Protected Overrides Sub OnHandleCreated(e As EventArgs)
      MyBase.OnHandleCreated(e)
      Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
      ' Add a separator and then your custom item
      AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
      AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "About...")
   End Sub

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
   ' ScanRanges
   Private Sub ScanRanges(ranges As List(Of ParsedRange))
      isScanning = True
      appExit = False

      lvDevices.Items.Clear()

      For Each r In ranges
         For index As Integer = r.MinIP To r.MaxIP
            Dim currentIP = r.BaseIP & "." & index.ToString()
            Dim item = lvDevices.Items.Add("Pending")
            item.SubItems.Add(currentIP)
            item.SubItems.Add("") ' Host Name
            item.SubItems.Add("") ' MAC Address
            item.SubItems.Add("") ' Vendor
            item.Checked = True
            item.Tag = IPToDWORD(currentIP)
         Next
      Next

      Dim itemsToProcess As New List(Of ListViewItem)()
      For Each item As ListViewItem In lvDevices.Items
         If item.Checked Then
            itemsToProcess.Add(item)
         End If
      Next

      pbLoad.Maximum = itemsToProcess.Count
      pbLoad.Value = 0
      Dim completedCount = 0

      bkTask = Task.Run(Sub()
                           Parallel.For(0, itemsToProcess.Count, Sub(index As Integer)
                                                                    If appExit Then Exit Sub

                                                                    Dim processItem = itemsToProcess(index)
                                                                    Dim targetAddr = CUInt(processItem.Tag)
                                                                    If targetAddr = INADDR_NONE Then Exit Sub
                                                                    Dim hostName As String = ""
                                                                    Dim macAddress As String = ""
                                                                    Dim vendor As String = "Unknown"


                                                                    Dim isOnline = PingIP(targetAddr)
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
                                                                                 If isOnline Then
                                                                                    processItem.Text = If(isOnline, "Online", "Offline")
                                                                                    If Not hideHostname Then processItem.SubItems(2).Text = hostName
                                                                                    If Not hideMAC Then processItem.SubItems(3).Text = macAddress
                                                                                    If Not hideVendor Then processItem.SubItems(4).Text = vendor
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
                           Invoke(Sub()
                                     isScanning = False
                                     tsBtn.Enabled = True
                                     If Not appExit Then pbLoad.Value = pbLoad.Maximum
                                  End Sub)
                        End Sub)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmNetScan: OnLoad
   Private Sub frmNetScan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      Me.Text = appTitle

      lvDevices.View = View.Details
      lvDevices.CheckBoxes = True
      lvDevices.FullRowSelect = True
      lvDevices.Columns.Add("Status", 100, HorizontalAlignment.Left)
      lvDevices.Columns.Add("IP Address", 120, HorizontalAlignment.Left)
      lvDevices.Columns.Add("Hostname", 120, HorizontalAlignment.Left)
      lvDevices.Columns.Add("MAC Address", 120, HorizontalAlignment.Left)
      lvDevices.Columns.Add("Vendor", 225, HorizontalAlignment.Left)

      pbLoad = New rrProgressBar()
      pbLoad.Dock = DockStyle.None
      pbLoad.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      pbLoad.Location = New Point(3, Me.ClientSize.Height - pbLoad.Height - 5)
      pbLoad.Size = New Size(Me.ClientSize.Width - 10, 20)
      Me.Controls.Add(pbLoad)

      txtBoxIPRange.Text = IPToRange(GetLocalIP())

      LoadOuiTable()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmNetScan: OnFormClosing
   Private Sub frmNetScan_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
      If Not isScanning Then Return

      e.Cancel = True
      appExit = True ' signal background loop to stop

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
   ' txBtnScan: OnClick
   Private Sub txBtnScan_Click(sender As Object, e As EventArgs) Handles txBtnScan.Click
      tsBtn.Enabled = False
      Dim ranges As New List(Of ParsedRange)()
      If Not ParseMultipleRanges(txtBoxIPRange.Text, ranges) Then
         MessageBox.Show("Invalid range")
         Exit Sub
      End If
      ScanRanges(ranges)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' tsBtnHideOffline: OnClick
   Private Sub tsBtnHideOffline_Click(sender As Object, e As EventArgs) Handles tsBtnHideOffline.Click
      hideOffline = tsBtnHideOffline.Checked
   End Sub

   Private Sub tsBtnHideHostname_Click(sender As Object, e As EventArgs) Handles tsBtnHideHostname.Click
      hideHostname = tsBtnHideHostname.Checked
      If hideHostname Then
         lvDevices.Columns(2).Width = 0
      Else
         lvDevices.Columns(2).Width = 120
      End If
   End Sub

   Private Sub tsBtnHideMAC_Click(sender As Object, e As EventArgs) Handles tsBtnHideMAC.Click
      hideMAC = tsBtnHideMAC.Checked
      If hideMAC Then
         lvDevices.Columns(3).Width = 0
      Else
         lvDevices.Columns(3).Width = 120
      End If
   End Sub

   Private Sub tsBtnHideVendor_Click(sender As Object, e As EventArgs) Handles tsBtnHideVendor.Click
      hideVendor = tsBtnHideVendor.Checked
      If hideVendor Then
         lvDevices.Columns(4).Width = 0
      Else
         lvDevices.Columns(4).Width = 225
      End If
   End Sub

End Class

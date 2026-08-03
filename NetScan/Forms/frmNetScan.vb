Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports System.Threading

Public Class frmNetScan

   Private pbLoad As rrProgressBar

   Private itemsMin As Integer
   Private itemsMax As Integer
   Private hideOffline As Boolean = False

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

   '-----------------------------------------------------------------------------------------------
   ' Parse Range
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
   ' Parse Multiple Ranges
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
   ' Scan Range
   Private Sub ScanRange(baseIP As String, minIP As Integer, maxIP As Integer)
      itemsMin = minIP
      itemsMax = maxIP

      Dim localBaseIP = baseIP & "."
      Dim totalIPs = (maxIP - minIP) + 1

      isScanning = True
      appExit = False

      lvDevices.Items.Clear()

      ' Add IP addresses from the range ----------------------------------
      For index As Integer = minIP To maxIP
         Dim currentIP = localBaseIP & index.ToString()
         Dim item = lvDevices.Items.Add("Pending") 'Status
         item.SubItems.Add(currentIP) 'IP Address
         item.SubItems.Add("") 'Host Name
         item.SubItems.Add("") 'MAC Address
         item.Checked = True
         item.Tag = IPToDWORD(currentIP) ' stash the target address for the scan step
      Next

      ' Items to process -------------------------------------------------
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

                                                                    Dim isOnline = PingIP(targetAddr)
                                                                    If isOnline Then
                                                                       hostName = GetHostNameFromIP(targetAddr)
                                                                       macAddress = GetMACFromIP(targetAddr)
                                                                    End If

                                                                    If Not appExit Then
                                                                       Invoke(Sub()
                                                                                 If isOnline Then
                                                                                    processItem.Text = If(isOnline, "Online", "Offline")
                                                                                    processItem.SubItems(2).Text = hostName
                                                                                    processItem.SubItems(3).Text = macAddress
                                                                                 Else
                                                                                    lvDevices.Items.Remove(processItem) ' Remove offline devices if needed
                                                                                 End If
                                                                              End Sub)
                                                                    End If

                                                                    Dim currentProgress = Interlocked.Increment(completedCount)
                                                                    If Not appExit Then
                                                                       Invoke(Sub() pbLoad.Value = Math.Min(currentProgress, pbLoad.Maximum))
                                                                    End If
                                                                 End Sub)
                           Invoke(Sub()
                                     isScanning = False
                                     If Not appExit Then
                                        pbLoad.Value = pbLoad.Maximum
                                     End If
                                  End Sub)
                        End Sub)
   End Sub

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
                                                                       hostName = GetHostNameFromIP(targetAddr)
                                                                       macAddress = GetMACFromIP(targetAddr)
                                                                       ' already running inside Task.Run on a background thread
                                                                       ' block the async call instead of awaiting it
                                                                       ' this: vendor = If(macAddress <> "Unknown", Await GetVendorFromMAC(macAddress), "Unknown")
                                                                       ' would return before the await completes, since Async Sub is fire-and-forget
                                                                       ' with no way for Parallel.For to wait for it

                                                                       'vendor = If(macAddress <> "Unknown", GetVendorOnlineFromMAC(macAddress).GetAwaiter().GetResult(), "Unknown")

                                                                       If macAddress <> "Unknown" Then
                                                                          vendor = GetVendorFromMAC(macAddress)
                                                                       End If
                                                                    End If

                                                                    If Not appExit Then
                                                                       Invoke(Sub()
                                                                                 If isOnline Then
                                                                                    processItem.Text = If(isOnline, "Online", "Offline")
                                                                                    processItem.SubItems(2).Text = hostName
                                                                                    processItem.SubItems(3).Text = macAddress
                                                                                    processItem.SubItems(4).Text = vendor
                                                                                 Else
                                                                                    lvDevices.Items.Remove(processItem) ' Remove offline devices if needed
                                                                                 End If
                                                                              End Sub)
                                                                    End If

                                                                    Dim currentProgress = Interlocked.Increment(completedCount)
                                                                    If Not appExit Then
                                                                       Invoke(Sub() pbLoad.Value = Math.Min(currentProgress, pbLoad.Maximum))
                                                                    End If
                                                                 End Sub)

                           Invoke(Sub()
                                     isScanning = False
                                     If Not appExit Then pbLoad.Value = pbLoad.Maximum
                                  End Sub)
                        End Sub)
   End Sub

   Private Sub frmNetScan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

   Private Sub txBtnScan_Click(sender As Object, e As EventArgs) Handles txBtnScan.Click
      'Dim parsedRange As New ParsedRange()
      'If Not ParseRangeStr(txtBoxIPRange.Text, parsedRange) Then
      '   MessageBox.Show("Invalid range")
      '   Exit Sub
      'End If
      'ScanRange(parsedRange.BaseIP, parsedRange.MinIP, parsedRange.MaxIP)

      Dim ranges As New List(Of ParsedRange)()
      If Not ParseMultipleRanges(txtBoxIPRange.Text, ranges) Then
         MessageBox.Show("Invalid range")
         Exit Sub
      End If
      ScanRanges(ranges)
   End Sub

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

   Private Sub tsBtnHideOffline_Click(sender As Object, e As EventArgs) Handles tsBtnHideOffline.Click
      hideOffline = tsBtnHideOffline.Checked
   End Sub
End Class

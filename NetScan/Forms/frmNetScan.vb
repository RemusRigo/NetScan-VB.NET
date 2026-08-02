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

   Private ReadOnly INVALID_HANDLE_VALUE As IntPtr = New IntPtr(-1)
   Public Const INADDR_NONE As UInteger = &HFFFFFFFFUI

   Private Structure ParsedRange
      Public BaseIP As String
      Public MinIP As Integer
      Public MaxIP As Integer
      Public Total As Integer
   End Structure

   ''' <summary>Equivalent of GetIPFromHost — local machine's primary IPv4 address.</summary>
   Public Function GetLocalIP() As String
      Try
         Dim hostName = Dns.GetHostName()
         Dim entry = Dns.GetHostEntry(hostName)
         Dim ipv4 = entry.AddressList.FirstOrDefault(Function(a) a.AddressFamily = AddressFamily.InterNetwork)
         Return If(ipv4?.ToString(), "Unknown")
      Catch
         Return "Unknown (Winsock is not responding)"
      End Try
   End Function

   Public Function IPToDWORD(ip As String) As UInteger
      Dim addr As IPAddress = Nothing
      If Not IPAddress.TryParse(ip, addr) OrElse addr.AddressFamily <> AddressFamily.InterNetwork Then
         Return INADDR_NONE
      End If
      Return BitConverter.ToUInt32(addr.GetAddressBytes(), 0)
   End Function

   ''' <summary>Equivalent of IPToRange — "192.168.1.50" -> "192.168.1.1-255".</summary>
   Public Function IPToRange(ip As String) As String
      Dim lastDot = ip.LastIndexOf("."c)
      If lastDot < 0 Then Return ip
      Return ip.Substring(0, lastDot + 1) & "1-255"
   End Function

   Public Function PingIP(targetAddr As UInteger) As Boolean
      Dim icmpHandle As IntPtr = IcmpCreateFile()
      If icmpHandle = INVALID_HANDLE_VALUE Then Return False

      Try
         Dim replySize As Integer = Marshal.SizeOf(Of ICMP_ECHO_REPLY)() + 8
         Dim replyBuffer As IntPtr = Marshal.AllocHGlobal(replySize)
         Try
            Dim ret = IcmpSendEcho(icmpHandle, targetAddr, IntPtr.Zero, 0,
                                        IntPtr.Zero, replyBuffer, CUInt(replySize), 500)
            If ret = 0 Then Return False

            Dim reply = Marshal.PtrToStructure(Of ICMP_ECHO_REPLY)(replyBuffer)
            Return reply.Status = 0 ' IP_SUCCESS
         Finally
            Marshal.FreeHGlobal(replyBuffer)
         End Try
      Finally
         IcmpCloseHandle(icmpHandle)
      End Try
   End Function

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

                                                                    Dim isOnline = PingIP(targetAddr)
                                                                    If Not appExit Then
                                                                       Invoke(Sub()
                                                                                 processItem.Text = If(isOnline, "Online", "Offline")
                                                                                 If Not isOnline Then
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


   Private Sub frmNetScan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      lvDevices.View = View.Details
      lvDevices.CheckBoxes = True
      lvDevices.FullRowSelect = True
      lvDevices.Columns.Add("Status", 100, HorizontalAlignment.Left)
      lvDevices.Columns.Add("IP Address", 120, HorizontalAlignment.Left)
      lvDevices.Columns.Add("MAC Address", 120, HorizontalAlignment.Left)

      pbLoad = New rrProgressBar()
      pbLoad.Dock = DockStyle.None
      pbLoad.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      pbLoad.Location = New Point(3, Me.ClientSize.Height - pbLoad.Height - 5)
      pbLoad.Size = New Size(Me.ClientSize.Width - 10, 20)
      Me.Controls.Add(pbLoad)


      txtBoxIPRange.Text = IPToRange(GetLocalIP())
   End Sub

   Private Sub txBtnScan_Click(sender As Object, e As EventArgs) Handles txBtnScan.Click
      Dim parsedRange As New ParsedRange()
      If Not ParseRangeStr(txtBoxIPRange.Text, parsedRange) Then
         MessageBox.Show("Invalid range")
         Exit Sub
      End If
      ScanRange(parsedRange.BaseIP, parsedRange.MinIP, parsedRange.MaxIP)
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

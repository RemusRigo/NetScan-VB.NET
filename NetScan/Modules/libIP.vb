Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports System.Threading

Module libIP

   Public Const INADDR_NONE As UInteger = &HFFFFFFFFUI
   Public ReadOnly INVALID_HANDLE_VALUE As IntPtr = New IntPtr(-1)
   Private ReadOnly httpClient As New HttpClient()
   Private ReadOnly ouiTable As New Dictionary(Of String, String)()

   Public Function IPToDWORD(ip As String) As UInteger
      Dim addr As IPAddress = Nothing
      If Not IPAddress.TryParse(ip, addr) OrElse addr.AddressFamily <> AddressFamily.InterNetwork Then
         Return INADDR_NONE
      End If
      Return BitConverter.ToUInt32(addr.GetAddressBytes(), 0)
   End Function

   ''' <summary>Equivalent of IPToRange: "192.168.1.13" -> "192.168.1.1-255".</summary>
   Public Function IPToRange(ip As String) As String
      Dim lastDot = ip.LastIndexOf("."c)
      If lastDot < 0 Then Return ip
      Return ip.Substring(0, lastDot + 1) & "1-255"
   End Function

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

   Public Function GetHostNameFromIP(targetAddr As UInteger) As String
      Try
         Dim ip As New IPAddress(BitConverter.GetBytes(targetAddr))
         Return Dns.GetHostEntry(ip).HostName
      Catch
         Return "Unknown"
      End Try
   End Function

   Public Function GetMACFromIP(targetAddr As UInteger) As String
      Dim macBytes(5) As Byte
      Dim macLen As UInteger = CUInt(macBytes.Length)

      ' ARP can't see beyond the local subnet or gateway
      If SendARP(targetAddr, 0, macBytes, macLen) = 0 Then
         Return String.Join("-", macBytes.Take(CInt(macLen)).Select(Function(b) b.ToString("x2")))
      End If
      Return "Unknown"
   End Function

   Public Async Function GetVendorOnlineFromMAC(macAddr As String) As Task(Of String)
      If macAddr = "Unknown" OrElse String.IsNullOrEmpty(macAddr) Then Return "Unknown"

      Try
         Dim url = $"https://api.macvendors.com/{macAddr}"
         Dim response = Await httpClient.GetStringAsync(url)
         Return response
      Catch
         Return "Unknown"
      End Try
   End Function

   Public Sub LoadOuiTable()
      Dim path = System.IO.Path.Combine(Application.StartupPath, "oui.csv")
      Try
         For Each line In File.ReadLines(path)
            Dim commaIdx = line.IndexOf(","c)
            If commaIdx > 0 Then
               Dim assignment = line.Substring(0, commaIdx).Trim().ToUpperInvariant()
               Dim orgName = line.Substring(commaIdx + 1).Trim()
               orgName = orgName.Trim(""""c) ' strip surrounding quotes, if present
               ouiTable(assignment) = orgName
            End If
         Next
      Catch ex As Exception
         MessageBox.Show($"Could not load OUI table: {ex.Message}")
      End Try
      'MessageBox.Show($"Loaded {ouiTable.Count} entries")
   End Sub

   Public Function GetVendorFromMAC(macAddr As String) As String
      If macAddr = "Unknown" OrElse String.IsNullOrEmpty(macAddr) Then Return "Unknown"

      Dim oui = macAddr.Replace("-", "").ToUpperInvariant()
      If oui.Length < 6 Then Return "Unknown"
      oui = oui.Substring(0, 6)
      Dim vendor As String = Nothing
      Return If(ouiTable.TryGetValue(oui, vendor), vendor, "Unknown")
   End Function


End Module

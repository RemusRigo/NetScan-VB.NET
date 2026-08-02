Imports System.Runtime.InteropServices

Module IPHlpAPI

   <StructLayout(LayoutKind.Sequential)>
   Public Structure IP_OPTION_INFORMATION
      Public Ttl As Byte
      Public Tos As Byte
      Public Flags As Byte
      Public OptionsSize As Byte
      Public OptionsData As IntPtr
   End Structure

   <StructLayout(LayoutKind.Sequential)>
   Public Structure ICMP_ECHO_REPLY
      Public Address As UInteger
      Public Status As UInteger
      Public RoundTripTime As UInteger
      Public DataSize As UShort
      Public Reserved As UShort
      Public DataPtr As IntPtr
      Public Options As IP_OPTION_INFORMATION
   End Structure

   <DllImport("iphlpapi.dll", SetLastError:=True)>
   Public Function IcmpCreateFile() As IntPtr
   End Function

   <DllImport("iphlpapi.dll", SetLastError:=True)>
   Public Function IcmpCloseHandle(icmpHandle As IntPtr) As Boolean
   End Function

   <DllImport("iphlpapi.dll", SetLastError:=True)>
   Public Function IcmpSendEcho(icmpHandle As IntPtr,
                                destinationAddress As UInteger,
                                requestData As IntPtr,
                                requestSize As UShort,
                                requestOptions As IntPtr,
                                replyBuffer As IntPtr,
                                replySize As UInteger,
                                timeout As UInteger) As UInteger
   End Function

   <DllImport("iphlpapi.dll", SetLastError:=True)>
   Public Function SendARP(destIP As UInteger, srcIP As UInteger, pMacAddr As Byte(), ByRef phyAddrLen As UInteger) As Integer
   End Function

End Module

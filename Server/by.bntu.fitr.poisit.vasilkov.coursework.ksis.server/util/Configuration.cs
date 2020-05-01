using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Server.by.bntu.fitr.poisit.vasilkov.coursework.ksis.server.util
{
    public class Configuration
    {
        private string LocalName
        {
            get { return IPGlobalProperties.GetIPGlobalProperties().HostName; }
        }

        private string IPv4
        {
            get { return Dns.GetHostAddresses(Environment.MachineName)[3].ToString(); }
        }

        private string IPv6
        {
            get { return Dns.GetHostAddresses(Environment.MachineName)[1].ToString(); }
        }

        private string IPconfig
        {
            get
            {
                string text = "";
                text = "Настройка протокола IP для Windows \n\n";
                text += GetNetworkInterfacesInfo(NetworkInterface.GetAllNetworkInterfaces());
                return text;
            }
        }

        private string ToAll
        {
            get
            {
                string text = "";
                text = "Настройка протокола IP для Windows \n\n";
                text += "Имя компьютера: .................." + LocalName + "\n";
                text += "Основной DNS-суффикс: ............" + DNSSuffix + "\n";
                text += GetNetworkInterfacesInfoAll(NetworkInterface.GetAllNetworkInterfaces());
                return text;
            }
        }
        
         private string DNSSuffix
        {
            get
            {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                IPInterfaceProperties properties = adapters[0].GetIPProperties();
                return properties.DnsSuffix;
            }
        }

        private static string GetNetworkInterfacesInfoAll(NetworkInterface[] interfaces)
        {
            StringBuilder info = new StringBuilder();

            foreach (NetworkInterface networkInterface in interfaces)
            {
                info.Append(networkInterface.Name);
                info.Append("\n\t DNS-суффикс подключения: ........." + networkInterface.GetIPProperties().DnsSuffix);
                info.Append("\n\t Описание: ........................" + networkInterface.Description);
                info.Append("\n\t Физический адрес: ................" + BitConverter.ToString(networkInterface.GetPhysicalAddress().GetAddressBytes()));
                info.Append("\n\t DHCP включен: ...................." + networkInterface.GetIPProperties().GetIPv4Properties().IsDhcpEnabled);
                info.Append("\n\t Локальный IPv6-адрес канала: ....." + networkInterface.GetIPProperties().UnicastAddresses[0].Address.ToString());
                info.Append("\n\t Ipv4-адрес: ......................" + networkInterface.GetIPProperties().UnicastAddresses[1].Address.ToString());
                info.Append("\n\t Маска подсети: ..................." + networkInterface.GetIPProperties().UnicastAddresses[1].IPv4Mask.ToString());
                info.Append("\n\t Основной шлюз: ...................");
                info.Append(GatewayAddress(networkInterface));
                info.Append("\n\n");
            }
            return info.ToString();
        }
        private static string GetNetworkInterfacesInfo(NetworkInterface[] interfaces)
        {
            StringBuilder info = new StringBuilder();

            foreach (NetworkInterface networkInterface in interfaces)
            {
                info.Append(networkInterface.Name);
                info.Append("\n\t DNS-суффикс подключения: ........." + networkInterface.GetIPProperties().DnsSuffix);
                info.Append("\n\t Локальный IPv6-адрес канала: ....." + networkInterface.GetIPProperties().UnicastAddresses[0].Address.ToString());
                info.Append("\n\t Ipv4-адрес: ......................" + networkInterface.GetIPProperties().UnicastAddresses[1].Address.ToString());
                info.Append("\n\t Маска подсети: ..................." + networkInterface.GetIPProperties().UnicastAddresses[1].IPv4Mask.ToString());
                info.Append("\n\t Основной шлюз: ...................");
                info.Append(GatewayAddress(networkInterface));
                info.Append("\n\n");
            }
            return info.ToString();
        }

        private static string GatewayAddress(NetworkInterface networkInterface)
        {
            StringBuilder info = new StringBuilder();

            IPInterfaceProperties adapterProperties = networkInterface.GetIPProperties();
            GatewayIPAddressInformationCollection addresses = adapterProperties.GatewayAddresses;
            if (addresses.Count > 0)
            {
                foreach (GatewayIPAddressInformation address in addresses)
                {
                   info.Append(address.Address.ToString() + "\n");
                }
            }
            return info.ToString();
        }

        public static string DisplayIPv4NetworkInterfaces
        {
            get
            {
                StringBuilder info = new StringBuilder();
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
                info.Append("IPv4 interface information for" + properties.HostName + "." + properties.DomainName + "\n");

                foreach (NetworkInterface adapter in nics)
                {
                    // Only display informatin for interfaces that support IPv4.
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4) == false)
                    {
                        continue;
                    }
                    info.Append(adapter.Description + "\n");
                    // Underline the description.
                    info.Append(String.Empty.PadLeft(adapter.Description.Length, '=') + "\n");
                    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                    // Try to get the IPv4 interface properties.
                    IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();

                    if (p == null)
                    {
                        info.Append("No IPv4 information is available for this interface." + "\n");
                        continue;
                    }
                    // Display the IPv4 specific data.
                    info.Append("  Index ............................. : " + p.Index + "\n");
                    info.Append("  MTU ............................... : " + p.Mtu + "\n");
                    info.Append("  APIPA active....................... : " + p.IsAutomaticPrivateAddressingActive + "\n");
                    info.Append("  APIPA enabled...................... : " + p.IsAutomaticPrivateAddressingEnabled + "\n");
                    info.Append("  Forwarding enabled................. : " + p.IsForwardingEnabled + "\n");
                    info.Append("  Uses WINS ......................... : " + p.UsesWins + "\n");
                }
                return info.ToString();
            }
        }

        public static string ShowIPStatistics
        {
            get
            {
                NetworkInterfaceComponent version = new NetworkInterfaceComponent();
                StringBuilder info = new StringBuilder();
                IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
                IPGlobalStatistics ipstat = null;
                switch (version)
                {
                    case NetworkInterfaceComponent.IPv4:
                        ipstat = properties.GetIPv4GlobalStatistics();
                        info.Append(Environment.NewLine + " IPv4 Statistics " + "\n");
                        break;
                    case NetworkInterfaceComponent.IPv6:
                        ipstat = properties.GetIPv4GlobalStatistics();
                        info.Append(Environment.NewLine + " IPv6 Statistics " + "\n");
                        break;
                    default:
                        throw new ArgumentException("version");
                        //    break;
                }
                info.Append("  Forwarding enabled ...................... : " + ipstat.ForwardingEnabled + "\n");
                info.Append("  Interfaces .............................. : " + ipstat.NumberOfInterfaces + "\n");
                info.Append("  IP addresses ............................ : " + ipstat.NumberOfIPAddresses + "\n");
                info.Append("  Routes .................................. : " + ipstat.NumberOfRoutes + "\n");
                info.Append("  Default TTL ............................. : " + ipstat.DefaultTtl + "\n");
                info.Append("\n");
                info.Append("  Inbound Packet Data:" + "\n");
                info.Append("      Received ............................ : " + ipstat.ReceivedPackets + "\n");
                info.Append("      Forwarded ........................... : " + ipstat.ReceivedPacketsForwarded + "\n");
                info.Append("      Delivered ........................... : " + ipstat.ReceivedPacketsDelivered + "\n");
                info.Append("      Discarded ........................... : " + ipstat.ReceivedPacketsDiscarded + "\n");
                info.Append("      Header Errors ....................... : " + ipstat.ReceivedPacketsWithHeadersErrors + "\n");
                info.Append("      Address Errors ...................... : " + ipstat.ReceivedPacketsWithAddressErrors + "\n");
                info.Append("      Unknown Protocol Errors ............. : " + ipstat.ReceivedPacketsWithUnknownProtocol + "\n");
                info.Append("\n");
                info.Append("  Outbound Packet Data:" + "\n");
                info.Append("      Requested ........................... : " + ipstat.OutputPacketRequests + "\n");
                info.Append("      Discarded ........................... : " + ipstat.OutputPacketsDiscarded + "\n");
                info.Append("      No Routing Discards ................. : " +  ipstat.OutputPacketsWithNoRoute + "\n");
                info.Append("      Routing Entry Discards .............. : " + ipstat.OutputPacketRoutingDiscards + "\n");
                info.Append("\n");
                info.Append("  Reassembly Data:" + "\n");
                info.Append("      Reassembly Timeout .................. : " + ipstat.PacketReassemblyTimeout + "\n");
                info.Append("      Reassemblies Required ............... : " + ipstat.PacketReassembliesRequired + "\n");
                info.Append("      Packets Reassembled ................. : " + ipstat.PacketsReassembled + "\n");
                info.Append("      Packets Fragmented .................. : " + ipstat.PacketsFragmented + "\n");
                info.Append("      Fragment Failures ................... : " + ipstat.PacketFragmentFailures + "\n");
                info.Append("\n");

                return info.ToString();
            }
        }

        public string IPConfig(string code)
        {
            switch (code)
            {
                case "name":
                    return LocalName;

                case "ipv4":
                    return IPv4;

                case "ipv6":
                    return IPv6;

                case "ipconfig":
                    return IPconfig;

                case "ipconfig/all":
                    return ToAll;

                case "ipv4Interface":
                    return DisplayIPv4NetworkInterfaces;

                case "ipstatistics":
                    return ShowIPStatistics;

                default:
                    return "ERROR: Данная команда отсутствует!!!";
            }
        }
    }
}
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SWSoft.Net
{
    public partial class Wireless
    {
        private static string m_errorMessage = string.Empty;
        private static IntPtr m_pClientHandle = IntPtr.Zero;
        private static uint m_ServiceVersion = 0;

        public static void CloseHandle()
        {
            IntPtr pClientHandle = m_pClientHandle;
            if (WlanCloseHandle(m_pClientHandle, IntPtr.Zero) != 0)
            {
                m_errorMessage = "Failed WlanCloseHandle()";
            }
        }

        public static WLAN_INTERFACE_INFO_LIST EnumerateNICs()
        {
            IntPtr pClientHandle = m_pClientHandle;
            IntPtr ppInterfaceList = IntPtr.Zero;
            if (WlanEnumInterfaces(m_pClientHandle, IntPtr.Zero, out ppInterfaceList) != 0)
            {
                m_errorMessage = "Failed WlanEnumInterfaces()";
            }
            WLAN_INTERFACE_INFO_LIST interfaceList = new WLAN_INTERFACE_INFO_LIST(ppInterfaceList);
            if (ppInterfaceList != IntPtr.Zero)
            {
                WlanFreeMemory(ppInterfaceList);
            }
            return interfaceList;
        }

        public static uint GetSignalQuality(Guid gg)
        {
            WLAN_OPCODE_VALUE_TYPE pOpcodeValueType;
            uint dwSize = 0;
            IntPtr ppData = IntPtr.Zero;
            if (WlanQueryInterface(m_pClientHandle, ref gg, WLAN_INTF_OPCODE.wlan_intf_opcode_current_connection, IntPtr.Zero, out dwSize, out ppData, out pOpcodeValueType) != 0)
            {
                m_errorMessage = "Failed WlanQueryInterface() - Current Connection Attributes";
                return 0;
            }
            if (ppData != IntPtr.Zero)
            {
                WLAN_CONNECTION_ATTRIBUTES connectionAttributes = new WLAN_CONNECTION_ATTRIBUTES(ppData);
                return connectionAttributes.wlanAssociationAttributes.wlanSignalQuality;
            }
            return 0;
        }

        public static void OpenHandle(uint dwClientVersion = 2)
        {
            if (WlanOpenHandle(dwClientVersion, IntPtr.Zero, out m_ServiceVersion, out m_pClientHandle) != 0)
            {
                m_errorMessage = "Failed WlanOpenHandle()";
            }
        }

        /// <summary>
        /// 扫描可用的无线网络
        /// </summary>
        /// <param name="interfaceGuid">无线局域网接口的GUID</param>
        public static void WlanScan(Guid interfaceGuid)
        {
            WlanScan(m_pClientHandle, interfaceGuid, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        public static WlanAvailableNetwork[] AvailableNetworkList(Guid interfaceGuid)
        {
            IntPtr availableNetworkListPtr;
            WlanGetAvailableNetworkList(m_pClientHandle, interfaceGuid, WlanGetAvailableNetworkFlags.IncludeAllAdhocProfiles, IntPtr.Zero, out availableNetworkListPtr);
            return ConvertAvailableNetworkListPtr(availableNetworkListPtr);
        }

        public static WlanBssEntry[] NetworkBssList(WLAN_INTERFACE_INFO wanInter)
        {
            IntPtr bssListPtr;
            WlanGetNetworkBssList(m_pClientHandle, wanInter.InterfaceGuid, IntPtr.Zero, DOT11_BSS_TYPE.dot11_BSS_type_any, false, IntPtr.Zero, out bssListPtr);
            return ConvertBssListPtr(bssListPtr);
        }

        /// <summary>
        /// Converts a pointer to a available networks list (header + entries) to an array of available network entries.
        /// </summary>
        /// <param name="bssListPtr">A pointer to an available networks list's header.</param>
        /// <returns>An array of available network entries.</returns>
        private static WlanAvailableNetwork[] ConvertAvailableNetworkListPtr(IntPtr availNetListPtr)
        {
            WlanAvailableNetworkListHeader availNetListHeader = (WlanAvailableNetworkListHeader)Marshal.PtrToStructure(availNetListPtr, typeof(WlanAvailableNetworkListHeader));
            long availNetListIt = availNetListPtr.ToInt64() + Marshal.SizeOf(typeof(WlanAvailableNetworkListHeader));
            WlanAvailableNetwork[] availNets = new WlanAvailableNetwork[availNetListHeader.numberOfItems];
            for (int i = 0; i < availNetListHeader.numberOfItems; ++i)
            {
                availNets[i] = (WlanAvailableNetwork)Marshal.PtrToStructure(new IntPtr(availNetListIt), typeof(WlanAvailableNetwork));
                availNetListIt += Marshal.SizeOf(typeof(WlanAvailableNetwork));
            }
            return availNets;
        }

        /// <summary>
        /// Converts a pointer to a BSS list (header + entries) to an array of BSS entries.
        /// </summary>
        /// <param name="bssListPtr">A pointer to a BSS list's header.</param>
        /// <returns>An array of BSS entries.</returns>
        private static WlanBssEntry[] ConvertBssListPtr(IntPtr bssListPtr)
        {
            WlanBssListHeader bssListHeader = (WlanBssListHeader)Marshal.PtrToStructure(bssListPtr, typeof(WlanBssListHeader));
            long bssListIt = bssListPtr.ToInt64() + Marshal.SizeOf(typeof(WlanBssListHeader));
            WlanBssEntry[] bssEntries = new WlanBssEntry[bssListHeader.numberOfItems];
            for (int i = 0; i < bssListHeader.numberOfItems; ++i)
            {
                bssEntries[i] = (WlanBssEntry)Marshal.PtrToStructure(new IntPtr(bssListIt), typeof(WlanBssEntry));
                bssListIt += Marshal.SizeOf(typeof(WlanBssEntry));
            }
            return bssEntries;
        }
    }


}
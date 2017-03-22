using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SWSoft.Net
{
    public partial class Wireless
    {
        private const int DOT11_RATE_SET_MAX_LENGTH = 0x7e;
        private const int DOT11_SSID_MAX_LENGTH = 0x20;
        private const int ERROR_SUCCESS = 0;
        private const int WLAN_API_VERSION_2_0 = 2;
        private const int WLAN_MAX_NAME_LENGTH = 0x100;
        private const int WLAN_NOTIFICATION_SORCE_ACM = 8;
        private const int WLAN_NOTIFICATION_SOURCE_ALL = 0xffff;

        [DllImport("Kernel32.dll")]
        public static extern uint GetLastError();

        /// <summary>
        /// The header of an array of information about available networks.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct WlanAvailableNetworkListHeader
        {
            /// <summary>
            /// Contains the number of <see cref=""/> items following the header.
            /// </summary>
            public uint numberOfItems;
            /// <summary>
            /// The index of the current item. The index of the first item is 0.
            /// </summary>
            public uint index;
        }

        #region wlanapi.dll
        /// <summary>
        /// Contains information about an available wireless network.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WlanAvailableNetwork
        {
            /// <summary>
            /// Contains the profile name associated with the network.
            /// If the network doesn't have a profile, this member will be empty.
            /// If multiple profiles are associated with the network, there will be multiple entries with the same SSID in the visible network list. Profile names are case-sensitive.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string profileName;
            /// <summary>
            /// Contains the SSID of the visible wireless network.
            /// </summary>
            public DOT11_SSID dot11Ssid;
            /// <summary>
            /// Specifies whether the network is infrastructure or ad hoc.
            /// </summary>
            public DOT11_PHY_TYPE dot11BssType;
            /// <summary>
            /// Indicates the number of BSSIDs in the network.
            /// </summary>
            public uint numberOfBssids;
            /// <summary>
            /// Indicates whether the network is connectable or not.
            /// </summary>
            public bool networkConnectable;
            /// <summary>
            /// Indicates why a network cannot be connected to. This member is only valid when <see cref="networkConnectable"/> is <c>false</c>.
            /// </summary>
            public WlanReasonCode wlanNotConnectableReason;
            /// <summary>
            /// The number of PHY types supported on available networks.
            /// The maximum value of this field is 8. If more than 8 PHY types are supported, <see cref="morePhyTypes"/> must be set to <c>true</c>.
            /// </summary>
            private uint numberOfPhyTypes;
            /// <summary>
            /// Contains an array of <see cref="Dot11PhyType"/> values that represent the PHY types supported by the available networks.
            /// When <see cref="numberOfPhyTypes"/> is greater than 8, this array contains only the first 8 PHY types.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            private DOT11_PHY_TYPE[] dot11PhyTypes;
            /// <summary>
            /// Gets the <see cref="Dot11PhyType"/> values that represent the PHY types supported by the available networks.
            /// </summary>
            public DOT11_PHY_TYPE[] Dot11PhyTypes
            {
                get
                {
                    DOT11_PHY_TYPE[] ret = new DOT11_PHY_TYPE[numberOfPhyTypes];
                    Array.Copy(dot11PhyTypes, ret, numberOfPhyTypes);
                    return ret;
                }
            }
            /// <summary>
            /// Specifies if there are more than 8 PHY types supported.
            /// When this member is set to <c>true</c>, an application must call <see cref="WlanClient.WlanInterface.GetNetworkBssList"/> to get the complete list of PHY types.
            /// <see cref="WlanBssEntry.phyId"/> contains the PHY type for an entry.
            /// </summary>
            public bool morePhyTypes;
            /// <summary>
            /// 信号强度
            /// </summary>
            public uint wlanSignalQuality;
            /// <summary>
            /// 是否启用了安全设置
            /// </summary>
            public bool securityEnabled;
            /// <summary>
            /// 加入此网络需要的默认身份验证
            /// </summary>
            public DOT11_AUTH_ALGORITHM dot11DefaultAuthAlgorithm;
            /// <summary>
            /// 指示加入此网络时要使用的默认密码算法
            /// </summary>
            public DOT11_CIPHER_ALGORITHM dot11DefaultCipherAlgorithm;
            /// <summary>
            /// 包含网络的各种标志
            /// </summary>
            public WlanAvailableNetworkFlags flags;
            /// <summary>
            /// 供将来使用的保留值
            /// </summary>
            uint reserved;
        }
        /// <summary>
        /// Contains various flags for the network.
        /// </summary>
        [Flags]
        public enum WlanAvailableNetworkFlags
        {
            /// <summary>
            /// 当前连接
            /// </summary>
            Connected = 0x00000001,
            /// <summary>
            /// 有此网络的配置文件 
            /// </summary>
            HasProfile = 0x00000002
        }


        [DllImport("wlanapi.dll", SetLastError = true)]
        public static extern uint WlanOpenHandle(uint dwClientVersion, IntPtr pReserved, out uint pdwNegotiatedVersion, out IntPtr phClientHandle);
        [DllImport("wlanapi.dll", SetLastError = true)]
        public static extern uint WlanCloseHandle(IntPtr hClientHandle, IntPtr pReserved);
        [DllImport("wlanapi.dll", SetLastError = true)]
        public static extern uint WlanEnumInterfaces(IntPtr hClientHandle, IntPtr pReserved, out IntPtr ppInterfaceList);
        [DllImport("wlanapi.dll", SetLastError = true)]
        public static extern void WlanFreeMemory(IntPtr pmemory);
        [DllImport("wlanapi.dll", SetLastError = true)]
        public static extern uint WlanQueryInterface(IntPtr hClientHandle, [In] ref Guid ppInterfaceGuid, WLAN_INTF_OPCODE OpCode, IntPtr pReserved, out uint pdwDataSize, out IntPtr ppData, out WLAN_OPCODE_VALUE_TYPE pWlanOpcodeValueType);

        [DllImport("wlanapi.dll")]
        public static extern int WlanSetInterface(
         [In] IntPtr clientHandle,
         [In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
         [In] WLAN_INTF_OPCODE opCode,
         [In] uint dataSize,
         [In] IntPtr pData,
         [In, Out] IntPtr pReserved);

        [DllImport("wlanapi.dll")]
        public static extern int WlanScan(
         [In] IntPtr clientHandle,
         [In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
         [In] IntPtr pDot11Ssid,
         [In] IntPtr pIeData,
         [In, Out] IntPtr pReserved);

        [DllImport("wlanapi.dll")]
        public static extern int WlanGetNetworkBssList(
         [In] IntPtr clientHandle,
         [In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
         [In] IntPtr dot11SsidInt,
         [In] DOT11_BSS_TYPE dot11BssType,
         [In] bool securityEnabled,
         IntPtr reservedPtr,
         [Out] out IntPtr wlanBssList
        );

        /// <summary>
        /// Contains information about a basic service set (BSS).
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct WlanBssEntry
        {
            /// <summary>
            /// AP接入点的SSID
            /// </summary>
            public DOT11_SSID dot11Ssid;
            /// <summary>
            /// AP 操作的物理层的标识符
            /// </summary>
            public uint phyId;
            /// <summary>
            /// Contains the BSS identifier.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] dot11Bssid;
            /// <summary>
            /// Specifies whether the network is infrastructure or ad hoc.
            /// </summary>
            public DOT11_BSS_TYPE dot11BssType;
            public DOT11_PHY_TYPE dot11BssPhyType;
            /// <summary>
            /// The received signal strength in dBm.
            /// </summary>
            public int rssi;
            /// <summary>
            /// The link quality reported by the driver. Ranges from 0-100.
            /// </summary>
            public uint linkQuality;
            /// <summary>
            /// If 802.11d is not implemented, the network interface card (NIC) must set this field to TRUE. If 802.11d is implemented (but not necessarily enabled), the NIC must set this field to TRUE if the BSS operation complies with the configured regulatory domain.
            /// </summary>
            public bool inRegDomain;
            /// <summary>
            /// Contains the beacon interval value from the beacon packet or probe response.
            /// </summary>
            public ushort beaconPeriod;
            /// <summary>
            /// The timestamp from the beacon packet or probe response.
            /// </summary>
            public ulong timestamp;
            /// <summary>
            /// The host timestamp value when the beacon or probe response is received.
            /// </summary>
            public ulong hostTimestamp;
            /// <summary>
            /// The capability value from the beacon packet or probe response.
            /// </summary>
            public ushort capabilityInformation;
            /// <summary>
            /// The frequency of the center channel, in kHz.
            /// </summary>
            public uint chCenterFrequency;
            /// <summary>
            /// Contains the set of data transfer rates supported by the BSS.
            /// </summary>
            public WlanRateSet wlanRateSet;
            /// <summary>
            /// Offset of the information element (IE) data blob.
            /// </summary>
            public uint ieOffset;
            /// <summary>
            /// Size of the IE data blob, in bytes.
            /// </summary>
            public uint ieSize;
        }
        internal struct WlanBssListHeader
        {
            internal uint totalSize;
            internal uint numberOfItems;
        }


        /// <summary>
        /// Contains the set of supported data rates.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct WlanRateSet
        {
            /// <summary>
            /// The length, in bytes, of <see cref="rateSet"/>.
            /// </summary>
            private uint rateSetLength;
            /// <summary>
            /// An array of supported data transfer rates.
            /// If the rate is a basic rate, the first bit of the rate value is set to 1.
            /// A basic rate is the data transfer rate that all stations in a basic service set (BSS) can use to receive frames from the wireless medium.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 126)]
            private ushort[] rateSet;

            public ushort[] Rates
            {
                get
                {
                    ushort[] rates = new ushort[rateSetLength / sizeof(ushort)];
                    Array.Copy(rateSet, rates, rates.Length);
                    return rates;
                }
            }

            /// <summary>
            /// CalculateS the data transfer rate in Mbps for an arbitrary supported rate.
            /// </summary>
            /// <param name="rate"></param>
            /// <returns></returns>
            public double GetRateInMbps(int rate)
            {
                return (rateSet[rate] & 0x7FFF) * 0.5;
            }
        }

        [DllImport("wlanapi.dll")]
        public static extern int WlanGetAvailableNetworkList(
         [In] IntPtr clientHandle,
         [In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
         [In] WlanGetAvailableNetworkFlags flags,
         [In, Out] IntPtr reservedPtr,
         [Out] out IntPtr availableNetworkListPtr);
        public enum WlanGetAvailableNetworkFlags
        {
            /// <summary>
            /// Include all ad-hoc network profiles in the available network list, including profiles that are not visible.
            /// </summary>
            IncludeAllAdhocProfiles = 0x00000001,
            /// <summary>
            /// Include all hidden network profiles in the available network list, including profiles that are not visible.
            /// </summary>
            IncludeAllManualHiddenProfiles = 0x00000002
        }

        [Flags]
        public enum WlanProfileFlags
        {
            /// <remarks>
            /// The only option available on Windows XP SP2.
            /// </remarks>
            AllUser = 0,
            GroupPolicy = 1,
            User = 2
        }

        [DllImport("wlanapi.dll")]
        public static extern int WlanSetProfile(
         [In] IntPtr clientHandle,
         [In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
         [In] WlanProfileFlags flags,
         [In, MarshalAs(UnmanagedType.LPWStr)] string profileXml,
         [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string allUserProfileSecurity,
         [In] bool overwrite,
         [In] IntPtr pReserved,
         [Out] out WlanReasonCode reasonCode);

        /// <summary>
        /// Defines the access mask of an all-user profile.
        /// </summary>
        [Flags]
        public enum WlanAccess
        {
            /// <summary>
            /// The user can view profile permissions.
            /// </summary>
            ReadAccess = 0x00020000 | 0x0001,
            /// <summary>
            /// The user has read access, and the user can also connect to and disconnect from a network using the profile.
            /// </summary>
            ExecuteAccess = ReadAccess | 0x0020,
            /// <summary>
            /// The user has execute access and the user can also modify and delete permissions associated with a profile.
            /// </summary>
            WriteAccess = ReadAccess | ExecuteAccess | 0x0002 | 0x00010000 | 0x00040000
        }

        /// <param name="flags">Not supported on Windows XP SP2: must be a <c>null</c> reference.</param>
        [DllImport("wlanapi.dll")]
        public static extern int WlanGetProfile(
         [In] IntPtr clientHandle,
         [In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
         [In, MarshalAs(UnmanagedType.LPWStr)] string profileName,
         [In] IntPtr pReserved,
         [Out] out IntPtr profileXml,
         [Out, Optional] out WlanProfileFlags flags,
         [Out, Optional] out WlanAccess grantedAccess);

        [DllImport("wlanapi.dll")]
        public static extern int WlanGetProfileList(
         [In] IntPtr clientHandle,
         [In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
         [In] IntPtr pReserved,
         [Out] out IntPtr profileList
        );

        [DllImport("wlanapi.dll")]
        public static extern int WlanReasonCodeToString(
         [In] WlanReasonCode reasonCode,
         [In] int bufferSize,
         [In, Out] StringBuilder stringBuffer,
         IntPtr pReserved
        );
        public enum WlanReasonCode
        {
            Success = 0,
            // general codes
            UNKNOWN = 0x10000 + 1,

            RANGE_SIZE = 0x10000,
            BASE = 0x10000 + RANGE_SIZE,

            // range for Auto Config
            //
            AC_BASE = 0x10000 + RANGE_SIZE,
            AC_CONNECT_BASE = (AC_BASE + RANGE_SIZE / 2),
            AC_END = (AC_BASE + RANGE_SIZE - 1),

            // range for profile manager
            // it has profile adding failure reason codes, but may not have 
            // connection reason codes
            //
            PROFILE_BASE = 0x10000 + (7 * RANGE_SIZE),
            PROFILE_CONNECT_BASE = (PROFILE_BASE + RANGE_SIZE / 2),
            PROFILE_END = (PROFILE_BASE + RANGE_SIZE - 1),

            // range for MSM
            //
            MSM_BASE = 0x10000 + (2 * RANGE_SIZE),
            MSM_CONNECT_BASE = (MSM_BASE + RANGE_SIZE / 2),
            MSM_END = (MSM_BASE + RANGE_SIZE - 1),

            // range for MSMSEC
            //
            MSMSEC_BASE = 0x10000 + (3 * RANGE_SIZE),
            MSMSEC_CONNECT_BASE = (MSMSEC_BASE + RANGE_SIZE / 2),
            MSMSEC_END = (MSMSEC_BASE + RANGE_SIZE - 1),

            // AC network incompatible reason codes
            //
            NETWORK_NOT_COMPATIBLE = (AC_BASE + 1),
            PROFILE_NOT_COMPATIBLE = (AC_BASE + 2),

            // AC connect reason code
            //
            NO_AUTO_CONNECTION = (AC_CONNECT_BASE + 1),
            NOT_VISIBLE = (AC_CONNECT_BASE + 2),
            GP_DENIED = (AC_CONNECT_BASE + 3),
            USER_DENIED = (AC_CONNECT_BASE + 4),
            BSS_TYPE_NOT_ALLOWED = (AC_CONNECT_BASE + 5),
            IN_FAILED_LIST = (AC_CONNECT_BASE + 6),
            IN_BLOCKED_LIST = (AC_CONNECT_BASE + 7),
            SSID_LIST_TOO_LONG = (AC_CONNECT_BASE + 8),
            CONNECT_CALL_FAIL = (AC_CONNECT_BASE + 9),
            SCAN_CALL_FAIL = (AC_CONNECT_BASE + 10),
            NETWORK_NOT_AVAILABLE = (AC_CONNECT_BASE + 11),
            PROFILE_CHANGED_OR_DELETED = (AC_CONNECT_BASE + 12),
            KEY_MISMATCH = (AC_CONNECT_BASE + 13),
            USER_NOT_RESPOND = (AC_CONNECT_BASE + 14),

            // Profile validation errors
            //
            INVALID_PROFILE_SCHEMA = (PROFILE_BASE + 1),
            PROFILE_MISSING = (PROFILE_BASE + 2),
            INVALID_PROFILE_NAME = (PROFILE_BASE + 3),
            INVALID_PROFILE_TYPE = (PROFILE_BASE + 4),
            INVALID_PHY_TYPE = (PROFILE_BASE + 5),
            MSM_SECURITY_MISSING = (PROFILE_BASE + 6),
            IHV_SECURITY_NOT_SUPPORTED = (PROFILE_BASE + 7),
            IHV_OUI_MISMATCH = (PROFILE_BASE + 8),
            // IHV OUI not present but there is IHV settings in profile
            IHV_OUI_MISSING = (PROFILE_BASE + 9),
            // IHV OUI is present but there is no IHV settings in profile
            IHV_SETTINGS_MISSING = (PROFILE_BASE + 10),
            // both/conflict MSMSec and IHV security settings exist in profile 
            CONFLICT_SECURITY = (PROFILE_BASE + 11),
            // no IHV or MSMSec security settings in profile
            SECURITY_MISSING = (PROFILE_BASE + 12),
            INVALID_BSS_TYPE = (PROFILE_BASE + 13),
            INVALID_ADHOC_CONNECTION_MODE = (PROFILE_BASE + 14),
            NON_BROADCAST_SET_FOR_ADHOC = (PROFILE_BASE + 15),
            AUTO_SWITCH_SET_FOR_ADHOC = (PROFILE_BASE + 16),
            AUTO_SWITCH_SET_FOR_MANUAL_CONNECTION = (PROFILE_BASE + 17),
            IHV_SECURITY_ONEX_MISSING = (PROFILE_BASE + 18),
            PROFILE_SSID_INVALID = (PROFILE_BASE + 19),
            TOO_MANY_SSID = (PROFILE_BASE + 20),

            // MSM network incompatible reasons
            //
            UNSUPPORTED_SECURITY_SET_BY_OS = (MSM_BASE + 1),
            UNSUPPORTED_SECURITY_SET = (MSM_BASE + 2),
            BSS_TYPE_UNMATCH = (MSM_BASE + 3),
            PHY_TYPE_UNMATCH = (MSM_BASE + 4),
            DATARATE_UNMATCH = (MSM_BASE + 5),

            // MSM connection failure reasons, to be defined
            // failure reason codes
            //
            // user called to disconnect
            USER_CANCELLED = (MSM_CONNECT_BASE + 1),
            // got disconnect while associating
            ASSOCIATION_FAILURE = (MSM_CONNECT_BASE + 2),
            // timeout for association
            ASSOCIATION_TIMEOUT = (MSM_CONNECT_BASE + 3),
            // pre-association security completed with failure
            PRE_SECURITY_FAILURE = (MSM_CONNECT_BASE + 4),
            // fail to start post-association security
            START_SECURITY_FAILURE = (MSM_CONNECT_BASE + 5),
            // post-association security completed with failure
            SECURITY_FAILURE = (MSM_CONNECT_BASE + 6),
            // security watchdog timeout
            SECURITY_TIMEOUT = (MSM_CONNECT_BASE + 7),
            // got disconnect from driver when roaming
            ROAMING_FAILURE = (MSM_CONNECT_BASE + 8),
            // failed to start security for roaming
            ROAMING_SECURITY_FAILURE = (MSM_CONNECT_BASE + 9),
            // failed to start security for adhoc-join
            ADHOC_SECURITY_FAILURE = (MSM_CONNECT_BASE + 10),
            // got disconnection from driver
            DRIVER_DISCONNECTED = (MSM_CONNECT_BASE + 11),
            // driver operation failed
            DRIVER_OPERATION_FAILURE = (MSM_CONNECT_BASE + 12),
            // Ihv service is not available
            IHV_NOT_AVAILABLE = (MSM_CONNECT_BASE + 13),
            // Response from ihv timed out
            IHV_NOT_RESPONDING = (MSM_CONNECT_BASE + 14),
            // Timed out waiting for driver to disconnect
            DISCONNECT_TIMEOUT = (MSM_CONNECT_BASE + 15),
            // An internal error prevented the operation from being completed.
            INTERNAL_FAILURE = (MSM_CONNECT_BASE + 16),
            // UI Request timed out.
            UI_REQUEST_TIMEOUT = (MSM_CONNECT_BASE + 17),
            // Roaming too often, post security is not completed after 5 times.
            TOO_MANY_SECURITY_ATTEMPTS = (MSM_CONNECT_BASE + 18),

            // MSMSEC reason codes
            //

            MSMSEC_MIN = MSMSEC_BASE,

            // Key index specified is not valid
            MSMSEC_PROFILE_INVALID_KEY_INDEX = (MSMSEC_BASE + 1),
            // Key required, PSK present
            MSMSEC_PROFILE_PSK_PRESENT = (MSMSEC_BASE + 2),
            // Invalid key length
            MSMSEC_PROFILE_KEY_LENGTH = (MSMSEC_BASE + 3),
            // Invalid PSK length
            MSMSEC_PROFILE_PSK_LENGTH = (MSMSEC_BASE + 4),
            // No auth/cipher specified
            MSMSEC_PROFILE_NO_AUTH_CIPHER_SPECIFIED = (MSMSEC_BASE + 5),
            // Too many auth/cipher specified
            MSMSEC_PROFILE_TOO_MANY_AUTH_CIPHER_SPECIFIED = (MSMSEC_BASE + 6),
            // Profile contains duplicate auth/cipher
            MSMSEC_PROFILE_DUPLICATE_AUTH_CIPHER = (MSMSEC_BASE + 7),
            // Profile raw data is invalid (1x or key data)
            MSMSEC_PROFILE_RAWDATA_INVALID = (MSMSEC_BASE + 8),
            // Invalid auth/cipher combination
            MSMSEC_PROFILE_INVALID_AUTH_CIPHER = (MSMSEC_BASE + 9),
            // 802.1x disabled when it's required to be enabled
            MSMSEC_PROFILE_ONEX_DISABLED = (MSMSEC_BASE + 10),
            // 802.1x enabled when it's required to be disabled
            MSMSEC_PROFILE_ONEX_ENABLED = (MSMSEC_BASE + 11),
            MSMSEC_PROFILE_INVALID_PMKCACHE_MODE = (MSMSEC_BASE + 12),
            MSMSEC_PROFILE_INVALID_PMKCACHE_SIZE = (MSMSEC_BASE + 13),
            MSMSEC_PROFILE_INVALID_PMKCACHE_TTL = (MSMSEC_BASE + 14),
            MSMSEC_PROFILE_INVALID_PREAUTH_MODE = (MSMSEC_BASE + 15),
            MSMSEC_PROFILE_INVALID_PREAUTH_THROTTLE = (MSMSEC_BASE + 16),
            // PreAuth enabled when PMK cache is disabled
            MSMSEC_PROFILE_PREAUTH_ONLY_ENABLED = (MSMSEC_BASE + 17),
            // Capability matching failed at network
            MSMSEC_CAPABILITY_NETWORK = (MSMSEC_BASE + 18),
            // Capability matching failed at NIC
            MSMSEC_CAPABILITY_NIC = (MSMSEC_BASE + 19),
            // Capability matching failed at profile
            MSMSEC_CAPABILITY_PROFILE = (MSMSEC_BASE + 20),
            // Network does not support specified discovery type
            MSMSEC_CAPABILITY_DISCOVERY = (MSMSEC_BASE + 21),
            // Passphrase contains invalid character
            MSMSEC_PROFILE_PASSPHRASE_CHAR = (MSMSEC_BASE + 22),
            // Key material contains invalid character
            MSMSEC_PROFILE_KEYMATERIAL_CHAR = (MSMSEC_BASE + 23),
            // Wrong key type specified for the auth/cipher pair
            MSMSEC_PROFILE_WRONG_KEYTYPE = (MSMSEC_BASE + 24),
            // "Mixed cell" suspected (AP not beaconing privacy, we have privacy enabled profile)
            MSMSEC_MIXED_CELL = (MSMSEC_BASE + 25),
            // Auth timers or number of timeouts in profile is incorrect
            MSMSEC_PROFILE_AUTH_TIMERS_INVALID = (MSMSEC_BASE + 26),
            // Group key update interval in profile is incorrect
            MSMSEC_PROFILE_INVALID_GKEY_INTV = (MSMSEC_BASE + 27),
            // "Transition network" suspected, trying legacy 802.11 security
            MSMSEC_TRANSITION_NETWORK = (MSMSEC_BASE + 28),
            // Key contains characters which do not map to ASCII
            MSMSEC_PROFILE_KEY_UNMAPPED_CHAR = (MSMSEC_BASE + 29),
            // Capability matching failed at profile (auth not found)
            MSMSEC_CAPABILITY_PROFILE_AUTH = (MSMSEC_BASE + 30),
            // Capability matching failed at profile (cipher not found)
            MSMSEC_CAPABILITY_PROFILE_CIPHER = (MSMSEC_BASE + 31),

            // Failed to queue UI request
            MSMSEC_UI_REQUEST_FAILURE = (MSMSEC_CONNECT_BASE + 1),
            // 802.1x authentication did not start within configured time 
            MSMSEC_AUTH_START_TIMEOUT = (MSMSEC_CONNECT_BASE + 2),
            // 802.1x authentication did not complete within configured time
            MSMSEC_AUTH_SUCCESS_TIMEOUT = (MSMSEC_CONNECT_BASE + 3),
            // Dynamic key exchange did not start within configured time
            MSMSEC_KEY_START_TIMEOUT = (MSMSEC_CONNECT_BASE + 4),
            // Dynamic key exchange did not succeed within configured time
            MSMSEC_KEY_SUCCESS_TIMEOUT = (MSMSEC_CONNECT_BASE + 5),
            // Message 3 of 4 way handshake has no key data (RSN/WPA)
            MSMSEC_M3_MISSING_KEY_DATA = (MSMSEC_CONNECT_BASE + 6),
            // Message 3 of 4 way handshake has no IE (RSN/WPA)
            MSMSEC_M3_MISSING_IE = (MSMSEC_CONNECT_BASE + 7),
            // Message 3 of 4 way handshake has no Group Key (RSN)
            MSMSEC_M3_MISSING_GRP_KEY = (MSMSEC_CONNECT_BASE + 8),
            // Matching security capabilities of IE in M3 failed (RSN/WPA)
            MSMSEC_PR_IE_MATCHING = (MSMSEC_CONNECT_BASE + 9),
            // Matching security capabilities of Secondary IE in M3 failed (RSN)
            MSMSEC_SEC_IE_MATCHING = (MSMSEC_CONNECT_BASE + 10),
            // Required a pairwise key but AP configured only group keys
            MSMSEC_NO_PAIRWISE_KEY = (MSMSEC_CONNECT_BASE + 11),
            // Message 1 of group key handshake has no key data (RSN/WPA)
            MSMSEC_G1_MISSING_KEY_DATA = (MSMSEC_CONNECT_BASE + 12),
            // Message 1 of group key handshake has no group key
            MSMSEC_G1_MISSING_GRP_KEY = (MSMSEC_CONNECT_BASE + 13),
            // AP reset secure bit after connection was secured
            MSMSEC_PEER_INDICATED_INSECURE = (MSMSEC_CONNECT_BASE + 14),
            // 802.1x indicated there is no authenticator but profile requires 802.1x
            MSMSEC_NO_AUTHENTICATOR = (MSMSEC_CONNECT_BASE + 15),
            // Plumbing settings to NIC failed
            MSMSEC_NIC_FAILURE = (MSMSEC_CONNECT_BASE + 16),
            // Operation was cancelled by caller
            MSMSEC_CANCELLED = (MSMSEC_CONNECT_BASE + 17),
            // Key was in incorrect format 
            MSMSEC_KEY_FORMAT = (MSMSEC_CONNECT_BASE + 18),
            // Security downgrade detected
            MSMSEC_DOWNGRADE_DETECTED = (MSMSEC_CONNECT_BASE + 19),
            // PSK mismatch suspected
            MSMSEC_PSK_MISMATCH_SUSPECTED = (MSMSEC_CONNECT_BASE + 20),
            // Forced failure because connection method was not secure
            MSMSEC_FORCED_FAILURE = (MSMSEC_CONNECT_BASE + 21),
            // ui request couldn't be queued or user pressed cancel
            MSMSEC_SECURITY_UI_FAILURE = (MSMSEC_CONNECT_BASE + 22),

            MSMSEC_MAX = MSMSEC_END
        }

        #endregion

        public enum DOT11_AUTH_ALGORITHM : uint
        {
            DOT11_AUTH_ALGO_80211_OPEN = 1,
            DOT11_AUTH_ALGO_80211_SHARED_KEY = 2,
            DOT11_AUTH_ALGO_IHV_END = 0xffffffff,
            DOT11_AUTH_ALGO_IHV_START = 0x80000000,
            DOT11_AUTH_ALGO_RSNA = 6,
            DOT11_AUTH_ALGO_RSNA_PSK = 7,
            DOT11_AUTH_ALGO_WPA = 3,
            DOT11_AUTH_ALGO_WPA_NONE = 5,
            DOT11_AUTH_ALGO_WPA_PSK = 4
        }

        public enum DOT11_BSS_TYPE
        {
            dot11_BSS_type_any = 3,
            dot11_BSS_type_independent = 2,
            dot11_BSS_type_infrastructure = 1
        }

        public enum DOT11_CIPHER_ALGORITHM : uint
        {
            DOT11_CIPHER_ALGO_CCMP = 4,
            DOT11_CIPHER_ALGO_IHV_END = 0xffffffff,
            DOT11_CIPHER_ALGO_IHV_START = 0x80000000,
            DOT11_CIPHER_ALGO_NONE = 0,
            DOT11_CIPHER_ALGO_RSN_USE_GROUP = 0x100,
            DOT11_CIPHER_ALGO_TKIP = 2,
            DOT11_CIPHER_ALGO_WEP = 0x101,
            DOT11_CIPHER_ALGO_WEP104 = 5,
            DOT11_CIPHER_ALGO_WEP40 = 1,
            DOT11_CIPHER_ALGO_WPA_USE_GROUP = 0x100
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct DOT11_MAC_ADDRESS
        {
            [MarshalAs(UnmanagedType.LPWStr), FieldOffset(0)]
            public char[] ucDot11MacAddress;

            public DOT11_MAC_ADDRESS(IntPtr pMacAddress)
            {
                this.ucDot11MacAddress = new char[6];
                for (int index = 0; index < 6; index++)
                {
                    this.ucDot11MacAddress[index] = (char)Marshal.ReadByte(pMacAddress, index);
                }
            }
        }

        public enum DOT11_PHY_TYPE : uint
        {
            dot11_phy_type_any = 0,
            dot11_phy_type_dsss = 2,
            dot11_phy_type_erp = 6,
            dot11_phy_type_fhss = 1,
            dot11_phy_type_hrdsss = 5,
            dot11_phy_type_IHV_end = 0xffffffff,
            dot11_phy_type_IHV_start = 0x80000000,
            dot11_phy_type_irbaseband = 3,
            dot11_phy_type_ofdm = 4,
            dot11_phy_type_unknown = 0
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct DOT11_SSID
        {
            /// <summary>
            /// 服务集标识SSID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20), FieldOffset(4)]
            public char[] ucSSID;
            /// <summary>
            /// SSID长度
            /// </summary>
            [FieldOffset(0)]
            public uint uSSIDLength;

            public DOT11_SSID(IntPtr pSSID)
            {
                this.uSSIDLength = (uint)Marshal.ReadInt32(pSSID, 0);
                this.ucSSID = Marshal.PtrToStringAnsi((IntPtr)((uint)pSSID + 4), (int)this.uSSIDLength).ToCharArray();
            }
        }

        public enum Wifi_Signal_Quality
        {
            /// <summary>
            /// 无信号
            /// </summary>
            No_Signal,
            /// <summary>
            /// 非常低
            /// </summary>
            Very_Low,
            /// <summary>
            /// 低
            /// </summary>
            Low,
            /// <summary>
            /// 好
            /// </summary>
            Good,
            /// <summary>
            /// 很好
            /// </summary>
            Very_Good,
            /// <summary>
            /// 非常好
            /// </summary>
            Excellent
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct WLAN_ASSOCIATION_ATTRIBUTES
        {
            [FieldOffset(40)]
            public Wireless.DOT11_MAC_ADDRESS dot11Bssid;
            [FieldOffset(0x24)]
            public Wireless.DOT11_BSS_TYPE dot11BssType;
            [FieldOffset(0x30)]
            public Wireless.DOT11_PHY_TYPE dot11PhyType;
            [FieldOffset(0)]
            public Wireless.DOT11_SSID dot11Ssid;
            [FieldOffset(0x34)]
            public uint uDot11PhyIndex;
            [FieldOffset(60)]
            public uint ulRxRate;
            [FieldOffset(0x40)]
            public uint ulTxRate;
            [FieldOffset(0x38)]
            public uint wlanSignalQuality;

            public WLAN_ASSOCIATION_ATTRIBUTES(IntPtr pAssociationData)
            {
                this.dot11Ssid = new Wireless.DOT11_SSID(new IntPtr(pAssociationData.ToInt32()));
                this.dot11BssType = (Wireless.DOT11_BSS_TYPE)Marshal.ReadInt32(pAssociationData, 0x24);
                this.dot11Bssid = new Wireless.DOT11_MAC_ADDRESS(new IntPtr(pAssociationData.ToInt32() + 40));
                this.dot11PhyType = (Wireless.DOT11_PHY_TYPE)Marshal.ReadInt32(pAssociationData, 0x30);
                this.uDot11PhyIndex = (uint)Marshal.ReadInt32(pAssociationData, 0x34);
                this.wlanSignalQuality = (uint)Marshal.ReadInt64(pAssociationData, 0x38);
                this.ulRxRate = (uint)Marshal.ReadInt32(pAssociationData, 60);
                this.ulTxRate = (uint)Marshal.ReadInt32(pAssociationData, 0x40);
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct WLAN_CONNECTION_ATTRIBUTES
        {
            [FieldOffset(0)]
            public Wireless.WLAN_INTERFACE_STATE isState;
            [MarshalAs(UnmanagedType.LPWStr), FieldOffset(8)]
            public string strProfileName;
            [FieldOffset(520)]
            public Wireless.WLAN_ASSOCIATION_ATTRIBUTES wlanAssociationAttributes;
            [FieldOffset(4)]
            public Wireless.WLAN_CONNECTION_MODE wlanConnectionMode;
            [FieldOffset(0x24c)]
            public Wireless.WLAN_SECURITY_ATTRIBUTES wlanSecurityAttributes;

            public WLAN_CONNECTION_ATTRIBUTES(IntPtr pData)
            {
                this.isState = (Wireless.WLAN_INTERFACE_STATE)Marshal.ReadInt32(pData, 0);
                this.wlanConnectionMode = (Wireless.WLAN_CONNECTION_MODE)Marshal.ReadInt32(pData, 4);
                this.strProfileName = Marshal.PtrToStringUni(new IntPtr(pData.ToInt32() + 8), 0x100).Replace("\0", "");
                this.wlanAssociationAttributes = new Wireless.WLAN_ASSOCIATION_ATTRIBUTES(new IntPtr(pData.ToInt32() + 520));
                this.wlanSecurityAttributes = new Wireless.WLAN_SECURITY_ATTRIBUTES(new IntPtr(pData.ToInt32() + 0x24c));
            }
        }

        public enum WLAN_CONNECTION_MODE : uint
        {
            WLAN_CONNECTION_MODE_AUTO = 4,
            WLAN_CONNECTION_MODE_DISCOVERY_SECURE = 2,
            WLAN_CONNECTION_MODE_DISCOVERY_UNSECURE = 3,
            WLAN_CONNECTION_MODE_INVALID = 5,
            WLAN_CONNECTION_MODE_PROFILE = 0,
            WLAN_CONNECTION_MODE_TEMPORARY_PROFILE = 1
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WLAN_CONNECTION_PARAMETERS
        {
            public Wireless.WLAN_CONNECTION_MODE wlanConnectionMode;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string profilename;
            public IntPtr Dot11Ssid;
            public IntPtr desiredBssidList;
            public Wireless.DOT11_BSS_TYPE dot1BssType;
            public uint dwFlag;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WLAN_INTERFACE_INFO
        {
            public Guid InterfaceGuid;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string strInterfaceDescription;
            public Wireless.WLAN_INTERFACE_STATE isState;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WLAN_INTERFACE_INFO_LIST
        {
            public uint dwNumberofItems;
            public uint dwIndex;
            public Wireless.WLAN_INTERFACE_INFO[] InterfaceInfo;
            public WLAN_INTERFACE_INFO_LIST(IntPtr pList)
            {
                this.dwNumberofItems = (uint)Marshal.ReadInt32(pList, 0);
                this.dwIndex = (uint)Marshal.ReadInt32(pList, 4);
                this.InterfaceInfo = new Wireless.WLAN_INTERFACE_INFO[this.dwNumberofItems];
                IntPtr pItemList = new IntPtr(pList.ToInt32() + 8);
                for (int i = 0; i < this.dwNumberofItems; i++)
                {
                    Wireless.WLAN_INTERFACE_INFO wii = new Wireless.WLAN_INTERFACE_INFO();
                    byte[] intGuid = new byte[0x10];
                    for (int j = 0; j < 0x10; j++)
                    {
                        intGuid[j] = Marshal.ReadByte(pItemList, j);
                    }
                    wii.InterfaceGuid = new Guid(intGuid);
                    wii.strInterfaceDescription = Marshal.PtrToStringUni(new IntPtr(pItemList.ToInt32() + 0x10), 0x100).Replace("\0", "");
                    wii.isState = (Wireless.WLAN_INTERFACE_STATE)Marshal.ReadInt32(pItemList, 0x110);
                    this.InterfaceInfo[i] = wii;
                    pItemList = new IntPtr(pItemList.ToInt32() + 0x214);
                }
            }
        }

        public enum WLAN_INTERFACE_STATE
        {
            wlan_interface_state_not_ready,
            wlan_interface_state_connected,
            wlan_interface_state_ad_hoc_network_formed,
            wlan_interface_state_disconnecting,
            wlan_interface_state_disconnected,
            wlan_interface_state_associating,
            wlan_interface_state_discovering,
            wlan_interface_state_authenticating
        }

        public enum WLAN_INTF_OPCODE
        {
            wlan_intf_opcode_autoconf_enabled = 1,
            wlan_intf_opcode_autoconf_end = 0xfffffff,
            wlan_intf_opcode_autoconf_start = 0,
            wlan_intf_opcode_background_scan_enabled = 2,
            wlan_intf_opcode_bss_type = 5,
            wlan_intf_opcode_channel_number = 8,
            wlan_intf_opcode_current_connection = 7,
            wlan_intf_opcode_current_operation_mode = 12,
            wlan_intf_opcode_ihv_end = 0x3fffffff,
            wlan_intf_opcode_ihv_start = 0x30000000,
            wlan_intf_opcode_interface_state = 6,
            wlan_intf_opcode_media_streaming_mode = 3,
            wlan_intf_opcode_msm_end = 0x1fffffff,
            wlan_intf_opcode_msm_start = 0x10000100,
            wlan_intf_opcode_radio_state = 4,
            wlan_intf_opcode_rssi = 0x10000102,
            wlan_intf_opcode_security_end = 0x2fffffff,
            wlan_intf_opcode_security_start = 0x20010000,
            wlan_intf_opcode_statistics = 0x10000101,
            wlan_intf_opcode_supported_adhoc_auth_cipher_pairs = 10,
            wlan_intf_opcode_supported_country_or_region_string_list = 11,
            wlan_intf_opcode_supported_infrastructure_auth_cipher_pairs = 9
        }

        public enum WLAN_OPCODE_VALUE_TYPE
        {
            wlan_opcode_value_type_query_only,
            wlan_opcode_value_type_set_by_group_policy,
            wlan_opcode_value_type_set_by_user,
            wlan_opcode_value_type_invalid
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct WLAN_SECURITY_ATTRIBUTES
        {
            [FieldOffset(4)]
            public int bOneXEnabled;
            [FieldOffset(0)]
            public int bSecurityEnabled;
            [FieldOffset(8)]
            public Wireless.DOT11_AUTH_ALGORITHM dot11AuthAlgorithm;
            [FieldOffset(12)]
            public Wireless.DOT11_CIPHER_ALGORITHM dot11CipherAlgorithm;

            public WLAN_SECURITY_ATTRIBUTES(IntPtr pSecurityAttributes)
            {
                this.bSecurityEnabled = Marshal.ReadInt32(pSecurityAttributes, 0);
                this.bOneXEnabled = Marshal.ReadInt32(pSecurityAttributes, 4);
                this.dot11AuthAlgorithm = (Wireless.DOT11_AUTH_ALGORITHM)Marshal.ReadInt32(pSecurityAttributes, 8);
                this.dot11CipherAlgorithm = (Wireless.DOT11_CIPHER_ALGORITHM)Marshal.ReadInt32(pSecurityAttributes, 12);
            }
        }
    }
}


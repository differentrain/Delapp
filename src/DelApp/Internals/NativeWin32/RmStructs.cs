using System;
using System.Runtime.InteropServices;

namespace DelApp.Internals.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RmUniqueProcess
    {
        public int ProcessId;
        public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
    }

    internal enum RmAppType
    {
        RmUnknownApp = 0,
        RmMainWindow = 1,
        RmOtherWindow = 2,
        RmService = 3,
        RmExplorer = 4,
        RmConsole = 5,
        RmCritical = 1000
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal unsafe struct RmProcessInfo
    {
        public RmUniqueProcess Process;
        //CCH_RM_MAX_APP_NAME+1
        private fixed char ApplicationName[256];
        //CCH_RM_MAX_SVC_NAME+1
        private fixed char ServiceShortName[64];
        public RmAppType ApplicationType;
        public RmAppStatus AppStatus;
        public int TSSessionId;
        public bool bRestartable;
        public static int Size => Marshal.SizeOf(typeof(RmProcessInfo));
    }
    [Flags]
    internal enum RmRebootReason
    {
        None = 0x0,
        PermissionDenied = 0x1,
        SessionMismatch = 0x2,
        CriticalProcess = 0x4,
        CriticalService = 0x8,
        DetectedSelf = 0x10
    }

    [Flags]
    internal enum RmAppStatus
    {
        RmStatusUnknown = 0x0,
        RmStatusRunning = 0x1,
        RmStatusStopped = 0x2,
        RmStatusStoppedOther = 0x4,
        RmStatusRestarted = 0x8,
        RmStatusErrorOnStop = 0x10,
        RmStatusErrorOnRestart = 0x20,
        RmStatusShutdownMasked = 0x40,
        RmStatusRestartMasked = 0x80
    }


}

﻿using System.Runtime.InteropServices;

namespace WinFormsApp1
{
    public class COM
    {

        [DllImport("oleaut32.dll", PreserveSig = false)]
        static extern void GetActiveObject(
                                               ref Guid rclsid,
                                                   IntPtr pvReserved,
           [MarshalAs(UnmanagedType.IUnknown)] out Object ppunk
         );

        [DllImport("ole32.dll")]
        static extern int CLSIDFromProgID(
            [MarshalAs(UnmanagedType.LPWStr)] string lpszProgID,
                                               out Guid pclsid
         );

        public static object GetActiveObject(string progId)
        {
            Guid clsid;
            CLSIDFromProgID(progId, out clsid);

            object obj;
            GetActiveObject(ref clsid, IntPtr.Zero, out obj);

            return obj;
        }
    }
}
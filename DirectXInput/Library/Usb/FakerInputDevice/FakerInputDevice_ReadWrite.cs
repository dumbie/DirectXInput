using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static LibraryUsb.NativeMethods_File;

namespace LibraryUsb
{
    public partial class FakerInputDevice
    {
        private byte[] ConvertToByteArray(object targetObject)
        {
            try
            {
                int arraySize = Marshal.SizeOf(targetObject);
                byte[] byteArray = new byte[arraySize];
                using AVFin marshalPtr = new AVFin(AVFinMethod.FreeMarshal, Marshal.AllocHGlobal(arraySize));
                Marshal.StructureToPtr(targetObject, marshalPtr.Get(), false);
                Marshal.Copy(marshalPtr.Get(), byteArray, 0, arraySize);
                return byteArray;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to convert object to byte array: " + ex.Message);
                return null;
            }
        }

        private byte[] MergeHeaderInputByteArray(int arraySize, byte[] firstArray, byte[] secondArray)
        {
            try
            {
                byte[] byteArray = new byte[arraySize];
                Buffer.BlockCopy(firstArray, 0, byteArray, 0, firstArray.Length);
                Buffer.BlockCopy(secondArray, 0, byteArray, firstArray.Length, secondArray.Length);
                return byteArray;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to merge input byte array: " + ex.Message);
                return null;
            }
        }

        private bool WriteBytesFile(byte[] outputBuffer)
        {
            try
            {
                if (!Connected) { return false; }
                WriteFile(FileHandle.Get(), outputBuffer, outputBuffer.Length, out int lpNumberOfBytesWritten, IntPtr.Zero);
                return lpNumberOfBytesWritten > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to write file bytes: " + ex.Message);
                return false;
            }
        }
    }
}
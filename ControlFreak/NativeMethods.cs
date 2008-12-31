using System;
using System.Runtime.InteropServices;

namespace Aspirations {
  class NativeMethods {
    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref ParaFormat lParam);

    [StructLayout(LayoutKind.Sequential)]
    public struct ParaFormat {
      public int cbSize;
      public uint dwMask;
      public short wNumbering;
      public short wReserved;
      public int dxStartIndent;
      public int dxRightIndent;
      public int dxOffset;
      public short wAlignment;
      public short cTabCount;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
      public int[] rgxTabs;

      // PARAFORMAT2 from here onwards
      public int dySpaceBefore;
      public int dySpaceAfter;
      public int dyLineSpacing;
      public short sStyle;
      public byte bLineSpacingRule;
      public byte bOutlineLevel;
      public short wShadingWeight;
      public short wShadingStyle;
      public short wNumberingStart;
      public short wNumberingStyle;
      public short wNumberingTab;
      public short wBorderSpace;
      public short wBorderWidth;
      public short wBorders;
    }

    private NativeMethods() {
    }
  }
}
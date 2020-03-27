﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    using PEF = ComponentFormat;

    public enum ComponentFormat
    {
        // 0 bits
        Empty = 0,        

        // 1 bit
        Undefined1, Alpha1,

        // 4 bits
        Undefined4, Red4, Green4, Blue4, Alpha4, // PremulAlpha4

        // 5 bits
        Undefined5, Red5, Green5, Blue5,

        // 6 bits
        Undefined6, Green6,

        // 8 bits
        Undefined8, Index8, Red8, Green8, Blue8, Alpha8, Gray8, // PremulAlpha8

        // 16 bits
        Undefined16, Index16, Gray16, Red16, Green16, Blue16, DepthMM16,

        // 32 bits
        Undefined32, Red32F, Green32F, Blue32F, Alpha32F, Gray32F,
    }

    // TODO: Rename to PixelEncoding
    [System.Diagnostics.DebuggerDisplay("{Element0}-{Element1}-{Element2}-{Element3}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public readonly struct PixelFormat : IEquatable<PixelFormat>
    {
        #region constants
        
        /// <summary>
        /// Predefined pixel formats encoded as const UInt32 values,
        /// which makes them suitable for switch blocks.
        /// </summary>
        public static class Packed
        {
            private const uint B0 = 1;
            private const uint B1 = 256;
            private const uint B2 = 256 * 256;
            private const uint B3 = 256 * 256 * 256;

            public const uint Empty = (uint)PEF.Empty;
            
            public const uint Gray8 = B0 * (uint)PEF.Gray8;
            public const uint Alpha8 = B0 * (uint)PEF.Alpha8;

            public const uint Gray16 = B0 * (uint)PEF.Gray16;
            public const uint BGR565 = B0 * (uint)PEF.Blue5 | B1 * (uint)PEF.Green6 | B2 * (uint)PEF.Red5;
            public const uint BGRA4444 = B0 * (uint)PEF.Blue4 | B1 * (uint)PEF.Green4 | B2 * (uint)PEF.Red4 | B3 * (uint)PEF.Alpha4;
            public const uint BGRA5551 = B0 * (uint)PEF.Blue5 | B1 * (uint)PEF.Green5 | B2 * (uint)PEF.Red5 | B3 * (uint)PEF.Alpha1;
            public const uint DepthMM16 = B0 * (uint)PEF.DepthMM16;

            public const uint RGB24 = B0 * (uint)PEF.Red8 | B1 * (uint)PEF.Green8 | B2 * (uint)PEF.Blue8;
            public const uint BGR24 = B0 * (uint)PEF.Blue8 | B1 * (uint)PEF.Green8 | B2 * (uint)PEF.Red8;

            public const uint RGBA32 = B0 * (uint)PEF.Red8 | B1 * (uint)PEF.Green8 | B2 * (uint)PEF.Blue8 | B3 * (uint)PEF.Alpha8;
            public const uint BGRA32 = B0 * (uint)PEF.Blue8 | B1 * (uint)PEF.Green8 | B2 * (uint)PEF.Red8 | B3 * (uint)PEF.Alpha8;
            public const uint ARGB32 = B0 * (uint)PEF.Alpha8 | B1 * (uint)PEF.Red8 | B2 * (uint)PEF.Green8 | B3 * (uint)PEF.Blue8;

            public const uint RGBA128F = B0 * (uint)PEF.Red32F | B1 * (uint)PEF.Green32F | B2 * (uint)PEF.Blue32F | B3 * (uint)PEF.Alpha32F;
        }

        /// <summary>
        /// Predefined pixel formats, mirroring <see cref="Packed"/> definitions.
        /// </summary>
        public static class Standard
        {
            public static readonly PixelFormat Empty = new PixelFormat(Packed.Empty);

            public static readonly PixelFormat Gray8 = new PixelFormat(Packed.Gray8);
            public static readonly PixelFormat Alpha8 = new PixelFormat(Packed.Alpha8);

            public static readonly PixelFormat Gray16 = new PixelFormat(Packed.Gray16);
            public static readonly PixelFormat BGR565 = new PixelFormat(Packed.BGR565);
            public static readonly PixelFormat BGRA4444 = new PixelFormat(Packed.BGRA4444);
            public static readonly PixelFormat BGRA5551 = new PixelFormat(Packed.BGRA5551);
            public static readonly PixelFormat DepthMM16 = new PixelFormat(Packed.DepthMM16);

            public static readonly PixelFormat RGB24 = new PixelFormat(Packed.RGB24);
            public static readonly PixelFormat BGR24 = new PixelFormat(Packed.BGR24);

            public static readonly PixelFormat RGBA32 = new PixelFormat(Packed.RGBA32);
            public static readonly PixelFormat BGRA32 = new PixelFormat(Packed.BGRA32);
            public static readonly PixelFormat ARGB32 = new PixelFormat(Packed.ARGB32);

            public static readonly PixelFormat RGBA128F = new PixelFormat(Packed.RGBA128F);
        }

        #endregion

        #region constructors

        public static implicit operator UInt32 (PixelFormat fmt) { return fmt.PackedFormat; }

        //public static implicit operator PixelFormat(UInt32 fmt) { return new PixelFormat(fmt); }

        public PixelFormat(UInt32 packedFormat)
        {
            _Element0 = _Element1 = _Element2 = _Element3 = 0;
            PackedFormat = packedFormat;
        }

        public PixelFormat(PEF e0)
        {
            PackedFormat = 0;
            _Element0 = (Byte)e0;
            _Element1 = _Element2 = _Element3 = (Byte)ComponentFormat.Empty;
        }

        public PixelFormat(PEF e0, PEF e1)
        {
            PackedFormat = 0;
            _Element0 = (Byte)e0;
            _Element1 = (Byte)e1;
            _Element2 = _Element3 = (Byte)ComponentFormat.Empty;
        }

        public PixelFormat(PEF e0, PEF e1, ComponentFormat e2)
        {
            PackedFormat = 0;
            _Element0 = (Byte)e0;
            _Element1 = (Byte)e1;
            _Element2 = (Byte)e2;
            _Element3 = (Byte)ComponentFormat.Empty;
        }

        public PixelFormat(PEF e0, PEF e1, PEF e2, PEF e3)
        {
            PackedFormat = 0;
            _Element0 = (Byte)e0;
            _Element1 = (Byte)e1;
            _Element2 = (Byte)e2;
            _Element3 = (Byte)e3;
        }

        public static unsafe PixelFormat GetUndefined<TPixel>() where TPixel:unmanaged
        {
            return GetUndefinedOfSize(sizeof(TPixel));
        }

        public static PixelFormat GetUndefinedOfSize(int byteCount)
        {
            switch(byteCount)
            {
                case 1: return new PixelFormat(PEF.Undefined8);
                case 2: return new PixelFormat(PEF.Undefined8, PEF.Undefined8);
                case 3: return new PixelFormat(PEF.Undefined8, PEF.Undefined8, PEF.Undefined8);
                case 4: return new PixelFormat(PEF.Undefined8, PEF.Undefined8, PEF.Undefined8, PEF.Undefined8);
                case 8: return new PixelFormat(PEF.Undefined16, PEF.Undefined16, PEF.Undefined16, PEF.Undefined16);
                case 16: return new PixelFormat(PEF.Undefined32, PEF.Undefined32, PEF.Undefined32, PEF.Undefined32);
                default:throw new NotImplementedException();
            }
        }        

        #endregion

        #region data

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly UInt32 PackedFormat;

        [System.Runtime.InteropServices.FieldOffset(0)]
        private readonly Byte _Element0;

        [System.Runtime.InteropServices.FieldOffset(1)]
        private readonly Byte _Element1;

        [System.Runtime.InteropServices.FieldOffset(2)]
        private readonly Byte _Element2;

        [System.Runtime.InteropServices.FieldOffset(3)]
        private readonly Byte _Element3;

        public override int GetHashCode() { return PackedFormat.GetHashCode(); }

        public override bool Equals(object obj) { return obj is PixelFormat other ? this.Equals(other) : false; }

        public bool Equals(PixelFormat other) { return this.PackedFormat == other.PackedFormat; }

        public static bool operator == (PixelFormat a, PixelFormat b) { return a.PackedFormat == b.PackedFormat; }

        public static bool operator != (PixelFormat a, PixelFormat b) { return a.PackedFormat != b.PackedFormat; }

        #endregion

        #region properties

        public PEF Element0 => (PEF)_Element0;

        public PEF Element1 => (PEF)_Element1;

        public PEF Element2 => (PEF)_Element2;

        public PEF Element3 => (PEF)_Element3;

        public int ByteCount => _GetByteLength();

        /// <summary>
        /// Gets the pixel format used by the Odd scanlines.
        /// </summary>
        /// <remarks>
        /// Some formats use a byte pattern where odd and even scanlines reverse the byte ordering.
        /// </remarks>
        public PixelFormat ScanlineOddFormat
        {
            get => this;
        }

        public IEnumerable<PEF> Elements
        {
            get
            {
                yield return Element0;
                yield return Element1;
                yield return Element2;
                yield return Element3;
            }
        }

        #endregion

        #region API

        private int _GetByteLength()
        {
            int c = 0;
            c += _GetBitLen(Element0);
            c += _GetBitLen(Element1);
            c += _GetBitLen(Element2);
            c += _GetBitLen(Element3);

            if (c == 0) throw new InvalidOperationException("Format must not have a zero length");
            if ((c & 7) != 0) throw new InvalidOperationException("Format must have a length multiple of 8");

            return c / 8;
        }

        internal static int _GetBitLen(PEF pef)
        {
            // an alternative to the switch is to have a lookup table

            switch(pef)
            {
                case PEF.Empty: return 0;

                case PEF.Alpha1:
                case PEF.Undefined1: return 1;

                case PEF.Red4:
                case PEF.Green4:
                case PEF.Blue4:
                case PEF.Alpha4:
                case PEF.Undefined4: return 4;

                case PEF.Red5:
                case PEF.Green5:
                case PEF.Blue5:
                case PEF.Undefined5: return 5;

                case PEF.Green6:
                case PEF.Undefined6: return 6;

                case PEF.Index8:
                case PEF.Alpha8:
                case PEF.Gray8:
                case PEF.Red8:
                case PEF.Green8:
                case PEF.Blue8:
                case PEF.Undefined8: return 8;

                case PEF.Index16:
                case PEF.Gray16:
                case PEF.Red16:
                case PEF.Green16:
                case PEF.Blue16:
                case PEF.DepthMM16:
                case PEF.Undefined16: return 16;

                case PEF.Gray32F:
                case PEF.Red32F:
                case PEF.Green32F:
                case PEF.Blue32F:
                case PEF.Alpha32F:
                case PEF.Undefined32: return 32;

                default: throw new NotImplementedException($"Not implemented:{pef}");
            }
        }

        private int _FindIndex(PEF pef)
        {
            if (Element0 == pef) return 0;
            if (Element1 == pef) return 1;
            if (Element2 == pef) return 2;
            if (Element3 == pef) return 3;
            return -1;
        }

        private PEF _GetComponentAt(int byteIndex)
        {
            switch(byteIndex)
            {
                case 0: return Element0;
                case 1: return Element1;
                case 2: return Element2;
                case 3: return Element3;
                default: return PEF.Empty;
            }
        }

        #endregion

        #region static

        public static bool IsAlpha(PEF pef)
        {
            switch (pef)
            {
                case PEF.Alpha1: return true;
                case PEF.Alpha4: return true;
                case PEF.Alpha8: return true;
                case PEF.Alpha32F: return true;
                default: return false;
            }
        }

        public static bool IsGrey(PEF pef)
        {
            switch(pef)
            {
                case PEF.Gray8: return true;
                case PEF.Gray16: return true;
                case PEF.Gray32F: return true;
                default: return false;
            }
        }

        public static void Convert(SpanBitmap dst, SpanBitmap src)
        {
            Guard.AreEqual(nameof(src), dst.Width, src.Width);
            Guard.AreEqual(nameof(src), dst.Height, src.Height);            

            var byteIndices = new int[dst.PixelSize];

            for (int i = 0; i < byteIndices.Length; ++i)
            {
                var c = dst.PixelFormat._GetComponentAt(i);
                var idx = src.PixelFormat._FindIndex(c);
                if (idx < 0) throw new ArgumentException(nameof(src));
                byteIndices[i] = idx;
            }            

            for (int y = 0; y < dst.Height; ++y)
            {
                var dstRow = dst.UseBytesScanline(y);
                var srcRow = src.GetBytesScanline(y);

                for(int x=0; x < dst.Width; ++x)
                {
                    for(int z=0; z < byteIndices.Length; ++z)
                    {
                        var idx = byteIndices[z];
                        dstRow[z] = srcRow[idx];
                    }

                    dstRow = dstRow.Slice(dst.PixelSize);
                    srcRow = srcRow.Slice(src.PixelSize);

                    throw new NotImplementedException();
                }
            }
        }

        #endregion
    }

    
}

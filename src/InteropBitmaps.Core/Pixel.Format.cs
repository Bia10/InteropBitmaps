﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;

namespace InteropBitmaps
{
    using PEF = Pixel.ElementID;

    partial class Pixel
    {
        // TODO: Rename to PixelEncoding
        [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplay(),nq}")]
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
        public readonly partial struct Format : IEquatable<Format>
        {
            #region debug

            internal string _GetDebuggerDisplay()
            {
                switch (this.PackedFormat)
                {
                    case Packed.Alpha8: return "A8";

                    case Packed.Gray8: return "Gray8";
                    case Packed.Gray16: return "Gray16";

                    case Packed.RGB24: return "RGB24";
                    case Packed.BGR24: return "BGR24";

                    case Packed.ARGB32: return "ARGB32";
                    case Packed.RGBA32: return "RGBA32";
                    case Packed.BGRA32: return "BGRA32";
                }

                var elements = Elements
                    .Where(item => item.Id != PEF.Empty)
                    .Select(item => item.Id);

                return string.Join("-", elements);

            }

            #endregion            

            #region constructors

            public static implicit operator UInt32(Format fmt) { return fmt.PackedFormat; }

            //public static implicit operator PixelFormat(UInt32 fmt) { return new PixelFormat(fmt); }

            public Format(UInt32 packedFormat)
            {
                _Element0 = _Element1 = _Element2 = _Element3 = 0;
                PackedFormat = packedFormat;
            }

            public Format(PEF e0)
            {
                PackedFormat = 0;
                _Element0 = (Byte)e0;
                _Element1 = _Element2 = _Element3 = (Byte)PEF.Empty;

                _Validate();
            }

            public Format(PEF e0, PEF e1)
            {
                PackedFormat = 0;
                _Element0 = (Byte)e0;
                _Element1 = (Byte)e1;
                _Element2 = _Element3 = (Byte)PEF.Empty;

                _Validate();
            }

            public Format(PEF e0, PEF e1, PEF e2)
            {
                PackedFormat = 0;
                _Element0 = (Byte)e0;
                _Element1 = (Byte)e1;
                _Element2 = (Byte)e2;
                _Element3 = (Byte)PEF.Empty;

                _Validate();
            }

            public Format(PEF e0, PEF e1, PEF e2, PEF e3)
            {
                PackedFormat = 0;
                _Element0 = (Byte)e0;
                _Element1 = (Byte)e1;
                _Element2 = (Byte)e2;
                _Element3 = (Byte)e3;

                _Validate();
            }

            private void _Validate()
            {
                int l = 0;
                l += Element0.BitCount;
                l += Element1.BitCount;
                l += Element2.BitCount;
                l += Element3.BitCount;

                if (l == 0) throw new InvalidOperationException("Format must not have a zero length");
                if ((l & 7) != 0) throw new InvalidOperationException("Format must have a length multiple of 8");
            }

            public static unsafe Format GetUndefined<TPixel>() where TPixel : unmanaged
            {
                var tp = typeof(TPixel);

                if (tp == typeof(float)) return new Format(PEF.Undefined32F);
                if (tp == typeof(Vector2)) return new Format(PEF.Undefined32F, PEF.Undefined32F);
                if (tp == typeof(Vector3)) return new Format(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);
                if (tp == typeof(Vector4)) return new Format(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);

                return GetUndefinedOfSize(sizeof(TPixel));
            }

            public static Format GetUndefinedOfSize(int byteCount)
            {
                switch (byteCount)
                {
                    case 1: return new Format(PEF.Undefined8);
                    case 2: return new Format(PEF.Undefined8, PEF.Undefined8);
                    case 3: return new Format(PEF.Undefined8, PEF.Undefined8, PEF.Undefined8);
                    case 4: return new Format(PEF.Undefined8, PEF.Undefined8, PEF.Undefined8, PEF.Undefined8);
                    case 8: return new Format(PEF.Undefined16, PEF.Undefined16, PEF.Undefined16, PEF.Undefined16);
                    case 12: return new Format(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);
                    case 16: return new Format(PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F, PEF.Undefined32F);
                    default: throw new NotImplementedException();
                }
            }

            public static Format GetFromDepthAndChannels(Type depth, int channels)
            {
                if (depth == typeof(Byte))
                {
                    if (channels == 1) return Standard.Gray8;
                    if (channels == 3) return Standard.BGR24;
                    if (channels == 4) return Standard.BGRA32;
                }

                if (depth == typeof(UInt16))
                {
                    if (channels == 1) return Standard.Gray16;
                    if (channels == 3) return new Format(PEF.Blue16, PEF.Green16, PEF.Red16);
                    if (channels == 4) return new Format(PEF.Blue16, PEF.Green16, PEF.Red16, PEF.Alpha16);
                }

                if (depth == typeof(Single))
                {
                    if (channels == 1) return Standard.Gray32F;
                    if (channels == 3) return Standard.BGR96F;
                    if (channels == 4) return Standard.BGRA128F;
                }

                throw new NotImplementedException();
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

            public override bool Equals(object obj) { return obj is Format other && this.Equals(other); }

            public bool Equals(Format other) { return this.PackedFormat == other.PackedFormat; }

            public static bool operator ==(Format a, Format b) { return a.PackedFormat == b.PackedFormat; }

            public static bool operator !=(Format a, Format b) { return a.PackedFormat != b.PackedFormat; }

            #endregion

            #region properties

            public Element Element0 => new Element(_Element0);

            public Element Element1 => new Element(_Element1);

            public Element Element2 => new Element(_Element2);

            public Element Element3 => new Element(_Element3);

            public int ByteCount => _GetByteLength();

            /// <summary>
            /// Gets the pixel format used by the Odd scanlines.
            /// </summary>
            /// <remarks>
            /// Some formats use a byte pattern where odd and even scanlines reverse the byte ordering.
            /// </remarks>
            public Format ScanlineOddFormat
            {
                get => this;
            }

            public IEnumerable<Element> Elements
            {
                get
                {
                    yield return Element0;
                    yield return Element1;
                    yield return Element2;
                    yield return Element3;
                }
            }

            public int MaxElementBitLength
            {
                get
                {
                    var l = Element0.BitCount;
                    l = Math.Max(l, Element1.BitCount);
                    l = Math.Max(l, Element2.BitCount);
                    l = Math.Max(l, Element3.BitCount);
                    return l;
                }
            }

            #endregion

            #region API

            private int _GetByteLength()
            {
                int l = 0;
                l += Element0.BitCount;
                l += Element1.BitCount;
                l += Element2.BitCount;
                l += Element3.BitCount;
                return l / 8;
            }

            private int _FindIndex(PEF pef)
            {
                if (Element0.Id == pef) return 0;
                if (Element1.Id == pef) return 1;
                if (Element2.Id == pef) return 2;
                if (Element3.Id == pef) return 3;
                return -1;
            }

            private Element _GetComponentAt(int byteIndex)
            {
                switch (byteIndex)
                {
                    case 0: return Element0;
                    case 1: return Element1;
                    case 2: return Element2;
                    case 3: return Element3;
                    default: return PEF.Empty;
                }
            }

            public Type GetDepthType()
            {
                var e0Len = Element0.BitCount;
                var e1Len = Element1.BitCount; if (e1Len != 0 && e1Len != e0Len) return null;
                var e2Len = Element2.BitCount; if (e2Len != 0 && e2Len != e0Len) return null;
                var e3Len = Element3.BitCount; if (e3Len != 0 && e3Len != e0Len) return null;

                if (e0Len == 8) return typeof(Byte);
                if (e0Len == 16) return typeof(UInt16);
                if (e0Len == 32) return typeof(Single);

                return null;
            }

            public (Type Depth, int Channels) GetDepthTypeAndChannels()
            {
                int ch = 1;

                var len = Element0.BitCount;
                var next = Element1.BitCount;
                if (next != 0) { if (next == len) ++ch; else return (null, 0); }

                next = Element2.BitCount;
                if (next != 0) { if (next == len) ++ch; else return (null, 0); }

                next = Element3.BitCount;
                if (next != 0) { if (next == len) ++ch; else return (null, 0); }

                if (len == 8) return (typeof(Byte), ch);
                if (len == 16) return (typeof(UInt16), ch);
                if (len == 32) return (typeof(Single), ch);

                return (null, 0);
            }

            #endregion

            #region static        

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

                    for (int x = 0; x < dst.Width; ++x)
                    {
                        for (int z = 0; z < byteIndices.Length; ++z)
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

    
}

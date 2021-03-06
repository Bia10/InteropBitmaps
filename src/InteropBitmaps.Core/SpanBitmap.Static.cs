﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropBitmaps
{
    partial struct SpanBitmap
    {
        public static (Single Min, Single Max) MinMax(SpanBitmap<float> src)
        {
            var min = float.PositiveInfinity;
            var max = float.NegativeInfinity;            

            for (int y = 0; y < src.Height; ++y)
            {
                var srcRow = src.GetPixelsScanline(y);
                
                var (rMin, rMax) = _SpanFloatOps.MinMax(srcRow);

                if (min > rMin) min = rMin;
                if (max < rMax) max = rMax;
            }

            return (min, max);
        }

        public static (Vector3 Min, Vector3 Max) MinMax(SpanBitmap<Vector3> src)
        {
            var min = new Vector3(float.PositiveInfinity);
            var max = new Vector3(float.NegativeInfinity);

            for (int y = 0; y < src.Height; ++y)
            {
                var srcRow = src.GetPixelsScanline(y);

                for(int x=0; x < srcRow.Length; ++x)
                {
                    var p = srcRow[x];
                    min = Vector3.Min(min, p);
                    max = Vector3.Max(max, p);
                }
            }

            return (min, max);
        }

        public static (Vector4 Min, Vector4 Max) MinMax(SpanBitmap<Vector4> src)
        {
            var min = new Vector4(float.PositiveInfinity);
            var max = new Vector4(float.NegativeInfinity);

            for (int y = 0; y < src.Height; ++y)
            {
                var srcRow = src.GetPixelsScanline(y);

                for (int x = 0; x < srcRow.Length; ++x)
                {
                    var p = srcRow[x];
                    min = Vector4.Min(min, p);
                    max = Vector4.Max(max, p);
                }
            }

            return (min, max);
        }

        public static void CopyPixels(SpanBitmap<Byte> src, SpanBitmap<Single> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            System.Diagnostics.Debug.Assert(dst.Width == src.Width);
            System.Diagnostics.Debug.Assert(dst.Height == src.Height);            

            for(int y=0; y < dst.Height; ++y)
            {
                var srcRow = src.GetPixelsScanline(y);
                var dstRow = dst.UsePixelsScanline(y);
                _SpanFloatOps.CopyPixels(srcRow, dstRow, transform, range);
            }
        }

        public static void CopyPixels(SpanBitmap<Single> src, SpanBitmap<Byte> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            for (int y = 0; y < dst.Height; ++y)
            {
                var srcRow = src.GetPixelsScanline(y);
                var dstRow = dst.UsePixelsScanline(y);
                _SpanFloatOps.CopyPixels(srcRow, dstRow, transform, range);
            }
        }

        public static void CopyPixels(SpanBitmap src, SpanBitmap<Vector3> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            if (src.PixelSize == 3)
            {
                Guard.AreTheSame(nameof(dst.Info.PixelFormat), dst.Info.PixelFormat.GetDepthType(), typeof(Byte));

                for (int y = 0; y < dst.Height; ++y)
                {
                    var srcRow = src.GetBytesScanline(y);
                    var dstRow = dst.UsePixelsScanline(y);
                    
                    var dstFFF = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(dstRow);

                    _SpanFloatOps.CopyPixels(srcRow, dstFFF, transform, range);
                }

                return;
            }

            throw new NotImplementedException();
        }

        public static void CopyPixels(SpanBitmap<Vector3> src, SpanBitmap dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst.Info.Bounds), dst.Info.Bounds, src.Info.Bounds);

            if (dst.PixelSize == 3)
            {
                Guard.AreTheSame(nameof(dst.Info.PixelFormat), dst.Info.PixelFormat.GetDepthType(), typeof(Byte));

                if (range.min == 0 && range.max == 255)
                {
                    for (int y = 0; y < dst.Height; ++y)
                    {
                        var srcRow = src.GetPixelsScanline(y);
                        var dstRow = dst.UseBytesScanline(y);

                        var srcFFF = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(srcRow);

                        _SpanFloatOps.CopyPixels(srcFFF, dstRow, transform);
                    }
                }
                else
                {
                    for (int y = 0; y < dst.Height; ++y)
                    {
                        var srcRow = src.GetPixelsScanline(y);
                        var dstRow = dst.UseBytesScanline(y);

                        var srcFFF = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(srcRow);

                        _SpanFloatOps.CopyPixels(srcFFF, dstRow, transform, range);
                    }
                }

                return;
            }

            throw new NotImplementedException();
        }          

        public static void SplitPixels(SpanBitmap<Vector3> src, SpanBitmap<Single> dstB, SpanBitmap<Single> dstG, SpanBitmap<Single> dstR)
        {
            if (src.PixelFormat.Element0.IsBlue)
            {
                for (int y = 0; y < src.Height; ++y)
                {
                    var srcRow = src.GetPixelsScanline(y);
                    var dstRowX = dstB.UsePixelsScanline(y);
                    var dstRowY = dstG.UsePixelsScanline(y);
                    var dstRowZ = dstR.UsePixelsScanline(y);

                    for (int x = 0; x < srcRow.Length; ++x)
                    {
                        var bgr = srcRow[x];
                        dstRowX[x] = bgr.X;
                        dstRowY[x] = bgr.Y;
                        dstRowZ[x] = bgr.Z;
                    }
                }
            }

            if (src.PixelFormat.Element0.IsRed)
            {
                for (int y = 0; y < src.Height; ++y)
                {
                    var srcRow = src.GetPixelsScanline(y);
                    var dstRowX = dstR.UsePixelsScanline(y);
                    var dstRowY = dstG.UsePixelsScanline(y);
                    var dstRowZ = dstB.UsePixelsScanline(y);

                    for (int x = 0; x < srcRow.Length; ++x)
                    {
                        var bgr = srcRow[x];
                        dstRowX[x] = bgr.X;
                        dstRowY[x] = bgr.Y;
                        dstRowZ[x] = bgr.Z;
                    }
                }
            }
        }

        public static void FitPixels(SpanBitmap src, SpanBitmap dst, (Single offset, Single scale) transform)
        {
            Processing._BilinearResizeImplementation.FitPixels(src, dst, transform);
        }

        public static bool ArePixelsEqual(SpanBitmap a, SpanBitmap b)
        {
            if (a.Info.Bounds != b.Info.Bounds) return false;
            if (a.Info.PixelFormat != b.Info.PixelFormat) return false;

            if (a.Info.PixelFormat.GetDepthType() == typeof(float))
            {
                Guard.AreEqual(nameof(a.Info.PixelFormat), a.PixelSize & 3, 0);

                for (int y = 0; y < a.Height; ++y)
                {
                    var aRow = a.UseBytesScanline(y);
                    var bRow = b.UseBytesScanline(y);
                    var aFlt = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, float>(aRow);
                    var bFlt = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, float>(bRow);
                    if (!_SpanFloatOps.SequenceEqual(aFlt, bFlt)) return false;
                }

                return true;
            }
            else
            {
                for (int y = 0; y < a.Height; ++y)
                {
                    var aRow = a.UseBytesScanline(y);
                    var bRow = b.UseBytesScanline(y);

                    if (!aRow.SequenceEqual(bRow)) return false;
                }

                return true;
            }


            throw new NotImplementedException();
        }

        public static void ApplyAddMultiply(SpanBitmap<Single> target, Single add, Single multiply)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                _SpanFloatOps.AddAndMultiply(row, add, multiply);
            }
        }

        public static void ApplyMultiplyAndAdd(SpanBitmap<Single> target, Single multiply, Single add)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                _SpanFloatOps.MultiplyAndAdd(row, multiply, add);
            }
        }

        public static void ApplyAddMultiply(SpanBitmap<Vector3> target, Single add, Single multiply)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(row);
                _SpanFloatOps.AddAndMultiply(fRow, add, multiply);
            }
        }

        public static void ApplyMultiplyAndAdd(SpanBitmap<Vector3> target, Single multiply, Single add)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(row);
                _SpanFloatOps.MultiplyAndAdd(fRow, multiply, add);
            }
        }

        public static void ApplyAddMultiply(SpanBitmap<Vector4> target, Single add, Single multiply)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector4, Single>(row);
                _SpanFloatOps.AddAndMultiply(fRow, add, multiply);
            }
        }

        public static void ApplyMultiplyAndAdd(SpanBitmap<Vector4> target, Single multiply, Single add)
        {
            for (int y = 0; y < target.Height; ++y)
            {
                var row = target.UsePixelsScanline(y);
                var fRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector4, float>(row);
                _SpanFloatOps.MultiplyAndAdd(fRow, multiply, add);
            }
        }
    }
}

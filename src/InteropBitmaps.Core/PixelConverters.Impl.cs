﻿
// <auto-generated />
using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class _PixelConverters
    {
    
    
        private struct _CvtGray8 : IRGBConverter
        {
            const int SIZE = 1;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = _PixelBGRA32.FromGray8(src);
                    src = src.Slice(SIZE);
                }
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                for (int i = 0; i < src.Length; ++i)
                {
                    src[i].ToGray8(dst);
                    dst = dst.Slice(SIZE);
                }
            }
        }

    
        private struct _CvtGray16 : IRGBConverter
        {
            const int SIZE = 2;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = _PixelBGRA32.FromGray16(src);
                    src = src.Slice(SIZE);
                }
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                for (int i = 0; i < src.Length; ++i)
                {
                    src[i].ToGray16(dst);
                    dst = dst.Slice(SIZE);
                }
            }
        }

    
        private struct _CvtRgb24 : IRGBConverter
        {
            const int SIZE = 3;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = _PixelBGRA32.FromRgb24(src);
                    src = src.Slice(SIZE);
                }
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                for (int i = 0; i < src.Length; ++i)
                {
                    src[i].ToRgb24(dst);
                    dst = dst.Slice(SIZE);
                }
            }
        }

    
        private struct _CvtBgr24 : IRGBConverter
        {
            const int SIZE = 3;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = _PixelBGRA32.FromBgr24(src);
                    src = src.Slice(SIZE);
                }
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                for (int i = 0; i < src.Length; ++i)
                {
                    src[i].ToBgr24(dst);
                    dst = dst.Slice(SIZE);
                }
            }
        }

    
        private struct _CvtRgba32 : IRGBConverter
        {
            const int SIZE = 4;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = _PixelBGRA32.FromRgba32(src);
                    src = src.Slice(SIZE);
                }
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                for (int i = 0; i < src.Length; ++i)
                {
                    src[i].ToRgba32(dst);
                    dst = dst.Slice(SIZE);
                }
            }
        }

    
        private struct _CvtBgra32 : IRGBConverter
        {
            const int SIZE = 4;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = _PixelBGRA32.FromBgra32(src);
                    src = src.Slice(SIZE);
                }
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                for (int i = 0; i < src.Length; ++i)
                {
                    src[i].ToBgra32(dst);
                    dst = dst.Slice(SIZE);
                }
            }
        }

    
        private struct _CvtArgb32 : IRGBConverter
        {
            const int SIZE = 4;

            public void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<byte> src)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    dst[i] = _PixelBGRA32.FromArgb32(src);
                    src = src.Slice(SIZE);
                }
            }

            public void ConvertTo(Span<byte> dst, ReadOnlySpan<_PixelBGRA32> src)
            {
                for (int i = 0; i < src.Length; ++i)
                {
                    src[i].ToArgb32(dst);
                    dst = dst.Slice(SIZE);
                }
            }
        }

    
    }
}
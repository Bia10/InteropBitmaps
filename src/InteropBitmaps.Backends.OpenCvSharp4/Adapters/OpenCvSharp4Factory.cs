﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Adapters
{
    public struct OpenCvSharp4Factory
    {
        internal OpenCvSharp4Factory(BitmapInfo binfo)
        {
            _Info = binfo;
            
        }

        private readonly BitmapInfo _Info;

        public OpenCvSharp.Mat CreateMat()
        {
            var mtype = OpenCvSharp.MatType.CV_8UC(_Info.PixelByteSize);

            return new OpenCvSharp.Mat(_Info.Height, _Info.Width, mtype);
        }
    }
}

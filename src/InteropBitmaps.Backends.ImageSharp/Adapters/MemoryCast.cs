﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace InteropBitmaps.Adapters
{
    sealed class CastMemoryManager<TFrom, TTo> : MemoryManager<TTo>
        where TFrom : unmanaged
        where TTo : unmanaged
    {
        

        public CastMemoryManager(Memory<TFrom> from) => _from = from;

        protected override void Dispose(bool disposing) { }

        private readonly Memory<TFrom> _from;

        public override Span<TTo> GetSpan() => MemoryMarshal.Cast<TFrom, TTo>(_from.Span);


        public override MemoryHandle Pin(int elementIndex = 0) => throw new NotSupportedException();

        public override void Unpin() => throw new NotSupportedException();
    }
}

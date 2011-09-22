using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.Core.Interfaces
{
    public abstract class ISampler
    {
		public abstract int GetMoreSamples (Sample sample);
		public abstract ISampler GetSubSampler (int num, int count);
		public abstract int RoundSize (int size);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Art.Core.Geometry;

namespace Art.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class Scene
    {
        public async Task<bool> Intersect (Ray ray)
        {
            await Task.Run (() => { return; });
            return false;
        }
    }
}

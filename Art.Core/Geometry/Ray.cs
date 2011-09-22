using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.Core.Geometry
{
    /// <summary>
    /// 
    /// </summary>
    public class Ray
    {
        /// <summary>
        /// 
        /// </summary>
        public Point Origin;
        /// <summary>
        /// 
        /// </summary>
        public Vector Direction;
        /// <summary>
        /// 
        /// </summary>
        public double MinT;
        /// <summary>
        /// 
        /// </summary>
        public double MaxT;
        /// <summary>
        /// 
        /// </summary>
        public double Time;
        /// <summary>
        /// 
        /// </summary>
        public int Depth;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="time"></param>
        /// <param name="depth"></param>
        public Ray (Point origin = null, Vector direction = null, double start = 0.0, double end = double.PositiveInfinity, double time = 0.0, int depth = 0)
        {
            this.Origin = origin;
            this.Direction = direction;
            this.MinT = start;
            this.MaxT = end;
            this.Time = time;
            this.Depth = depth;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool HasNaNs { get { return this.Origin.HasNans || this.Direction.HasNans || double.IsNaN (this.MinT) || double.IsNaN (this.MaxT); } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public Point Apply (double time)
        {
            return this.Origin + this.Direction * time;
        }
    }
}

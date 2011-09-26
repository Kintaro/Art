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
    public class RayDifferential : Ray
    {
        /// <summary>
        /// 
        /// </summary>
        public bool HasDifferentials = false;
        /// <summary>
        /// 
        /// </summary>
        public Point rxOrigin = new Point ();
        /// <summary>
        /// 
        /// </summary>
        public Point ryOrigin = new Point ();
        /// <summary>
        /// 
        /// </summary>
        public Vector rxDirection = new Vector ();
        /// <summary>
        /// 
        /// </summary>
        public Vector ryDirection = new Vector ();

        /// <summary>
        /// 
        /// </summary>
        public RayDifferential ()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="t"></param>
        /// <param name="d"></param>
        public RayDifferential (Point origin, Vector direction, double start, double end = double.PositiveInfinity, double t = 0.0, int d = 0)
            : base(origin, direction)
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ray"></param>
		public RayDifferential (Ray ray)
		{
			
		}

        /// <summary>
        /// 
        /// </summary>
        public override bool HasNaNs
        {
            get
            {
                return base.HasNaNs ||
                    (this.HasDifferentials && (this.rxOrigin.HasNans || this.ryOrigin.HasNans || this.rxDirection.HasNans || this.ryDirection.HasNans));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void ScaleDifferentials (double s)
        {
            this.rxOrigin = this.Origin + (this.rxOrigin - this.Origin) * s;
            this.ryOrigin = this.Origin + (this.ryOrigin - this.Origin) * s;
            this.rxDirection = this.Direction + (this.rxDirection - this.Direction) * s;
            this.ryDirection = this.Direction + (this.ryDirection - this.Direction) * s;
        }
    }
}

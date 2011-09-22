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
	/// <typeparam name="NodeData"></typeparam>
	public class OctNode<NodeData>
	{
		/// <summary>
		/// 
		/// </summary>
		public OctNode<NodeData>[] Children = new OctNode<NodeData>[8];
		/// <summary>
		/// 
		/// </summary>
		public List<NodeData> Data = new List<NodeData> ();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="NodeData"></typeparam>
	public class Octree<NodeData>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public delegate bool LookupProc (NodeData node);

		/// <summary>
		/// 
		/// </summary>
		private int maxDepth;
		/// <summary>
		/// 
		/// </summary>
		private BoundingBox bound = new BoundingBox ();
		/// <summary>
		/// 
		/// </summary>
		private OctNode<NodeData> root = new OctNode<NodeData> ();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataItem"></param>
		/// <param name="dataBound"></param>
		public async Task Add (NodeData dataItem, BoundingBox dataBound)
		{
			await this.AddPrivate (root, bound, dataItem, dataBound, Util.DistanceSquared (dataBound.pMin, dataBound.pMax));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="LookupProc"></typeparam>
		/// <param name="node"></param>
		/// <param name="nodeBound"></param>
		/// <param name="p"></param>
		/// <param name="process"></param>
		/// <returns></returns>
		public bool LookupPrivate (OctNode<NodeData> node, BoundingBox nodeBound, Point p, LookupProc process)
		{
			for (var i = 0; i < node.Data.Count; ++i)
				if (!process (node.Data[i]))
					return false;

			var pMid = .5 * nodeBound.pMin + .5 * nodeBound.pMax;
			var child = (p.x > pMid.x ? 4 : 0) + (p.y > pMid.y ? 2 : 0) +
						(p.z > pMid.z ? 1 : 0);

			if (node.Children[child] == null)
				return true;

			var childBound = this.OctreeChildBound (child, nodeBound, pMid);

			return this.LookupPrivate (node.Children[child], childBound, p, process);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <param name="nodeBound"></param>
		/// <param name="dataItem"></param>
		/// <param name="dataBound"></param>
		/// <param name="diag2"></param>
		/// <param name="depth"></param>
		private async Task AddPrivate (OctNode<NodeData> node, BoundingBox nodeBound, NodeData dataItem, BoundingBox dataBound, double diag2, int depth = 0)
		{
			if (depth == maxDepth ||
				Util.DistanceSquared (nodeBound.pMin, nodeBound.pMax) < diag2)
			{
				node.Data.Add (dataItem);
				return;
			}

			var pMid = .5 * nodeBound.pMin + .5 * nodeBound.pMax;

			var x = new bool[] { dataBound.pMin.x <= pMid.x, dataBound.pMax.x > pMid.x };
			var y = new bool[] { dataBound.pMin.y <= pMid.y, dataBound.pMax.y > pMid.y };
			var z = new bool[] { dataBound.pMin.z <= pMid.z, dataBound.pMax.z > pMid.z };

			var over = new bool[] 
			{
				x[0] & y[0] & z[0], x[0] & y[0] & z[1],
				x[0] & y[1] & z[0], x[0] & y[1] & z[1],
				x[1] & y[0] & z[0], x[1] & y[0] & z[1],
				x[1] & y[1] & z[0], x[1] & y[1] & z[1],
			};

			for (var child = 0; child < 8; ++child)
			{
				if (!over[child]) continue;
				if (node.Children[child] == null)
					node.Children[child] = new OctNode<NodeData> ();
				var childBound = this.OctreeChildBound (child, nodeBound, pMid);
				await this.AddPrivate (node.Children[child], childBound, dataItem, dataBound, diag2, depth + 1);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="child"></param>
		/// <param name="nodeBound"></param>
		/// <param name="pMid"></param>
		/// <returns></returns>
		private BoundingBox OctreeChildBound (int child, BoundingBox nodeBound, Point pMid)
		{
			var childBound = new BoundingBox();

			childBound.pMin.x = (child & 4) != 0 ? pMid.x : nodeBound.pMin.x;
			childBound.pMax.x = (child & 4) != 0 ? nodeBound.pMax.x : pMid.x;
			childBound.pMin.y = (child & 2) != 0 ? pMid.y : nodeBound.pMin.y;
			childBound.pMax.y = (child & 2) != 0 ? nodeBound.pMax.y : pMid.y;
			childBound.pMin.z = (child & 1) != 0 ? pMid.z : nodeBound.pMin.z;
			childBound.pMax.z = (child & 1) != 0 ? nodeBound.pMax.z : pMid.z;

			return childBound;
		}
	}
}

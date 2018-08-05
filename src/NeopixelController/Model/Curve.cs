using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeopixelController.Model
{
    public class Curve
    {
        public List<double> X { get; private set; } = new List<double>();
        public List<double> Y { get; private set; } = new List<double>();
        public List<double> W { get; private set; } = new List<double>();

        /// <summary>
        /// Add a new point to the bary curve
        /// </summary>
        /// <param name="x">The X value</param>
        /// <param name="y">The Y value</param>
        /// <param name="w">The weight of the point</param>
        public void AddPoint(double x,double y, double w = 1)
        {
            X.Add(x);
            Y.Add(y);
            W.Add(w);
        }

        /// <summary>
        /// Add a new point to the bary curve
        /// </summary>
        /// <param name="x">The X value</param>
        /// <param name="y">The Y value</param>
        /// <param name="w">The weight of the point</param>
        /// /// <param name="index">The index to add the point at</param>
        public void AddPointAt(int index, double x, double y, double w = 1)
        {
            X.Add(x);
            Y.Add(y);
            W.Add(w);
        }

        /// <summary>
        /// Add a new point to the bary curve
        /// </summary>
        /// <param name="x">The X value</param>
        /// <param name="y">The Y value</param>
        /// <param name="w">The weight of the point</param>
        /// /// <param name="index">The index to add the point at</param>
        public void RemovePointAt(int index)
        {
            X.RemoveAt(index);
            Y.RemoveAt(index);
            W.RemoveAt(index);
        }
    }
}

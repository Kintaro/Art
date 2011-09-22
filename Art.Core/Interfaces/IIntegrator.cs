using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.Core.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIntegrator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="camera"></param>
        /// <param name="renderer"></param>
        void Preprocess (Scene scene, ICamera camera, IRenderer renderer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sampler"></param>
        /// <param name="sample"></param>
        /// <param name="scene"></param>
        void RequestSamples (ISampler sampler, Sample sample, Scene scene);
    }
}

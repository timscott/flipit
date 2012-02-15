using System;

namespace FlipIt
{
    public interface IFeatureChecker
    {
        bool IsOn<T>(T feature) where T : IFeature;
        void DoIfOn<T>(T feature, Action action) where T : IFeature;
    }
}
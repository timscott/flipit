using System;

namespace FlipIt
{
    public interface IFeatureFlipper
    {
        bool IsOn<T>(T feature) where T : IFeature;
        void DoIfOn<T>(T feature, Action action) where T : IFeature;
    }
}
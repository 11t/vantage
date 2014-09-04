namespace Vantage.Animation3D.Layers
{
    using System.Collections.Generic;

    using SharpDX;

    using Vantage.Animation2D.OsbTypes;

    public interface ILayer
    {
        ILayer Root { get; }

        ILayer Parent { get; set; }

        IList<ILayer> Children { get; }

        double CurrentTime { get; }
        
        Vector3 Forward { get; }

        Vector3 Up { get; }

        Vector3 Right { get; }
        
        Vector3 WorldPosition { get; }

        Quaternion WorldRotation { get; }

        Vector3 WorldScale { get; }

        OsbColor WorldColor { get; }

        double WorldOpacity { get; }

        Matrix LocalToWorld { get; }

        bool Additive { get; set; }

        bool HorizontalFlip { get; set; }

        bool VerticalFlip { get; set; }

        bool DebugTrack { get; set; }

        TChild NewChild<TChild>(params object[] args) where TChild : class, ILayer;

        void UpdateToTime(double time);
    }
}

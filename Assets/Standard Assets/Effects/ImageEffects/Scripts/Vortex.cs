using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Displacement/Vortex")]
    public class Vortex : ImageEffectBase
    {
        public bool disabling = false;
        public Vector2 radius = new Vector2(0.4F,0.4F);
        public float angle;
        private float previousAngle;
        public Vector2 center = new Vector2(0.5F, 0.5F);
        private Vector2 previousCenter;
        public float rate = 0;

		
        // Called by camera to apply image effect
        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            //Debug.Log("Rendering");
            ImageEffects.RenderDistortion (material, source, destination, angle, center, radius);
        }

        void Start()
        {
            
        }

        void Update()
        {
            if (disabling)
            {
                angle = 0;
                center = new Vector2(0.5f, 0.5f);
            }
        }

        public void OnResetVision()
        {
            disabling = true;
            rate = 0;
            previousAngle = angle;
            previousCenter = center;
        }
    }
}

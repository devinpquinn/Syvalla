using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CRTFilter
{
    public class CRTRendererFeature : ScriptableRendererFeature
    {
        public enum Presets
        {
            none,
            subtle,
            retro,
            strong,
            oldCrt,
            arcade,
            custom
        }

        public Shader shader;
        public Presets preset;

        [Range(0f, 640f)]
        public float pixelResolutionX = 320;
        [Range(0f, 640f)]
        public float pixelResolutionY = 200;

        [Range(0f, 10f)]
        public float screenBend = 4f;
        [Range(0f, 10f)]
        public float screenOverscan = 1f;
        [Range(0f, 10f)]
        public float blur = 0;
        [Range(0f, 50f)]
        public float bleed = 0;
        [Range(0f, 50f)]
        public float smidge = 0;
        [Range(0f, 10f)]
        public float scanlinesStrength = 3;
        [Range(0f, 10f)]
        public float apertureStrength = 3;
        [Range(0f, 50f)]
        public float shadowlines = 8;
        [Range(-20f, 20f)]
        public float shadowlinesSpeed = -2;
        [Range(0f, 1f)]
        public float shadowlinesAlpha = 0.05f;
        [Range(0f, 50f)]
        public float noiseSize = 75f;
        [Range(0f, 10f)]
        public float noiseSpeed = 0.02f;
        [Range(0f, 1f)]
        public float noiseAlpha = 0.05f;
        [Range(0f, 10f)]
        public float vignetteSize = 5.3f;
        [Range(0f, 20f)]
        public float vignetteSmooth = 2;
        [Range(2f, 50f)]
        public float vignetteRound = 25f;
        [Range(-2f, 2f)]
        public float brightness = 0;
        [Range(-3f, 3f)]
        public float contrast = 1;
        [Range(-3f, 3f)]
        public float gamma = 1;
        [Range(0f, 2f)]
        public float red = 1;
        [Range(0f, 2f)]
        public float green = 1;
        [Range(0f, 2f)]
        public float blue = 1;
        [Range(-10f, 10f)]
        public float chromaticAberration = 1;

        public Vector2 redOffset = new Vector2(0.1f, -0.1f);
        public Vector2 blueOffset = new Vector2(0, 0.1f);
        public Vector2 greenOffset = new Vector2(-0.1f, 0f);

        private CRTRenderPass crtRenderPass;
        private Material material;

        #region Settings presets

        public void OnValidate()
        {
            switch (preset)
            {
                case Presets.none:
                    screenBend = 0;
                    screenOverscan = 0;
                    blur = 0;
                    bleed = 0;
                    smidge = 0;
                    scanlinesStrength = 0;
                    apertureStrength = 0;
                    shadowlines = 0;
                    shadowlinesSpeed = 0;
                    shadowlinesAlpha = 0;
                    vignetteSize = 0;
                    vignetteSmooth = 0;
                    vignetteRound = 25;
                    noiseSize = 0;
                    noiseAlpha = 0;
                    noiseSpeed = 0;
                    brightness = 0;
                    contrast = 1;
                    gamma = 1;
                    red = 1;
                    green = 1;
                    blue = 1;
                    chromaticAberration = 0;
                    redOffset = Vector2.zero;
                    blueOffset = Vector2.zero;
                    greenOffset = Vector2.zero;
                    break;
                case Presets.subtle:
                    screenBend = 0.51f;
                    screenOverscan = 0;
                    blur = 0.5f;
                    bleed = 0;
                    smidge = 0;
                    scanlinesStrength = 1;
                    apertureStrength = 0.1f;
                    shadowlines = 0;
                    shadowlinesSpeed = 0;
                    shadowlinesAlpha = 0;
                    vignetteSize = 5.65f;
                    vignetteSmooth = 2;
                    vignetteRound = 37;
                    noiseSize = 0;
                    noiseAlpha = 0;
                    noiseSpeed = 0;
                    chromaticAberration = 0;
                    break;
                case Presets.retro:
                    screenBend = 0;
                    screenOverscan = 0;
                    blur = 0.5f;
                    bleed = 1.1f;
                    smidge = 14;
                    scanlinesStrength = 9;
                    apertureStrength = 1;
                    shadowlines = 0;
                    shadowlinesSpeed = 0;
                    shadowlinesAlpha = 0;
                    vignetteSize = 5.7f;
                    vignetteSmooth = 4.3f;
                    vignetteRound = 50;
                    noiseSize = 0;
                    noiseAlpha = 0;
                    noiseSpeed = 0;
                    chromaticAberration = 0;
                    break;
                case Presets.strong:
                    screenBend = 6.5f;
                    screenOverscan = 0.5f;
                    blur = 0.8f;
                    bleed = 0;
                    smidge = 0;
                    scanlinesStrength = 2.8f;
                    apertureStrength = 1;
                    shadowlines = 3.5f;
                    shadowlinesSpeed = 0.5f;
                    shadowlinesAlpha = 0.1f;
                    vignetteSize = 5.7f;
                    vignetteSmooth = 2.8f;
                    vignetteRound = 30;
                    noiseSize = 0;
                    noiseAlpha = 0;
                    noiseSpeed = 0;
                    chromaticAberration = 0.5f;
                    break;
                case Presets.oldCrt:
                    screenBend = 8.3f;
                    screenOverscan = 1.5f;
                    blur = 1;
                    bleed = 0.1f;
                    smidge = 0;
                    scanlinesStrength = 9;
                    apertureStrength = 4;
                    shadowlines = 3.5f;
                    shadowlinesSpeed = 1.5f;
                    shadowlinesAlpha = 0.2f;
                    vignetteSize = 5.7f;
                    vignetteSmooth = 2;
                    vignetteRound = 13;
                    noiseSize = 26;
                    noiseAlpha = 0.25f;
                    noiseSpeed = 7.2f;
                    chromaticAberration = 1.5f;
                    break;
                case Presets.arcade:
                    screenBend = 7.2f;
                    screenOverscan = 0.5f;
                    blur = 0;
                    bleed = 3;
                    smidge = 15;
                    scanlinesStrength = 9;
                    apertureStrength = 4;
                    shadowlines = 0;
                    shadowlinesSpeed = 0;
                    shadowlinesAlpha = 0;
                    vignetteSize = 5.7f;
                    vignetteSmooth = 1;
                    vignetteRound = 15;
                    noiseSize = 0;
                    noiseAlpha = 0;
                    noiseSpeed = 0;
                    chromaticAberration = 1;
                    break;
                case Presets.custom:
                default:
                    break;
            }

            if (chromaticAberration != 0)
            {
                redOffset = new Vector2(chromaticAberration / 10, chromaticAberration / 10);
                blueOffset = new Vector2(0, -(chromaticAberration / 10) * 1.4f);
                greenOffset = new Vector2(-chromaticAberration / 10, chromaticAberration / 10);
            }
        }

        #endregion

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (material == null || crtRenderPass == null)
                return;

            material.SetFloat("m_screenBend", screenBend == 0 ? 1000 : 13 - screenBend);
            material.SetFloat("m_screenOverscan", screenOverscan * 0.025f);
            material.SetFloat("m_blur", blur / 1000);
            material.SetFloat("m_smidge", smidge / 50);
            material.SetFloat("m_bleedr", bleed);
            material.SetFloat("m_bleedg", bleed > 0 ? 1 : 0);
            material.SetFloat("m_bleedb", bleed > 0 ? 1 : 0);
            material.SetFloat("m_resX", pixelResolutionX);
            material.SetFloat("m_resY", pixelResolutionY);
            material.SetFloat("m_scanlinesStrength", scanlinesStrength / 10);
            material.SetFloat("m_apertureStrength", apertureStrength / 10);
            material.SetFloat("m_shadowlines", shadowlines);
            material.SetFloat("m_shadowlinesSpeed", shadowlinesSpeed);
            material.SetFloat("m_shadowlinesAlpha", shadowlinesAlpha * 0.2f);
            material.SetFloat("m_vignetteSize", vignetteSize * 0.35f);
            material.SetFloat("m_vignetteSmooth", vignetteSmooth * 0.1f);
            material.SetFloat("m_vignetteRound", vignetteRound);
            material.SetFloat("m_noiseSize", noiseSize * 20);
            material.SetFloat("m_noiseAlpha", noiseAlpha * 0.2f);
            material.SetFloat("m_noiseSpeed", noiseSpeed * 0.0001f);
            material.SetFloat("m_brightness", brightness);
            material.SetFloat("m_contrast", contrast);
            material.SetFloat("m_gamma", gamma);
            material.SetFloat("m_red", red);
            material.SetFloat("m_green", green);
            material.SetFloat("m_blue", blue);
            material.SetVector("m_redOffset", redOffset / 100);
            material.SetVector("m_greenOffset", greenOffset / 100);
            material.SetVector("m_blueOffset", blueOffset / 100);

            crtRenderPass.Init(renderer, material);
            renderer.EnqueuePass(crtRenderPass);
        }

        public override void Create()
        {
            if (material == null)
                material = new Material(shader);

            if (crtRenderPass == null)
                crtRenderPass = new CRTRenderPass();
        }

        protected override void Dispose(bool disposing)
        {
            if (crtRenderPass != null)
                crtRenderPass = null;
            if (material != null)
            {
                CoreUtils.Destroy(material);
                material = null;
            }
        }

        class CRTRenderPass : ScriptableRenderPass
        {
            private const string PROFTAG = "CRTFilter";

            private ScriptableRenderer renderer;
            private Material material;
            private RenderTargetIdentifier cameraRT;
            private RenderTargetIdentifier tempRT;

            public CRTRenderPass()
            {
                renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            }

            public void Init(ScriptableRenderer renderer, Material material)
            {
                this.renderer = renderer;
                this.material = material;
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                var width = cameraTextureDescriptor.width;
                var height = cameraTextureDescriptor.height;

                var textureId = Shader.PropertyToID("_CRTFilterTexture");
                cmd.GetTemporaryRT(textureId, width, height, 0, FilterMode.Point, RenderTextureFormat.ARGB32);
                tempRT = new RenderTargetIdentifier(textureId);
                ConfigureTarget(tempRT);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (material == null)
                    return;

                var cameraColorTexture = renderingData.cameraData.renderer.cameraColorTarget;
                if ((cameraColorTexture == new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget)))
                {
                    Debug.LogWarning("CRT Filter: camera doesn't render to the texture. Please make sure, that there is PixelPerfectCamera component attached with CropFrame setting anything but 'None'");
                    return;
                }

                CommandBuffer cmd = CommandBufferPool.Get(PROFTAG);

                material.SetFloat("m_time", Time.time);
                cmd.Blit(cameraColorTexture, tempRT, material, 0);
                cmd.Blit(tempRT, cameraColorTexture);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                CommandBufferPool.Release(cmd);
            }
        }
    }
}
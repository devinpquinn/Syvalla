# Overview

Customizable full screen CRT effect, working with URP and PixelPerfectCamera component. It uses
high-performance single pass shader to create many CRT "artifacts".
**Please read 'Installation instructions' below to properly configure the filter.**
Thanks for using my CRT Filter for URP. If you need any support, please contact me at: curio124@gmail.com


# Package contents:

This package comes with the following asset files:
- SampleScene folder - samplescene and assets requires for it (screenshot from the game Cursed
Castilla - credits to Locomalito, Gryzor87 - Abylight Studios)
**please note, that the sample scene will not work properly until you configure your URP pipeline**.
For more info see 'Installation instructions' below
- **Scripts\CRTRendererFeature** script - main script for the ScriptableRendererFeature
- Scripts\ReadMeInfo script - script used in sample scene to point you to this documentation
- **Shaders\CRTFilter shader** - main shader for the filter
- Settings\ExampleCRTFilter2DRenderer - example of renderer settings. For more info see 'Installation instructions' below
- Settings\ExampleUniversalRPSettings - example of URP settings. For more info see 'Installation instructions' below

# Installation instructions:

1. **Create a new 2DRenderer or adjust provided ExampleCRTFilter2DRenderer**
You can either create **a new 2DRenderer** anywhere in your assets folder by righ-click in the project explorer under Assets
section and selecting **Create->Rendering->URP 2D Renderer**, or you can use (optionaly also rename and relocate) provided
**ExampleCRTFilter2DRenderer** file from the Settings subfolder.
2. **URP settings location**
This package is compatible only with the URP, as it uses URP way of adding renderer passes. If the project is configured
properly for the URP, there is a an existing URP settings. It's usually located either in the root Assets folder or in the
Settings subfolder with a default name **"UniversalRP"**.
If you have any difficulties to locate URP settings file, open "Project Settings" and select section "Graphics". Used URP
settings file is visible in the first configuration item - "Scriptable Render Pipeline Settings". By clicking on the item,
URP settings file will be located in the project explorer.
3. **URP settings configuration**
note: URP use render passes also for the scene editor. If you change default renderer (first item in the "Renderer List"),
CRT filter will be applied also to the scene editor. Better option is to add another renderer, that will be used only with
selected camera(s), what is described in this step.
Open settings file from the step 2. in the inspector and **add a new renderer to a "Renderer List" by clicking "+" button**
at the end of the "Renderer List". **Add reference to a rendered from the step 1** to a newly created record in the renderer list.
4. **Camera configuration - necessary for the sample scene as well**
Once a new renderer was added to the renderer list in the URP configuration (step 3.), it can be selected for any camera in
the project. **If you wish to see the CRT Filter in the sample scene, camera in the sample scena has to be adjusted as well.**
	1. select the camera to which you would like to apply the CRT filter
	2. find the **"Camera"** component and expand **"Rendering"** section
	3. select proper renderer (from the step 1.) in the **"Renderer"** setting
5. **Configure 2DRenderer**
note: if you've used provided ExampleCRTFilter2DRenderer, it already contains CRT Renderer Feature with proper configuration,
so you may skip this step and all it's substeps
	1. open 2DRenderer from the step 1. in the inspector and scroll down to the section "Renderer Features"
	2. add CRT Filter Renderer Feature by clicking **"Add Renderer Feature"** button at the end of settings list and
	chosing **"CRT Renderer Feature"**
	3. expand section "CRT Renderer Feature" if it's collapsed
	4. rename the name of the feature (there is/was a known issue, if the feature has the same name as the object).
	Name it "CRT Filter" for example.
	5. set the **"Shader"** setting to the **"CRTFilter"** (the provided shader)
6. **Configure CRT Filter**
note: CRT Filter is configured in the 2DRenderer. You may create multiple 2DRenderers with different CRT Filter configuration if you
need it for your game (valid option for in-game monitors).
	1. open 2DRenderer from the step 1. in the inspector and scroll down to the section "Renderer Features"
	2. expand section "CRT Renderer Feature" if it's collapsed
	3. choose any preset to see CRT effect on all properly configured cameras - with selected preset, most values can't be changed
	4. or choose a **"custom" preset** and modify any values to your liking - for more info see 'Reference' below

# Requirements

CRT filter requires URP to work properly. Shader can be used also in other pipelines, but provided script and
instructions are implemented for URP.

# Limitations

CRT filter can't be used without component that creates camera texture such as PixelPerfectCamera or postprocessor
on the camera. Any other component that renders to camera texture can be used, but the filter works best with
PixelPerfectCamera and with resolution set to same values in the CRT filter and PixelPerfectCamera componet.

# Reference

CRT filter is configured in the 2DRenderer asset (see 'Installation instructions' above).
Most paramaters are either self-explanatory or their effect can be easily seen if changed.
Some important notes:
- if preset is set to any value except "custom", most parameters can't be changed and are fixed by selected preset
- you may choose any preset and then change preset to "custom" to modify all values
- or you may choose preset "none" and then change it to "custom" to start from scratch
- Pixel Resolution X & Y has to be set to the same values as in PixelPerfectCamera component - otherwise
  some effects may be misalligned
- smidge effect is design to be used only with the bleed effect, without bleed, smidge doesn't look right
- to set custom offsets for R, G and B, Chromatic Aberration has to be set to 0
- see values used for presets for inspiration

# Troubleshooting

- **sample scene doesn't use CRT filter**
	- if you've opened sample scene in your existing project, your URP settings still use standard renderer.
	  You have to configure your URP settings (see 'settings' above) and properly set camera in the sample scene
	  to use a renderer that you've properly configured
- **no output of the camera**
	- disable CRT filter in the 2DRenderer to be sure, that camera is working properly without the filter
	- check the console, if there isn't a warning regarding not existing camera texture. If there is the 
	  message, make sure there is some component that renders to camera texture (such as PixelPerfectCamera
	  with crop enabled or standard camera with postprocessing enabled)
	- try change any other value in 2DRenderer (for example disable and enable post-processing)
- **CRT effect is not visible**
    - make sure, that camera is using proper renderer (camera settings in camera inspector)
	- make sure, that renderer has added CRT Filter and it's enabled
	- make sure, that filter has some settings (if everything is set to 0, there is no effect from the filter)
- **there is an error in the console**
    - try to fix it by yourself - both renderer class and shader is available to you
	- conntact me: curio124@gmail.com

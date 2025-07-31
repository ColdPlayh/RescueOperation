This Asset can be used for commercial purposes if you purchased it in the "Asset store" from the seller "Hovl Studio".
All that is in the folder "Realistic explosions" can be used in commerce, even demo scene files.
-----------------------------------------------------

If you want to use post-effect like in the demo video:

Enable post-effect bloom from Package manager post-processing.

or download better bloom from asset store:

1) Download unity free post effects 
https://assetstore.unity.com/packages/essentials/post-processing-stack-83912
2) Add "PostProcessingBehaviour.cs" on main Camera.
3) Set the "Post-effects" profile. ("\Assets\ErbGameArt\Demo scene\CC.asset")
4) You should turn on "HDR" on main camera for correct post-effects. (bloom post-effect works correctly only with HDR)
If you have forward rendering path (by default in Unity), you need disable antialiasing "edit->project settings->quality->antialiasing"
or turn of "MSAA" on main camera, because HDR does not works with msaa. If you want to use HDR and MSAA then use "MSAA of post effect". 
It's faster then default MSAA and have the same quality.



Using:
Don't press the "apply" button on the explosion prefabs from the demo scene. Use only prefabs from the folder!

The "Soft Particle Factor" on the material from the custom shaders is responsible for the depths of the material.
Textures are not displayed in materials with custom shaders if you don't use depth in your Graphics Settings (or use checkmark "Depth off?" in the materials).

Blend_crater shader only works with particles with "Custom vertex stream" (Uv0.Custom.xy) in tab "Render". And don't forget to use "Custom data" parameters in your PS.
If you want explosions to be brighter - just add the brightness in the material.
All the textures of explosions are 8192x8192 by default, but you can reduce them in the material settings.
-----------------------------------------------------

Contact me if you have any problems or questions: gorobecn2@gmail.com
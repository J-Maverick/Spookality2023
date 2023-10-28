Shader "InvisibleMaskBehind" {
  SubShader {
    // draw after all opaque objects 
    Tags { "Queue"="Transparent-50" }
    Pass {
      Blend Zero One // keep the image behind it
      Cull Off
    }
  } 
}
// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.9918864,fgcg:0.9632353,fgcb:1,fgca:1,fgde:0.002,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33050,y:32223,varname:node_3138,prsc:2|emission-857-RGB;n:type:ShaderForge.SFN_ViewVector,id:9763,x:31317,y:32321,varname:node_9763,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6020,x:31491,y:32321,varname:node_6020,prsc:2|A-9763-OUT,B-9888-OUT;n:type:ShaderForge.SFN_Vector1,id:9888,x:31317,y:32452,varname:node_9888,prsc:2,v1:-1;n:type:ShaderForge.SFN_Tex2d,id:857,x:32869,y:32321,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_857,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-803-OUT;n:type:ShaderForge.SFN_Code,id:778,x:31696,y:32515,varname:node_778,prsc:2,code:ZgBsAG8AYQB0ADMAIABuAG8AcgBtAGEAbABpAHoAZQBkAEMAbwBvAHIAZABzACAAPQAgAG4AbwByAG0AYQBsAGkAegBlACgAYwBvAG8AcgBkAHMAKQA7AA0ACgBmAGwAbwBhAHQAIABsAGEAdABpAHQAdQBkAGUAIAA9ACAAYQBjAG8AcwAoAG4AbwByAG0AYQBsAGkAegBlAGQAQwBvAG8AcgBkAHMALgB5ACkAOwANAAoAZgBsAG8AYQB0ACAAbABvAG4AZwBpAHQAdQBkAGUAIAA9ACAAYQB0AGEAbgAyACgAbgBvAHIAbQBhAGwAaQB6AGUAZABDAG8AbwByAGQAcwAuAHoALAAgAG4AbwByAG0AYQBsAGkAegBlAGQAQwBvAG8AcgBkAHMALgB4ACkAOwANAAoAZgBsAG8AYQB0ADIAIABzAHAAaABlAHIAZQBDAG8AbwByAGQAcwAgAD0AIABmAGwAbwBhAHQAMgAoAGwAbwBuAGcAaQB0AHUAZABlACwAIABsAGEAdABpAHQAdQBkAGUAKQAgACoAIABmAGwAbwBhAHQAMgAoADAALgA1AC8AVQBOAEkAVABZAF8AUABJACwAIAAxAC4AMAAvAFUATgBJAFQAWQBfAFAASQApADsADQAKAHMAcABoAGUAcgBlAEMAbwBvAHIAZABzACAAPQAgAGYAbABvAGEAdAAyACgAMAAuADUALAAxAC4AMAApACAALQAgAHMAcABoAGUAcgBlAEMAbwBvAHIAZABzADsACgByAGUAdAB1AHIAbgAgACgAcwBwAGgAZQByAGUAQwBvAG8AcgBkAHMAIAArACAAZgBsAG8AYQB0ADQAKAAwACwAIAAxAC0AdQBuAGkAdAB5AF8AUwB0AGUAcgBlAG8ARQB5AGUASQBuAGQAZQB4ACwAMQAsADAALgA1ACkALgB4AHkAKQAgACoAIABmAGwAbwBhAHQANAAoADAALAAgADEALQB1AG4AaQB0AHkAXwBTAHQAZQByAGUAbwBFAHkAZQBJAG4AZABlAHgALAAxACwAMAAuADUAKQAuAHoAdwA7AA==,output:1,fname:StereoPanoProjection,width:891,height:132,input:2,input_1_label:coords|A-6020-OUT;n:type:ShaderForge.SFN_Code,id:7754,x:31695,y:32321,varname:node_7754,prsc:2,code:ZgBsAG8AYQB0ADMAIABuAG8AcgBtAGEAbABpAHoAZQBkAEMAbwBvAHIAZABzACAAPQAgAG4AbwByAG0AYQBsAGkAegBlACgAYwBvAG8AcgBkAHMAKQA7AA0ACgBmAGwAbwBhAHQAIABsAGEAdABpAHQAdQBkAGUAIAA9ACAAYQBjAG8AcwAoAG4AbwByAG0AYQBsAGkAegBlAGQAQwBvAG8AcgBkAHMALgB5ACkAOwANAAoAZgBsAG8AYQB0ACAAbABvAG4AZwBpAHQAdQBkAGUAIAA9ACAAYQB0AGEAbgAyACgAbgBvAHIAbQBhAGwAaQB6AGUAZABDAG8AbwByAGQAcwAuAHoALAAgAG4AbwByAG0AYQBsAGkAegBlAGQAQwBvAG8AcgBkAHMALgB4ACkAOwANAAoAZgBsAG8AYQB0ADIAIABzAHAAaABlAHIAZQBDAG8AbwByAGQAcwAgAD0AIABmAGwAbwBhAHQAMgAoAGwAbwBuAGcAaQB0AHUAZABlACwAIABsAGEAdABpAHQAdQBkAGUAKQAgACoAIABmAGwAbwBhAHQAMgAoADEALgAwAC8AVQBOAEkAVABZAF8AUABJACwAIAAxAC4AMAAvAFUATgBJAFQAWQBfAFAASQApADsADQAKAHMAcABoAGUAcgBlAEMAbwBvAHIAZABzACAAPQAgAGYAbABvAGEAdAAyACgAMQAuADAALAAxAC4AMAApACAALQAgAHMAcABoAGUAcgBlAEMAbwBvAHIAZABzADsACgByAGUAdAB1AHIAbgAgACgAcwBwAGgAZQByAGUAQwBvAG8AcgBkAHMAIAArACAAZgBsAG8AYQB0ADQAKAAwACwAIAAxAC0AdQBuAGkAdAB5AF8AUwB0AGUAcgBlAG8ARQB5AGUASQBuAGQAZQB4ACwAMQAsADEALgAwACkALgB4AHkAKQAgACoAIABmAGwAbwBhAHQANAAoADAALAAgADEALQB1AG4AaQB0AHkAXwBTAHQAZQByAGUAbwBFAHkAZQBJAG4AZABlAHgALAAxACwAMQAuADAAKQAuAHoAdwA7AA==,output:1,fname:MonoPanoProjection,width:892,height:132,input:2,input_1_label:coords|A-6020-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:803,x:32667,y:32321,ptovrint:False,ptlb:Stereo Enabled,ptin:_StereoEnabled,varname:node_803,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-7754-OUT,B-778-OUT;proporder:857-803;pass:END;sub:END;*/

Shader "VRChat/Panosphere" {
     Properties {
       _MainTex ("Diffuse(RGB) Spec(A)", 2D) = "white" {}
       _BumpMap ("Bumpmap", 2D) = "bump" {}
       _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
       _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
       _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
    _Shininess ("Shininess", Range (0.01, 1)) = 0.078125 
     }
     SubShader {
       Tags { "RenderType" = "Transparent" }
       CGPROGRAM
 
        #pragma surface surf SimpleSpecular alpha
    float _Shininess;
 
       half4 LightingSimpleSpecular (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
           half3 h = normalize (lightDir + viewDir);
           half diff = max (0, dot (s.Normal, lightDir));
           float nh = max (0, dot (s.Normal, h));
           float spec = pow (nh, 48.0);
           half4 c;
           c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec * s.Alpha * _Shininess * _SpecColor) * (atten * 2);
           c.a = s.Alpha;
           return c;
       }
 
       struct Input {
           float2 uv_MainTex;
           float2 uv_BumpMap;
           float3 viewDir;
       };
       sampler2D _MainTex;
       sampler2D _BumpMap;
       float4 _RimColor;
       float _RimPower;
 
       void surf (Input IN, inout SurfaceOutput o) {
           fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
           o.Albedo = c.rgb;
           o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
           half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
           o.Emission = _RimColor.rgb * pow (rim, _RimPower);
           o.Alpha = c.a;
       }
       ENDCG
     } 
     Fallback "Diffuse"
   }
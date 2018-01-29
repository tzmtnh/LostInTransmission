// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33136,y:32471,varname:node_3138,prsc:2|emission-668-OUT,alpha-910-A;n:type:ShaderForge.SFN_Tex2d,id:910,x:32418,y:32534,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_910,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:668,x:32890,y:32611,varname:node_668,prsc:2|A-910-RGB,B-4913-OUT;n:type:ShaderForge.SFN_Color,id:1973,x:32428,y:32842,ptovrint:False,ptlb:GlowColor,ptin:_GlowColor,varname:node_1973,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.948783,c3:0.2573529,c4:1;n:type:ShaderForge.SFN_Multiply,id:4913,x:32661,y:32932,varname:node_4913,prsc:2|A-1973-RGB,B-3434-OUT,C-5116-OUT;n:type:ShaderForge.SFN_Sin,id:8104,x:32271,y:32996,varname:node_8104,prsc:2|IN-9228-OUT;n:type:ShaderForge.SFN_RemapRange,id:3434,x:32442,y:32996,varname:node_3434,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-8104-OUT;n:type:ShaderForge.SFN_Time,id:4497,x:31877,y:32953,varname:node_4497,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9228,x:32092,y:32996,varname:node_9228,prsc:2|A-4497-T,B-5630-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5630,x:31877,y:33106,ptovrint:False,ptlb:GlowFrequency,ptin:_GlowFrequency,varname:node_5630,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:10;n:type:ShaderForge.SFN_Slider,id:5116,x:32605,y:33130,ptovrint:False,ptlb:Glow,ptin:_Glow,varname:node_5116,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:910-1973-5630-5116;pass:END;sub:END;*/

Shader "Shader Forge/Ship" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _GlowColor ("GlowColor", Color) = (1,0.948783,0.2573529,1)
        _GlowFrequency ("GlowFrequency", Float ) = 10
        _Glow ("Glow", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 2.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _GlowColor;
            uniform float _GlowFrequency;
            uniform float _Glow;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 node_4497 = _Time;
                float3 emissive = (_MainTex_var.rgb+(_GlowColor.rgb*(sin((node_4497.g*_GlowFrequency))*0.5+0.5)*_Glow));
                float3 finalColor = emissive;
                return fixed4(finalColor,_MainTex_var.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

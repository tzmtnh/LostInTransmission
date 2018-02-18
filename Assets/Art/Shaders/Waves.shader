// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:1,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33968,y:32858,varname:node_3138,prsc:2|emission-6843-OUT;n:type:ShaderForge.SFN_Sin,id:5717,x:32245,y:32730,varname:node_5717,prsc:2|IN-2174-OUT;n:type:ShaderForge.SFN_Multiply,id:2174,x:32072,y:32730,varname:node_2174,prsc:2|A-7675-V,B-5502-OUT;n:type:ShaderForge.SFN_Pi,id:5502,x:31830,y:32806,varname:node_5502,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4733,x:32072,y:32598,varname:node_4733,prsc:2|A-7675-U,B-5502-OUT;n:type:ShaderForge.SFN_Sin,id:6380,x:32245,y:32598,varname:node_6380,prsc:2|IN-4733-OUT;n:type:ShaderForge.SFN_Multiply,id:2231,x:32749,y:32907,varname:node_2231,prsc:2|A-6380-OUT,B-5717-OUT;n:type:ShaderForge.SFN_TexCoord,id:7675,x:31385,y:32795,varname:node_7675,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:831,x:32573,y:33076,varname:node_831,prsc:2|A-2070-OUT,B-6542-OUT;n:type:ShaderForge.SFN_Subtract,id:2070,x:32387,y:33051,varname:node_2070,prsc:2|A-8183-OUT,B-5314-OUT;n:type:ShaderForge.SFN_RemapRange,id:1860,x:31646,y:33067,varname:node_1860,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-7675-V;n:type:ShaderForge.SFN_Multiply,id:939,x:31820,y:33067,varname:node_939,prsc:2|A-1860-OUT,B-1504-OUT;n:type:ShaderForge.SFN_Vector1,id:1504,x:31646,y:33256,varname:node_1504,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Frac,id:3953,x:32734,y:33076,varname:node_3953,prsc:2|IN-831-OUT;n:type:ShaderForge.SFN_Multiply,id:7340,x:32953,y:33076,varname:node_7340,prsc:2|A-3953-OUT,B-3953-OUT;n:type:ShaderForge.SFN_Multiply,id:6843,x:33761,y:32963,varname:node_6843,prsc:2|A-5048-RGB,B-2231-OUT,C-7340-OUT,D-1694-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8459,x:31779,y:32281,ptovrint:False,ptlb:Delay,ptin:_Delay,varname:node_8459,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Tex2d,id:5048,x:32336,y:32295,ptovrint:False,ptlb:Gradient,ptin:_Gradient,varname:node_5048,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9043-OUT;n:type:ShaderForge.SFN_Divide,id:2075,x:31980,y:32295,varname:node_2075,prsc:2|A-8459-OUT,B-6642-OUT;n:type:ShaderForge.SFN_Append,id:9043,x:32160,y:32295,varname:node_9043,prsc:2|A-2075-OUT,B-1850-OUT;n:type:ShaderForge.SFN_Vector1,id:1850,x:31980,y:32456,varname:node_1850,prsc:2,v1:0.5;n:type:ShaderForge.SFN_ValueProperty,id:6642,x:31792,y:32369,ptovrint:False,ptlb:MaxDelay,ptin:_MaxDelay,varname:node_6642,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:5314,x:32194,y:33184,ptovrint:False,ptlb:DelayIntegral,ptin:_DelayIntegral,varname:node_5314,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Set,id:6107,x:31980,y:32234,varname:delay,prsc:2|IN-8459-OUT;n:type:ShaderForge.SFN_Vector1,id:6542,x:32387,y:33184,varname:node_6542,prsc:2,v1:3;n:type:ShaderForge.SFN_Get,id:3375,x:33110,y:33237,varname:node_3375,prsc:2|IN-6107-OUT;n:type:ShaderForge.SFN_Clamp01,id:2088,x:33490,y:33237,varname:node_2088,prsc:2|IN-6668-OUT;n:type:ShaderForge.SFN_Slider,id:1694,x:33401,y:33533,ptovrint:False,ptlb:Opasity,ptin:_Opasity,varname:node_1694,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_RemapRange,id:6668,x:33304,y:33237,varname:node_6668,prsc:2,frmn:0,frmx:0.1,tomn:0.2,tomx:1|IN-3375-OUT;n:type:ShaderForge.SFN_Append,id:7243,x:32021,y:33028,varname:node_7243,prsc:2|A-7675-U,B-939-OUT;n:type:ShaderForge.SFN_Length,id:8183,x:32194,y:33028,varname:node_8183,prsc:2|IN-7243-OUT;n:type:ShaderForge.SFN_Get,id:9581,x:32194,y:33363,varname:node_9581,prsc:2|IN-6107-OUT;n:type:ShaderForge.SFN_RemapRange,id:1910,x:32423,y:33331,varname:node_1910,prsc:2,frmn:0,frmx:1,tomn:1,tomx:3|IN-9581-OUT;proporder:8459-6642-5048-5314-1694;pass:END;sub:END;*/

Shader "Shader Forge/Waves" {
    Properties {
        _Delay ("Delay", Float ) = 0
        _MaxDelay ("MaxDelay", Float ) = 2
        _Gradient ("Gradient", 2D) = "white" {}
        _DelayIntegral ("DelayIntegral", Float ) = 0
        _Opasity ("Opasity", Range(0, 1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "DisableBatching"="True"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Delay;
            uniform sampler2D _Gradient; uniform float4 _Gradient_ST;
            uniform float _MaxDelay;
            uniform float _DelayIntegral;
            uniform float _Opasity;
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
                float2 node_9043 = float2((_Delay/_MaxDelay),0.5);
                float4 _Gradient_var = tex2D(_Gradient,TRANSFORM_TEX(node_9043, _Gradient));
                float node_5502 = 3.141592654;
                float node_3953 = frac(((length(float2(i.uv0.r,((i.uv0.g*2.0+-1.0)*0.2)))-_DelayIntegral)*3.0));
                float3 emissive = (_Gradient_var.rgb*(sin((i.uv0.r*node_5502))*sin((i.uv0.g*node_5502)))*(node_3953*node_3953)*_Opasity);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

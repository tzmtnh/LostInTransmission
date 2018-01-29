// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33980,y:32604,varname:node_3138,prsc:2|emission-8352-OUT;n:type:ShaderForge.SFN_Tex2d,id:4646,x:32470,y:32509,varname:node_4646,prsc:2,ntxv:0,isnm:False|TEX-8339-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:8339,x:32190,y:32539,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_8339,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2dAsset,id:1829,x:32149,y:32800,ptovrint:False,ptlb:SecondaryTex,ptin:_SecondaryTex,varname:_MainTex_copy,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:5074,x:32523,y:32956,ptovrint:False,ptlb:Blend,ptin:_Blend,varname:node_5074,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Tex2d,id:7262,x:32388,y:32730,varname:node_7262,prsc:2,ntxv:0,isnm:False|TEX-1829-TEX;n:type:ShaderForge.SFN_Blend,id:5228,x:32714,y:32730,varname:node_5228,prsc:2,blmd:6,clmp:True|SRC-4646-RGB,DST-7262-RGB;n:type:ShaderForge.SFN_Lerp,id:9337,x:32935,y:32652,varname:node_9337,prsc:2|A-4646-RGB,B-5228-OUT,T-5074-OUT;n:type:ShaderForge.SFN_Slider,id:4688,x:32897,y:32940,ptovrint:False,ptlb:LightSpeed,ptin:_LightSpeed,varname:node_4688,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_RemapRange,id:5140,x:33219,y:32730,varname:node_5140,prsc:2,frmn:0,frmx:1,tomn:1,tomx:3|IN-4688-OUT;n:type:ShaderForge.SFN_Power,id:8352,x:33639,y:32730,varname:node_8352,prsc:2|VAL-3688-OUT,EXP-226-OUT;n:type:ShaderForge.SFN_Multiply,id:3688,x:33384,y:32678,varname:node_3688,prsc:2|A-9337-OUT,B-5140-OUT;n:type:ShaderForge.SFN_RemapRange,id:226,x:33384,y:32890,varname:node_226,prsc:2,frmn:0,frmx:1,tomn:1,tomx:2|IN-4688-OUT;proporder:8339-1829-5074-4688;pass:END;sub:END;*/

Shader "Hidden/PixelCombine" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _SecondaryTex ("SecondaryTex", 2D) = "white" {}
        _Blend ("Blend", Range(0, 1)) = 0
        _LightSpeed ("LightSpeed", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 2.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _SecondaryTex; uniform float4 _SecondaryTex_ST;
            uniform float _Blend;
            uniform float _LightSpeed;
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
                float4 node_4646 = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 node_7262 = tex2D(_SecondaryTex,TRANSFORM_TEX(i.uv0, _SecondaryTex));
                float3 emissive = pow((lerp(node_4646.rgb,saturate((1.0-(1.0-node_4646.rgb)*(1.0-node_7262.rgb))),_Blend)*(_LightSpeed*2.0+1.0)),(_LightSpeed*1.0+1.0));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33345,y:32716,varname:node_3138,prsc:2|emission-5317-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32471,y:32813,ptovrint:False,ptlb:Color1,ptin:_Color1,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Color,id:2269,x:32471,y:32992,ptovrint:False,ptlb:Color2,ptin:_Color2,varname:_Color2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.1764706,c3:0.3970588,c4:1;n:type:ShaderForge.SFN_Lerp,id:593,x:32911,y:32904,varname:node_593,prsc:2|A-7241-RGB,B-2269-RGB,T-7607-OUT;n:type:ShaderForge.SFN_ScreenPos,id:1034,x:32643,y:33099,varname:node_1034,prsc:2,sctp:0;n:type:ShaderForge.SFN_RemapRange,id:7607,x:32833,y:33140,varname:node_7607,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-1034-V;n:type:ShaderForge.SFN_Multiply,id:5317,x:33112,y:32788,varname:node_5317,prsc:2|A-722-RGB,B-593-OUT;n:type:ShaderForge.SFN_Tex2d,id:722,x:32884,y:32701,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_722,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5459-OUT;n:type:ShaderForge.SFN_ScreenPos,id:5283,x:32237,y:32542,varname:node_5283,prsc:2,sctp:2;n:type:ShaderForge.SFN_Add,id:5760,x:32497,y:32646,varname:node_5760,prsc:2|A-5283-V,B-5588-OUT;n:type:ShaderForge.SFN_Append,id:5459,x:32686,y:32596,varname:node_5459,prsc:2|A-5283-U,B-5760-OUT;n:type:ShaderForge.SFN_Time,id:1492,x:32071,y:32682,varname:node_1492,prsc:2;n:type:ShaderForge.SFN_Multiply,id:5588,x:32310,y:32726,varname:node_5588,prsc:2|A-1492-T,B-179-OUT;n:type:ShaderForge.SFN_ValueProperty,id:179,x:32058,y:32862,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_179,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:7241-2269-722-179;pass:END;sub:END;*/

Shader "Shader Forge/BG" {
    Properties {
        _Color1 ("Color1", Color) = (0.07843138,0.3921569,0.7843137,1)
        _Color2 ("Color2", Color) = (0,0.1764706,0.3970588,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _Speed ("Speed", Float ) = 1
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 2.0
            uniform float4 _Color1;
            uniform float4 _Color2;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Speed;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 projPos : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
////// Lighting:
////// Emissive:
                float4 node_1492 = _Time;
                float2 node_5459 = float2(sceneUVs.r,(sceneUVs.g+(node_1492.g*_Speed)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5459, _MainTex));
                float3 emissive = (_MainTex_var.rgb*lerp(_Color1.rgb,_Color2.rgb,((sceneUVs * 2 - 1).g*0.5+0.5)));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

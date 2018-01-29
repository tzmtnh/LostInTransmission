// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33482,y:32749,varname:node_3138,prsc:2|emission-7241-RGB,clip-6377-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32471,y:32812,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_ScreenPos,id:8429,x:32017,y:32875,varname:node_8429,prsc:2,sctp:0;n:type:ShaderForge.SFN_ScreenParameters,id:7091,x:31155,y:32681,varname:node_7091,prsc:2;n:type:ShaderForge.SFN_If,id:8510,x:32471,y:33057,varname:node_8510,prsc:2|A-2424-OUT,B-7511-OUT,GT-483-OUT,EQ-483-OUT,LT-1982-OUT;n:type:ShaderForge.SFN_Vector1,id:483,x:32179,y:33214,varname:node_483,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:1982,x:32282,y:33317,varname:node_1982,prsc:2,v1:0;n:type:ShaderForge.SFN_Divide,id:5686,x:31566,y:32672,cmnt:X,varname:node_5686,prsc:2|A-3266-OUT,B-7091-PXW;n:type:ShaderForge.SFN_ValueProperty,id:7661,x:31155,y:32850,ptovrint:False,ptlb:AspectRatio,ptin:_AspectRatio,varname:node_7661,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:3266,x:31375,y:32760,varname:node_3266,prsc:2|A-7091-PXH,B-7661-OUT;n:type:ShaderForge.SFN_Vector1,id:7553,x:31566,y:32576,varname:node_7553,prsc:2,v1:1;n:type:ShaderForge.SFN_Subtract,id:1318,x:31779,y:32619,varname:node_1318,prsc:2|A-7553-OUT,B-5686-OUT;n:type:ShaderForge.SFN_Divide,id:9127,x:31990,y:32706,varname:node_9127,prsc:2|A-1318-OUT,B-3659-OUT;n:type:ShaderForge.SFN_Vector1,id:3659,x:31779,y:32759,varname:node_3659,prsc:2,v1:2;n:type:ShaderForge.SFN_Set,id:1928,x:32171,y:32721,varname:a,prsc:2|IN-9127-OUT;n:type:ShaderForge.SFN_Get,id:7511,x:32211,y:33112,varname:node_7511,prsc:2|IN-1928-OUT;n:type:ShaderForge.SFN_If,id:2008,x:32951,y:33139,varname:node_2008,prsc:2|A-1467-OUT,B-2424-OUT,GT-483-OUT,EQ-483-OUT,LT-1982-OUT;n:type:ShaderForge.SFN_Vector1,id:366,x:32525,y:33177,varname:node_366,prsc:2,v1:1;n:type:ShaderForge.SFN_Subtract,id:1467,x:32584,y:33331,varname:node_1467,prsc:2|A-366-OUT,B-7511-OUT;n:type:ShaderForge.SFN_RemapRange,id:2424,x:32211,y:32895,varname:node_2424,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-8429-U;n:type:ShaderForge.SFN_Multiply,id:1378,x:33136,y:33037,varname:node_1378,prsc:2|A-8510-OUT,B-2008-OUT;n:type:ShaderForge.SFN_Vector1,id:677,x:33136,y:32959,varname:node_677,prsc:2,v1:1;n:type:ShaderForge.SFN_Subtract,id:6377,x:33321,y:32989,varname:node_6377,prsc:2|A-677-OUT,B-1378-OUT;proporder:7241-7661;pass:END;sub:END;*/

Shader "Shader Forge/SideBars" {
    Properties {
        _Color ("Color", Color) = (0,0,0,1)
        _AspectRatio ("AspectRatio", Float ) = 0.5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
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
            uniform float4 _Color;
            uniform float _AspectRatio;
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
                float node_2424 = ((sceneUVs * 2 - 1).r*0.5+0.5);
                float a = ((1.0-((_ScreenParams.g*_AspectRatio)/_ScreenParams.r))/2.0);
                float node_7511 = a;
                float node_8510_if_leA = step(node_2424,node_7511);
                float node_8510_if_leB = step(node_7511,node_2424);
                float node_1982 = 0.0;
                float node_483 = 1.0;
                float node_2008_if_leA = step((1.0-node_7511),node_2424);
                float node_2008_if_leB = step(node_2424,(1.0-node_7511));
                clip((1.0-(lerp((node_8510_if_leA*node_1982)+(node_8510_if_leB*node_483),node_483,node_8510_if_leA*node_8510_if_leB)*lerp((node_2008_if_leA*node_1982)+(node_2008_if_leB*node_483),node_483,node_2008_if_leA*node_2008_if_leB))) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = _Color.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 2.0
            uniform float _AspectRatio;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float node_2424 = ((sceneUVs * 2 - 1).r*0.5+0.5);
                float a = ((1.0-((_ScreenParams.g*_AspectRatio)/_ScreenParams.r))/2.0);
                float node_7511 = a;
                float node_8510_if_leA = step(node_2424,node_7511);
                float node_8510_if_leB = step(node_7511,node_2424);
                float node_1982 = 0.0;
                float node_483 = 1.0;
                float node_2008_if_leA = step((1.0-node_7511),node_2424);
                float node_2008_if_leB = step(node_2424,(1.0-node_7511));
                clip((1.0-(lerp((node_8510_if_leA*node_1982)+(node_8510_if_leB*node_483),node_483,node_8510_if_leA*node_8510_if_leB)*lerp((node_2008_if_leA*node_1982)+(node_2008_if_leB*node_483),node_483,node_2008_if_leA*node_2008_if_leB))) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

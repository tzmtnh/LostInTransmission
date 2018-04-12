// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32920,y:32211,varname:node_3138,prsc:2|emission-4840-OUT;n:type:ShaderForge.SFN_Code,id:6489,x:31654,y:32556,varname:node_6489,prsc:2,code:YwBvAG4AcwB0ACAAaQBuAHQAIABpAHQAZQByAGEAdABpAG8AbgBzACAAPQAgADEANwA7AA0ACgBjAG8AbgBzAHQAIABmAGwAbwBhAHQAIABmAG8AcgBtAHUAcABhAHIAYQBtACAAPQAgADAALgA1ADMAOwANAAoADQAKAGMAbwBuAHMAdAAgAGkAbgB0ACAAdgBvAGwAcwB0AGUAcABzACAAPQAgADIAMAA7AA0ACgBjAG8AbgBzAHQAIABmAGwAbwBhAHQAIABzAHQAZQBwAHMAaQB6AGUAIAA9ACAAMAAuADEAOwANAAoADQAKAC8ALwBjAG8AbgBzAHQAIABmAGwAbwBhAHQAIAB6AG8AbwBtACAAPQAgADAALgA4ADAAMAA7AA0ACgBjAG8AbgBzAHQAIABmAGwAbwBhAHQAIAB0AGkAbABlACAAPQAgADAALgA4ADUAMAA7AA0ACgAvAC8AYwBvAG4AcwB0ACAAZgBsAG8AYQB0ACAAcwBwAGUAZQBkACAAPQAgADAALgAwADEAMAA7AA0ACgANAAoAYwBvAG4AcwB0ACAAZgBsAG8AYQB0ACAAYgByAGkAZwBoAHQAbgBlAHMAcwAgAD0AIAAwAC4AMAAwADEANQA7AA0ACgBjAG8AbgBzAHQAIABmAGwAbwBhAHQAIABkAGEAcgBrAG0AYQB0AHQAZQByACAAPQAgADAALgAzADAAMAA7AA0ACgBjAG8AbgBzAHQAIABmAGwAbwBhAHQAIABkAGkAcwB0AGYAYQBkAGkAbgBnACAAPQAgADAALgA3ADMAMAA7AA0ACgBjAG8AbgBzAHQAIABmAGwAbwBhAHQAIABzAGEAdAB1AHIAYQB0AGkAbwBuACAAPQAgADAALgA4ADUAMAA7AA0ACgANAAoALwAvAGcAZQB0ACAAYwBvAG8AcgBkAHMAIABhAG4AZAAgAGQAaQByAGUAYwB0AGkAbwBuAA0ACgBmAGwAbwBhAHQAMgAgAHUAdgA9AGYAcgBhAGcAQwBvAG8AcgBkAC4AeAB5AC8AXwBTAGMAcgBlAGUAbgBQAGEAcgBhAG0AcwAuAHgAeQAtAC4ANQA7AA0ACgB1AHYALgB5ACoAPQBfAFMAYwByAGUAZQBuAFAAYQByAGEAbQBzAC4AeQAvAF8AUwBjAHIAZQBlAG4AUABhAHIAYQBtAHMALgB4ADsADQAKAGYAbABvAGEAdAAzACAAZABpAHIAPQBmAGwAbwBhAHQAMwAoAHUAdgAqAHoAbwBvAG0ALAAxAC4AKQA7AA0ACgBmAGwAbwBhAHQAIAB0AGkAbQBlAD0AXwBUAGkAbQBlAC4AeQAqAHMAcABlAGUAZAArAC4AMgA1ADsADQAKAA0ACgAvAC8AcgBvAHQAYQB0AGkAbwBuAA0ACgBmAGwAbwBhAHQAIABhADEAPQAwAC4ANQA7AA0ACgBmAGwAbwBhAHQAIABhADIAPQAwAC4AOAA7AA0ACgBmAGwAbwBhAHQAMgB4ADIAIAByAG8AdAAxAD0AZgBsAG8AYQB0ADIAeAAyACgAYwBvAHMAKABhADEAKQAsAHMAaQBuACgAYQAxACkALAAtAHMAaQBuACgAYQAxACkALABjAG8AcwAoAGEAMQApACkAOwANAAoAZgBsAG8AYQB0ADIAeAAyACAAcgBvAHQAMgA9AGYAbABvAGEAdAAyAHgAMgAoAGMAbwBzACgAYQAyACkALABzAGkAbgAoAGEAMgApACwALQBzAGkAbgAoAGEAMgApACwAYwBvAHMAKABhADIAKQApADsADQAKAGQAaQByAC4AeAB6ACAAPQAgAG0AdQBsACgAZABpAHIALgB4AHoALAAgAHIAbwB0ADEAKQA7AA0ACgBkAGkAcgAuAHgAeQAgAD0AIABtAHUAbAAoAGQAaQByAC4AeAB5ACwAIAByAG8AdAAyACkAOwANAAoAZgBsAG8AYQB0ADMAIABmAHIAbwBtAD0AZgBsAG8AYQB0ADMAKAAxAC4ALAAuADUALAAwAC4ANQApADsADQAKAC8ALwBmAHIAbwBtACsAPQBmAGwAbwBhAHQAMwAoAHQAaQBtAGUAKgAyAC4ALAB0AGkAbQBlACwALQAyAC4AKQA7AA0ACgBmAHIAbwBtACsAPQBmAGwAbwBhAHQAMwAoADAALgAsAHQAaQBtAGUALAAtADIALgApADsADQAKAC8ALwBmAGwAbwBhAHQAMgAgAHAAZQByAHMAcAAgAD0AIABmAGwAbwBhAHQAMgAoAF8AUwBjAHIAZQBlAG4AUABhAHIAYQBtAHMALgB4AC8AXwBTAGMAcgBlAGUAbgBQAGEAcgBhAG0AcwAuAHoAIAAqACAAMgAuACAALQAgADEALgAsACAAMQApADsADQAKAC8ALwBmAHIAbwBtACsAPQBmAGwAbwBhAHQAMwAoAHQAaQBtAGUAIAAqACAAcABlAHIAcwBwACwALQAyAC4AKQA7AA0ACgBmAHIAbwBtAC4AeAB6ACAAPQAgAG0AdQBsACgAZgByAG8AbQAuAHgAegAsACAAcgBvAHQAMQApADsADQAKAGYAcgBvAG0ALgB4AHkAIAA9ACAAbQB1AGwAKABmAHIAbwBtAC4AeAB5ACwAIAByAG8AdAAyACkAOwANAAoADQAKAC8ALwB2AG8AbAB1AG0AZQB0AHIAaQBjACAAcgBlAG4AZABlAHIAaQBuAGcADQAKAGYAbABvAGEAdAAgAHMAPQAwAC4AMQAsAGYAYQBkAGUAPQAxAC4AOwANAAoAZgBsAG8AYQB0ADMAIAB2AD0AKABmAGwAbwBhAHQAMwApADAALgA7AA0ACgBmAG8AcgAgACgAaQBuAHQAIAByAD0AMAA7ACAAcgA8AHYAbwBsAHMAdABlAHAAcwA7ACAAcgArACsAKQAgAHsADQAKAAkAZgBsAG8AYQB0ADMAIABwAD0AZgByAG8AbQArAHMAKgBkAGkAcgAqAC4ANQA7AA0ACgAJAHAAIAA9ACAAYQBiAHMAKAAoAGYAbABvAGEAdAAzACkAdABpAGwAZQAtACgAcAAlACgAZgBsAG8AYQB0ADMAKQAoAHQAaQBsAGUAKgAyAC4AKQApACkAOwAgAC8ALwAgAHQAaQBsAGkAbgBnACAAZgBvAGwAZAANAAoACQBmAGwAbwBhAHQAIABwAGEALABhAD0AcABhAD0AMAAuADsADQAKAAkAZgBvAHIAIAAoAGkAbgB0ACAAaQA9ADAAOwAgAGkAPABpAHQAZQByAGEAdABpAG8AbgBzADsAIABpACsAKwApACAAewAgAA0ACgAJAAkAcAA9AGEAYgBzACgAcAApAC8AZABvAHQAKABwACwAcAApAC0AZgBvAHIAbQB1AHAAYQByAGEAbQA7ACAALwAvACAAdABoAGUAIABtAGEAZwBpAGMAIABmAG8AcgBtAHUAbABhAA0ACgAJAAkAYQArAD0AYQBiAHMAKABsAGUAbgBnAHQAaAAoAHAAKQAtAHAAYQApADsAIAAvAC8AIABhAGIAcwBvAGwAdQB0AGUAIABzAHUAbQAgAG8AZgAgAGEAdgBlAHIAYQBnAGUAIABjAGgAYQBuAGcAZQANAAoACQAJAHAAYQA9AGwAZQBuAGcAdABoACgAcAApADsADQAKAAkAfQANAAoACQBmAGwAbwBhAHQAIABkAG0APQBtAGEAeAAoADAALgAsAGQAYQByAGsAbQBhAHQAdABlAHIALQBhACoAYQAqAC4AMAAwADEAKQA7ACAALwAvAGQAYQByAGsAIABtAGEAdAB0AGUAcgANAAoACQBhACoAPQBhACoAYQA7ACAALwAvACAAYQBkAGQAIABjAG8AbgB0AHIAYQBzAHQADQAKAAkAaQBmACAAKAByAD4ANgApACAAZgBhAGQAZQAqAD0AMQAuAC0AZABtADsAIAAvAC8AIABkAGEAcgBrACAAbQBhAHQAdABlAHIALAAgAGQAbwBuACcAdAAgAHIAZQBuAGQAZQByACAAbgBlAGEAcgANAAoACQAvAC8AdgArAD0AZgBsAG8AYQB0ADMAKABkAG0ALABkAG0AKgAuADUALAAwAC4AKQA7AA0ACgAJAHYAKwA9AGYAYQBkAGUAOwANAAoACQB2ACsAPQBmAGwAbwBhAHQAMwAoAHMALABzACoAcwAsAHMAKgBzACoAcwAqAHMAKQAqAGEAKgBiAHIAaQBnAGgAdABuAGUAcwBzACoAZgBhAGQAZQA7ACAALwAvACAAYwBvAGwAbwByAGkAbgBnACAAYgBhAHMAZQBkACAAbwBuACAAZABpAHMAdABhAG4AYwBlAA0ACgAJAGYAYQBkAGUAKgA9AGQAaQBzAHQAZgBhAGQAaQBuAGcAOwAgAC8ALwAgAGQAaQBzAHQAYQBuAGMAZQAgAGYAYQBkAGkAbgBnAA0ACgAJAHMAKwA9AHMAdABlAHAAcwBpAHoAZQA7AA0ACgB9AA0ACgB2AD0AbABlAHIAcAAoACgAZgBsAG8AYQB0ADMAKQAoAGwAZQBuAGcAdABoACgAdgApACkALAB2ACwAcwBhAHQAdQByAGEAdABpAG8AbgApADsAIAAvAC8AYwBvAGwAbwByACAAYQBkAGoAdQBzAHQADQAKAHIAZQB0AHUAcgBuACAAZgBsAG8AYQB0ADQAKAB2ACoALgAwADEALAAxAC4AKQA7AA==,output:3,fname:StarNest,width:669,height:798,input:1,input:0,input:0,input_1_label:fragCoord,input_2_label:zoom,input_3_label:speed|A-6868-UVOUT,B-5812-OUT,C-2671-OUT;n:type:ShaderForge.SFN_ScreenPos,id:5714,x:32057,y:32287,varname:node_5714,prsc:2,sctp:1;n:type:ShaderForge.SFN_ValueProperty,id:5812,x:31136,y:32566,ptovrint:False,ptlb:Zoom,ptin:_Zoom,varname:node_5812,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.8;n:type:ShaderForge.SFN_ValueProperty,id:2671,x:31136,y:32662,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_2671,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_RemapRange,id:3180,x:32267,y:32319,varname:node_3180,prsc:2,frmn:0,frmx:1,tomn:1,tomx:0.5|IN-5714-V;n:type:ShaderForge.SFN_Multiply,id:4840,x:32541,y:32457,varname:node_4840,prsc:2|A-3180-OUT,B-6489-OUT;n:type:ShaderForge.SFN_Vector1,id:1491,x:31245,y:32029,varname:node_1491,prsc:2,v1:30;n:type:ShaderForge.SFN_TexCoord,id:6868,x:31392,y:32277,varname:node_6868,prsc:2,uv:0,uaff:False;proporder:5812-2671;pass:END;sub:END;*/

Shader "Shader Forge/Space" {
    Properties {
        _Zoom ("Zoom", Float ) = 0.8
        _Speed ("Speed", Float ) = 0.1
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
            float4 StarNest( float2 fragCoord , float zoom , float speed ){
            const int iterations = 17;
            const float formuparam = 0.53;
            
            const int volsteps = 20;
            const float stepsize = 0.1;
            
            //const float zoom = 0.800;
            const float tile = 0.850;
            //const float speed = 0.010;
            
            const float brightness = 0.0015;
            const float darkmatter = 0.300;
            const float distfading = 0.730;
            const float saturation = 0.850;
            
            //get coords and direction
            float2 uv=fragCoord.xy/_ScreenParams.xy-.5;
            uv.y*=_ScreenParams.y/_ScreenParams.x;
            float3 dir=float3(uv*zoom,1.);
            float time=_Time.y*speed+.25;
            
            //rotation
            float a1=0.5;
            float a2=0.8;
            float2x2 rot1=float2x2(cos(a1),sin(a1),-sin(a1),cos(a1));
            float2x2 rot2=float2x2(cos(a2),sin(a2),-sin(a2),cos(a2));
            dir.xz = mul(dir.xz, rot1);
            dir.xy = mul(dir.xy, rot2);
            float3 from=float3(1.,.5,0.5);
            //from+=float3(time*2.,time,-2.);
            from+=float3(0.,time,-2.);
            //float2 persp = float2(_ScreenParams.x/_ScreenParams.z * 2. - 1., 1);
            //from+=float3(time * persp,-2.);
            from.xz = mul(from.xz, rot1);
            from.xy = mul(from.xy, rot2);
            
            //volumetric rendering
            float s=0.1,fade=1.;
            float3 v=(float3)0.;
            for (int r=0; r<volsteps; r++) {
            	float3 p=from+s*dir*.5;
            	p = abs((float3)tile-(p%(float3)(tile*2.))); // tiling fold
            	float pa,a=pa=0.;
            	for (int i=0; i<iterations; i++) { 
            		p=abs(p)/dot(p,p)-formuparam; // the magic formula
            		a+=abs(length(p)-pa); // absolute sum of average change
            		pa=length(p);
            	}
            	float dm=max(0.,darkmatter-a*a*.001); //dark matter
            	a*=a*a; // add contrast
            	if (r>6) fade*=1.-dm; // dark matter, don't render near
            	//v+=float3(dm,dm*.5,0.);
            	v+=fade;
            	v+=float3(s,s*s,s*s*s*s)*a*brightness*fade; // coloring based on distance
            	fade*=distfading; // distance fading
            	s+=stepsize;
            }
            v=lerp((float3)(length(v)),v,saturation); //color adjust
            return float4(v*.01,1.);
            }
            
            uniform float _Zoom;
            uniform float _Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
////// Lighting:
////// Emissive:
                float3 emissive = ((float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).g*-0.5+1.0)*StarNest( i.uv0 , _Zoom , _Speed )).rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

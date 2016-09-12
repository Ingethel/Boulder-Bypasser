Shader "Custom/CaveShader" {
	Properties{
	_MainTex("Texture", 2D) = "white" {}
	_HeightMap("Texture", 2D) = "grey" {}
	_Scale("Extrusion Amount", Range(0,25)) = 0.5
	}
		SubShader{
		Tags { "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		
		struct Input {
			float2 uv_MainTex;
		};

		float randomNum(in float2 uv) {
			float2 noise = (frac(sin(dot(uv ,float2(12.9898,78.233)*2.0)) * 43758.5453));
			return abs(noise.x + noise.y) * 0.5;
		}

		float _Scale;
		sampler2D _HeightMap;
		void vert(inout appdata_full v) {
			#if !defined(SHADER_API_OPENGL)
			float4 tex = tex2Dlod(_HeightMap, float4(v.texcoord.xy, 0, 0));
			v.vertex.xyz += v.normal * tex.a * _Scale;
			#endif
		}

		sampler2D _MainTex;
		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).a;
		}
		ENDCG
	}

		Fallback "Diffuse"
}
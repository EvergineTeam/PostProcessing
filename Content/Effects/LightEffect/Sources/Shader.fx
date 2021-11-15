[Begin_ResourceLayout]

	cbuffer Base : register(b0)
	{
		float4x4 WorldViewProj	: packoffset(c0);	[WorldViewProjection]
		float4x4 World 			: packoffset(c4);   [World]
	};

	cbuffer Matrices : register(b1)
	{
		float3 Color			: packoffset(c0.x);   [Default(0.3, 0.3, 1.0)]
		float Intensity 		: packoffset(c0.w);   [Default(1.0)]
		float Roughness			: packoffset(c1.x);   [Default(0.1)]
		float Metallic			: packoffset(c1.y);   [Default(0.3)]
	};

[End_ResourceLayout]

[Begin_Pass:ZPrePass]
	[profile 10_0]
	[entrypoints VS = VS PS = PS]
	
	struct VS_IN
	{
		float4 Position : POSITION;
		float3 Normal	: NORMAL;		
	};

	struct PS_IN
	{
		float4 Position : SV_POSITION;
		float3 NormalWS	: NORMAL;		
	};

	
	PS_IN VS(VS_IN input)
	{
		PS_IN output = (PS_IN)0;
			
		output.Position = mul(input.Position, WorldViewProj);
		output.NormalWS = mul(float4(input.Normal, 0), World).xyz;				
	
		return output;
	}
	
	float2 OctWrap( float2 v )
	{
	    return ( 1.0 - abs( v.yx ) ) * ( v.xy >= 0.0 ? 1.0 : -1.0 );
	}
	
	float2 Encode( float3 n )
	{
	    n /= ( abs( n.x ) + abs( n.y ) + abs( n.z ) );
	    n.xy = n.z >= 0.0 ? n.xy : OctWrap( n.xy );
	    n.xy = n.xy * 0.5 + 0.5;
	    return n.xy;
	}
	
	float4 PS(PS_IN input) : SV_Target
	{		
		return float4(Encode(input.NormalWS),Roughness, Metallic);
	}
	
[End_Pass]

[Begin_Pass:Default]
	[profile 10_0]
	[entrypoints VS=VS PS=PS]

	struct VS_IN
	{
		float4 Position : POSITION;
		float3 Normal	: NORMAL;
		float2 TexCoord : TEXCOORD;
	};

	struct PS_IN
	{
		float4 pos : SV_POSITION;
		float3 Nor	: NORMAL;
		float2 Tex : TEXCOORD;
	};

	PS_IN VS(VS_IN input)
	{
		PS_IN output = (PS_IN)0;

		output.pos = mul(input.Position, WorldViewProj);
		output.Nor = input.Normal;
		output.Tex = input.TexCoord;

		return output;
	}

	float4 PS(PS_IN input) : SV_Target
	{
		return float4(Color * Intensity,1);
	}

[End_Pass]

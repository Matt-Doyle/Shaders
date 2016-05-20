#define FarZ 500.0

#define MinPrecision 0.001

//#define AA 1

float sdSphere(vec3 p, float r) {
    //vec3 t = fract(p) * 2.0 - 1.0;
    return length(p) - r;
}

float sdPlane(vec3 p, float yOffset) {
    return p.y + yOffset;
}

float map(vec3 p) {
	return min(sdSphere(p, 2.0), sdPlane(p, 1.0));
}

vec2 trace(vec3 o, vec3 r) { // Traces a ray from origin o in direction r
    float t = 0.0;
    int iterations = 0;
    for (int i = 0; i < 64; i++) {
    	vec3 p = o + r * t;
        t += map(p) * 0.5;
        if (t > FarZ || t < MinPrecision) {
        	break;
        }
        iterations = i;
    }
    
    return vec2(t, iterations);
}


mat2 calculateRotationMatrix(float angle) { // Calculates a basic rotation matrix
    return mat2(cos(angle), -sin(angle), sin(angle), cos(angle));
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    float aspectRatio = iResolution.y / iResolution.x;
	vec2 uv = fragCoord.xy / iResolution.xy;
    uv = uv * 2.0 - 1.0;
    uv.x = uv.x / aspectRatio;
    //uv *= sin(iGlobalTime) * 2.0 + 4.0;
	//fragColor = vec4(uv,0.5+0.5*sin(iGlobalTime),1.0);
    
    float angle = iGlobalTime;
    mat2 rot = calculateRotationMatrix(angle);
    
    vec3 origin = vec3(iGlobalTime / 4.0, iGlobalTime / 4.0, -3);
    vec3 ray = normalize(vec3(uv, 1));
    //ray.xz *= rot;
    ray.yz *= rot;
    
    vec2 traceData = trace(origin, ray);
    float col = traceData.x;
    
    #ifdef AA
    if (traceData.y > 30.0) { // Basically MSAA
        for (int i = 0; i < 4; i++) {
            ray = normalize(vec3(uv + float(i) / 10000.0, 1));
        	col += trace(origin, ray).x;
        }
    	col /= 5.0;
    }
    #endif
    
    col = 1.0 / col;
    
    //col = 1.0 / traceData.x; // Intensity increases inversely proportionate to the square of how far the shape is away
    
    fragColor = vec4(col, col, col, 1.0);
}
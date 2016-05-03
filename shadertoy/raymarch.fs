float sdSphere(vec3 p, float r) {
    vec3 t = fract(p) * 2.0 - 1.0;
    return length(t) - r;
}

float map(vec3 p) {
	return sdSphere(p, 0.4);
}

float trace(vec3 o, vec3 r) { // Traces a ray from origin o in direction r
    float t = 0.0;
    for (int i = 0; i < 64; i++) {
    	vec3 p = o + r * t;
        t += map(p) * 0.5;
    }
    
    return t;
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
    
    float col = trace(origin, ray);
    
    col = 1.0 / col; // Intensity increases inversely proportionate to the square of how far the shape is away
    
    fragColor = vec4(col, col * 2.0, col, 1.0);
}
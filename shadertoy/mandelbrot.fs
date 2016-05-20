/*
Origin = 0 + 0i
Delta X = Reals
Delta Y = Imaginaries

let Z = 0 + 0i
let C = mapped cell
Z new = Z^2 + C
*/


float mapMandelBrot(vec2 uv) {
    uv = fract(uv) * 4.0 - 2.0;
    vec2 Z = vec2(0, 0);
    
    int iterations = 0;
    
    for (int i = 0; i < 1000; i++) {
        if (length(Z) > 2.0) {
            break;
        }
        //x*x + 2*x*y + y*y
    	Z = vec2(Z.x*Z.x + -(Z.y*Z.y), 2.0*Z.x*Z.y) + uv;
        iterations = i;
        
    }
    
    return float(iterations);
}

mat2 calcRotMatrix(float theta) {
	return mat2(cos(theta), -sin(theta), sin(theta), cos(theta));
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    float aspectRatio = iResolution.y / iResolution.x;
	vec2 uv = fragCoord.xy / iResolution.xy;
    uv = uv * 2.0 - 1.0;
    uv.x = uv.x / aspectRatio;
    //uv.x += iGlobalTime;
    uv = uv * calcRotMatrix(iGlobalTime);
    
    float mappedMandelBrot = mapMandelBrot(uv);
    
    vec3 col = vec3(mappedMandelBrot / 100.0, mappedMandelBrot / 10.0, mappedMandelBrot / 2.0); 
    
	fragColor = vec4(col, 1.0);
}
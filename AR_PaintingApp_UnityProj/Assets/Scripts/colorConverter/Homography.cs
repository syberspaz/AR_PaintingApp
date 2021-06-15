using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homography : MonoBehaviour
{
    /*
    [Range(0.0f, 1.0f)]
    public float x = 0.9505f;
    [Range(0.0f, 1.0f)]
    public float y = 1.0f;
    [Range(0.0f, 1.0f)]
    public float z = 1.00f;
    */
    public Color[] inputColors = new Color[4];
    public Color[] desiredColors = new Color[4];
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    float gammaCIEtoRgb(float rgbVal){
        if(rgbVal <= 0.0031308f){
            return rgbVal * 12.92f;
        }

        return (1.055f * Mathf.Pow(rgbVal, (1.0f/2.4f)) - 0.055f);
    }

    Color CIEtoRGB(float x, float y, float z){
        float r = 3.2404542f * x - 1.5371385f * y - 0.4985314f * z;
        float g = -0.9692660f * x + 1.8760108f * y + 0.0415560f * z;
        float b = 0.0556434f * x  - 0.2040259f * y + 1.0572252f * z;

        r = gammaCIEtoRgb(r);
        g = gammaCIEtoRgb(g);
        b = gammaCIEtoRgb(b);
        return new Color(r, g, b);
    }

    float gammaRgbToCIE(float rgbValue){
        if(rgbValue <= 0.04045f){
            return (rgbValue / 12.92f);
        }

        float temp = (rgbValue + 0.055f);
        temp = temp / 1.055f;

        return Mathf.Pow(temp, 2.4f);
    }

    Vector3 RGBtoCIE(Color rgb){
        
        float currentR = gammaRgbToCIE(rgb.r);
        float currentG = gammaRgbToCIE(rgb.g);
        float currentB = gammaRgbToCIE(rgb.b);
        float x = 0.4124564f * currentR + 0.3575761f * currentG + 0.1804375f * currentB;
        float y = 0.2126729f * currentR + 0.7151522f * currentG + 0.0721750f * currentB;
        float z = 0.0193339f * currentR + 0.1191920f * currentG + 0.9503041f * currentB;

        return new Vector3(x, y, z);
    }
    
    // Copyright (C) 2012 Chirag Raman 
    // GNU General Public License
    //https://github.com/chiragraman/Unity3DProjectionMapping/blob/c9fba8f1bf9b6b5d0a66722d1b2bb3ba55f5b3ea/Assets/Scripts/Homography.cs#L76
    Matrix4x4 FindHomography(Vector3[] src, Vector3[] dest){
        Matrix4x4 result = Matrix4x4.zero;

        
        	float[,] P = new float [,]{  
        {-src[0].x, -src[0].y, -1,   0,   0,  0, src[0].x*dest[0].x, src[0].y*dest[0].x, -dest[0].x }, // h11  
        {  0,   0,  0, -src[0].x, -src[0].y, -1, src[0].x*dest[0].y, src[0].y*dest[0].y, -dest[0].y }, // h12  
          
        {-src[1].x, -src[1].y, -1,   0,   0,  0, src[1].x*dest[1].x, src[1].y*dest[1].x, -dest[1].x }, // h13  
        {  0,   0,  0, -src[1].x, -src[1].y, -1, src[1].x*dest[1].y, src[1].y*dest[1].y, -dest[1].y }, // h21  
          
        {-src[2].x, -src[2].y, -1,   0,   0,  0, src[2].x*dest[2].x, src[2].y*dest[2].x, -dest[2].x }, // h22  
        {  0,   0,  0, -src[2].x, -src[2].y, -1, src[2].x*dest[2].y, src[2].y*dest[2].y, -dest[2].y }, // h23  
          
        {-src[3].x, -src[3].y, -1,   0,   0,  0, src[3].x*dest[3].x, src[3].y*dest[3].x, -dest[3].x }, // h31  
        {  0,   0,  0, -src[3].x, -src[3].y, -1, src[3].x*dest[3].y, src[3].y*dest[3].y, -dest[3].y }, // h32  
    	};  
		

        GaussianElimination(ref P,9);
        for(int i = 0; i < 4; i ++){
            result.SetRow(i,new Vector4(P[i,0], P[i,1], P[i,2], P[i,3]));
        }
        return result;
    }


    
    
    
    // Copyright (C) 2012 Chirag Raman 
    // GNU General Public License
    //https://github.com/chiragraman/Unity3DProjectionMapping/blob/c9fba8f1bf9b6b5d0a66722d1b2bb3ba55f5b3ea/Assets/Scripts/Homography.cs#L76
	void GaussianElimination (ref float[,] A, int n)
	{
		// originally by arturo castro - 08/01/2010  
	    //  
	    // ported to c from pseudocode in  
	    // http://en.wikipedia.org/wiki/Gaussian_elimination  
	      
	    int i = 0;  
	    int j = 0;  
	    int m = n-1;  
	    while (i < m && j < n){  
	        // Find pivot in column j, starting in row i:  
	        int maxi = i;  
	        for(int k = i+1; k<m; k++){  
	            if(Mathf.Abs(A[k,j]) > Mathf.Abs(A[maxi,j])){  
	                maxi = k;  
	            }  
	        }  
	        if (A[maxi,j] != 0){  
	            //swap rows i and maxi, but do not change the value of i  
	            if(i!=maxi)  
	                for(int k=0;k<n;k++){  
	                    float aux = A[i,k];  
	                    A[i,k]=A[maxi,k];  
	                    A[maxi,k]=aux;  
	                }  
	            //Now A[i,j] will contain the old value of A[maxi,j].  
	            //divide each entry in row i by A[i,j]  
	            float A_ij=A[i,j];  
	            for(int k=0;k<n;k++){  
	                A[i,k]/=A_ij;  
	            }  
	            //Now A[i,j] will have the value 1.  
	            for(int u = i+1; u< m; u++){  
	                //subtract A[u,j] * row i from row u  
	                float A_uj = A[u,j];  
	                for(int k=0;k<n;k++){  
	                    A[u,k]-=A_uj*A[i,k];  
	                }  
	                //Now A[u,j] will be 0, since A[u,j] - A[i,j] * A[u,j] = A[u,j] - 1 * A[u,j] = 0.  
	            }  
	              
	            i++;  
	        }  
	        j++;  
	    }  
	      
	    //back substitution  
	    for(int k=m-2;k>=0;k--){  
	        for(int l=k+1;l<n-1;l++){  
	            A[k,m]-=A[k,l]*A[l,m];  
	            //A[i*n+j]=0;  
	        }  
	    }  
	}

    // Update is called once per frame
    void Update()
    {
        Vector3 firstColor = RGBtoCIE(inputColors[0]);
        Vector3 secondColor = RGBtoCIE(inputColors[1]);

        Vector3 thirdColor = Vector3.Cross(firstColor, secondColor);

        Color resultColor = CIEtoRGB(thirdColor.x, thirdColor.y, thirdColor.z);


        rend.material.color = resultColor;



    }
}

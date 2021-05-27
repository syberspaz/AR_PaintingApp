Hi! 

The painterly look of this model requires the following things to be true in order to appear as intended. 

Both materials must be set to shadeless rendering. They should ignore light conditions or at least not produce shadows.

There are two materials included an inner and outer shell both of these materials use the same colour texture for albedo.
Use the Outer Shell transparency texture on the outershell material in order to achieve the effect of a broken painterly silhouette.

the inner part must be set to enable back face culling otherwise known as single sided rendering.
 finally the outer shell needs to be left without backface culling enabled.

the subtle addition of a sharpen post-processing effect can add the perfect final touch.

		~~ Jack
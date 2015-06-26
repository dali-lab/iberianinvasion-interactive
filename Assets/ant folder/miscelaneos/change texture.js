var personaje : GameObject ;
var gSkin : GUISkin;
var texture_a : Texture;
var texture_b : Texture;
var texture_c : Texture;

var texture_d : Texture;
/*
var texture_e : Texture;
var texture_f : Texture;
*/


function OnGUI(){


if(gSkin)
GUI.skin = gSkin;
else
Debug.Log("StartMenuGUI : GUI Skin object missing!");



 if (GUI.Button( Rect( 850, 25, 90,40), "texture 01"))
{ 
personaje.GetComponent.<Renderer>().material.mainTexture = texture_a;
 }



 if (GUI.Button( Rect( 850, 75, 90,40), "texture 02"))
{ 
personaje.GetComponent.<Renderer>().material.mainTexture = texture_b;
 }


 if (GUI.Button( Rect( 850, 125, 90,40), "texture 03"))
{ 
personaje.GetComponent.<Renderer>().material.mainTexture = texture_c;
 }

 if (GUI.Button( Rect( 850, 175, 90,40), "texture 04"))
{ 
personaje.GetComponent.<Renderer>().material.mainTexture = texture_d;
 }
/*
 if (GUI.Button( Rect( 850, 225, 90,40), "texture 05"))
{ 
renderer.material.mainTexture = texture_e;
 }

 if (GUI.Button( Rect( 850, 275, 90,40), "texture 06"))
{ 
renderer.material.mainTexture = texture_f;
 }
*/

}


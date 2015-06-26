var gSkin : GUISkin;
var personaje : GameObject ;






function OnGUI(){


if(gSkin)
GUI.skin = gSkin;
else
Debug.Log("StartMenuGUI : GUI Skin object missing!");



 if (GUI.Button( Rect( 5, 50, 80, 40), "idle1"))
{  personaje.GetComponent.<Animation>().CrossFade("idle1", 0.2);
 }
 
 
 
 if (GUI.Button( Rect( 5, 100, 80, 40), "idle2"))
{    personaje.GetComponent.<Animation>().CrossFade("idle2", 0.2);
  }
   //else
   // {  personaje.animation.CrossFade ("idle"); }

  if (GUI.Button( Rect( 5, 150, 80, 40), "walk"))
{  personaje.GetComponent.<Animation>().CrossFade("walk", 0.2);
 } 
 
   if (GUI.Button( Rect( 5, 200, 80, 40), "run"))
{  personaje.GetComponent.<Animation>().CrossFade("run", 0.2);

 } 
 
 
 
  if (GUI.Button( Rect( 5, 250, 80, 40), "damage1"))
{ personaje.GetComponent.<Animation>().CrossFade("damage1", 0.2);
dieA (); 
 }
 
   if (GUI.Button( Rect( 5, 300, 80, 40), "damage2"))
{ personaje.GetComponent.<Animation>().CrossFade("damage2", 0.2);
dieA (); 
 }
 
   if (GUI.Button( Rect( 5, 350, 80, 40), "death"))
{ personaje.GetComponent.<Animation>().CrossFade("death", 0.2);
dieA (); 
 }
 
   if (GUI.Button( Rect( 5, 400, 80, 40), "attack"))
{ personaje.GetComponent.<Animation>().CrossFade("attack", 0.2);
dieA (); 
 }
 

 
}



///////////////// ----------------------------------


function dieA () {
yield WaitForSeconds (4.11);
personaje.GetComponent.<Animation>().CrossFade("idle1", 0.2);
}





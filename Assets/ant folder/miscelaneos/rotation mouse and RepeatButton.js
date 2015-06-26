/*
var horizontalSpeed : float = 2.0;
var verticalSpeed : float = 2.0;
function Update () {
    // Get the mouse delta. This is not in the range -1...1
    var h : float = horizontalSpeed * Input.GetAxis ("Mouse X");
    var v : float = verticalSpeed * Input.GetAxis ("Mouse Y");
    transform.Rotate (v, h, 0);
}
*/

var object3d: GameObject;
var gSkin : GUISkin;

function OnGUI(){


if(gSkin)
GUI.skin = gSkin;
else
Debug.Log("StartMenuGUI : GUI Skin object missing!");



 if (GUI.RepeatButton( Rect( 515, 700, 100, 50), "rotation <<"))
{ 
object3d.transform.Rotate (0, 1, 0);
 }
 
  if (GUI.RepeatButton( Rect( 625, 700, 100, 50), "rotation >>"))
{ 
object3d.transform.Rotate (0, -1, 0);
 }
 
  }
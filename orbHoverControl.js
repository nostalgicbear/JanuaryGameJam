#pragma strict

 var floatup;
 function Start(){
     floatup = false;
 }
 function Update(){
     if(floatup)
         floatingup();
     else if(!floatup)
         floatingdown();
 }
 
 function floatingup(){
 
 
 	
     transform.position.y += 0.15 * Time.deltaTime;
     yield WaitForSeconds(1);
     floatup = false;
 }
 function floatingdown(){
     transform.position.y -= 0.15 * Time.deltaTime;;
     yield WaitForSeconds(1);
     floatup = true;
 }
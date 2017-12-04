 using UnityEngine;
 
 public class Menu : MonoBehaviour {
 
     
     RectTransform menu;
     public float speed;
 
     void Start()
     {
         menu = gameObject.GetComponent<RectTransform>();
         speed = 0f;//-1f;
     }
 
     void Update () {
     	Debug.Log(menu.position.y);
         	transform.Translate(0f, speed, 0f);
         	if (menu.position.y < 400f) 
         	{
         		speed = 0f;
         		menu.position = new Vector3(640f,400f,0f);
         	}
         	if (menu.position.y > 1201f) 
         	{
         		speed = 0f;
         		menu.position = new Vector3(640f,1200f,0f);
         	}
             	
            
     }

     public void updatespeed(float value)
     {
     	speed = value;
     }


 }
/*------------------------------------------------------*
*CREATED BY:											*
*Name: David Tiscareno									*
*Class: GSP362											*
*Team B Course Project									*
*Date: 9/18/13											*
*Daylight.cs											*
*-------												*
*This file contains the implementation of a daylight	*
*system which increments time every update. It then		*
*changes the light intensity and orientation every		*
*update.												*
*-------------------------------------------------------*/
 
using UnityEngine;
using System.Collections;

public class Daylight : MonoBehaviour {
	
	//Constants for the amount of time incremented each update and the maximum light from the sun
	public const float TIME_INCREMENT = 0.0001f;
	public const float LIGHT_MAX = 0.4f;
	
	//These booleans tell if it is day/night and if we have reached midday
	bool day,midday; 
	
	//This float will keep time during nighttime
	float nightTimer;
	
	//This is the light object itself
	GameObject daylightObject;
	
	// Use this for initialization
	void Start ()
	{
		//Initialize the values
		day = true;
		midday = false;
		nightTimer = 0;
		
		//The next four lines of code come from docs.unity3d.com
		daylightObject = new GameObject("sunlight");
        daylightObject.AddComponent<Light>();
		daylightObject.light.type = LightType.Directional;
        daylightObject.light.color = Color.white;
		
		//Begin the light from the east with a zero intensity
		daylightObject.light.intensity = 0;
        daylightObject.transform.position = new Vector3(0, 0, 0);
		daylightObject.transform.LookAt(new Vector3(-1,0,0));
	}
	
	// Update is called once per frame
	void Update ()
	{
		//If it is daytime
		if (day)
		{
			//If we have not hit midday, increase the intensity
			if (!midday)
				daylightObject.light.intensity += TIME_INCREMENT;
			//Otherwise decrease the intensity
			else
				daylightObject.light.intensity -= TIME_INCREMENT;
			
			//Calculate the orientation of the light dependent on the time of day
			float cosineVal = daylightObject.light.intensity * (1.0f/LIGHT_MAX) * (Mathf.PI/2.0f);
			cosineVal = Mathf.Cos(cosineVal);
			float sineVal = daylightObject.light.intensity * (1.0f/LIGHT_MAX) * (Mathf.PI/2.0f);
			sineVal = -(Mathf.Sin(sineVal));
			
			//If we are not at midday yet, have the sun still come from an eastward direction
			if (!midday)
				cosineVal = -cosineVal;
			
			//Set the orientation
			daylightObject.transform.LookAt(new Vector3(cosineVal,sineVal,0));
			
			//If we have hit midday, set the variables properly
			if (!midday && daylightObject.light.intensity >= LIGHT_MAX)
			{
				daylightObject.light.intensity = 0.4f;
				midday = true;
			}
			//If we have hit nighttime, set the variables properly
			if (midday && daylightObject.light.intensity <= 0)
			{
				daylightObject.light.intensity = 0;
				day = false;
				midday = false;
				//Reset the light orientation
				daylightObject.transform.LookAt(new Vector3(-1,0,0));
			}
		}
		else
		{
			//Increment the timer
			nightTimer += TIME_INCREMENT;
			
			//If we have reached daytime, set the variables properly
			if (nightTimer >= LIGHT_MAX * 2)
			{
				day = true;
				nightTimer = 0;
			}
		}
	}
}

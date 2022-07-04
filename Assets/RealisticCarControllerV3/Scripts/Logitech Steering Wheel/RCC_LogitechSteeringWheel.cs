//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2020 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

#if RCC_LOGITECH
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RCC_LogitechSteeringWheel : MonoBehaviour{

	#region singleton
	private static RCC_LogitechSteeringWheel instance;
	public static RCC_LogitechSteeringWheel Instance{

		get{

			if (instance == null) {

				instance = FindObjectOfType<RCC_LogitechSteeringWheel> ();

				if (instance == null) {

					GameObject sceneManager = new GameObject ("_RCCLogitechSteeringWheelManager");
					instance = sceneManager.AddComponent<RCC_LogitechSteeringWheel> ();

				}

			}

			return instance;

		}

	}

	#endregion

	//private LogitechGSDK.LogiControllerPropertiesData properties;
	//internal LogitechGSDK.DIJOYSTATE2ENGINES rec;

	//public RCC_Inputs inputs = new RCC_Inputs();

	//public bool useHShifter = true;
	//public bool atNGear = true;

	public bool useForceFeedback = true;
	public float roughness = 70f;
	public float collisionForce = 40f;

    void Start(){

		LogitechGSDK.LogiSteeringInitialize (false);

    }

	void OnEnable(){

		RCC_CarControllerV3.OnRCCPlayerCollision += RCC_CarControllerV3_OnRCCPlayerCollision;

	}

	void RCC_CarControllerV3_OnRCCPlayerCollision (RCC_CarControllerV3 RCC, Collision collision){
		
		if(RCC == RCC_SceneManager.Instance.activePlayerVehicle)
			LogitechGSDK.LogiPlayFrontalCollisionForce (0, Mathf.CeilToInt(collision.impulse.magnitude / 10000f * collisionForce));
		
	}
		
    void Update(){

		if (LogitechGSDK.LogiUpdate () && LogitechGSDK.LogiIsConnected (0)) {

			//RCC_InputManager.logitechSteeringUsed = true;

			//rec = LogitechGSDK.LogiGetStateUnity (0);

			//if(useHShifter)
			//	HShifter (rec);

			if (useForceFeedback)
				ForceFeedback ();

			//inputs.steerInput = Mathf.Clamp(rec.lX / 32768f, -1f, 1f);
			//inputs.throttleInput = Mathf.Clamp01 (rec.lY / -32768f);
			//inputs.brakeInput = Mathf.Clamp01 ((1f - Mathf.Abs(rec.lRz / -32768f)));
			//inputs.clutchInput = Mathf.Clamp01 (rec.rglSlider [0] / -32768f);

		}
        
    }

	//void HShifter(LogitechGSDK.DIJOYSTATE2ENGINES shifter){

	//	bool atGear = false;

	//	for (int i = 0; i < 128; i++) {

	//		if (shifter.rgbButtons [i] == 128) {

	//			switch (i) {

	//			case 12:

	//				inputs.gearInput = 0;
	//				atGear = true;
	//				break;

	//			case 13:

	//				inputs.gearInput = 1;
	//				atGear = true;
	//				break;

	//			case 14:

	//				inputs.gearInput = 2;
	//				atGear = true;
	//				break;

	//			case 15:

	//				inputs.gearInput = 3;
	//				atGear = true;
	//				break;

	//			case 16:

	//				inputs.gearInput = 4;
	//				atGear = true;
	//				break;

	//			case 17:

	//				inputs.gearInput = 5;
	//				atGear = true;
	//				break;

	//			case 11:

	//				inputs.gearInput = -1;
	//				atGear = true;
	//				break;

	//			}
					
	//		}

	//	}

	//	atNGear = !atGear;

	//}

	void ForceFeedback(){
		
		RCC_CarControllerV3 playerVehicle = RCC_SceneManager.Instance.activePlayerVehicle;

		if (!playerVehicle)
			return;

		float sidewaysForce = playerVehicle.FrontLeftWheelCollider.wheelSlipAmountSideways + playerVehicle.FrontRightWheelCollider.wheelSlipAmountSideways;
		sidewaysForce *= Mathf.Abs(sidewaysForce);
		sidewaysForce *= -roughness;

		LogitechGSDK.LogiStopConstantForce(0);
		LogitechGSDK.LogiPlayConstantForce (0, (int)(sidewaysForce));
		 
	}

	//public static bool GetKeyTriggered(int controllerIndex, int keycode){

	//	return LogitechGSDK.LogiButtonTriggered (controllerIndex, keycode);

	//}

	//public static bool GetKeyPressed(int controllerIndex, int keycode){

	//	return LogitechGSDK.LogiButtonIsPressed (controllerIndex, keycode);

	//}

	//public static bool GetKeyReleased(int controllerIndex, int keycode){

	//	return LogitechGSDK.LogiButtonReleased (controllerIndex, keycode);

	//}

	void OnDisable(){

		RCC_CarControllerV3.OnRCCPlayerCollision -= RCC_CarControllerV3_OnRCCPlayerCollision;

	}

}
#endif
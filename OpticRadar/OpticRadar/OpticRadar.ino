/*
 Name:		OpticRadar.ino
 Created:	9/27/2015 18:05:39
 Author:	Piotr
*/
#include <Servo.h>
#include <SharpIR.h>

#define SensorId 20150

SharpIR sharpIr(A0,80,97,SensorId);
Servo rotateServo;
//uint16_t spaceMap[180];
int8_t incr = 1;
int16_t rotation = 0;
int32_t range;

void setup() {

	
	rotateServo.attach(5);
	pinMode(13, OUTPUT);
	Serial.begin(56000);
	
}

void loop() {
	
	
	//for (byte rotation = 0; rotation < 180; rotation += incr)
	//{
	//if (Serial.available())
	{
		range = sharpIr.distance();
		if (range > 300) range = 200;
		if (range < 20) range = 20;

		if (rotation >= 179) incr = -1;
		if (rotation <= 0) incr = 1;

		rotateServo.write(rotation);
		//spaceMap[rotation] = range;

		rotation += incr;


		delay(10);

		String line = String(rotation) + ';' + String(range);
		Serial.println(line);
		delay(10);
	}

	//}

	
	/*if (range > 20 && range < 200 )
	{
		int conv = map(range,20, 100, 0, 180);
		rotateServo.write(conv);
		Serial.println(range);

	}
	delay(500);*/
	
}

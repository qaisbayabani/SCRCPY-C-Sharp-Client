program.cs contain code to push an scrcpy-server to mobile phone with adb commands and than running server processes to control the stream sent by mobile phone and recovnert it to movie.

in program.cs just change the ip in line No.57  to your mobile ip address. 

ps.Arguments = "connect " + "127.0.0.1:5555"; to your mobile ip address.

TURN ON DEVELOPER OPTIONS ON YOUR MOBILE TO USE SCRCPY


IF UYOU WANT TO DO IT WITHOUT DEVELOPER OPTIONS THAN

A pc client and mobile phone server is written by me is consists of two following files

clientformyserver.cs contain PC client

sra6.java contain server written by me delivering screen capture of mobile phone screen TO the pc client WIOTHOUT USING ADB OR DEVELOPER OPTIONS OF MOBILE. This server has teChnically killed the SCRCPY which needs ADB/Developer Options

debug.apk is app compiled in result of sra6.java

android layouts without using xml is also a feature of sra6.java

multithreaded java server on mobile phone is a feature of sra6.java

scrcpy-server is scrcpy server 1.13


Subscribe and View my Channel to say thanks and get more........:)

https://www.youtube.com/results?search_query=qaisbayabanni

No jumbeled classes and files liabriries just a simple program.cs file.

it covers.

multithreading

a single file program.cs converting smooth capture from scrcpy-server to c# client

adb commands using c#

server port opening

reading server received data and forwarding the stream to other port.

opencv capture from network stream and decode H264 stream from android screen capture.
cross plateform stuff.
enjoy

install nuget package opencv sharp and opencv runtime from visual studio (2022 cummunity at my side).

put scrcpy-server-v1.13 in your visual studio projects bin/debug

download android sdk plateform tools from https://developer.android.com/studio/releases/platform-tools and unzip or copy all files in visual studio projects bin/debug

copy program.cs file

change the name of namespace according to ur environment.

run

may have to change the adb commands according to your own environment.

100% smooth capture result.

Subscribe and View my Channel plz.

https://www.youtube.com/results?search_query=qaisbayabanni

clientformyserver.cs contain PC client for the server written by me


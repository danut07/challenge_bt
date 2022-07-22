# BT Challenge

This repo contains all the projects neccessary for the challenge provided by BT.

A quick glance into this repo and three folders can be seen, as follows:

-Frontend
-TOTP
-TestsTOTP

#Frontend

Starting up with the client side of the challenge, we have a frontend developed with Next.js for Server-Side Rendering using a simple and modular component library called Chakra UI for a very pleasant UI/UX.

The user is shown a nice form with a single Input for UserId, and a button that generates the user's Time-based One-Time Password. The FE makes a HTTP Post Request to the server with two inputs: the UserId as string and a DateTime as Unix TimeStamp. The response provided by the server will be shown at the bottom of the form, which will display the TOTP and the remaining time of it's validity.

#TOTP

Representing the server side of the challenge, made with .Net Core Web Api. The BE contains Model files for RequestToken and ResponseToken, a Service file in which the logic for the TOTP is handled.

Both Model files contains Records for the Request and the Response type that needs to be returned to the FE.

Inside the Service file we can find a class called TotpService which contains properties for the server secret (held hidden in config.ini file on local machine), the length for the TOTP and the maximum time allocated for the validity of the TOTP and methods for generating the TOTP. The logic is structured in different other methods for the ease of reviewing the source code. The following methods make the TOTP to be generated:

-An event counter generator, which calculates the number of time steps between the initial counter time and the current Unix time;
-An unique user input, created by the userId concatenated to the event counter;
-An HMAC SHA512 hash generator used for the UserSecret and the TOTP;
-Multiple methods used as converters required for creating the UserSecret and the TOTP.

!!! Notes !!!

There are a few things that are worth mentioning:

- Even though it is a bad practice, CORS has been disabled because the BE is in a development state.

#TestsTOTP

Which represents the UnitTesting for the backend. The UnitTotpService.cs file contains TestMethods that verifies different scenarios of generating TOTP, expiry time and secret used with different data sets, labeled as good or wrong data sets.

Each data set has 3 records which contains the UserId, the initial Unix TimeStamp(sent by the FE), the TOTP generated and the expiry time in Unix TimeStamp.

# South Tees Hospitals NHS Foundation Trust Barometry App
The Baromery 2 (version 2.4) web app developed by Clinical Measurement in the Medical Physics Department at The James Cook University Hospital, replaces Barometry Version 1. Barometry 2 includes the ability to read the pressure from a Druck Pace 1001 directly over ethernet. 

This readme is mainly only useful for staff at south tees. For an example of how to connect to a Druck Pace 1001 Barometer via ethernet in C#, click [here](https://github.com/AndrewLSimpson/Druck1001Example/blob/main/readme.md).

## What's New?

-	A full re-design of the look and feel of the app, which utilise the NHS Digital Design Library.
-	Complete re-write of the code in C# MVC .Net .
-	Better webscraping of Barometer 1 (Medical Physics Department Barometer) using the htmlagilitypack.
-	Ivi.Visa library interface with the new Druck Pace 1001 barometer for reading VX11. 
-	In-built change log.
-	Display of calibration dates.

## Key Technical Information
This section identifies some of the important technical information for Barometry 2.
### Barometer 1 - Medical Physics Department
The app web scrapes the HTML from Barometer 1 by using the HtmlAgilityPack. Specifically, it reads the HTML div tags ‘pres_mbar’ and ‘pres_mmhg’ and populates the data into a view.
### Barometer 2 - The Endeavour Unit
The web app utilises Ivi.Visa to connect to the Druck Pace 1001 via VXI-11 by using a Visa session TCPIP::10.115.46.84::inst0::INSTR. It then sends the command :SENSe:PRESsure? to the Druck Pace 1001 and receives the mBar payload.

Currently, the mBar is converted into mmHg by using mBar * 0.750061561306. 

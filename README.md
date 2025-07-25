# EconomicEvents

Events that affect the prices of items in selected ports.

### Requires

* [BepInEx 5.4.23](https://github.com/BepInEx/BepInEx/releases)
* [ModSaveBackups 1.1.1](https://thunderstore.io/c/sailwind/p/RadDude/ModSaveBackups/)

## Features

Port dude now will give you news on active events affecting the economies of ports. Once 
you hear what he has to say you will find the active events in your logbook. The details 
in your logbook will tell you which ports the events are in, what items are affected by 
the events, what the price multipliers are, and how much time is remaining for the events.  
<br>
Even if you do not hear about an event prices are still affected by an active event.  
<br>

![Screenshot of port dude with news](https://github.com/bryon82/SailwindEconomicEvents/blob/main/Screenshots/portDude.png)

<br>

![Screenshot of events log](https://github.com/bryon82/SailwindEconomicEvents/blob/main/Screenshots/eventsLog.png)

### How Events Are Scheduled

For normal events, events that only affect one port, each region can only have one event 
scheduled. Every 14th day, each region is checked to see if it has an event. If there is 
not an event scheduled in a region, a check against it's chance level is made to see if 
it will get one. If it doesn't get an event, the chance it gets one made is raised by a 
random amount. If it does get an event scheduled, it's chance level is dropped to its 
baseline level. When an event is scheduled, it is assigned a random day that it will 
become active. Only when an event is active is when port dude will tell you about them.  
<br>
For global events, events that will affect every port, if no region has a scheduled 
event there is a chance that a global event will be scheduled.

## Configurable

* The baseline percent chance amount for a region to get an event scheduled. Default is 20.
* If global events are enabled. Default is true.
* Percent chance amount for a global event. Default is 5.

## Installation

If updating, remove EconomicEvents folders and/or EconomicEvents.dll files from previous installations.  
<br>
Extract the downloaded zip. Inside the extracted EconomicEvents-\<version\> folder copy 
the EconomicEvents folder and paste it into the Sailwind/BepInEx/Plugins folder.  

#### Consider supporting me 🤗

<a href='https://www.paypal.com/donate/?business=WKY25BB3TSH6E&no_recurring=0&item_name=Thank+you+for+your+support%21+I%27m+glad+you+are+enjoying+my+mods%21&currency_code=USD' target='_blank'><img src="https://www.paypalobjects.com/en_US/i/btn/btn_donate_LG.gif" border="0" alt="Donate with PayPal button" />
<a href='https://ko-fi.com/S6S11DDLMC' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://storage.ko-fi.com/cdn/kofi6.png?v=6' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>

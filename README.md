# Google Calendar API's Calendar API

## Summary

This project was designed for studying Google Calendar API, which is available for public usage, and use it in .NET while practicing the framework. This solution has 2 projects, *Calendar* and *Calendar.Domain*.

## Libaries and Frameworks used

This solution uses the following libraries and frameworks:

* NET 6
* AutoMapper
* AutoMapper.Extensions.Microsoft.DependencyInjection
* AWSSDK.SecretsManager
* Google.Apis.Calendar.v3
* Microsoft.AspNet.WebApi.Core

## Projects

### Calendar

The main API which can be called when ran locally, or from the AWS Elastic Beanstalk Endpoint that it is configured to use. This information is currently unavailable for the public.

This project contains is divided into 2 folders and the root classes. The root has the appsettings.json file, which is a default to .NET, and also contains the Program.cs class to initialize the application and also for adding Dependency Injection and services.

#### Controller 

The Controller folder contains the ScheduleController, which is the main and only Controller for the API. This controller contains 5 endpoints.

 * *GET `/schedule/detailed?next=X`* -> This endpoint will return the next "X" events of the selected Google Account, in detail. If the "?next=X" is not specified, the application will return the next 10 events.

 * *GET `/schedule/detailed/datePeriod`* -> This endpoint will return the events that are inside a timespan provided by the caller, and this span must be provided through a JSON body object when executing the call.
	Example for the body:
	```
	{
	  "minDate": "2022-03-04T20:18:24.307Z",
	  "maxDate": "2022-03-04T20:19:24.307Z"
	}
	```
	The date and time must use the ISO DateTime Format.
 * *POST `/schedule/create`* -> Creates an event for the specified account. The event must be sent through a JSON Body object, as described below:
     ```
     {
      "summary": "string",
      "location": "string",
      "description": "string",
      "start": {
        "dateTime": "2022-03-25T00:44:43.138Z",
        "timeZone": "America/Fortaleza"
      },
      "end": {
        "dateTime": "2022-03-25T01:44:43.138Z",
        "timeZone": "America/Fortaleza"
      },
      "recurrence": [
      "RRULE:FREQ=WEEKLY;BYDAY=FR,SA,MO;UNTIL=20220701T170000Z;INTERVAL=2"
      ],
      "attendees": [
        {
          "email": "attendeeemail@gmail.com"
        }
      ],
      "reminders": {
        "overrides": [
          {
            "minutes": 10,
            "method": "popup"
          }
        ],
        "useDefault": false
      }
    }
     ```
  The recurrence will receive a RRULE, which follows the standard for such. FREQ is the frequency of repeating the event, BYDAY the days of the week that it happens, if in more than one day, UNTIL is the limit date, and INTERVAL the amount of time between events (e.g. every 2 weeks)
 * *PUT `/schedule/update/{id}`* -> Updates an event for the specified account and event Id. The event must be sent through a JSON Body object, with the same format of the create method. 
 * *DELETE `/schedule/delete/{id}`* -> Delete an event for the specified account and event Id. 

* Middleware

This folder contains a Middleware for exception handling to better inform the users of their mistakes or what happened in a process.

#### Calendar.Domain

Here you may find the services for the application, and also models, DTOs, Exceptions and credentials getter, alongside AutoMapper Profile.

* Credentials

This will get the Google API Credentials to be used by the services.

* Exceptions

Defines the exception models to be returned when an error happens.

* Models / DTOs

DTOs for when a request is created. 

* Services & Implementation

The Service which will make calls to the Google API, based on the user's needs, and also make checks on objects sent by the users on create and update methods.

* AutoMapper

Maps the DTOs created by the developer to the objects that Google uses on the API.
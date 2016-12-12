# PI-AF-SDK-AsyncToObservable

Many use cases of [async PI AF SDK methods][1] will involve making multiple async calls and awaiting their results. For example, we can make an async call for each PI Point in a collection of these objects. This query seems similar to a synchronous bulk call using PIPointList or AFListData. However, the difference is that with async, we can vary the query parameters (such as time range) for each PI Point, which is not possible with the bulk call.

This repository contains the supporting content of the following blog post on PI Square:
[Async with PI AF SDK: From Tasks to Observables][2]

Make sure to refer to the post to get all the information.  The post covers the following topics:
- Motivation
- Problem definition
	1. 	Find the Event Frames
	1. 	Synchronous loop over the list of event frames
	1. 	Parallel tasks over the event frames
	1. 	Async calls that are processed only after all tasks complete
	1. 	Async calls using Task.WhenAny to process tasks as they complete
	1. 	Async calls using IObservable<T> to process tasks as they complete
- Bulk call analogy
- Throttling
- References


#Related Content
The content in this repository can be considered as advanced content.  The reader may be interested to start with more basic content on PI AF SDK as well as about the Async topic, the following links can be of interest:

- [Working with PI AF SDK 2016 - Introduction to the blog post series][5]
- [Async with PI AF SDK: Introduction][3]
- [PI AF SDK Best Practices - Summary of Existing Resources][4]



# Licensing

Copyright 2016 OSIsoft, LLC
 
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
 
http://www.apache.org/licenses/LICENSE-2.0
 
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.


[1]:https://techsupport.osisoft.com/Documentation/PI-AF-SDK/html/af7c4d4e-e994-4349-929b-caf562be1c43.htm
[2]:https://pisquare.osisoft.com/community/developers-club/blog/2016/11/04/asynchronous-data-access-with-pi-af-sdk-from-async-to-observable
[3]:https://pisquare.osisoft.com/community/developers-club/blog/2016/10/24/async-with-pi-af-sdk-introduction
[4]:https://pisquare.osisoft.com/community/developers-club/blog/2016/09/08/pi-af-sdk-best-practices-summary-of-existing-resources
[5]:https://pisquare.osisoft.com/community/developers-club/blog/2016/06/23/working-with-af-sdk-2016-introduction
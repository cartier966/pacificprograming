# pacificprograming
small test project of api

1. How did you verify that everything works correctly?

   I wrote some unit tests of the controller action method. I tested the logic by verifying the correct service method was being called for each case.

   Also I ran the solution in Dev mode and navigated to the swagger end point (https://localhost:7206/swagger/index.html) and ran various test cases.

   Also I ran the index.html file and tested by changing the value in the input field and pushing the button
   
3. How long did it take you to complete the task?

   4 hours (I struggled a bit remembering how to set up a test...)
   
5. What else could be done to your solution to make it ready for production?

   Many, many things:

   -error handling

   -the base urls used could come from appsettings, as well as path to file

   -more logging could be implemented

   -a seperate provider should be implemented for the SQLite db actions, the service call to external API (and the JsonFile actions which are not used)


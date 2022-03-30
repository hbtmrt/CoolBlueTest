# Insurance Cost Calculator

## Test cases
[![Passed Test Cases](https://github.com/hbtmrt/CoolBlueTest/blob/master/PassedTest.PNG "Passed Test Cases")](https://github.com/hbtmrt/CoolBlueTest/blob/master/PassedTest.PNG "Passed Test Cases")

## Endpoints

## Tasks
### Task-1 - Bug fixing
- Here, it adds 0 instead of 500; changing it fixed the problem

### Task-2 Refactoring
- Moved InsuranceDto class into separate files
- Retrieved the 'ProductAPI' value from the configuration and removed its hardcoded value.
- Changed HTTPPOST to HTTPGET for 'CalculateInsuranceAsync' endpoint.
- Changed the route name since it was not into standards.
- Moved constants to a new file.
- Moved messages to a resource file so that we can do localization easily in the future.
- Added 'sealed' keyword to some classes if they are not going to be inherited.
- Used strategy pattern and iterator pattern to calculate the insurance cost. This way, we can have new logic without touching the existing classes, making the code maintainable.
- Moved HTTP class to a separate file. 'HttpClientWrapper'
- Added exception handling
- Created custom exceptions
- Renamed some unreadable variables.
- Renamed 'UnitTest1' class to 'InsuranceTest'
- Removed unwanted imports.

### Task-3 Feature 1: Calculate insurance cost for an order.
- Added a new 'Controller' called 'OrdersController' and added a new endpoint.
- Made the method asynchronous.
- The insurance cost is calculated asynchronously for each product in the order. 
- Used HTTPPOST rather than HTTPGET since we need to send a huge list of products. If we use HTTPGET and have a huge list of products, the URL will be long, and at some point, it breaks.

### Task-4 Feature 2: Adding special insurance costs
- Used iterator patterns to get all the special insurance costs; in this case, we have only one special insurance for Digital Cameras per order. But in the future, we can add more special rates easily by adding them to the list.

### Task-5 Feature 3: Adding product types with surcharges
- Only the administrators/staff should access this endpoint, but it was not handled since it is out of scope; I was also confirmed that there was no need to add it through the slack communication.
- The surcharge is stored in a JSON file.
- Since this will be used by multiple users simultaneously, I have added 'ReaderWriterLockSlim' to prevent errors.
- Every time the application loads, the product type surcharges are loaded.

## Things to improve
- More documentation
- More test cases
- More testing using with Product API.

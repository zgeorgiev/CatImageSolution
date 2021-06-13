# Cat Images
## Requirements
Create a RESTful API that generates upside-down kitten pictures:
1. The API should fetch cat images from the Cat as a service API (https://cataas.com) and flip the image upside down.
2. The endpoint should require basic authentication to authorize its use.
3. The API should provide some method to register new users.
4. Users should be stored in a database (can be in-memory).
5. Some form of API documentation

You could make the endpoints like so:

- An endpoint to show the upside-down cat. Accessing in the browser should display the image directly without requiring download.
- Endpoints, which return different types of images which are manipulated in different ways (eg.: upside down, black-white, blurred, ...)
- An endpoint to create a new user (registration)
- An endpoint to get user information

Please include a readme file which outlines your decision-making process while undertaking the test.

## Design phase
Since the application should communicate with external system first I researched what are the capabilities of the external system and noticed that it not only provides the cat images but also we can request image with additional filters like  BLUR, MONO, SEPIA, NEGATIVE, PAINT and PIXEL.

For covering the flip functionality I decided to use System.Drawing.Image class RotateFlip method. Documentation can be found here: https://docs.microsoft.com/en-us/dotnet/api/system.drawing.image.rotateflip?view=net-5.0

## Architecture phase
Application is designed according clean architecture principles which splits it to several logical layers and provides easier testing of each layer and cleaner code.

### Domain
Contains common models and entities used throughout the application like common Exceptions, extension methods, entities, enums, etc.
This project does not depends on any other project from the application
### Application
This project contains application logic definisions and implementations. All application related login is implemented here except the logic related to external components like DB, user management, third party systems.
This project depends on Domain project
### Infrastructure 
Locig which relates to an external sources like user management, db storage, external system communication like in this case the Cats API is implemented in Infrastructure application. This will allow much easier in future to switch implementation of these systems without this to affect the application logic. Image service is implemented as abstraction if we decide to switch the image source in future.
This project depends on Application and Domain projects
### Web Api
This is the main web api application. This project depends on Application and Domain projects as well as Infrastructure project but Infrastructure project is accessed only in Startup.cs class where we configure services dependency injection and db seed.
### Web Api Data Contracts
This project holds Web Api data contract models which later can be exported as nuget package if we plan to implement our api client library

## Storage
For the purpose of this application I used in-memory db and communicate with it by Entity Framework. On application start if the DB is empty I insert one user in it for easier testing of the application
**Username: demouser**
**Password: demopass**

## Authorization
By requirements the authorization of the requests should be Basic. Implementation of basic authorization is plugged-in as authentication scheme in .net core Authentication middleware. Since .net core does not provide built-in implementation I had to implement a new authorization handler for this basic scheme.

## Error handling
A global exception filter is implemented to unify the structure of the errors returned by the API. This filter handles Unauthorized exceptions, custom exceptions from the system, Invalid model state exceptions, and all other unknown exceptions

## API documentation
Web Api documentation uses Open API/ Swagger documentation for documenting and testing of the API endpoints. It also allows basic authentication user to be set for the test requests.

## Testing
Two additional projects are added containing tests. Application.UnitTests contains all unit tests which covers Application logic. WebApi.IntegrationTests contains all integration tests for testing web api functionality. All of these tests can be extended. I haven't targeted 100% code coverage.

# API Endpoints
## Cat
- **/Cat/flipped?filter={filter}** - **GET** Get flipped cat image. This endpoint requires basic authentication and returns flipped image of a cat from external cat service. It also allows adding filter as query parameter which can be one of the following values BLUR, MONO, SEPIA, NEGATIVE, PAINT or PIXEL. Filter is optional
- **/Cat?filter={filter}&rotationType={rotationType}** - **GET** Get cat image. This endpoint requires basic authentication and returns cat image. If Filter is one of the following values (BLUR, MONO, SEPIA, NEGATIVE, PAINT and PIXEL) it will be applied to the image. If rotation type is one of the following values (Rotate180FlipXY, RotateNoneFlipNone, Rotate270FlipXY, Rotate90FlipNone, Rotate180FlipNone, RotateNoneFlipXY, Rotate270FlipNone, Rotate90FlipXY, Rotate180FlipY, RotateNoneFlipX, Rotate270FlipY, Rotate90FlipX, Rotate180FlipX, RotateNoneFlipY, Rotate270FlipX, Rotate90FlipY) a rotation will be applied to the image. Filter and rotation type are optional
## User
- **/User** - **GET** Get user details. This request must be authorized and will return user details of the authorized user.
- **/User** - **POST** Register new user. This request does not requeres authorization and is used to register a new user.

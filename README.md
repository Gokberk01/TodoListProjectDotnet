ToDoList Web API This project is a Web API developed for a ToDoList application. The project is built using .NET Web API, with JWT and Access Token implemented for authentication. Additionally, a MongoDB database is set up using Docker and integrated into the project. The frontend interface is developed using Angular.

Features
CRUD Operations: Create, Read, Update, and Delete (CRUD) operations are implemented for ToDo items.
JWT Authentication: JWT and Access Token are used for user authentication.
MongoDB Integration: The MongoDB database is set up via Docker and integrated with the API.
Unit Testing: Unit tests are written using xUnit, with repositories being mocked for testing purposes.
Project Structure
Backend (.NET Web API)
Layers: The project consists of Controller, Service, Repository, and Model layers.
Authentication: JWT-based authentication mechanism is implemented to ensure user security.
Database: MongoDB is used as the database, which is set up via Docker.
Frontend (Angular)
Project: The frontend application is developed using Angular.
Connections: The necessary connections between the Web API and the Angular application are established.
Technologies Used
Backend:
.NET 8.0
MongoDB.Driver
xUnit
Moq
JWT
Frontend:
Angular
Database:
MongoDB
Docker
Development and Testing Process
Unit Testing: Throughout the project, unit tests have been written using xUnit. In these tests, repositories were mocked, and service layers were tested.
Mocking: To reduce data dependencies during the testing process, repositories were mocked using Moq.

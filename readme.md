# Api Test Demo
ApiTestDemo project is a demonstration of setting up a test suite for running integration tests on a REST API built with ASP.NET Core.

This project is designed to provide developers with a clear and practical guide on how to implement integration tests using NUnit, a popular testing framework for .NET applications.

## Requirements to run the project

* .NET 7
* MSSQL Server database or compatible (e.g. Azure SQL datbase)

## The application

The application deliberately keeps its logic and architectural patterns simple to highlight the core purpose of demonstrating integration tests. As a result, it exposes only two straightforward endpoints: one for creating an item in a to-do list and another for retrieving it.

### Api documentation

#### Get Todo by ID

<details>
  <summary><code>GET /api/todos/{id}</code></summary>

#### Parameters

| Name   | Type     | Data Type | Description                  |
|--------|----------|-----------|------------------------------|
| `id`   | Path     | Integer   | The ID of the todo item.     |

#### Responses

| HTTP Code | Content-Type           | Response                          |
|-----------|------------------------|----------------------------------|
| `200`     | `text/plain`, `application/json`, `text/json` | [TodoDto](#tododto) |
| `400`     | `text/plain`, `application/json`, `text/json` | [ProblemDetails](#problemdetails) |
| `500`     | -                      | Server Error                      |

#### Example Request

curl -X GET http://api.example.com/api/todos/1

</details>

#### Create a Todo

<details>
  <summary><code>POST /api/todos</code></summary>

#### Request Body

| Content-Type           | Schema                         |
|------------------------|--------------------------------|
| `application/json`     | [TodoForCreationDto](#todoforcreationdto) |
| `text/json`            | [TodoForCreationDto](#todoforcreationdto) |
| `application/*+json`   | [TodoForCreationDto](#todoforcreationdto) |

#### Responses

| HTTP Code | Content-Type           | Response                          |
|-----------|------------------------|----------------------------------|
| `201`     | `text/plain`, `application/json`, `text/json` | [TodoDto](#tododto) |
| `400`     | `text/plain`, `application/json`, `text/json` | [ProblemDetails](#problemdetails) |
| `500`     | -                      | Server Error                      |

#### Example Request

curl -X POST -H "Content-Type: application/json" -d '{"title":"New Todo","description":"This is a new todo"}' http://api.example.com/api/todos


</details>

#### Data Models

<details>

  <summary><code>TodoDto</code></summary>

A representation of a Todo item.

| Property      | Type    | Constraints                   |
|---------------|---------|-------------------------------|
| `id`          | Integer | 64-bit integer                |
| `title`       | String  | Min length: 3, Max length: 100 |
| `description` | String  | Min length: 3, Max length: 500 |
| `isCompleted` | Boolean | -                             |

</details>

<details>

  <summary><code>TodoForCreationDto</code></summary>
A representation of a Todo item for creation.

| Property      | Type    | Constraints                   |
|---------------|---------|-------------------------------|
| `title`       | String  | Min length: 3, Max length: 100 |
| `description` | String  | Min length: 3, Max length: 500 |

</details>

<details>

  <summary><code>ProblemDetails</code></summary>

A representation of a problem details response.

| Property      | Type    | Nullable |
|---------------|---------|----------|
| `type`        | String  | true     |
| `title`       | String  | true     |
| `status`      | Integer | true     |
| `detail`      | String  | true     |
| `instance`    | String  | true     |

</details>

## Integration Testing against APIs

### Introduction
Integration testing ensures that an application's API functions correctly by testing how its various components interact. It verifies that API endpoints produce expected results when called, helping ensure seamless communication within the system.

#### Pros:

1. Comprehensive Validation: It provides a holistic assessment of the entire system's functionality by testing interactions between different components, catching potential issues early.

2. Realistic Testing: Integration tests mimic real-world scenarios, offering confidence that the API works as expected when different parts of the application communicate.

3. End-to-End Validation: These tests verify the entire data flow and communication pathways within the application, ensuring that all integrated components function together seamlessly.

#### Cons:

1. Complexity: Writing and maintaining integration tests can be more challenging than unit tests due to the need to set up various system components and dependencies.

2. Resource-Intensive: Integration tests may require substantial resources, including external services or databases, which can slow down the testing process.

3. Time-Consuming: Running integration tests can be time-consuming, especially as the application grows, potentially affecting development and CI/CD pipelines.

### Setup 

The main challenge that this project will face is accessing the database when calling the API. Since the goal is to establish a proper integration test setup, it is not possible to mock the repositories or the data access layer. Additionally, it may be difficult to maintain a database with only the expected data for our tests. Therefore, we will rely on an in-memory SQL-Lite database.

1. Create a test project with NUnit (e.g. ApiTestDemo.IntegrationTests)
2. Install the package <code>Microsoft.AspNetCore.Mvc.Testing</code>. This package provides the following features:

    * TestServer: It includes a TestServer class that allows you to host your ASP.NET Core application in-memory during testing.

    * Integration Testing: You can use the TestServer to send HTTP requests to your application's API endpoints, just like a real client.

    * Dependency Injection: The package seamlessly integrates with the dependency injection system in ASP.NET Core, allowing you to inject services and dependencies required for your tests. This makes it easy to configure your application's services and dependencies specifically for testing.

    * WebApplicationFactory: It provides the WebApplicationFactory class, which simplifies the setup of the TestServer and application configuration for testing. This class helps you create a test environment that closely resembles the production environment.

3. Install <code>Microsoft.EntityFrameworkCore.Sqlite.Core</code>. This package provides SQLite database support for Entity Framework Core, allowing to use SQLite as database.

### Test suite 

 1. First of all, in order to correctly inherit from <code>WebApplicationFactory<Program></code>, it is necessary to create a proper class for your Program.cs file. The class should be structured as follows:
```mermaid
classDiagram
 class Program {
    #Program()
    +Main(args: string[])$
 }
 ```

2. The <code>WebAppFactory</code>: it has to inherit from the <code>WebApplicationFactory`<Program>`</code> and overrides the <code>Configure</code>.

```mermaid
classDiagram
	class WebAppFactory {
	- ReplaceDbContextWithInMemoryDb(services: IServiceCollection)$
	+ ConfigureWebHost(builder: IWebHostBuilder)
	+ CreateDbContext() : DbContext
	}
	class WebApplicationFactory~TEntryPoint~{
	+ ConfigureWebHost(builder: IWebHostBuilder)
	}

WebApplicationFactory <|-- WebAppFactory
```

This class will be responsible for the configuration of all services required for API integration tests.
In this example, the <code>ReplaceDbContextWithInMemoryDb</code> method replaces the actual database configuration to use the <code>SQLite</code> database.

The <code>CreateDbContext</code> method must ensure that the <code>SQLite</code> database has been created before making calls to the endpoints.

3. The <code>HttpClientFactory</code>: this is a simple yet useful class. Given the <code>WebAppFactory</code>, it will create the instance of the <code>HttpClient</code>. This allows a single location to configure the client properly. It is possible to configure settings such as the <code>BaseAddress</code> or, in more advanced scenario, the <code>Authentication</code> headers.

4. <code>ApiIntegrationTestFixture</code>: This abastract class serves as template for the actual tests. It defines the <code>Setup</code> and <code>Teardown</code> procedures.
To prevent tests from interfearing each others, it is important follow tese guidelines:
* Setup:
    * Create a new istance of the <code>WebAppFactory</code>
    * Ensure that the <code>SqlLite</code> database has been created correctly.
    * Create a new istance of the <code>HttpClient</code>
* Teardown
  * Delete the database and dispose it
  * dispose the WebAppFactory and HttClient

5. Finally we can test the code:

<code>

    [Test]
    public async Task Post_ValidTodo_Returns201Created()
    {
        var todo = new TodoForCreationDto { Title = "Title", Description = "Description" };

        var httpResponseMessage = await HttpClient.PostAsJsonAsync("api/todos", todo);

        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

</code>

### Conclusion
Setting up an entrire test suite for your application will be quite challenging but at the end the efforts will pay high dividends.
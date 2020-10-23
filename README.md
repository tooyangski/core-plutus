# core-plutus
EF Core, Repository Pattern, UnitOfWork Pattern

This are all in Swagger.

/GET
- Get All Employees
    - Example:
        - https://localhost:44363/api/v1/employees/

- Get All Employees with firstname or lastname
    - Example:
        - https://localhost:44363/api/v1/employees/?FirstName=John&LastName=Sun

- Get All Employees with Created Date Range, Do not forget to encode first the query string because it wont work, you can encode it here: https://www.urlencoder.org/ after encoding you can copy and paste it to the url.
    - Example:
        - https://localhost:44363/api/v1/employees/?createdAtStart=2020-10-23T20%3A23%3A58.54241%2B08%3A00&createdAtEnd=2020-10-23T21%3A45%3A23.7934656%2B08%3A00

- Get All Employees with Temperature Range
    - Example:
        - https://localhost:44363/api/v1/employees/?temperatureStart=35&temperatureEnd=37

/POST
- Post an Emloyee
    - Example:
        https://localhost:44363/api/v1/employees
    - Data: 
    {
    "firstName" : "Axl",
    "lastName" : "Doe",
    "temperature" : 35.5
    }
    
/PATCH
- Post an Emloyee
    - Example:
        https://localhost:44363/api/v1/employees
    - Data: 
    {
    "id" : 3,
    "firstName" : "Max",
    "lastName" : "Sun",
    "temperature" : 35.5
    }
    
    
Any comments/suggestions would be appreciated, thanks! :)

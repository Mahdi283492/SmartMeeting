A Controller is responisble for handling HTTP requests and it execute bussiness logic and database operation, returning http response
map routes /api/className and http operation (GET, POST, PUT, DELETE)
take the DTOs and validate them and uses the AppDbContext to query or update database
it acts between the view which is the client and UI and the Model which is the data 
Example:
receives the http request (User input) at the defined routes 
interacts with Model layer to perform data operation
applies the bussiness logic and validitions 
returns an HTTP response 
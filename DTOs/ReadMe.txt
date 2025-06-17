A Data Transfer Object (DTO) defines the precise shape of data exchanged with the API endpoints,
specifying exactly which fields a request must include and which the response will return.
By exposing only the necessary properties for each operation, DTOs enhance security and clarity, 
preventing over posting (sending extra, unused data) and under posting (omitting required data).
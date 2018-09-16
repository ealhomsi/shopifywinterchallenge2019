# shopify winterchallenge 2019
This is a small store written in asp.net core using cosmos-db as persistence.

This project is written in C# using asp.net core latest version and cosmos db.

Running instructions:
1. download and install the asp.net core sdk from https://www.microsoft.com/net/download
2. download and install cosmosdb emulator from https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator
3. open the project using visual studio and you are all set!

Documentation:
The structure of cosmos db are documents which represents different shops (every shop has its own document)
The code for every model/controller is organized in a way; there is a seperate controller for each model.

Use:
This is a backend-server api therefore it should be tested using postman or a similar tool

example:

sending a post request to https://localhost:44395/api/shop/2223/product
with json body of
```
{
	"id": "2223",
	"products": {"lemons" : {"name": "lemons", "price": 3.14}},
	"orders": null
}
```

ToDo:
1. Errors returned are all 500 for now (return more detailed errors like 400, ...)
2. Adding more detailed operations. 
3. Adding DTOs and other services
4. Adding tests.

Thanks you!!!!


Authors:
Elias Al Homsi

# The Shopping App
## Project Description
The Shopping App is a web app designed for Small and medium store owners plus their customers. The purpose of the app is to increase social distancing in stores by having customers make their orders online and then pick them up when are they ready.
## Technologies Used
- C#
- PostgreSQL DB
- EF Core
- Xunit
- Serilog or Nlog
- Azure
- Github Actions
- ASP.NET Core MVC
## Features
- Customer signup and login
- Creation of an order by customer
- View of customer order history
- Creation of a store by a manager
- View of store orders by a manager
- Addition of new products in the store 
- Replenishment of inventory by manager
- View of store order history
- Sorting of order histories by Cost and Date

Features to add or improve
- Create a dynamic cart
- Add product images
- Add product description 
- Enable user to delete items from a cart

# User stories
- As a customer, I can use my phone number to sign in.
- As a customer, I can check if I already have an existing account when signing up.
- As a customer, I can select a store location to make orders to.
- As a customer, I can view a list of available products in a store.
- As a customer, I can select multiple products and add them to a shopping cart.
- As a customer, I can view my shopping cart and go back to shopping.
- As a customer, I can edit items in a shopping cart.
- As a customer, I can get confirmation if my order is placed successfully.
- As a customer, I can view my order histories with details.
- As a manager, I can choose a designated store branch location.
- As a manager, I can add a new branch location.
- As a manager, I can add a new product.
- As a manager, I can view and select products that have no inventory yet.
- As a manager, I can add inventory to a specific product.
- As a manager, I can view a list of products that have inventory.
- As a manager, I can replenish inventory to a specific product.

## Additional Features
- Exception Handling
- Input validation
- Logging
- Unit tests
- Sqlite for testing DB Access methods
- Data is persisted to a PostgreSQL DB
- Used code first approach to establish a connection to the DB
- WebApp is deployed using Azure App Services
- A CI/CD pipeline is established using Azure Pipelines
- Used ASP.NET MVC for the UI
- DB structure is 3NF
- Have an ER Diagram

##ERD Diagrams
![image](https://user-images.githubusercontent.com/30950839/138115637-6d3851ce-f064-4d43-a915-2a7e77f6f5da.png)





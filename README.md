# Sample REST API 

Simple REST API(s) demonstrating swagger gen with an increasingly complex range of methods (eventually multiple apps to covert the different levels)

## Tech used

The API is emulating a ticket management like system mostly because I couldn't think of anything better to model.  It's written using .NET6 ASP.NET Core with Entity Framework Core using SQLite database

## Database

The database is migrated (created) on first run so no additional work required there, it even seeds a few entries.

## OpenAPI

The API is documented using Swashbuckle to produce OpenAPI v3 documents for the v1 and v2 of the API.

## Code structure

the projects are separated mostly using HostedStartup assemblies which is a bit more complex to pull off and was done mostly as an excuse to try that out.  The App still directly links each of the projects and their abstractions but the eventual goal is to maybe publish them to a folder like ```target/``` so they don't need to (they're found by filename convention and HostingStartup class).

The projet layout is inspired by domain driven design:

### Core 
The models/projections and value types - these should have no dependencies or very very minimal interface dependencies, the intent is that the business logic goes here and most of the unit test benefit comes from here

## Infrastructure
Databases would be the big example for this, external tools used by the application in conjuction with the domain layer

## Services
The service layer is intended to co-ordinate between the domain and infrastructure layers

## App 
The application itself which for the most part should just translate inputs and send to the service layer to execute

As there are multiple Applications they are prepending by the type, currently this consists of RestApi - A RESTful service and GrcpApi, the beginnings of an equivalent GRPC API

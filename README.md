# Sample API(s)

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

### Domain 
The entities / projections and domain services as well as some of the entity configuration (only those related to validation rules)

## Infrastructure
DbContext and repository wrapper, repository may one day be changed to a facade exposing one DbSet<T> and a context itself as a unit of work

## API
The application layer, including REST controllers, GRPC Services, ... though for the time being these are kept separate for more isolated examples.  These could also represent a microservice if deployed as a full application

## App 
Since this isn't a Microsoervice for now we instead use an App for each though this may yet change

As there are multiple Applications they are prepending by the type, currently this consists of RestApi - A RESTful service and GrcpApi, the beginnings of an equivalent GRPC API

### GRPC Notes

The GRPC app requires a valid certifacate, as a way around this for development purposes the HTTP port should be used but does still require HTTP/2 usage which has been configured in the app

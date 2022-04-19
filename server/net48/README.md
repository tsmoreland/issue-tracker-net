# Sample API(s)

Simple REST API(s) demonstrating swagger gen with an increasingly complex range of methods (eventually multiple apps to covert the different levels).  A .NET Framework equivalent of [Issue Tracker .NET6](https://github.com/tsmoreland/issue-tracker-testapp)

## Tech used

The API is emulating a ticket management like system mostly because I couldn't think of anything better to model.  It's written using .NET Framework ASP.NET using WebApi 2 and Webservices (SOAP) using Dapper ORM with a SQLite database

## Database

The database is migrated (created) on first run so no additional work required there, it even seeds a few entries.

## OpenAPI

The API is documented using Swashbuckle to produce OpenAPI documents for the v1 and v2 of the API, v2 is still in its early stages and the exact version of OpenAPI document has not been checked (simple enough to do I just haven't done it yet)

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


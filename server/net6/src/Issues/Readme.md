# Issues Domain

## Intro

Issues Domain covers the bounded context of issues which will refer the eventual project domain by ids but with no direct interaction so as to keep each separate.
This is leaning towards a micro-service like architecture but keeping it all in a single solutions so more of a distributed monolith in part to serve as a sample
API to test against and in part to experiment with clean arcitecture concepts.

As such it may at times be more complex than it needs to be, particularly with regards to API and REST, GRPC, GraphQL layers - that's done to allow separate applications
for each while re-using the main Issue domain.  A more realistic approach would be to have the App (as a micro-service) contain all the parts of those projects leaving

- Domain
- Infrastructure
- Application

Where the application layer contains MediatR handlers for requests from the REST API, and notifications are used as domain events to notify other domains.  This would
vary slightly if it stayed as a distributed monolith on in so far as the application itself would be a separate project which would reference the 'Application' projects
of multiple domains 

In either case authentication would be handled separately leaving only role permission checks in these domains at the controller level (or by filter if conceivable).  At
the time of writing I don't see any way around that


## Structure

As described above the project(s) are arranged in a domain-driven-design like way with 3 primary parts:

### Domain

Data entities / models with the business logic associated with them, and ideally minimal dependencies.  This library is where the most benefit of unit tests should be seen.
In addition the data contracts implemented by the infrastruture layer.

### Infrastructure

Database Context and repository implementations.  Additionally some specification implementations for the query/select specification interfaces defined in Domain.  Ideally those
would all live in domain but because the domain object uses private fields we require ```Microsoft.EntityFrameworkCore.EF.Property<T>(entity, "fieldName")``` to perform the queries;
for that to work in the domain layer it would need to reference EF Core

### Application 

This is presently split across multiple libraries though typically would be one;

- the API and its abstractions for Version 1 and Version 2
- REST Layer Version 1 and Version 2 
- GRPC Layer services
- GraphQL Schemas

In a typical setup those would all be one and would share as much as it can, for example the messages in GraphQL could double as the DTOs for REST, here we've kept them separate so
we can have separate applications for more isolated experimentation, as such there are extra projects to cover the abstractions used by REST, GraphQL and GRPC projects.

#### Versioning

Primarily for the benefit of REST there are 2 versions of the API which a fair amount of duplication, in part because swashbuckle didn't respond well to having a controller with multiple
```[ApiVersion]``` attributes.  The other reason is open to debate, it violates the DRY (don't repeat yourself) principal in favour of protecting the older version.  It comes down to
a question of maintainance - can we maintain DRY and still follow the open-closed principal - yes in part but Api versions aren't suppost to ever change - they're contracts.

So are we better with a bit of duplication for the benefit of fewer restrictions on future versions as well as better safety for the older versions
(they're less likely to break if they do exactly what they did before); or do we reduce the duplication but need to take extra care with future additions to maintain backwards compatability.

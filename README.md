## About

This is a PoC of a library. The project aims to incorporate various architectural styles as well as software design methods and software tooling to help a development team build a maintainable, scalable and resilient platform. The presented approach involves multiple techniques such as:
- DDD tactical patterns
- monolith modularization
- module vertical slicing
- publisher-subscriber model
- event-driven communication
- domain-centric modelling
- CRUD style modelling
- CQRS
- Inbox/Outbox pattern
- data locality 
- business process oriented modules
- relational modelling
- document modelling
- indexing
- database clustering
- database subscriptions
- optimistic concurrency control
- unit testing
- integration testing
- architecture testing
- structured logging
- containerization

## Main goals

At the functional level *The Library* implements a few core requirements that any system of this kind is expected to meet. It allows users to register new books, search the book catalogue with several criteria, place a book on hold depending on a patron type or manage holds. Also, the system allows neither overlapping nor duplicate reservations in case the same two or more requests are submitted by different users concurrently. All the modules are intended to be process-oriented and rooted in the deep model based on the domain analysis.<br/> At the quality level the platform's design primarily aims to let users reserve resources in a non-blocking fashion. Most importantly, patrons or library staff should not be hindered from accessing books and checking their availability status by:
- other users browsing or querying the system at the same time
- periods of increased system load
- data being scattered across the system modules
- complex queries
- a downtime of a database node
- the breaking of the monolith design into microservices

## Disclaimer

This is not a production-ready code. It is mainly intended to serve educational and training purposes. *The Library* focuses on the backend solution only. Despite being larger and possibly more complex than an average educational project of this sort, it is still a scaffolding rather than an impressive and completed skyscraper. <br/> Secondly, the architectural decisions made during the software design stage always have their upsides and downsides. There are no silver bullets and one-size-fits-all solutions. This is also true about *The Library*. As long as you are aware of the trade-offs or limitations and accept the responsibility for the design, the strategy you have implemented is probably the best one you can afford at a given moment.

## Domain description

- library members are referred to as *patrons*
- patrons can place books on hold at various library branches
- for each available book, only one patron is allowed to place it on hold at any given time
- books are either classified as circulating or restricted
- regular patrons can place holds only on circulating books, while researcher patrons can place holds on both circulating and restricted books
- regular patrons are limited to five holds at any given time
- researcher patrons are allowed to place an unlimited number of holds
- regular patrons can place only closed-ended holds, which expire after a fixed number of days
- researcher patrons place open-ended holds, which do not expire and are fulfilled only when the patron checks out the book
- a daily sheet is used to monitor expiring holds
- any patron with more than two overdue checkouts will not be allowed to place a hold
- a book can be checked out for up to 60 days
- a daily sheet is used to monitor expiring checkouts

## Reservation Archetype

Archetypes are well-known and well-defined solutions to a business problem. The reservation archetype is used when a user wants to reserve a resource for future use. It ensures that resources like appointments, flights, hotel rooms or library items are temporarily blocked or allocated, preventing overbooking or double-booking. The key concepts are:
- *resource availability*: The system checks the availability of a resource before confirming the reservation.
- *reservation hold*: Once its availability is confirmed, a resource can be held for a specified time.
- *confirmation and expiry*: When a user or a system confirms the reservation, the resource is locked in. If not confirmed in time, e.g. a library member does not pick up a book or a hotel room reservation is not confirmed through payment, the reservation expires and the resource is released for other users.

When implementing the reservation archetype in software systems, one of the significant challenges is handling multiple simultaneous attempts to lock the same resource. This can typically be addressed in several ways, depending on system requirements and the specific use case. *The Library* opts to use a simple messaging mechanism to avoid race conditions and ensure data consistency in a concurrent environment while maintaining modularity and scalability.

## Persistence

The persistence layer usually introduces its own complexity to software systems. As much as the domain-centric approach aims to offload complex infrastructure concerns to the external boundaries of a business application, integrating storage middleware with business components will always pose challenges. These include mapping aggregates to database entities, storing value objects, or emitting domain events while persisting them alongside other business objects in a single transaction. All of these considerations must align with scalability, performance, and data access patterns. One important architectural decision regarding data persistence is that *The Library* chooses not to separate domain entities from database records or documents. Such separation is often illusory, as both tend to evolve together. Typically, an update to the structure of a domain entity requires a corresponding update to the database record. Unless the two models can truly evolve independently, maintaining both introduces unnecessary complexity into the software system. Additionally, keeping a single entity, rather than a database record and its mapped domain counterpart, allows for more optimal resource utilization with change tracking capabilities.
<br/>
*The Library* uses *PostgreSQL* and *RavenDB* as its data storage engines, which seem to work well in tandem with the system’s modularization and data locality. While this is not the most straightforward solution and could be replaced with a single database server, it allows for comparing the two technologies and their approaches to data persistence and serves educational purposes, with ease-of-use and configuration or integration effort in mind. In the case of PostgreSQL and RavenDB, the project explores the options such as:

- client-server communication
- unit of work pattern
- client side entity change tracking
- command and query side of data persistence
- data seeding
- replication
- clustering
- high availability and failover 
- integration testing
- indexing
- schema-oriented vs schemaless models
- listening to data updates
- optimistic concurrency control
- contenerization

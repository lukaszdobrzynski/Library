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

This is not a production-ready code. It is mainly intended to serve educational and training purposes. *The Library* focuses on the backend solution only. Despite being larger and possibly more complex than an average educational project of this sort, it is still a scaffold rather than an impressive and completed skyscraper. <br/> Secondly, the architectural decisions made during the software design stage always have their upsides and downsides. There are no silver bullets and one-size-fits-all solutions. This is also true about *The Library*. As long as you are aware of the trade-offs or limitations and accept the responsibility for the design, the strategy you have implemented is probably the best one you can afford at a given moment.

## Reservation Archetype

Archetypes are well-known and well-defined solutions to a business problem. The reservation archetype is used when a user wants to reserve a resource for future use. It ensures that resources like appointments, flights, hotel rooms or library items are temporarily blocked or allocated, preventing overbooking or double-booking. The key concepts are:
- *resource availability*: The system checks the availability of a resource before confirming the reservation.
- *reservation hold*: Once its availability is confirmed, a resource can be held for a specified time.
- *confirmation and expiry*: When a user or a system confirms the reservation, the resource is locked in. If not confirmed in time, e.g. a library member does not pick up a book or a hotel room reservation is not confirmed through payment, the reservation expires and the resource is released for other users.

When implementing the reservation archetype in software systems, one of the significant challenges is handling multiple simultaneous attempts to lock the same resource. This can typically be addressed in several ways, depending on system requirements and the specific use case. *The Library* opts to use a simple messaging mechanism to avoid race conditions and ensure data consistency in a concurrent environment while maintaining modularity and scalability. 
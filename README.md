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
- optimistic concurrency control
- unit testing
- integration testing
- architecture testing
- structured logging
- containerization

## Main goals

At the functional level *The Library* implements a few core requirements that any system of this kind is expected to meet. It allows users to register new books, search the book catalogue with several criteria, place a book on hold depending on a patron type or manage holds. All the modules are intended to be process oriented and rooted in the deep model which stems from the domain analysis. At the quality level the platform's design primarily aims to let users reserve resources in a non-blocking fashion. Most importantly, patrons or library staff should not be hindered from accessing books and checking their availability status by:
- other users browsing or querying the system at the same time
- periods of increased system load
- data being scattered across the system modules
- complex queries
- a downtime of a database node
- the breaking of the monolith design into microservices

## Disclaimer

This is not a production-ready code. It is mainly intended to serve educational and training purposes. *The Library* focuses on the backend solution only. Despite being larger and possibly more complex than an average project of this sort, it is still a scaffold rather than an impressive and completed skyscraper. <br/> Secondly, the architectural decisions made during the software design stage always have their upsides and downsides. There are no silver bullets and one-size-fits-all solutions. This is also true about *The Library*. As long as you are aware of the trade-offs or limitations and accept the responsibility for the design, the strategy you implement is probably the best one you can afford at a given moment.
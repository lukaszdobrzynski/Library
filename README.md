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

## Domain

The following are the detailed functional criteria which *The Library* implements:

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

It is worth emphasizing that *The Library* does not cover the domain discovery process. The techniques available for gathering knowledge about the problem space such as event storming or user story mapping are outside the scope of the project. Instead, *The Library*'s domain structure is largely informed by the widely recognized [ddd-by-examples / library](https://github.com/ddd-by-examples/library) repository. The domain analysis, context distillation or semantic examination for all the requirements have been well-described there and can provide valuable insights into *The Library*'s core functionality, business logic or some of the implementation decisions. Therefore, it is strongly recommended to become familiar with the `ddd-by-examples / library`'s domain description to better understand the practices that underpin *The Library*'s design and approach.<br/> 

## Application ports

*The Library*'s application layer can be run by different types of clients, as described by Alistair Cockburn in his renowned 2005 [article](https://web.archive.org/web/20180822100852/http://alistair.cockburn.us/Hexagonal+architecture) on ports and adapters. These clients can include humans, test runners, message queues, databases, or other externally hosted applications, such as SPAs. They are also referred to as actors or adapters, as highlighted by the article's title, and together they constitute the application's environment.

The interactions between the application layer and the environment are organized into two groups of interfaces known as driver ports and driven ports. Driver ports expose the application's functionality to the outside world, while driven ports define the functionalities required by the application to implement and process business logic. Among *The Library*'s interfaces, which define the contract between the core system and its actors, are:

- `ICommand`, `IQuery`, and `IRequest`, used by clients to obtain information from the system or send requests to change the system's state.
- `IPatronRepository`, `IBookRepository`, `ICheckoutRepository` or `IHoldRepository`, used by the Reservation module to access data needed for processing business logic.
- `IDocumentStoreHolder`, used by the Catalogue module to persist or query data.
- `IDomainNotification`, used to notify system actors about changes in the application, prevent domain events leakage and serve as public contract.

Decoupling an application from its underlying technology using ports will inevitably introduce a layer of abstraction, which results in additional method calls during the interaction between the port and the adapter. This is necessary because the adapter facilitates communication between the generic application logic and the specific technology interfaces. Furthermore, it may require mapping or translating objects between the internal application domain and external systems, ensuring that the application remains modular. This indirection introduces most of the complexity within *The Library* and can present challenges when navigating the project or understanding the overall architecture at first glance.    

## Modularization

*The Library* is deployed and run as a single process, but it is architecturally designed to be modular. The project is organized into three primary modules: the API module, the Reservation module, and the Catalogue module.         

- API module serves as one of the driver ports, handling all request processing to ensure smooth communication between users and the system.
- Reservation module enables library members to manage their interactions with the library. It allows them to place books on hold, view, and manage their reservations and loan activities.
- Catalogue module is responsible for reliable search functionality, enabling users to find and locate resources efficiently. It also includes an availability engine, which maintains resource availability and consistency by preventing double-bookings.

The Reservation and Catalogue modules each have their own encapsulated and independent data structures and components. They abstract their implementation details and internal structures from external systems, exposing only a public interface that is accessed through the API module. Each module has its own dependency injection container and utilizes a dedicated data store.

Furthermore, each module implements a deep model tailored to its specific problem domain. The Reservation module is centered around a pure domain-driven design core, focusing on complex business logic and rules. In contrast, the Catalogue module follows a CRUD-oriented approach, prioritizing efficient management of resources and data persistence. 

Monolithic modularization enables the independent and flexible evolution of models within a shared codebase. It facilitates independent team collaboration by defining clear module and language boundaries and responsibilities. Additionally, a well-modularized monolith can serve as a foundation for a relatively straightforward migration to a microservices architecture when needed.     

## Persistence

The persistence layer usually introduces its own complexity to software systems. As much as the domain-centric approach aims to offload complex infrastructure concerns to the external boundaries of a business application, integrating storage middleware with business components will always pose challenges. These include mapping aggregates to database entities, storing value objects, or emitting domain events while persisting them alongside other business objects in a single transaction. All of these considerations must align with scalability, performance, and data access patterns.<br/> 
One important architectural decision regarding data persistence is that *The Library* chooses not to separate domain entities from database records or documents. Such separation is often illusory, as both tend to evolve together. Typically, an update to the structure of a domain entity requires a corresponding update to the database record. Unless the two models can truly evolve independently, maintaining both introduces unnecessary complexity into the software system. Additionally, keeping a single entity, rather than a database record and its mapped domain counterpart, allows for more optimal resource utilization with change tracking capabilities that database client libraries offer. 
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

## Reservation Archetype

Archetypes are well-known and well-defined solutions to a business problem. The reservation archetype is used when a user wants to reserve a resource for future use. It ensures that resources like appointments, flights, hotel rooms or library items are temporarily blocked or allocated, preventing overbooking or double-booking. The key concepts are:
- *resource availability*: The system checks the availability of a resource before confirming the reservation.
- *reservation hold*: Once its availability is confirmed, a resource can be held for a specified time.
- *confirmation and expiry*: When a user or a system confirms the reservation, the resource is locked in. If not confirmed in time, e.g. a library member does not pick up a book or a hotel room reservation is not confirmed through payment, the reservation expires and the resource is released for other users.

When implementing the reservation archetype in software systems, one of the significant challenges is handling multiple simultaneous attempts to lock the same resource. This can typically be addressed in several ways, depending on system requirements and the specific use case. Some of the common options are:
- pessimistic locking
- optimistic concurrency control
- transaction management with strict isolation level such as SERIALIZABLE
- resource locking within an application using in-memory structures or distributed locking mechanisms
- message passing with actor-based model or queues

To avoid race conditions and ensure data consistency in a concurrent environment while maintaining modularity and scalability *The Library* opts to use a real time messaging tool based on RavenDB subscriptions. The mechanism comes with such features as:
- built-in support for leader election among multiple clients
- processing without duplication or loss
- batch size configuration to adjust throughput
- concurrent processing with multiple workers and a document lock applied

When implementing a reservation archetype in a software system, using message passing as a communication mechanism introduces specific trade-offs. One notable drawback is the potential for increased latency, as reservation requests are routed through additional processes or intermediaries, such as message brokers or intermediate services, rather than being handled directly within a single system component. This trade-off, however, is a deliberate architectural decision made during *The Library*'s design phase to prioritize modularity and the independence of domain models.  

## Setup and Run

- [download](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) and install .NET 7.0 SDK
- install Docker
- checkout [Patroni](https://github.com/patroni/patroni) repository and adjust docker-compose build context in `Library.Database.Migrator.Psql` project
- edit the local DNS mappings file (System32\drivers\etc\hosts on a Windows machine) and add `library-one`, `library-two` and `library-three` domain names to point to the loopback IP address (127.0.0.1)
- navigate to Database directory and run `dbup.ps1` script
- set the startup item in your IDE to `Library.API` project (not IIS Express) and run the selected configuration
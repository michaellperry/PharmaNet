An example project illustrating patterns of:
* Domain Driven Design
* CQRS
* Event Sourcing
* Event Driven Architecture

# The problem domain

This example is taken from a project that I led at AmerisourceBergen Specialty Group.
The project was to build a rebate performance tracker for a pharmaceutical network.
Clinics joined this network in order to earn special rebates on drugs. The margins on
healthcare are so small that these rebates often meant the difference between serving
patients and going out of business. This system showed them the progress that they were
making toward earning rebates.

## Rebates

The Rebates bounded context captures all of the business rules governing a rebate
program. An account manager constructs the rules for a rebate. These rules tell the
system how progress is to be measured. For example, the account manager could elect to
measure percent growth over the prior period, or percent of sales vs. a competing
product.

The account manager then sets the thresholds for various award tiers. For example,
they might specify that when a participant exceeds 5% growth over the prior period,
they earn a 3% rebate. When they exceed 20% growth, they earn a 5% rebate.

The account manager enters this information using a rich client application. This
example will probably not ever feature the actual client application, but it does
include the WCF service layer that the client app calls.

## Sales

The Sales bounded context records the sales of a participant over a measurement period.
This bounded context integrates with the e-commerce platform to pull in actual sales
figures. It includes an anti-corruption layer to protect it against any anomalous data
that may otherwise creep in from the external system.

## Performance

The Performance bounded context calculates the performance of a participant against a
rebate program. It produces report cards that participants can view on-line. The
report card tells them how many units they have purchased within the measured product
group, which award that qualifies them for, and how many units they need to purchase
in order to reach the next tier.

# Layers

Each bounded context has its own solution. This allows different teams to work within
each bounded context without interference from the others. Dependencies between bounded
contexts are strictly managed through messages or service interfaces.

Within a bounded context, the solution is broken into layers. The layers are as Eric Evans
defined them, with the exception of the Infrastructure layer:
* Domain - Entities and value objects
* Application - Services
* Presentation - User Interface or API

The Infrastructure layer is not represented, because it is not domain-specific. Infrastructure
components, when present, are external dependencies on separate solutions.

## Domain

The domain layer is what we are primarily interested in. It contains all of the business
logic. It has all of the entities and value objects of the problem domain. All of these
objects are expressed in terms of the ubiquitous language. A business owner looking at a
model of the domain will be able to verify whether it is an accurate model.

## Application

Above that, the application layer contains all of the business services. These are
behaviors that don’t necessarily belong to a specific entity. For example, the
PerformanceCalculator service creates ReportCard entities. It would be inappropriate
to make this a responsibility of the ReportCard itself. So this service is separated
from the domain and moved up into the application layer.

## Presentation

At the top, the presentation layer exposes the services and domain objects to outside
users. The presentation for some bounded contexts is a user interface. For example,
the Performance presentation layer is a web application that displays report cards
to participants. Or, the presentation layer may be an API that some client application
or external system can use to access the bounded context. For example, the Rebate
presentation layer is a WCF service layer that a rich client application can use to
set up rebates.
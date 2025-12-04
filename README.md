FlatHunt Identity Service

The FlatHunt Identity Service is the authentication and authorization module of the FlatHunt platform — a system for aggregating real-estate listings and performing sublease profitability analysis.

🚀 Features

User registration and login with JWT authentication

Role and permission management

Secure password hashing and token generation

Refresh-token support for long-lived sessions

ASP.NET Core Identity–based user and role management

Integration-ready REST API for other FlatHunt modules

🧩 Tech Stack

.NET 9

ASP.NET Core Identity

Entity Framework Core

SQL Server

JWT Authentication

FlatHunt Parser Service

The FlatHunt Parser Service is responsible for collecting, cleaning, and normalizing real-estate listings from external platforms such as Lun and Flatfy.

🚀 Features

Fetching raw listing data from third-party real-estate APIs

Transforming raw data into unified internal models (Advertisements, Flats)

Deduplication and normalization of incoming data

Error-resilient parsing and retry logic

Extensible architecture for adding new data sources

🧩 Tech Stack

.NET 9

Background processing (Worker Service)

REST API integration

EF Core for storing normalized listing data

FlatHunt Business Service

The FlatHunt Business Service contains the core business logic of the FlatHunt platform, providing filtering, analysis, and management of normalized real-estate data.

🚀 Features

Processing normalized flat listings (Flats table)

Flexible filtering (location, price range, apartment type, etc.)

Generating lists of available units based on user preferences

Basic investment profitability analysis (ROI estimation)

Integration with Identity and Parser services

🧩 Tech Stack

.NET 9

Entity Framework Core

SQL Server

REST API
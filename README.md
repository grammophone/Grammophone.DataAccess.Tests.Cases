# Grammophone.DataAccess.Tests.Cases

Provider-independent MSTest scenario definitions for the Grammophone data access abstractions.

This project contains abstract test classes that exercise standard LINQ behavior, portable terminal methods, portable query extensions, and exception normalization. Concrete provider-specific test projects inherit these cases and supply an `IMusicDomainContainer` implementation.

The tests are written against contracts from [Grammophone.DataAccess](https://github.com/grammophone/Grammophone.DataAccess/tree/adaptQueryOperations) and entities from `Grammophone.DataAccess.Tests.Domain`, allowing the same scenarios to run against EF6 and EF Core.

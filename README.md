RepositoryController
=====================

RepositoryController is a base class for ASP.NET [API controllers](http://www.asp.net/web-api) that takes (almost) all of the boilerplate out of creating basic CRUD functionality in your web APIs.

Quick Start
------------

Two steps. Ready? Ready.

1. Derive a new controller from ```RepositoryController<TValue, TKey>```, where ```TValue``` is the type of value this controller provides access to, 
and ```TKey``` is the type of the key property on ```TValue``` objects. Implement ```IRepository<TValue> { get; }```. 

2. Return anything that implements ```IRepository``` from the [Repository](https://github.com/matthewschrager/Repository) library.

That's it. Your new controller will accept GET, POST, and PATCH, and DELETE requests. GET requests will retrieve the appropriate objects from the repository (either a single record or all records, 
depending on whether an ID was supplied with the request). POST requests will store a new object in the repository. PATCH requests perform partial updates on existing objects. And
DELETE requests will, well, delete existing objects.

Also: ```RepositoryController``` returns ```IQueryable<T>``` on GET requests, so it supports the same subset of OData supported by ASP.NET Web API.

Variations
-----------

What if you want your controller to store a different type of value than it accepts during POSTs? This might be useful if you want to transform POSTed data before you store it. To support
this functionality, derive instead from ```RepositoryControllerWithPostType<TValue, TPost, TKey>```, where ```TPost``` is the type of value you want your controller to accept on POSTs.
You'll also need to implement one additional function, ```PostedObjectToStoredObject```, which transforms the POSTed data into the data you want to store in your repository.
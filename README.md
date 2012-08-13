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

Examples
--------

Creating a basic CRUD API controller with GET/POST/PATCH/DELETE functionality:

```C#
public class MyObjectsController : RepositoryController<MyClass, String>
{
    //===============================================================
    protected override IRepository<MyClass> Repository
    {
        get { return MyRepositoryInstance; }
    }
    //===============================================================
}
```

It's that simple. This API controller will respond to the following requests:

1. GET requests of the form ```/MyObjects/{id}```, where {id} is an optional object ID. If an ID is given, a single record is returned. If no ID is given, all records are returned. Note that
in this case the records can be filtered server-side using the subset of OData supported by ASP.NET Web API.

2. POST requests of the form ```/MyObjects```, with the JSON-encoded object as the message body. This will save the given object to the repository (overwriting any existing objects, by default).

3. PATCH requests of the form ```/MyObjects/{id}```, with a JSON-encoded ```PatchArguments``` object as the message body. This will patch the record with the given ID according to the given
patch descriptor. See below for more information about ```PatchArguments```.

4. DELETE requests of the form ```/MyObjects/{id}```. This will delete the object with the given ID, if it exists.

More On Patching
-----------------

One of the cooler pieces of functionality supplied by RepositoryController is the ability to perform partial updates (patches) to stored objects. To support this, we make use of the
[PATCH](http://tools.ietf.org/html/rfc5789) verb. Patches are specified using a ```PatchArguments``` object, which describes the update to be performed. ```PatchArguments``` objects
contain three properties:

```UpdateDescriptor```: A JSON string describing the update to be performed. A quick example: say you want to update the ```Speed``` property of a stored object, setting its value to 10.
The JSON string that describes this update looks like ```{ Speed: 10 }```. This is interpreted to mean, "Set the ```Speed``` property of the relevant object to 10", which is exactly what we want.

```UpdateType```: Either "set" or "add". Describes the type of the update to be performed. "set" is used when we want to set the value of a property, while "add" is used when we want to add a value
to a list.

```PathToProperty```: Used when patching a property on a nested object. If ```PathToProperty``` is specified, the update will be performed on the nested object it describes instead of the 
top-level object. For example, say we have an object with the following structure:

```
{
	TopLevelProperty: "blah",
	NestedObject: 
	{
		DoubleNestedObject:
		{
			NestedProperty: "nested value"
		}
	}
}
```

In order to update ```NestedProperty```, we would specify ```PathToProperty``` as "NestedObject.DoubleNestedObject", and ```UpdateDescriptor``` as ```{ NestedProperty: "new value" }```.

#### Potential Improvements ####

Admittedly, ```PathToProperty``` is a little clunky. Why couldn't we just specify nested properties in the update descriptor, like ```{ NestedObject.DoubleNestedObject.NestedProperty: "new value" }```?
Of course, we could do it this way. This will be implemented in a version soon.

Variations
-----------

What if you want your controller to store a different type of value than it accepts during POSTs? This might be useful if you want to transform POSTed data before you store it. To support
this functionality, derive instead from ```RepositoryControllerWithPostType<TValue, TPost, TKey>```, where ```TPost``` is the type of value you want your controller to accept on POSTs.
You'll also need to implement one additional function, ```PostedObjectToStoredObject```, which transforms the POSTed data into the data you want to store in your repository.
change! change 2

[![Build status](https://ci.appveyor.com/api/projects/status/9suu66447805c6os/branch/master?svg=true)](https://ci.appveyor.com/project/System1Group/lib-result/branch/master)

# Result
C# type that represents either the result of a task that may succeed, or the task itself. Similar to Result types in functional languages.

In some cases a method can be reasonably expected to fail - for instance if it attempts to read a file that may not exist, or accesses a network resource that may not be available. In these cases C# offers several patterns to deal with that method, but these are often verbose or unsuitable for many use cases. Consider returning a ```Result```!

## By Example
Consider the following method definition, which emulates a database call with a 1% chance of failing:

```csharp
// Return a Result that represents either a Customer object on success, or a string on failure.
public static Result<Customer, string> GetCustomer(int id)
{
    if (new Random().Next(100) != 1)
    {
        return new Customer { Id = id, Name = "John Smith" };
    }

    return "Could not retrieve customer";
}
```

Once we've got a ```Result``` type in hand it can be handled in several ways. The result type forces us to explicitly handle the possibility of an error state (even if we only handle it by saying 'assume success or throw'):

```csharp
// Give the type an Action to perform on success, and another on failure
var customerResult = GetCustomer(10);
customerResult.Do(
    onSuccess: customer => Console.WriteLine($"Customer name is: {customer.Name}"),
    onFailure: error    => Console.WriteLine($"Error! {error}"));

// If passed Funcs the results can be assigned:
var gotCustomer = customerResult.Do(
    customer => true,
    error    => false);

// There is a shortcut for the example above:
gotCustomer = customerResult.IsSuccess;
gotCustomer = !customerResult.IsFailure;

// We can assume success and attempt to unwrap the inner value.
// This will throw if our assumption is incorrect.
var customer = customerResult.Unwrap();

// To be a bit safer we can provide a default value - in this case null
customer = customerResult.UnwrapOr(null);

// Or lazily if calculating the default is an expensive operation
customer = customerResult.UnwrapOr(() => new Customer);

// We can also assume there was an error.
//This unwraps the 'failure' value or throws if the result represents a success.
var error = customerResult.UnwrapError();
```

## ```Result``` workflow
The ```Result``` type allows you to 'use' the inner value without unwrapping it using several ```Bind``` methods.

```csharp
var five = 5;
var successFive = (Result<int, string>)five; // Could also have used 'Result.Success<int, string>(five)';
var successTen = successFive.Bind(i => i + 5); // Value is accessed without unwrapping
successTen.Unwrap(); // 10!

var failureFive = (Result<int, string>)"some error"; // 'Result.Failure<int, string>("some error")';
var failureTen = failureFive.Bind(i => i + 5); // Value is mutated without unwrapping
failureTen.UnwrapError(); // "some error"!
```

But why is that helpful? It creates a nice workflow when you're working with chains of methods that might want to do work with a value, but might also fail.
Consider the (highly contrived) example below where you must get a customer, then the customer's address, then orders for that address. The resulting method might look like:

```csharp
public static IList<Order> GetOrdersForCustomer(int id) {
    Customer customer;
    try {
        customer = GetCustomer(id);
    }
    catch(Exception ex) {
        // Handle error case
    }

    Address address;
    try {
        address = GetAddress(customer);
    }
    catch(Exception ex) {
        // Handle error case
    }

    IList<Order> orders;
    try {
        orders = GetOrders(address);
    }
    catch(Exception ex) {
        // Handle error case
    }

    return orders;
}
```

Yes, I know. You wouldn't actually write code like that. But it illustrates how error handling often works in C#: there are multiple exit points from a method, most of them dealing with the many ways a method like this can fail. If each of these methods returned a ```Result<TSuccess, TFailure>``` type with a common ```TFailure``` (say a RetrievalError DO or an enum), the code could be simplified to:

```csharp
public static IList<Order> GetOrdersForCustomer(int id) {
    // BindToResult works like Bind, but expects a Result<TSuccess, TFailure>
    // where TFailure matches with the current Result
    return GetCustomer(id)        // We have a Result<Customer, TFailure>
        .BindToResult(GetAddress) // Now a Result<Address, TFailure>
        .BindToResult(GetOrders)  // And now a Result<IList<Orders>, TFailure>
        .UnwrapOr(error => /* Handle error case */); // Handle TFailure in error case
}
```

This compresses the code by removing a lot of boilerplate and moves the error handling to a single place. This style of pushing errors down to the end of the method and handling them there has been called 'railway oriented programming' and Scott Wlaschin did a brilliant talk on it you can find [here](https://vimeo.com/113707214) if you're interested.

## ```LazyResult```
Earlier I said a ```Result``` can represent the result of a task, or the task itself. If you want the result to wrap the task, unperformed, you need a ```LazyResult```

```csharp
var eagerCustomer = GetCustomer();
// Converts normal Result into LazyResult
Result<Customer, string> lazyCustomer = LazyResult.Create(GetCustomer);
// eagerCustomer and lazyCustomer are both Result<Customer, string>
// But eagerCustomer has already calculated it's value.

var eagerName = eagerCustomer.Bind(c => c.Name); // Bind is performed immediately
var lazyName = lazyCustomer.Bind(c => c.Name);   // Bind is deferred

eagerName.UnWrap(); // Work is already done
lazyName.UnWrap();  // Work is done here
// if the LazyResult is unwrapped again the work is NOT done again - you get the same value
```

## ```TryResult```
You can convert a method that might throw into a Result using TryResult

```csharp
Result<Customer, Exception> customerFive = new TryResult(() => GetCustomer(5));

// Result<TSuccess, Exception> can be unwrapped in a way that throws the Exception if it exists
customerFive.UnwrapOrThrow();
```

NOTE: The ```TryResult``` is not as lazy as a LazyResult. It'll get the value on ```Bind``` and ```Combine```.

## ```Guard```
Using a guard allows for advanced matching on the inner success and failure values of a result type.

```csharp
var result = Result.Success<int, string>(10).Guard()
  .Success() // Set up some success paths (we could also have set up failure paths)
  .Where(
      s => s == 5,                  // if success value is 5
      s => s.ToString() + " is 5!") // return a string "5 is 5!"
  .Where(s => s % 2 == 0, s => s.ToString() + " is even!")
  .Where(s => s < 0, i => s.ToString() + " is a negative number!")
  .Default(s => s + " is just some number") // We need to provide a default
  .Failure() // Since we "closed" successes we can't go back later to add more paths
  .Where(f => f.Length > 100, f => "Something very involved happened")
  .Where(f => i.StartsWith("Error"), f => "An error occurred: " + f)
  .Default(f => f + " is just some string")
  //// .Success() // Will not work - we've already defined our success paths!
  .Do();          // Both defaults have been defined, only Do() is left to run

Assert.That(result == "10 is even!");
```

## Ahh! So ```Result``` is a Monad then?
Yes. No. Maybe? I think so.
